using System.Linq;
using AutoFixture;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using SFA.DAS.ProviderPayments.Calc.Shared.Infrastructure.Data.Entities;
using SFA.DAS.ProviderPayments.Calc.Shared.IntegrationTests.Helpers;

namespace SFA.DAS.ProviderPayments.Calc.Shared.IntegrationTests.Attributes.Datalocks
{
    public class SetupDatalocksAttribute : TestActionAttribute
    {
        public override ActionTargets Targets { get; } = ActionTargets.Suite;

        public override void BeforeTest(ITest test)
        {
            DatalockDataHelper.Truncate();

            var fixture = new Fixture();
            var priceEpisodeIdentifier = $"{fixture.Create<string>().Substring(0, 5)}-01/03/2018";

            var dataLocks = fixture.Build<DatalockOutputEntity>()
                .With(earning => earning.Ukprn, 
                    fixture.Create<Generator<long>>()
                        .First(ukprn => ukprn != SharedTestContext.Ukprn))
                .With(x => x.PriceEpisodeIdentifier, priceEpisodeIdentifier)
                .CreateMany(3)
                .ToList();

            var dataLocksMatchingUkprn = fixture.Build<DatalockOutputEntity>()
                .With(earning => earning.Ukprn, SharedTestContext.Ukprn)
                .With(x => x.PriceEpisodeIdentifier, priceEpisodeIdentifier)
                .CreateMany(3)
                .ToList();

            dataLocks.AddRange(dataLocksMatchingUkprn);

            foreach (var dataLock in dataLocks)
            {
                DatalockDataHelper.CreateEntity(dataLock);
            }

            SharedTestContext.DataLockPriceEpisodePeriodMatches = dataLocks;
            
            base.BeforeTest(test);
        }
    }
}