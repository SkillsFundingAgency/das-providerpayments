using System.Linq;
using AutoFixture;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using SFA.DAS.ProviderPayments.Calc.Refunds.Infrastructure.Entities;
using SFA.DAS.ProviderPayments.Calc.Refunds.IntegrationTests.DataHelpers;

namespace SFA.DAS.ProviderPayments.Calc.Refunds.IntegrationTests.Attributes
{
    public class SetupPaymentsHistoryAttribute : TestActionAttribute
    {
        public override ActionTargets Targets { get; } = ActionTargets.Suite;

        public override void BeforeTest(ITest test)
        {
            PaymentsHistoryDataHelper.Truncate();

            var fixture = new Fixture();

            var historicalPayments = fixture.Build<HistoricalPaymentEntity>()
                .With(earning => earning.Ukprn, 
                    fixture.Create<Generator<long>>()
                        .First(ukprn => ukprn != RefundsTestContext.Ukprn))
                .CreateMany(3)
                .ToList();

            var historicalPaymentsMatchingUkprn = fixture.Build<HistoricalPaymentEntity>()
                .With(earning => earning.Ukprn, RefundsTestContext.Ukprn)
                .CreateMany(3)
                .ToList();

            historicalPayments.AddRange(historicalPaymentsMatchingUkprn);

            foreach (var historicalPayment in historicalPayments)
            {
                PaymentsHistoryDataHelper.CreateHistoricalPayment(historicalPayment);
            }

            RefundsTestContext.PaymentsHistory = historicalPayments;
            
            base.BeforeTest(test);
        }
    }
}