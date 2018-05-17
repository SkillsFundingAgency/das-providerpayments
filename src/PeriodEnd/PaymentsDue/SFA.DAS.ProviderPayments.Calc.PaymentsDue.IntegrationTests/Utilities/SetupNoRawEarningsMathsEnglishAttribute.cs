using System.Collections.Generic;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data.Entities;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.IntegrationTests.Utilities
{
    public class SetupNoRawEarningsMathsEnglishAttribute : TestActionAttribute
    {
        public override ActionTargets Targets { get; } = ActionTargets.Suite;

        public override void BeforeTest(ITest test)
        {
            RawEarningsMathsEnglishDataHelper.Truncate();

            PaymentsDueTestContext.RawEarningsMathsEnglish = new List<RawEarningMathsEnglishEntity>();

            base.BeforeTest(test);
        }
    }
}