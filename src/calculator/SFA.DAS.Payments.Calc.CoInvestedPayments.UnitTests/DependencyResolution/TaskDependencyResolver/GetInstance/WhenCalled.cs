using System.Collections.Generic;
using CS.Common.External.Interfaces;
using Moq;
using NLog;
using NUnit.Framework;
using SFA.DAS.ProviderPayments.Calc.Common.Context;
using StructureMap;

namespace SFA.DAS.Payments.Calc.CoInvestedPayments.UnitTests.DependencyResolution.TaskDependencyResolver.GetInstance
{
    public class WhenCalled
    {
        private CoInvestedPayments.DependencyResolution.TaskDependencyResolver _dependencyResolver;

        [SetUp]
        public void Arrange()
        {
            var context = new Mock<IExternalContext>();
            context.Setup(c => c.Properties)
                .Returns(new Dictionary<string, string>
                {
                    {ContextPropertyKeys.TransientDatabaseConnectionString, "TheDb"}
                });

            _dependencyResolver = new CoInvestedPayments.DependencyResolution.TaskDependencyResolver();
            _dependencyResolver.Init(typeof(TestClass), new ContextWrapper(context.Object));
        }

        [Test]
        public void ThenInstanceIsResolved()
        {
            // Act
            var logger = _dependencyResolver.GetInstance<ILogger>();

            // Assert
            Assert.IsNotNull(logger);
        }

        [Test]
        public void ForNotConfiguredConcreteTypeThenInstanceIsResolved()
        {
            // Act
            var instance = _dependencyResolver.GetInstance<TestClass>();

            // Assert
            Assert.IsNotNull(instance);
        }

        [Test]
        public void ForNotConfiguredAbstractTypeThenExceptionIsRaised()
        {
            // Assert
            Assert.That(() => _dependencyResolver.GetInstance<AbstractTestClass>(), Throws.Exception.TypeOf<StructureMapConfigurationException>());
        }

        [Test]
        public void ForNotConfiguredInterfaceTypeThenExceptionIsRaised()
        {
            // Assert
            Assert.That(() => _dependencyResolver.GetInstance<ITestInterface>(), Throws.Exception.TypeOf<StructureMapConfigurationException>());
        }

        internal class TestClass
        {
        }

        internal abstract class AbstractTestClass
        {
        }

        internal interface ITestInterface
        {
        }
    }
}
