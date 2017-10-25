using System;
using Moq;
using NLog;
using NUnit.Framework;
using SFA.DAS.Payments.Reference.Commitments.Application.SetLastSeenEventIdCommand;
using SFA.DAS.Payments.Reference.Commitments.Infrastructure.Data;

namespace SFA.DAS.Payments.Reference.Commitments.UnitTests.Application.SetLastSeenEventIdCommand.SetLastSeenEventIdCommandHandler
{
    public class WhenHandlingRequest
    {
        private Mock<IEventStreamPointerRepository> _eventStreamPointerRepository;
        private Mock<ILogger> _logger;
        private Commitments.Application.SetLastSeenEventIdCommand.SetLastSeenEventIdCommandHandler _handler;
        private SetLastSeenEventIdCommandRequest _request;

        [SetUp]
        public void Arrange()
        {
            _request = new SetLastSeenEventIdCommandRequest
            {
                LastSeenEventId = 1234
            };

            _eventStreamPointerRepository = new Mock<IEventStreamPointerRepository>();

            _logger = new Mock<ILogger>();

            _handler = new Commitments.Application.SetLastSeenEventIdCommand.SetLastSeenEventIdCommandHandler(_eventStreamPointerRepository.Object, _logger.Object);
        }

        [Test]
        public void ThenItShouldSetTheLastSeenEventIdInTheRepository()
        {
            // Act
            _handler.Handle(_request);

            // Assert
            _eventStreamPointerRepository.Verify(r => r.SetLastEventId(1234, It.IsAny<DateTime>()), Times.Once);
        }

        [Test]
        public void ThenItShouldThrowAPersistenceExceptionWhenTheRepositoryThrowsAnException()
        {
            // Arrange
            _eventStreamPointerRepository.Setup(r => r.SetLastEventId(It.IsAny<long>(), It.IsAny<DateTime>()))
                .Throws<Exception>();

            // Act + Assert
            Assert.Throws<PersistenceException>(() => { _handler.Handle(_request); });
        }
    }
}
