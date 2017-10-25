namespace SFA.DAS.Payments.DCFS.StructureMap.UnitTests.Infrastructure.DependencyResolution.StructureMapDependencyResolver.TestObjects
{
    public interface ITestObject
    {
        string TransientConnectionString { get; set; }
    }

    public class TestObject : ITestObject
    {
        public TestObject(string connectionString)
        {
            TransientConnectionString = connectionString;
        }
        public string TransientConnectionString { get; set; }
    }
}
