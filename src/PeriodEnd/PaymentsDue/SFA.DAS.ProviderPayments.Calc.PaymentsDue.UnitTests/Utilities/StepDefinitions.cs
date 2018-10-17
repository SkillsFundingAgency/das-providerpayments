using System.Collections.Generic;
using Castle.Components.DictionaryAdapter;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Dto;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data.Entities;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.UnitTests.FutureDomain;
using SFA.DAS.ProviderPayments.Calc.Shared.Infrastructure.Data.Entities;
using TechTalk.SpecFlow;
using RequiredPaymentEntity = SFA.DAS.ProviderPayments.Calc.Shared.Infrastructure.Data.Entities.RequiredPaymentEntity;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.UnitTests.Utilities
{
    class ScenarioContext
    {
        public List<DatalockOutputEntity> DatalockOutputEntities { get; } = new List<DatalockOutputEntity>();
        public List<DatalockValidationError> DatalockValidationErrors { get; } = new List<DatalockValidationError>();
        public List<RequiredPaymentEntity> PreviousPayments { get; } = new List<RequiredPaymentEntity>();
        public List<RawEarning> Earnings { get; } = new List<RawEarning>();
        public List<Commitment> Commitments { get; } = new List<Commitment>();
        public CollectionPeriod CollectionPeriod { get; set; }
    }

    [Binding]
    class StepDefinitions
    {
        private readonly ScenarioContext _scenarioData;

        public StepDefinitions(ScenarioContext context)
        {
            _scenarioData = context;
        }

        [Given(@"The collection period is (.*)")]
        public void SetTheCollectionPeriod(string name)
        {
            _scenarioData.CollectionPeriod = new CollectionPeriod(name);
        }

        [Given(@"I have the following datalocks")]
        public void SetDatalockData(Table table)
        {
            _scenarioData.DatalockOutputEntities.AddRange(TableParser.Parse<DatalockOutputEntity>(table));
        }

        [Given(@"I have the following datalock errors")]
        public void SetDatalockErrorsData(Table table)
        {
            _scenarioData.DatalockValidationErrors.AddRange(TableParser.Parse<DatalockValidationError>(table));
        }

        [Given(@"I have the following past payments")]
        public void SetPreviousPaymentsData(Table table)
        {
            _scenarioData.PreviousPayments.AddRange(TableParser.Parse<RequiredPaymentEntity>(table));
        }

        [Given(@"I have the following earnings")]
        public void SetEarningsData(Table table)
        {
            _scenarioData.Earnings.AddRange(TableParser.Parse<RawEarning>(table));
        }

        [Given(@"I have the following commitments")]
        public void SetCommitmentsData(Table table)
        {
            _scenarioData.Commitments.AddRange(TableParser.Parse<Commitment>(table));
        }


        [When(@"I process the provider")]
        public void ProcessProvider()
        {

        }

        [Then(@"The sum of the payments for learner (\d*) should be (\d*)")]
        public void CheckAllPayments(long uln, decimal amount)
        {

        }

        [Then(@"The sum of the transaction type (\d[1,2]) payments for learner (\d*) should be (\d*)")]
        public void CheckPaymentsForATransactionType(int transactionType, long uln, decimal amount)
        {

        }
    }
}
