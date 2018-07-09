using System.Collections.Generic;
using SFA.DAS.ProviderPayments.Calc.Shared.Infrastructure.Data.Entities;
using SFA.DAS.ProviderPayments.Calc.Shared.IntegrationTests.Attributes;

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
    }
}