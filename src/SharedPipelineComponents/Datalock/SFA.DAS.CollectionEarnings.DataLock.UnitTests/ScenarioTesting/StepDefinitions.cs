using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.CollectionEarnings.DataLock.Application.DataLock.Matcher;
using SFA.DAS.CollectionEarnings.DataLock.Domain;
using SFA.DAS.CollectionEarnings.DataLock.Infrastructure.Data.Entities;
using SFA.DAS.CollectionEarnings.DataLock.Services;
using SFA.DAS.CollectionEarnings.DataLock.UnitTests.ScenarioTesting.TestObjects;
using SFA.DAS.Payments.DCFS.Domain;
using SFA.DAS.ProviderPayments.Calc.Shared.Infrastructure.Data.Entities;
using SFA.DAS.TestUtilities.SpecflowTools;
using TechTalk.SpecFlow;

namespace SFA.DAS.CollectionEarnings.DataLock.UnitTests.ScenarioTesting
{
    public class EarningsContext
    {
        public EarningsContext()
        {
            RawEarnings = new List<RawEarning>();
        }
        public List<RawEarning> RawEarnings { get; }
    }

    public class CommitmentsContext
    {
        public CommitmentsContext()
        {
            CommitmentEntities = new List<CommitmentEntity>();
        }
        public List<CommitmentEntity> CommitmentEntities { get; }
    }

    public class ResultsContext
    {
        public DatalockValidationResult DatalockValidationResult { get; set; }
        public DatalockValidationResultBuilder DatalockValidationResultBuilder { get; set; }
    }

    [Binding]
    public class StepDefinitions
    {
        private readonly EarningsContext _earningsContext;
        private readonly CommitmentsContext _commitmentsContext;
        private readonly ResultsContext _resultsContext;

        public StepDefinitions(EarningsContext earningsContext, CommitmentsContext commitmentsContext, ResultsContext resultsContext)
        {
            _earningsContext = earningsContext;
            _commitmentsContext = commitmentsContext;
            _resultsContext = resultsContext;
        }

        [Given(@"I have the following earnings")]
        public void GivenIHaveTheFollowingOnProgrammeEarnings(Table table)
        {
            _earningsContext.RawEarnings.AddRange(DataHelper.CreateRawEarnings(table));
        }

        [Given(@"I have the following commitments")]
        public void GivenIHaveTheForCommitments(Table table)
        {
            _commitmentsContext.CommitmentEntities.AddRange(DataHelper.CreateCommitmentEntities(table));
        }

        [Given(@"I build using DatalockValidationResultBuilder")]
        public void BuildUsingDatalockValidationResultBuilder(Table table)
        {
            var testData = TableParser.Parse<DatalockRule>(table);
            
            _resultsContext.DatalockValidationResultBuilder = new DatalockValidationResultBuilder();

            foreach (var earning in _earningsContext.RawEarnings)
            {
                var rule = testData.FirstOrDefault(x => x.PriceEpisodeIdentifier.Equals(earning.PriceEpisodeIdentifier));

                if (rule == null)
                {
                    throw new Exception($"Please ensure that the price episode identifier: {earning.PriceEpisodeIdentifier} is present in the table");
                }

                var ruleCollection = string.IsNullOrEmpty(rule.RuleId)
                    ? new List<string>()
                    : new List<string> {rule.RuleId};

                _resultsContext.DatalockValidationResultBuilder.Add(earning, 
                    ruleCollection,
                    TransactionTypeGroup.OnProgLearning, 
                    _commitmentsContext.CommitmentEntities.First());
            }
        }

        [When(@"I call the service ValidataDatalockForProvider")]
        public void WhenICallTheServiceValidataDatalockForProvider()
        {
            var sut = new DatalockValidationService(MatcherFactory.CreateMatcher());

            _resultsContext.DatalockValidationResult = sut.ValidateDatalockForProvider(new ProviderCommitments(_commitmentsContext.CommitmentEntities), _earningsContext.RawEarnings,
                new HashSet<long>().ToImmutableHashSet());
        }

        [When(@"I call Build")]
        public void WhenICallBuild()
        {
            _resultsContext.DatalockValidationResult = _resultsContext.DatalockValidationResultBuilder.Build();
        }

        [Then(@"I get (.*) validation errors in the DataLockValidationResult")]
        public void ThenIGetValidationErrorsInTheDataLockValidationResult(int numberOfErorrs)
        {
            Assert.AreEqual(numberOfErorrs, _resultsContext.DatalockValidationResult.ValidationErrors.Count, 
                "Error list: {0}", string.Join(", ", _resultsContext.DatalockValidationResult.ValidationErrors.Select(x => x.RuleId)));
        }

        [Then(@"The DatalockValidatioResult contains (.*)")]
        public void TheValidationErrorsContain(string validationError)
        {
            _resultsContext.DatalockValidationResult.ValidationErrors.Should()
                .Contain(x => x.RuleId == validationError);
        }

        [Then(@"There are (.*) payable datalocks for price episode (.*)")]
        public void ThenThePayableDatalocksContain(int expectedNumber, string priceEpisodeIdentifier)
        {
            _resultsContext.DatalockValidationResult
                .PriceEpisodePeriodMatches
                .Where(x => x.Payable)
                .Where(x => x.PriceEpisodeIdentifier == priceEpisodeIdentifier)
                .Should()
                .HaveCount(expectedNumber);
        }

        [Then(@"There are (.*) non-payable datalocks for price episode (.*)")]
        public void ThenTheNonPayableDatalocksContain(int expectedNumber, string priceEpisodeIdentifier)
        {
            _resultsContext.DatalockValidationResult
                .PriceEpisodePeriodMatches
                .Where(x => !x.Payable)
                .Where(x => x.PriceEpisodeIdentifier == priceEpisodeIdentifier)
                .Should()
                .HaveCount(expectedNumber);
        }
    }
}