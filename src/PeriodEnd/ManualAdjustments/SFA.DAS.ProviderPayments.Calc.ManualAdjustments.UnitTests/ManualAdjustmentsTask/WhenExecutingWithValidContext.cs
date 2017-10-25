using System.Collections.Generic;
using CS.Common.External.Interfaces;
using Moq;
using NLog;
using NUnit.Framework;
using SFA.DAS.Payments.DCFS.Context;
using SFA.DAS.Payments.DCFS.Infrastructure.DependencyResolution;
using SFA.DAS.ProviderPayments.Calc.Common.Context;
using SFA.DAS.ProviderPayments.Calc.ManualAdjustments.UnitTests.Common;

namespace SFA.DAS.ProviderPayments.Calc.ManualAdjustments.UnitTests.ManualAdjustmentsTask
{
    public class WhenExecutingWithValidContext
    {
        private IExternalContext _context;
        private IExternalTask _task;

        private Mock<IDependencyResolver> _dependencyResolver;
        private Mock<ILogger> _logger;
        private Mock<ManualAdjustments.ManualAdjustmentsProcessor> _processor;

        [SetUp]
        public void Arrange()
        {
            _context = new ExternalContextStub();
            _logger = new Mock<ILogger>();
            _processor = new Mock<ManualAdjustments.ManualAdjustmentsProcessor>();

            _dependencyResolver = new Mock<IDependencyResolver>();
            _dependencyResolver.Setup(dr => dr.GetInstance<ILogger>()).Returns(_logger.Object);
            _dependencyResolver.Setup(dr => dr.GetInstance<ManualAdjustments.ManualAdjustmentsProcessor>()).Returns(_processor.Object);

            _task = new ManualAdjustments.ManualAdjustmentsTask(_dependencyResolver.Object);
        }

        [Test]
        public void ThenProcessorIsExecuted()
        {
            // Act
            var properties = new Dictionary<string, string>
            {
                { ContextPropertyKeys.TransientDatabaseConnectionString, "Ilr.Transient.Connection.String" },
                { ContextPropertyKeys.LogLevel, "Info" },
                { PaymentsContextPropertyKeys.YearOfCollection, "1617" }
            };

            _context.Properties = properties;

            _task.Execute(_context);

            // Assert
            _processor.Verify(p => p.Process(), Times.Once);
        }
    }
}