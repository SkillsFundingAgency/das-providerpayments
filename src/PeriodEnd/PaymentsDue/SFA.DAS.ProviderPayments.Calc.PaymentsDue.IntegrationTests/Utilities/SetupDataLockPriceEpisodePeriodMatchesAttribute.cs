using System.Linq;
using AutoFixture;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data.Entities;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.IntegrationTests.Utilities
{
    public class SetupDataLockPriceEpisodePeriodMatchesAttribute : TestActionAttribute
    {
        public override ActionTargets Targets { get; } = ActionTargets.Suite;

        public override void BeforeTest(ITest test)
        {
            DataLockPriceEpisodePeriodMatchDataHelper.Truncate();

            var fixture = new Fixture();
            var priceEpisodeIdentifier = $"{fixture.Create<string>().Substring(0, 5)}-01/03/2018";

            var dataLocks = fixture.Build<DatalockOutput>()
                .With(earning => earning.Ukprn, 
                    fixture.Create<Generator<long>>()
                        .First(ukprn => ukprn != PaymentsDueTestContext.Ukprn))
                .With(x => x.PriceEpisodeIdentifier, priceEpisodeIdentifier)
                .CreateMany(3)
                .ToList();

            var dataLocksMatchingUkprn = fixture.Build<DatalockOutput>()
                .With(earning => earning.Ukprn, PaymentsDueTestContext.Ukprn)
                .With(x => x.PriceEpisodeIdentifier, priceEpisodeIdentifier)
                .CreateMany(3)
                .ToList();

            dataLocks.AddRange(dataLocksMatchingUkprn);

            foreach (var dataLock in dataLocks)
            {
                DataLockPriceEpisodePeriodMatchDataHelper.CreateEntity(dataLock);
            }

            PaymentsDueTestContext.DataLockPriceEpisodePeriodMatches = dataLocks;
            
            base.BeforeTest(test);
        }
    }
}