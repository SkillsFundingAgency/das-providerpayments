using MediatR;
using Moq;
using NLog;
using NUnit.Framework;
using SFA.DAS.Payments.Calc.CoInvestedPayments;

namespace SFA.DAS.Payments.Calc.CoInvestedPayments.UnitTests.CoInvestedPaymentsProcessor
{
    public class WhenProcessCalled
    {
        private CoInvestedPayments.CoInvestedPaymentsProcessor _processor;
        private Mock<ILogger> _logger;
        private Mock<IMediator> _mediator;

        [SetUp]
        public void Arrange()
        {
            _logger = new Mock<ILogger>();
            _mediator = new Mock<IMediator>();

            _processor = new CoInvestedPayments.CoInvestedPaymentsProcessor(_logger.Object, _mediator.Object);
        }

        [Test]
        public void ShouldMakeGetCurrentCollectionPeriodQueryRequest()
        {
            // Act
            //_processor.Process();

            // Assert
            //_logger.Verify(l => l.Info(It.IsAny<string>()), Times.Exactly(2));

            Assert.Inconclusive();
        }
        [Test]
        public void ThenOutputsLogMessageThatStarting()
        {
            // Act
            //_processor.Process();

            // Assert
            Assert.Inconclusive();
        }
        [Test]
        public void ThenOutputsLogMessageThatEnding()
        {
            // Act
            //_processor.Process();

            // Assert
            Assert.Inconclusive();
        }
    }
}
