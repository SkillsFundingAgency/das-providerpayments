using NLog;
using NUnit.Framework;

namespace SFA.DAS.ProviderPayments.Calculator.LevyPayments.UnitTests.DependencyResolution.TaskDependencyResolver.Init
{
    public class WhenCalled
    {
        private LevyPayments.DependencyResolution.TaskDependencyResolver _dependencyResolver;

        [SetUp]
        public void Arrange()
        {
            _dependencyResolver = new LevyPayments.DependencyResolution.TaskDependencyResolver();
            _dependencyResolver.Init(typeof(WhenCalled));
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
