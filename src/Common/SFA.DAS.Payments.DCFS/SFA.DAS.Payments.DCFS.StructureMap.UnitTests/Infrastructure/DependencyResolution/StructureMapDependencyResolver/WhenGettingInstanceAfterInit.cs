using System.Collections.Generic;
using CS.Common.External.Interfaces;
using Moq;
using NUnit.Framework;
using SFA.DAS.Payments.DCFS.Context;
using SFA.DAS.Payments.DCFS.StructureMap.UnitTests.Infrastructure.DependencyResolution.StructureMapDependencyResolver.TestObjects;

namespace SFA.DAS.Payments.DCFS.StructureMap.UnitTests.Infrastructure.DependencyResolution.StructureMapDependencyResolver
{
    public class WhenGettingInstanceAfterInit
    {
        private const string ConnectionString = "TheTransientConnectionString";

        private Mock<IExternalContext> _externalContext;
        private ContextWrapper _contextWrapper;
        private TestStructureMapDependencyResolver _resolver;

        [SetUp]
        public void Arrange()
        {
            _externalContext = new Mock<IExternalContext>();
            _externalContext.Setup(c => c.Properties)
                .Returns(new Dictionary<string, string>
                {
                    { ContextPropertyKeys.TransientDatabaseConnectionString, ConnectionString }
                });

            _contextWrapper = new ContextWrapper(_externalContext.Object);

            _resolver = new TestStructureMapDependencyResolver();
        }

        [Test]
        public void ThenItShouldResolveInstanceOfInterface()
        {
            // Arrange
            _resolver.Init(GetType(), _contextWrapper);

            // Act
            var actual = _resolver.GetInstance<ITestObject>();

            // Assert
            Assert.IsNotNull(actual);
            Assert.IsInstanceOf<TestObject>(actual);
        }

        [Test]
        public void ThenItShouldApplyTheDcfsConnectionStringPolicy()
        {
            // Arrange
            _resolver.Init(GetType(), _contextWrapper);

            // Act
            var actual = _resolver.GetInstance<ITestObject>();

            // Assert
            Assert.AreEqual(ConnectionString, actual.TransientConnectionString);
        }
    }
}