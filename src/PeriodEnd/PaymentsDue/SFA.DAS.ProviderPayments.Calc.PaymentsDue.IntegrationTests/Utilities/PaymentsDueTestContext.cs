using System.Collections.Generic;
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
        /// Populated by applying the <see cref="SetupRawEarningsAttribute"/> to the test
        /// </summary>
        public static List<RawEarning> RawEarnings { get; set; }

        /// <summary>
        /// Populated by applying the <see cref="SetupRawEarningsMathsEnglishAttribute"/> to the test
        /// </summary>
        public static List<RawEarningForMathsOrEnglish> RawEarningsMathsEnglish { get; set; }

        /// <summary>
        /// Populated by applying the <see cref="SetupRequiredPaymentsHistoryAttribute"/> to the test
        /// </summary>
        public static List<RequiredPaymentEntity> RequiredPaymentsHistory { get; set; }

        /// <summary>
        /// Populated by applying the <see cref="SetupDataLockPriceEpisodePeriodMatchesAttribute"/> to the test
        /// </summary>
        public static List<DatalockOutput> DataLockPriceEpisodePeriodMatches { get; set; }
    }
}
