using System.Collections.Generic;
using SFA.DAS.ProviderPayments.Calc.Refunds.Infrastructure.Entities;
using SFA.DAS.ProviderPayments.Calc.Refunds.IntegrationTests.Attributes;


namespace SFA.DAS.ProviderPayments.Calc.Refunds.IntegrationTests
{
    /// <summary>
    /// Used for suite-wide data
    /// </summary>
    static class RefundsTestContext
    {
        /// <summary>
        /// Populated by applying the <see cref="SetupUkprnAttribute"/> to the test
        /// </summary>
        public static long Ukprn { get; set; }

        /// <summary>
        /// Populated by applying the <see cref="SetupPaymentsHistoryAttribute"/> to the test
        /// </summary>
        public static List<HistoricalPaymentEntity> PaymentsHistory { get; set; }
    }
}
