using System.Collections.Generic;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using SFA.DAS.ProviderPayments.Calc.Shared.Infrastructure.Data.Entities;
using SFA.DAS.ProviderPayments.Calc.Shared.IntegrationTests.Helpers;

namespace SFA.DAS.ProviderPayments.Calc.Shared.IntegrationTests.Attributes.RawEarnings
{
    public class SetupNoRawEarningsMathsEnglishAttribute : TestActionAttribute
    {
        public override ActionTargets Targets { get; } = ActionTargets.Suite;

        public override void BeforeTest(ITest test)
        {
            RawEarningsMathsEnglishDataHelper.Truncate();

            SharedTestContext.RawEarningsMathsEnglish = new List<RawEarningForMathsOrEnglish>();

            base.BeforeTest(test);
        }
    }
}