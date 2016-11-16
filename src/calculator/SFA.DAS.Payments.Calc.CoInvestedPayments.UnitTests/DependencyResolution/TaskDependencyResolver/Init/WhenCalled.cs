using System.Collections.Generic;
using CS.Common.External.Interfaces;
using Moq;
using NLog;
using NUnit.Framework;
using SFA.DAS.Payments.DCFS.Context;

namespace SFA.DAS.Payments.Calc.CoInvestedPayments.UnitTests.DependencyResolution.TaskDependencyResolver.Init
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
            _dependencyResolver.Init(typeof(WhenCalled), new ContextWrapper(context.Object));
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
