using System;
using System.Linq;
using AutoFixture;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data.Entities;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.IntegrationTests.Utilities
{
    public class SetupRawEarningsMathsEnglishAttribute : TestActionAttribute
    {
        public override ActionTargets Targets { get; } = ActionTargets.Suite;

        public override void BeforeTest(ITest test)
        {
            RawEarningsDataHelper.Truncate();

            var fixture = new Fixture();

            var earnings = fixture.Build<RawEarningForMathsOrEnglish>()
                .With(earning => earning.Ukprn, 
                    fixture.Create<Generator<long>>()
                        .First(ukprn => ukprn != PaymentsDueTestContext.Ukprn))
                .CreateMany(3)
                .ToList();

            var earningsMatchingUkprn = fixture.Build<RawEarningForMathsOrEnglish>()
                .With(earning => earning.Ukprn, PaymentsDueTestContext.Ukprn)
                .CreateMany(3)
                .ToList();

            earnings.AddRange(earningsMatchingUkprn);

            foreach (var rawEarning in earnings)
            {
                rawEarning.SfaContributionPercentage = Math.Round(rawEarning.SfaContributionPercentage, 4);
                RawEarningsMathsEnglishDataHelper.CreateRawEarningMathsEnglish(rawEarning);
            }

            PaymentsDueTestContext.RawEarningsMathsEnglish = earnings;
            
            base.BeforeTest(test);
        }
    }
}