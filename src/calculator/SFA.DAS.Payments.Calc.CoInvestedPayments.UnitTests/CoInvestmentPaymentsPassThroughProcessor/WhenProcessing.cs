using Moq;
using NLog;
using NUnit.Framework;

namespace SFA.DAS.Payments.Calc.CoInvestedPayments.UnitTests.CoInvestmentPaymentsPassThroughProcessor
{
    public class WhenProcessing
    {
        private CoInvestedPayments.CoInvestmentPaymentsPassThroughProcessor _processor;
        private Mock<ILogger> _logger;

        [SetUp]
        public void Arrange()
        {
            _logger = new Mock<ILogger>();

            _processor = new CoInvestedPayments.CoInvestmentPaymentsPassThroughProcessor(_logger.Object);
        }

        [Test]
        public void ThenOnlyLoggingIsPerformed()
        {
            // Act
            _processor.Process();
            
            // Assert
            _logger.Verify(l => l.Info(It.IsAny<string>()), Times.Exactly(2));
        }
    }
}