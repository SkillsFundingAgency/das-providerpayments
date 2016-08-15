using NUnit.Framework;

namespace SFA.DAS.ProviderPayments.Infrastructure.UnitTests.Mapping.AutoMapperMapperTests
{
    public abstract class AutoMapperMapperBaseClass
    {
        protected AutoMapper.MapperConfiguration CreateConfiguration()
        {
            return new AutoMapper.MapperConfiguration(cfg =>
            {
                cfg.CreateMap<TestDomainObject, TestDtoObject>();
            });
        }

        protected void AssertMappingIsCorrect(TestDomainObject source, TestDtoObject destination)
        {
            Assert.AreEqual(source.Id, destination.Id);
            Assert.AreEqual(source.Description, destination.Description);
        }
    }
}
