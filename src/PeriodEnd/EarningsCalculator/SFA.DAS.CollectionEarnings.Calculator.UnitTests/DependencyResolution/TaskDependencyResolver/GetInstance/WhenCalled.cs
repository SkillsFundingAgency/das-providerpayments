using System.Collections.Generic;
using NLog;
using NUnit.Framework;
using SFA.DAS.CollectionEarnings.Calculator.UnitTests.Tools;
using SFA.DAS.Payments.DCFS.Context;
using StructureMap;

namespace SFA.DAS.CollectionEarnings.Calculator.UnitTests.DependencyResolution.TaskDependencyResolver.GetInstance
{
    public class WhenCalled
    {
        private Calculator.DependencyResolution.TaskDependencyResolver _dependencyResolver;

        [SetUp]
        public void Arrange()
        {
            var context = new ExternalContextStub
            {
                Properties = new Dictionary<string, string>
                {
                    {ContextPropertyKeys.TransientDatabaseConnectionString, "Ilr.Transient.Connection.String"},
                    {ContextPropertyKeys.LogLevel, "Info"}
                }
            };

            _dependencyResolver = new Calculator.DependencyResolution.TaskDependencyResolver();
            _dependencyResolver.Init(
                typeof(TestClass),
                new ContextWrapper(context));
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
