using System.Collections.Generic;
using SFA.DAS.ProviderPayments.Calc.Shared.Infrastructure.Data.Entities;
using SFA.DAS.ProviderPayments.Calc.Shared.IntegrationTests.Attributes;
using SFA.DAS.ProviderPayments.Calc.Shared.IntegrationTests.Attributes.Datalocks;
using SFA.DAS.ProviderPayments.Calc.Shared.IntegrationTests.Attributes.RawEarnings;
using SFA.DAS.ProviderPayments.Calc.Shared.IntegrationTests.Helpers;

namespace SFA.DAS.ProviderPayments.Calc.Shared.IntegrationTests
{
    public static class SharedTestContext
    {
        /// <summary>
        /// Populated by applying the <see cref="SetupUkprnAttribute"/> to the test
        /// </summary>
        public static long Ukprn { get; set; }

        /// <summary>
        /// Populated by applying the <see cref="SetupAccountIdAttribute"/> to the test
        /// </summary>
        public static long AccountId { get; set; }

        /// <summary>
        /// Populated by applying the <see cref="SetupDasAccountsAttribute"/> to the test
        /// </summary>
        public static List<DasAccountEntity> DasAccounts { get; set; }

        /// <summary>
        /// Populated by applying the <see cref="SetupRawEarningsAttribute"/> to the test
        /// </summary>
        public static List<RawEarning> RawEarnings { get; set; }

        /// <summary>
        /// Populated by applying the <see cref="SetupRawEarningsMathsEnglishAttribute"/> to the test
        /// </summary>
        public static List<RawEarningForMathsOrEnglish> RawEarningsMathsEnglish { get; set; }

        /// <summary>
        /// Populated by applying the <see cref="SetupDatalocksAttribute"/> to the test
        /// </summary>
        public static List<DatalockOutputEntity> DataLockPriceEpisodePeriodMatches { get; set; }

        /// <summary>
        /// Populated by applying the <see cref="SetupDatalockValidationErrorsAttribute"/> to the test
        /// </summary>
        public static List<DatalockValidationError> DatalockValidationErrors { get; set; }
    }
}