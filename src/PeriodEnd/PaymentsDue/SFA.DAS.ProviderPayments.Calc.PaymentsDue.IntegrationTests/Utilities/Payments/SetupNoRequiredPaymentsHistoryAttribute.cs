using System.Collections.Generic;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Domain;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.IntegrationTests.Utilities
{
    public class SetupNoRequiredPaymentsHistoryAttribute : TestActionAttribute
    {
        public override ActionTargets Targets { get; } = ActionTargets.Suite;

        public override void BeforeTest(ITest test)
        {
            RequiredPaymentsHistoryDataHelper.Truncate();

            PaymentsDueTestContext.RequiredPaymentsHistory = new List<RequiredPayment>();

            base.BeforeTest(test);
        }
    }
}