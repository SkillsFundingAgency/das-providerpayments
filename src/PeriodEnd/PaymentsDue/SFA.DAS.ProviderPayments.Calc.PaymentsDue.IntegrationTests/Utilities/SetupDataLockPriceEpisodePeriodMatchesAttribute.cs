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

            var dataLocks = fixture.Build<DataLockPriceEpisodePeriodMatchEntity>()
                .With(earning => earning.Ukprn, 
                    fixture.Create<Generator<long>>()
                        .First(ukprn => ukprn != PaymentsDueTestContext.Ukprn))
                .CreateMany(3)
                .ToList();

            var dataLocksMatchingUkprn = fixture.Build<DataLockPriceEpisodePeriodMatchEntity>()
                .With(earning => earning.Ukprn, PaymentsDueTestContext.Ukprn)
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