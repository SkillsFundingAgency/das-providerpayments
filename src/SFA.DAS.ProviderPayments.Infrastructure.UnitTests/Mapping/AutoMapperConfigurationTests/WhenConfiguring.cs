using NUnit.Framework;
using SFA.DAS.ProdiverPayments.Infrastructure.Mapping;

namespace SFA.DAS.ProviderPayments.Infrastructure.UnitTests.Mapping.AutoMapperConfigurationTests
{
    public class WhenConfiguring
    {
        [Test]
        public void ThenItShouldBeValid()
        {
            // Act
            var actual = AutoMapperConfiguration.Configure();

            // Assert
            actual.AssertConfigurationIsValid();
        }
    }
}
