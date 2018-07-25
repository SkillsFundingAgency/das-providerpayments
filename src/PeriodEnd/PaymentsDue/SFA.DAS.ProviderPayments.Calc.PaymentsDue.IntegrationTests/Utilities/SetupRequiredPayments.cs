using System;
using System.Linq;
using AutoFixture;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Domain;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.IntegrationTests.Utilities
{
    public class SetupRequiredPayments : TestActionAttribute
    {
        public override ActionTargets Targets { get; } = ActionTargets.Suite;

        public override void BeforeTest(ITest test)
        {
            RequiredPaymentsDataHelper.Truncate();

            var fixture = new Fixture();
            var priceEpisodeIdentifier = $"{fixture.Create<string>().Substring(0, 5)}-01/03/2018";

            var requiredPayments = fixture.Build<RequiredPayment>()
                .With(earning => earning.Ukprn, 
                    fixture.Create<Generator<long>>()
                        .First(ukprn => ukprn != PaymentsDueTestContext.Ukprn))
                .With(x => x.PriceEpisodeIdentifier, priceEpisodeIdentifier)
                .CreateMany(3)
                .ToList();

            var earningsMatchingUkprn = fixture.Build<RequiredPayment>()
                .With(earning => earning.Ukprn, PaymentsDueTestContext.Ukprn)
                .With(x => x.PriceEpisodeIdentifier, priceEpisodeIdentifier)
                .CreateMany(3)
                .ToList();

            requiredPayments.AddRange(earningsMatchingUkprn);

            foreach (var payment in requiredPayments)
            {
                payment.SfaContributionPercentage = Math.Round(payment.SfaContributionPercentage, 4);
                RequiredPaymentsDataHelper.AddRequiredPayment(payment);
            }

            PaymentsDueTestContext.RequiredPayments = requiredPayments;
            
            base.BeforeTest(test);
        }
    }
}