﻿using System.Collections.Generic;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.IntegrationTests.Utilities
{
    /// <summary>
    /// Used for suite-wide data
    /// </summary>
    static class PaymentsDueTestContext
    {
        /// <summary>
        /// Ukprn used to search for provider
        /// </summary>
        public static long Ukprn { get; set; }

        /// <summary>
        /// Populated by applying the <see cref="SetupRawEarningsAttribute"/> to the test
        /// </summary>
        public static List<RawEarning> RawEarnings { get; set; }
    }
}