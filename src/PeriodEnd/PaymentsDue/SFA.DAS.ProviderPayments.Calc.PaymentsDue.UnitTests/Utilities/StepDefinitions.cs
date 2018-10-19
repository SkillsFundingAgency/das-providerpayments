using System.Collections.Generic;
using System.Linq;
using ClosedXML.Excel;
using FluentAssertions;
using Moq;
using NLog;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Domain;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data.Entities;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data.Repositories;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Services;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.UnitTests.FutureDomain;
using SFA.DAS.ProviderPayments.Calc.Shared.Infrastructure.Data.Entities;
using SFA.DAS.ProviderPayments.Calc.Shared.Infrastructure.Data.Repositories;
using TechTalk.SpecFlow;
using RequiredPaymentEntity = SFA.DAS.ProviderPayments.Calc.Shared.Infrastructure.Data.Entities.RequiredPaymentEntity;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.UnitTests.Utilities
{
    class ScenarioContext
    {
        public CollectionPeriod CollectionPeriod { get; set; }
        public CollectionPeriodEntity OpenCollectionPeriod { get; set; }
        public List<CollectionPeriodEntity> CollectionPeriods { get; } = new List<CollectionPeriodEntity>();

        public Dictionary<long, ProviderContext> ProviderContexts { get; } = new Dictionary<long, ProviderContext>();

        public ProviderContext ContextForUkprn(long ukprn)
        {
            if (!ProviderContexts.ContainsKey(ukprn))
            {
                ProviderContexts.Add(ukprn, new ProviderContext());
            }

            return ProviderContexts[ukprn];
        }
    }

    class ProviderContext
    {
        public List<DatalockOutputEntity> DatalockOutputEntities { get; } = new List<DatalockOutputEntity>();
        public List<DatalockValidationError> DatalockValidationErrors { get; } = new List<DatalockValidationError>();
        public List<RequiredPayment> PreviousPayments { get; } = new List<RequiredPayment>();
        public List<RawEarning> Earnings { get; } = new List<RawEarning>();
        public List<RawEarningForMathsOrEnglish> MathsAndEnglishEarnings { get; set; } = new List<RawEarningForMathsOrEnglish>();
        public List<Commitment> Commitments { get; } = new List<Commitment>();
    }

    class ResultsContext
    {
        public List<RequiredPayment> Payments { get; } = new List<RequiredPayment>();
        public List<NonPayableEarning> NonPayableEarnings { get; } = new List<NonPayableEarning>();
    }

    [Binding]
    class StepDefinitions
    {
        private readonly ScenarioContext _scenarioData;
        private readonly ResultsContext _results;

        public StepDefinitions(ScenarioContext context, ResultsContext results)
        {
            _scenarioData = context;
            _results = results;
        }

        [Given(@"The collection period is (.*)")]
        public void SetTheCollectionPeriod(string name)
        {
            _scenarioData.CollectionPeriod = new CollectionPeriod(name);

            for (var i = 1; i <= 14; i++)
            {
                var month = 7 + i;
                var year = int.Parse(_scenarioData.CollectionPeriod.AcademicYear.Substring(0, 2));
                if (month > 12)
                {
                    month -= 12;
                    year++;
                }

                if (i > 12)
                {
                    month++;
                }

                name = $"R{i:D2}";
                year += 2000;

                var collectionPeriod = new CollectionPeriodEntity
                {
                    AcademicYear = _scenarioData.CollectionPeriod.AcademicYear,
                    Id = i,
                    Month = month,
                    Name = name,
                    Open = false,
                    Year = year,
                };
                if (_scenarioData.CollectionPeriod.Name == collectionPeriod.CollectionPeriodName)
                {
                    collectionPeriod.Open = true;
                    _scenarioData.OpenCollectionPeriod = collectionPeriod;
                }
                _scenarioData.CollectionPeriods.Add(collectionPeriod);
            }
        }

        [Given(@"I have the following datalocks for provider (\d{1,8})")]
        public void SetDatalockData(long ukprn, Table table)
        {
            _scenarioData.ContextForUkprn(ukprn).DatalockOutputEntities
                .AddRange(TableParser.Parse<DatalockOutputEntity>(table));
        }

        [Given(@"I have the following datalock errors for provider (\d{1,8})")]
        public void SetDatalockErrorsData(long ukprn, Table table)
        {
            _scenarioData.ContextForUkprn(ukprn).DatalockValidationErrors
                .AddRange(TableParser.Parse<DatalockValidationError>(table));
        }

        [Given(@"I have the following previous payments for provider (\d{1,8})")]
        public void SetPreviousPaymentsData(long ukprn, Table table)
        {
            _scenarioData.ContextForUkprn(ukprn).PreviousPayments
                .AddRange(TableParser.Parse<RequiredPayment>(table));
        }

        [Given(@"I have the following earnings for provider (\d{1,8})")]
        public void SetEarningsData(long ukprn, Table table)
        {
            _scenarioData.ContextForUkprn(ukprn).Earnings
                .AddRange(TableParser.Parse<RawEarning>(table));
        }

        [Given(@"I have the following commitments for provider (\d{1,8})")]
        public void SetCommitmentsData(long ukprn, Table table)
        {
            _scenarioData.ContextForUkprn(ukprn).Commitments
                .AddRange(TableParser.Parse<Commitment>(table));
        }


        [When(@"I process period end")]
        public void ProcessProvider()
        {
            foreach (var ukprn in _scenarioData.ProviderContexts.Keys)
            {
                var earningsRepository = new Mock<IRawEarningsRepository>();
                var mathsAndEnglishEarningsRepository = new Mock<IRawEarningsMathsEnglishRepository>();
                var previousPaymentsRepository = new Mock<IRequiredPaymentsHistoryRepository>();
                var datalockRepository = new Mock<IDatalockRepository>();
                var commitmentRepository = new Mock<ICommitmentRepository>();
                var collectionPeriodRepository = new Mock<ICollectionPeriodRepository>();

                var providerContext = _scenarioData.ContextForUkprn(ukprn);

                earningsRepository.Setup(x => x.GetAllForProvider(ukprn))
                    .Returns(providerContext.Earnings);
                mathsAndEnglishEarningsRepository.Setup(x => x.GetAllForProvider(ukprn))
                    .Returns(providerContext.MathsAndEnglishEarnings);
                previousPaymentsRepository.Setup(x => x.GetAllForProvider(ukprn))
                    .Returns(providerContext.PreviousPayments);
                datalockRepository.Setup(x => x.GetDatalockOutputForProvider(ukprn))
                    .Returns(providerContext.DatalockOutputEntities);
                datalockRepository.Setup(x => x.GetValidationErrorsForProvider(ukprn))
                    .Returns(providerContext.DatalockValidationErrors);
                commitmentRepository.Setup(x => x.GetProviderCommitments(ukprn))
                    .Returns(providerContext.Commitments);
                collectionPeriodRepository.Setup(x => x.GetCurrentCollectionPeriod())
                    .Returns(_scenarioData.OpenCollectionPeriod);
                collectionPeriodRepository.Setup(x => x.GetAllCollectionPeriods())
                    .Returns(_scenarioData.CollectionPeriods);

                var providerDataService = new SortProviderDataIntoLearnerDataService(
                    earningsRepository.Object,
                    mathsAndEnglishEarningsRepository.Object,
                    previousPaymentsRepository.Object,
                    datalockRepository.Object,
                    commitmentRepository.Object,
                    collectionPeriodRepository.Object);

                var providerData = providerDataService.Sort(ukprn);
                var determineWhichEarningsShouldBePaid =
                    new DetermineWhichEarningsShouldBePaidService(collectionPeriodRepository.Object);
                var datalockService = new DatalockValidationService(LogManager.CreateNullLogger());
                var paymentsDueCalc = new PaymentsDueCalculationService();

                var learnerProcessor = new LearnerProcessor(
                    LogManager.CreateNullLogger(), 
                    determineWhichEarningsShouldBePaid, 
                    datalockService, 
                    paymentsDueCalc);

                foreach (var learnerData in providerData)
                {
                    var uln = learnerData.RawEarnings.FirstOrDefault()?.Uln ??
                              learnerData.RawEarningsMathsEnglish.FirstOrDefault()?.Uln ?? 0;
                    var result = learnerProcessor.Process(learnerData, ukprn);
                    result.NonPayableEarnings.ForEach(x => x.Uln = uln);
                    _results.NonPayableEarnings.AddRange(result.NonPayableEarnings);
                    result.PayableEarnings.ForEach(x => x.Uln = uln);
                    _results.Payments.AddRange(result.PayableEarnings);
                }
            }
        }

        [Then(@"The sum of the payments for learner (\d*) should be (\d*\.?\d*)")]
        public void CheckAllPayments(long uln, decimal expectedAmount)
        {
            var previousPayments = _scenarioData.ProviderContexts
                .Sum(x => x.Value.PreviousPayments
                    .Where(y => y.Uln == uln)
                    .Sum(y => y.AmountDue));

            _results.Payments.Where(x => x.Uln == uln).Sum(x => x.AmountDue).Should().Be(expectedAmount - previousPayments);
        }

        [Then(@"The sum of the transaction type (\d{1,2}) payments for learner (\d*) should be (\d*\.?\d*)")]
        public void CheckPaymentsForATransactionType(int transactionType, long uln, decimal expectedAmount)
        {
            var previousPayments = _scenarioData.ProviderContexts
                .Sum(x => x.Value.PreviousPayments
                    .Where(y => y.Uln == uln)
                    .Sum(y => y.AmountDue));

            _results.Payments
                .Where(x => x.Uln == uln)
                .Where(x => x.TransactionType == transactionType)
                .Sum(x => x.AmountDue)
                .Should().Be(expectedAmount - previousPayments);
        }
    }
}
