using SFA.DAS.ProviderPayments.Calc.Shared.IntegrationTests.Attributes;

namespace SFA.DAS.ProviderPayments.Calc.Shared.IntegrationTests
{
    public class SharedTestContext
    {
        /// <summary>
        /// Populated by applying the <see cref="SetupUkprnAttribute"/> to the test
        /// </summary>
        public static long Ukprn { get; set; }
    }
}