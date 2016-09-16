using System.Collections.Generic;
using CS.Common.External.Interfaces;
using Moq;
using NLog;
using NUnit.Framework;
using SFA.DAS.Payments.Calc.CoInvestedPayments.DependencyResolution;
using SFA.DAS.Payments.Calc.CoInvestedPayments.UnitTests.Tools;
using SFA.DAS.ProviderPayments.Calc.Common.Context;

namespace SFA.DAS.Payments.Calc.CoInvestedPayments.UnitTests.CoInvestmentPaymentsTask
{
    public class WhenExecutingWithInvalidContext
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
            _context = new ExternalContextStub();
            _logger = new Mock<ILogger>();

            _dependencyResolver = new Mock<IDependencyResolver>();
            _dependencyResolver.Setup(dr => dr.GetInstance<ILogger>()).Returns(_logger.Object);

            _task = new CoInvestedPayments.CoInvestmentPaymentsTask(_dependencyResolver.Object);
        }

        [Test]
        public void ThenExpectingExceptionForNullContextProvided()
        {
            // Assert
            var ex = Assert.Throws<InvalidContextException>(() => _task.Execute(null));
            Assert.IsTrue(ex.Message.Contains(InvalidContextException.ContextNullMessage));
        }

        [Test]
        [TestCaseSource(nameof(EmptyProperties))]
        public void ThenExpectingExceptionForNoContextPropertiesProvided(IDictionary<string, string> properties)
        {
            _context.Properties = properties;

            // Assert
            var ex = Assert.Throws<InvalidContextException>(() => _task.Execute(_context));
            Assert.IsTrue(ex.Message.Contains(InvalidContextException.ContextNoPropertiesMessage));
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
            var ex = Assert.Throws<InvalidContextException>(() => _task.Execute(_context));
            Assert.IsTrue(ex.Message.Contains(InvalidContextException.ContextPropertiesNoConnectionStringMessage));
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
            var ex = Assert.Throws<InvalidContextException>(() => _task.Execute(_context));
            Assert.IsTrue(ex.Message.Contains(InvalidContextException.ContextPropertiesNoLogLevelMessage));
        }
    }
}