using System.Collections.Generic;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using SFA.DAS.ProviderPayments.Calc.Refunds.Infrastructure.Entities;
using SFA.DAS.ProviderPayments.Calc.Refunds.IntegrationTests.DataHelpers;


namespace SFA.DAS.ProviderPayments.Calc.Refunds.IntegrationTests.Attributes
{
    public class SetupNoPaymentsHistoryAttribute : TestActionAttribute
    {
        public override ActionTargets Targets { get; } = ActionTargets.Suite;

        public override void BeforeTest(ITest test)
        {
            PaymentsHistoryDataHelper.Truncate();

            RefundsTestContext.PaymentsHistory = new List<HistoricalPaymentEntity>();

            base.BeforeTest(test);
        }
    }
}