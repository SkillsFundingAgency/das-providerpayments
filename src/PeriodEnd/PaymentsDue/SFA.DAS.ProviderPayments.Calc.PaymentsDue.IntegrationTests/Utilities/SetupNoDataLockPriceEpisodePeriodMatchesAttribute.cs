using System.Collections.Generic;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data.Entities;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.IntegrationTests.Utilities
{
    public class SetupNoDataLockPriceEpisodePeriodMatchesAttribute : TestActionAttribute
    {
        public override ActionTargets Targets { get; } = ActionTargets.Suite;

        public override void BeforeTest(ITest test)
        {
            DataLockPriceEpisodePeriodMatchDataHelper.Truncate();

            PaymentsDueTestContext.DataLockPriceEpisodePeriodMatches = new List<DatalockOutputEntity>();

            base.BeforeTest(test);
        }
    }
}