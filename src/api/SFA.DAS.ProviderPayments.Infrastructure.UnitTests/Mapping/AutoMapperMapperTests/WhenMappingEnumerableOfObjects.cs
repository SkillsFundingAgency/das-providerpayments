using System.Linq;
using NUnit.Framework;
using SFA.DAS.ProviderPayments.Infrastructure.Mapping;

namespace SFA.DAS.ProviderPayments.Infrastructure.UnitTests.Mapping.AutoMapperMapperTests
{
    public class WhenMappingEnumerableOfObjects : AutoMapperMapperBaseClass
    {
        private AutoMapperMapper _mapperWrapper;
        private TestDomainObject _source1;
        private TestDomainObject _source2;
        private TestDomainObject _source3;

        [SetUp]
        public void Arrange()
        {
            _mapperWrapper = new AutoMapperMapper(CreateConfiguration());

            _source1 = new TestDomainObject
            {
                Id = 1,
                Description = "Source item 1"
            };
            _source2 = new TestDomainObject
            {
                Id = 2,
                Description = "Source item 2"
            };
            _source3 = new TestDomainObject
            {
                Id = 3,
                Description = "Source item 3"
            };
        }

        [Test]
        public void ThenItShouldMapPropertiesCorrectly()
        {
            // Act
            var actual = _mapperWrapper.Map<TestDomainObject, TestDtoObject>(new[] { _source1, _source2, _source3 }).ToArray();

            // Assert
            AssertMappingIsCorrect(_source1, actual[0]);
            AssertMappingIsCorrect(_source2, actual[1]);
            AssertMappingIsCorrect(_source3, actual[2]);
        }
    }
}
