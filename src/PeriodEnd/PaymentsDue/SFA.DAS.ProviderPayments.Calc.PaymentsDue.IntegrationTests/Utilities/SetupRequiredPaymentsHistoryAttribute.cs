using System;
using System.Linq;
using AutoFixture;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Domain;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.IntegrationTests.Utilities
{
    public class SetupRequiredPaymentsHistoryAttribute : TestActionAttribute
    {
        public override ActionTargets Targets { get; } = ActionTargets.Suite;

        public override void BeforeTest(ITest test)
        {
            RequiredPaymentsHistoryDataHelper.Truncate();

            var fixture = new Fixture();

            var historicalPayments = fixture.Build<RequiredPayment>()
                .With(earning => earning.Ukprn,
                    fixture.Create<Generator<long>>()
                        .First(ukprn => ukprn != PaymentsDueTestContext.Ukprn))
                .CreateMany(3)
                .ToList();

            var historicalPaymentsMatchingUkprn = fixture.Build<RequiredPayment>()
                .With(earning => earning.Ukprn, PaymentsDueTestContext.Ukprn)
                .CreateMany(3)
                .ToList();

            historicalPayments.AddRange(historicalPaymentsMatchingUkprn);

            PaymentsDueTestContext.RequiredPaymentsHistory = historicalPayments;

            foreach (var historicalPayment in historicalPayments)
            {
                historicalPayment.SfaContributionPercentage = Math.Round(historicalPayment.SfaContributionPercentage, 4);
                RequiredPaymentsHistoryDataHelper.CreateEntity(historicalPayment);
            }

            base.BeforeTest(test);
        }
    }
}