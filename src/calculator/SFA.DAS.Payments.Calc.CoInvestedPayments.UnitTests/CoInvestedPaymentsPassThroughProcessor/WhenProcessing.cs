using Moq;
using NLog;
using NUnit.Framework;

namespace SFA.DAS.Payments.Calc.CoInvestedPayments.UnitTests.CoInvestedPaymentsPassThroughProcessor
{
    public class WhenProcessing
    {
        private CoInvestedPayments.CoInvestedPaymentsPassThroughProcessor _processor;
        private Mock<ILogger> _logger;

        [SetUp]
        public void Arrange()
        {
            _logger = new Mock<ILogger>();

            _processor = new CoInvestedPayments.CoInvestedPaymentsPassThroughProcessor(_logger.Object);
        }

        [Test]
        public void ThenOutputsTwoLogMessages()
        {
            // Act
            _processor.Process();
            
            // Assert
            _logger.Verify(l => l.Info(It.IsAny<string>()), Times.Exactly(2));
        }
        [Test]
        public void ThenTwoLogMessagesExplicitlyMentionPassThrough()
        {
            // Act
            _processor.Process();

            // Assert
            _logger.Verify(l => l.Info(It.IsRegex("Pass-Through")), Times.Exactly(2));
        }
    }
}