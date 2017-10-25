using System.Collections.Generic;
using CS.Common.External.Interfaces;
using MediatR;
using Moq;
using NUnit.Framework;
using SFA.DAS.CollectionEarnings.Calculator.Context;
using SFA.DAS.CollectionEarnings.Calculator.UnitTests.Tools;
using SFA.DAS.Payments.DCFS.Context;
using SFA.DAS.Payments.DCFS.Infrastructure.DependencyResolution;

namespace SFA.DAS.CollectionEarnings.Calculator.UnitTests.ApprenticeshipEarningsTask.Execute
{
    public class WhenCalledWithInvalidContext
    {
        private static readonly object[] EmptyProperties =
        {
            new object[] {null},
            new object[] {new Dictionary<string, string>()}
        };

        private IExternalContext _context = new ExternalContextStub();
        private IExternalTask _task;

        private Mock<IDependencyResolver> _dependencyResolver;
        private Mock<IMediator> _mediator;

        [SetUp]
        public void Arrange()
        {
            _mediator = new Mock<IMediator>();

            _dependencyResolver = new Mock<IDependencyResolver>();
            _dependencyResolver.Setup(dr => dr.GetInstance<IMediator>()).Returns(_mediator.Object);

            _task = new Calculator.ApprenticeshipEarningsTask(_dependencyResolver.Object);
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
                { ContextPropertyKeys.LogLevel, "Info" },
                { EarningsContextPropertyKeys.YearOfCollection, "1617" }
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
                { EarningsContextPropertyKeys.YearOfCollection, "1617" }
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
            var ex = Assert.Throws<InvalidContextException>(() => _task.Execute(_context));
            Assert.IsTrue(ex.Message.Contains(EarningsCalculatorException.ContextPropertiesNoYearOfCollectionMessage));
        }

        [TestCase(" ")]
        [TestCase("acbd")]
        [TestCase("12345")]
        [TestCase("1234")]
        [TestCase("9999")]
        public void ThenExpectingExceptionForInvalidYearOfCollectionProvided(string yearOfCollection)
        {
            var properties = new Dictionary<string, string>
            {
                { ContextPropertyKeys.TransientDatabaseConnectionString, "Ilr.Transient.Connection.String" },
                { ContextPropertyKeys.LogLevel, "Info" },
                { EarningsContextPropertyKeys.YearOfCollection, yearOfCollection }
            };

            _context.Properties = properties;

            // Assert
            var ex = Assert.Throws<InvalidContextException>(() => _task.Execute(_context));
            Assert.IsTrue(ex.Message.Contains(EarningsCalculatorException.ContextPropertiesInvalidYearOfCollectionMessage));
        }
    }
}