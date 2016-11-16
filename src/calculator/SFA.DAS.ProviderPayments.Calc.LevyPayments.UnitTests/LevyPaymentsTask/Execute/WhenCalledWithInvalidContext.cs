using System.Collections.Generic;
using CS.Common.External.Interfaces;
using Moq;
using NLog;
using NUnit.Framework;
using SFA.DAS.Payments.DCFS.Context;
using SFA.DAS.Payments.DCFS.Infrastructure.DependencyResolution;
using SFA.DAS.ProviderPayments.Calc.Common.Context;
using SFA.DAS.ProviderPayments.Calc.LevyPayments.UnitTests.Common;

namespace SFA.DAS.ProviderPayments.Calc.LevyPayments.UnitTests.LevyPaymentsTask.Execute
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
            _context = new ExternalContextStub();
            _logger = new Mock<ILogger>();

            _dependencyResolver = new Mock<IDependencyResolver>();
            _dependencyResolver.Setup(dr => dr.GetInstance<ILogger>()).Returns(_logger.Object);

            _task = new LevyPayments.LevyPaymentsTask(_dependencyResolver.Object);
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
                { PaymentsContextPropertyKeys.YearOfCollection, "1617" },
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
                { ContextPropertyKeys.TransientDatabaseConnectionString, "Ilr.Transient.Connection.String" },
                { PaymentsContextPropertyKeys.YearOfCollection, "1617" }
            };

            _context.Properties = properties;

            // Assert
            var ex = Assert.Throws<InvalidContextException>(() => _task.Execute(_context));
            Assert.IsTrue(ex.Message.Contains(InvalidContextException.ContextPropertiesNoLogLevelMessage));
        }

        [Test]
        public void ThenExpectingExceptionForNoYearOfCollectionProvided()
        {
            var properties = new Dictionary<string, string>
            {
                { ContextPropertyKeys.TransientDatabaseConnectionString, "Ilr.Transient.Connection.String" },
                { ContextPropertyKeys.LogLevel, "Info" }
            };

            _context.Properties = properties;

            // Assert
            var ex = Assert.Throws<PaymentsInvalidContextException>(() => _task.Execute(_context));
            Assert.IsTrue(ex.Message.Contains(PaymentsInvalidContextException.ContextPropertiesNoYearOfCollectionMessage));
        }

        [Test]
        [TestCase("abcd")]
        [TestCase("1618")]
        [TestCase("16-17")]
        [TestCase("16 18")]
        [TestCase("16/17")]
        [TestCase("16170")]
        public void ThenExpectingExceptionForInvalidYearOfCollectionProvided(string yearOfCollection)
        {
            var properties = new Dictionary<string, string>
            {
                { ContextPropertyKeys.TransientDatabaseConnectionString, "Ilr.Transient.Connection.String" },
                { ContextPropertyKeys.LogLevel, "Info" },
                { PaymentsContextPropertyKeys.YearOfCollection, yearOfCollection }
            };

            _context.Properties = properties;

            // Assert
            var ex = Assert.Throws<PaymentsInvalidContextException>(() => _task.Execute(_context));
            Assert.IsTrue(ex.Message.Contains(PaymentsInvalidContextException.ContextPropertiesInvalidYearOfCollectionMessage));
        }
    }
}
