using System.Collections.Generic;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using SFA.DAS.ProviderPayments.Calc.Shared.Infrastructure.Data.Entities;
using SFA.DAS.ProviderPayments.Calc.Shared.IntegrationTests.Helpers;

namespace SFA.DAS.ProviderPayments.Calc.Shared.IntegrationTests.Attributes.RawEarnings
{
    public class SetupNoRawEarningsAttribute : TestActionAttribute
    {
        public override ActionTargets Targets { get; } = ActionTargets.Suite;

        public override void BeforeTest(ITest test)
        {
            RawEarningsDataHelper.Truncate();

            SharedTestContext.RawEarnings = new List<RawEarning>();

            base.BeforeTest(test);
        }
    }
}