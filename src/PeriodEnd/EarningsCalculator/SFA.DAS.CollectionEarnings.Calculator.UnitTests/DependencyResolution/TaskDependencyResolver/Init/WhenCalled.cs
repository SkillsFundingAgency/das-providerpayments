using System.Collections.Generic;
using NLog;
using NUnit.Framework;
using SFA.DAS.CollectionEarnings.Calculator.UnitTests.Tools;
using SFA.DAS.Payments.DCFS.Context;

namespace SFA.DAS.CollectionEarnings.Calculator.UnitTests.DependencyResolution.TaskDependencyResolver.Init
{
    public class WhenCalled
    {
        private Calculator.DependencyResolution.TaskDependencyResolver _dependencyResolver;

        [SetUp]
        public void Arrange()
        {
            var context = new ExternalContextStub
            {
                Properties = new Dictionary<string, string>()
                {
                    {ContextPropertyKeys.TransientDatabaseConnectionString, "Ilr.Transient.Connection.String"},
                    {ContextPropertyKeys.LogLevel, "Info"}
                }
            };

            _dependencyResolver = new Calculator.DependencyResolution.TaskDependencyResolver();
            _dependencyResolver.Init(
                typeof(WhenCalled),
                new ContextWrapper(context));
        }

        [Test]
        public void ThenTheLoggerCanBeResolved()
        {
            // Act
            var logger = _dependencyResolver.GetInstance<ILogger>();

            // Assert
            Assert.IsNotNull(logger);
        }

        [Test]
        public void ThenTheLoggerHasBeenNamedWithTheFullNameOfTheType()
        {
            // Act
            var logger = _dependencyResolver.GetInstance<ILogger>();

            // Assert
            Assert.AreEqual(typeof(WhenCalled).FullName, logger.Name);
        }
    }
}
