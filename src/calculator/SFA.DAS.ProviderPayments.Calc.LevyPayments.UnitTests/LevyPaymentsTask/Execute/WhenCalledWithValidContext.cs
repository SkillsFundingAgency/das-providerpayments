using System.Collections.Generic;
using CS.Common.External.Interfaces;
using Moq;
using NLog;
using NUnit.Framework;
using SFA.DAS.ProviderPayments.Calc.Common.Context;
using SFA.DAS.ProviderPayments.Calc.LevyPayments.DependencyResolution;
using SFA.DAS.ProviderPayments.Calc.LevyPayments.UnitTests.Common;

namespace SFA.DAS.ProviderPayments.Calc.LevyPayments.UnitTests.LevyPaymentsTask.Execute
{
    public class WhenCalledWithValidContext
    {
        private IExternalContext _context;
        private IExternalTask _task;

        private Mock<IDependencyResolver> _dependencyResolver;
        private Mock<ILogger> _logger;
        private Mock<LevyPayments.LevyPaymentsProcessor> _processor;

        [SetUp]
        public void Arrange()
        {
            _context = new ExternalContextStub();
            _logger = new Mock<ILogger>();
            _processor = new Mock<LevyPayments.LevyPaymentsProcessor>();

            _dependencyResolver = new Mock<IDependencyResolver>();
            _dependencyResolver.Setup(dr => dr.GetInstance<ILogger>()).Returns(_logger.Object);
            _dependencyResolver.Setup(dr => dr.GetInstance<LevyPayments.LevyPaymentsProcessor>()).Returns(_processor.Object);

            _task = new LevyPayments.LevyPaymentsTask(_dependencyResolver.Object);
        }

        [Test]
        public void ThenProcessorIsExecuted()
        {
            // Act
            var properties = new Dictionary<string, string>
            {
                { ContextPropertyKeys.TransientDatabaseConnectionString, "Ilr.Transient.Connection.String" },
                { ContextPropertyKeys.LogLevel, "Info" },
                { ContextPropertyKeys.YearOfCollection, "1617" }
            };

            _context.Properties = properties;

            _task.Execute(_context);

            // Assert
            _processor.Verify(p => p.Process(), Times.Once);
        }
    }
}
