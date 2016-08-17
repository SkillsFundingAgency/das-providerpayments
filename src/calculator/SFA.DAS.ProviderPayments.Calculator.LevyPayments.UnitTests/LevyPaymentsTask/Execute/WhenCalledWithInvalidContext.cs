using System.Collections.Generic;
using CS.Common.External.Interfaces;
using Moq;
using NLog;
using NUnit.Framework;
using SFA.DAS.ProviderPayments.Calculator.LevyPayments.Context;
using SFA.DAS.ProviderPayments.Calculator.LevyPayments.DependencyResolution;
using SFA.DAS.ProviderPayments.Calculator.LevyPayments.Exceptions;
using SFA.DAS.ProviderPayments.Calculator.LevyPayments.UnitTests.Common;

namespace SFA.DAS.ProviderPayments.Calculator.LevyPayments.UnitTests.LevyPaymentsTask.Execute
{
    public class WhenCalledWithInvalidContext
    {
        private static readonly object[] EmptyProperties =
        {
            new object[] {null},
            new object[] {new Dictionary<string, string>()}
        };

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
        public void ThenExpectingExceptionForNullContextProvided()
        {
            // Assert
            var ex = Assert.Throws<LevyPaymentsInvalidContextException>(() => _task.Execute(null));
            Assert.IsTrue(ex.Message.Contains(LevyPaymentsExceptionMessages.ContextNull));
        }

        [Test]
        [TestCaseSource(nameof(EmptyProperties))]
        public void ThenExpectingExceptionForNoContextPropertiesProvided(IDictionary<string, string> properties)
        {
            _context.Properties = properties;

            // Assert
            var ex = Assert.Throws<LevyPaymentsInvalidContextException>(() => _task.Execute(_context));
            Assert.IsTrue(ex.Message.Contains(LevyPaymentsExceptionMessages.ContextNoProperties));
        }

        [Test]
        public void ThenExpectingExceptionForNoConnectionStringProvided()
        {
            var properties = new Dictionary<string, string>
            {
                { ContextPropertyKeys.LogLevel, "Info" }
            };

            _context.Properties = properties;

            // Assert
            var ex = Assert.Throws<LevyPaymentsInvalidContextException>(() => _task.Execute(_context));
            Assert.IsTrue(ex.Message.Contains(LevyPaymentsExceptionMessages.ContextPropertiesNoConnectionString));
        }

        [Test]
        public void ThenExpectingExceptionForNoLogLevelProvided()
        {
            var properties = new Dictionary<string, string>
            {
                { ContextPropertyKeys.TransientDatabaseConnectionString, "Ilr.Transient.Connection.String" }
            };

            _context.Properties = properties;

            // Assert
            var ex = Assert.Throws<LevyPaymentsInvalidContextException>(() => _task.Execute(_context));
            Assert.IsTrue(ex.Message.Contains(LevyPaymentsExceptionMessages.ContextPropertiesNoLogLevel));
        }
    }
}
