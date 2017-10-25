using StructureMap;

namespace SFA.DAS.Payments.DCFS.StructureMap.UnitTests.Infrastructure.DependencyResolution.StructureMapDependencyResolver.TestObjects
{
    public class TestRegistry : Registry
    {
        public TestRegistry()
        {
            For<ITestObject>().Use<TestObject>();
        }
    }
}
