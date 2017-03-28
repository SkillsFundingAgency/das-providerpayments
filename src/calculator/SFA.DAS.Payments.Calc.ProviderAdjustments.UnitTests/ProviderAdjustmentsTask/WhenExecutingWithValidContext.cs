using System.Collections.Generic;
using CS.Common.External.Interfaces;
using Moq;
using NLog;
using NUnit.Framework;
using SFA.DAS.Payments.Calc.ProviderAdjustments.UnitTests.Common;
using SFA.DAS.Payments.DCFS.Context;
using SFA.DAS.Payments.DCFS.Infrastructure.DependencyResolution;
using SFA.DAS.ProviderPayments.Calc.Common.Context;

namespace SFA.DAS.Payments.Calc.ProviderAdjustments.UnitTests.ProviderAdjustmentsTask
{
    public class WhenExecutingWithValidContext
    {
        private IExternalContext _context;
        private IExternalTask _task;

        private Mock<IDependencyResolver> _dependencyResolver;
        private Mock<ILogger> _logger;
        private Mock<ProviderAdjustments.ProviderAdjustmentsProcessor> _processor;

        [SetUp]
        public void Arrange()
        {
            _context = new ExternalContextStub();
            _logger = new Mock<ILogger>();
            _processor = new Mock<ProviderAdjustments.ProviderAdjustmentsProcessor>();

            _dependencyResolver = new Mock<IDependencyResolver>();
            _dependencyResolver.Setup(dr => dr.GetInstance<ILogger>()).Returns(_logger.Object);
            _dependencyResolver.Setup(dr => dr.GetInstance<ProviderAdjustments.ProviderAdjustmentsProcessor>()).Returns(_processor.Object);

            _task = new ProviderAdjustments.ProviderAdjustmentsTask(_dependencyResolver.Object);
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