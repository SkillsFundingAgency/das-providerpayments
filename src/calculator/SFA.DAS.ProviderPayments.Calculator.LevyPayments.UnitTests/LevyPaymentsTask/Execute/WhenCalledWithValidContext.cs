using System.Collections.Generic;
using CS.Common.External.Interfaces;
using Moq;
using NLog;
using NUnit.Framework;
using SFA.DAS.ProviderPayments.Calculator.LevyPayments.Context;
using SFA.DAS.ProviderPayments.Calculator.LevyPayments.DependencyResolution;
using SFA.DAS.ProviderPayments.Calculator.LevyPayments.UnitTests.Common;

namespace SFA.DAS.ProviderPayments.Calculator.LevyPayments.UnitTests.LevyPaymentsTask.Execute
{
    public class WhenCalledWithValidContext
    {
        private IExternalContext _context;
        private IExternalTask _task;

        private Mock<IDependencyResolver> _dependencyResolver;
        private Mock<ILogger> _logger;

        [SetUp]
        public void Arrange()
        {
            _context = new ExternalContext();
            _logger = new Mock<ILogger>();

            _dependencyResolver = new Mock<IDependencyResolver>();
            _dependencyResolver.Setup(dr => dr.GetInstance<ILogger>()).Returns(_logger.Object);

            _task = new LevyPayments.LevyPaymentsTask(_dependencyResolver.Object);
        }

        [Test]
        public void ThenLoggingIsDone()
        {
            // Act
            var properties = new Dictionary<string, string>
            {
                { ContextPropertyKeys.TransientDatabaseConnectionString, "Ilr.Transient.Connection.String" },
                { ContextPropertyKeys.LogLevel, "Info" }
            };

            _context.Properties = properties;

            _task.Execute(_context);

            // Assert
            _logger.Verify(l => l.Info(It.IsAny<string>()), Times.Exactly(2));
        }
    }
}
