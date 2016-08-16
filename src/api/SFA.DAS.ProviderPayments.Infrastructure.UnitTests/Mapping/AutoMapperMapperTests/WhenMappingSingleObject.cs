using NUnit.Framework;
using SFA.DAS.ProviderPayments.Infrastructure.Mapping;

namespace SFA.DAS.ProviderPayments.Infrastructure.UnitTests.Mapping.AutoMapperMapperTests
{
    public class WhenMappingSingleObject : AutoMapperMapperBaseClass
    {
        private AutoMapperMapper _mapperWrapper;
        private TestDomainObject _source;

        [SetUp]
        public void Arrange()
        {
            _mapperWrapper = new AutoMapperMapper(CreateConfiguration());

            _source = new TestDomainObject
            {
                Id = 1,
                Description = "Source item 1"
            };
        }

        [Test]
        public void ThenItShouldMapPropertiesCorrectly()
        {
            // Act
            var actual = _mapperWrapper.Map<TestDomainObject, TestDtoObject>(_source);

            // Assert
            AssertMappingIsCorrect(_source, actual);
        }
    }
}
