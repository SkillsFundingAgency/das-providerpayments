using NUnit.Framework;
using NUnit.Framework.Interfaces;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.IntegrationTests.Utilities
{
    public class SetupNoNonPayableEarningsAttribute : TestActionAttribute
    {
        public override ActionTargets Targets { get; } = ActionTargets.Suite;

        public override void BeforeTest(ITest test)
        {
            NonPayableEarningsDataHelper.Truncate();

            base.BeforeTest(test);
        }
    }
}