using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.CollectionEarnings.DataLock.Application.DataLock.Matcher;
using SFA.DAS.CollectionEarnings.DataLock.Domain;
using SFA.DAS.CollectionEarnings.DataLock.Infrastructure.Data.Entities;
using SFA.DAS.CollectionEarnings.DataLock.Services;
using SFA.DAS.ProviderPayments.Calc.Shared.Infrastructure.Data.Entities;
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

        [Given(@"I have the following on programme earnings")]
        public void GivenIHaveTheFollowingOnProgrammeEarnings(Table table)
        {
            _earningsContext.RawEarnings.AddRange(DataHelper.CreateRawEarnings(table));
        }

        [Given(@"I have the following commitments")]
        public void GivenIHaveTheForCommitments(Table table)
        {
            _commitmentsContext.CommitmentEntities.AddRange(DataHelper.CreateCommitmentEntities(table));
        }

        [When(@"I call the service ValidataDatalockForProvider")]
        public void WhenICallTheServiceValidataDatalockForProvider()
        {
            DatalockValidationService sut = new DatalockValidationService(MatcherFactory.CreateMatcher());

            _resultsContext.DatalockValidationResult = sut.ValidateDatalockForProvider(new ProviderCommitments(_commitmentsContext.CommitmentEntities), _earningsContext.RawEarnings,
                new HashSet<long>().ToImmutableHashSet());
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
        public void ThenTheDatalocksContain(int expectedNumber, string priceEpisodeIdentifier)
        {
            _resultsContext.DatalockValidationResult
                .PriceEpisodePeriodMatches
                .Where(x => x.Payable)
                .Should()
                .HaveCount(expectedNumber);
        }
    }
}