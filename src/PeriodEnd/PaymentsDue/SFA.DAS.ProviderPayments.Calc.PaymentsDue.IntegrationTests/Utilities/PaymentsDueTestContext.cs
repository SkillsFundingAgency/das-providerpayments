using System.Collections.Generic;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Domain;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data.Entities;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.IntegrationTests.Utilities
{
    /// <summary>
    /// Used for suite-wide data
    /// </summary>
    static class PaymentsDueTestContext
    {
        /// <summary>
        /// Populated by applying the <see cref="SetupUkprnAttribute"/> to the test
        /// </summary>
        public static long Ukprn { get; set; }

        /// <summary>
        /// Populated by applying the <see cref="SetupRequiredPaymentsHistoryAttribute"/> to the test
        /// </summary>
        public static List<RequiredPayment> RequiredPaymentsHistory { get; set; }

        /// <summary>
        /// Populated by applying the <see cref="SetupDataLockPriceEpisodePeriodMatchesAttribute"/> to the test
        /// </summary>
        public static List<DatalockOutputEntity> DataLockPriceEpisodePeriodMatches { get; set; }

        /// <summary>
        /// Populated by applying <see cref="SetupRequiredPayments"/> to the test
        /// </summary>
        public static List<RequiredPayment> RequiredPayments { get; set; }
    }
}
