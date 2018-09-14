using System;
using System.Linq;
using AutoFixture;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using SFA.DAS.ProviderPayments.Calc.Shared.Infrastructure.Data.Entities;
using SFA.DAS.ProviderPayments.Calc.Shared.IntegrationTests.Helpers;

namespace SFA.DAS.ProviderPayments.Calc.Shared.IntegrationTests.Attributes.RawEarnings
{
    public class SetupRawEarningsAttribute : TestActionAttribute
    {
        public override ActionTargets Targets { get; } = ActionTargets.Suite;

        public override void BeforeTest(ITest test)
        {
            RawEarningsDataHelper.Truncate();

            var fixture = new Fixture();
            var priceEpisodeIdentifier = $"{fixture.Create<string>().Substring(0, 5)}-01/03/2018";

            var earnings = fixture.Build<RawEarning>()
                .With(earning => earning.Ukprn, 
                    fixture.Create<Generator<long>>()
                        .First(ukprn => ukprn != SharedTestContext.Ukprn))
                .With(x => x.PriceEpisodeIdentifier, priceEpisodeIdentifier)
                .CreateMany(3)
                .ToList();

            var earningsMatchingUkprn = fixture.Build<RawEarning>()
                .With(earning => earning.Ukprn, SharedTestContext.Ukprn)
                .With(x => x.PriceEpisodeIdentifier, priceEpisodeIdentifier)
                .CreateMany(3)
                .ToList();

            earnings.AddRange(earningsMatchingUkprn);

            foreach (var rawEarning in earnings)
            {
                rawEarning.SfaContributionPercentage = Math.Round(rawEarning.SfaContributionPercentage, 4);
                RawEarningsDataHelper.CreateRawEarning(rawEarning);
            }

            SharedTestContext.RawEarnings = earnings;
            
            base.BeforeTest(test);
        }
    }
}