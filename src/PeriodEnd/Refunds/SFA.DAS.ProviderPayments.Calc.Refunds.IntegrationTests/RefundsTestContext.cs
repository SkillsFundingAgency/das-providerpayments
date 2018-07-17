using System.Collections.Generic;
using CS.Common.External.Interfaces;
using SFA.DAS.Payments.DCFS.Context;
using SFA.DAS.ProviderPayments.Calc.Refunds.Infrastructure.Entities;
using SFA.DAS.ProviderPayments.Calc.Refunds.IntegrationTests.Attributes;


namespace SFA.DAS.ProviderPayments.Calc.Refunds.IntegrationTests
{
    /// <summary>
    /// Used for suite-wide data
    /// </summary>
    class RefundsTestContext : IExternalContext
    {
        /// <summary>
        /// Populated by applying the <see cref="SetupUkprnAttribute"/> to the test
        /// </summary>
        public static long Ukprn { get; set; }

        /// <summary>
        /// Populated by applying the <see cref="SetupPaymentsHistoryAttribute"/> to the test
        /// </summary>
        public static List<HistoricalPaymentEntity> PaymentsHistory { get; set; }

        public IDictionary<string, string> Properties { get; set; } = new Dictionary<string, string>
        {
            { ContextPropertyKeys.TransientDatabaseConnectionString, GlobalTestContext.Instance.TransientConnectionString },
            { ContextPropertyKeys.CollectionYear, "1718" }
        };
    }
}
