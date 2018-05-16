using System.Linq;
using AutoFixture;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data.Entities;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.IntegrationTests.Utilities
{
    public class SetupRawEarningsAttribute : TestActionAttribute
    {
        public override ActionTargets Targets { get; } = ActionTargets.Suite;

        public override void BeforeTest(ITest test)
        {
            RawEarningsDataHelper.Truncate();

            var fixture = new Fixture();

            var earnings = fixture.Build<RawEarningEntity>()
                .With(earning => earning.Ukprn, 
                    fixture.Create<Generator<long>>()
                        .First(ukprn => ukprn != PaymentsDueTestContext.Ukprn))
                .CreateMany(3)
                .ToList();

            var earningsMatchingUkprn = fixture.Build<RawEarningEntity>()
                .With(earning => earning.Ukprn, PaymentsDueTestContext.Ukprn)
                .CreateMany(3)
                .ToList();

            earnings.AddRange(earningsMatchingUkprn);

            foreach (var rawEarning in earnings)
            {
                RawEarningsDataHelper.CreateRawEarning(rawEarning);
            }

            PaymentsDueTestContext.RawEarnings = earnings;
            
            base.BeforeTest(test);
        }
    }
}