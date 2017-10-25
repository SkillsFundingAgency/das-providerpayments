using Moq;
using NLog;
using NUnit.Framework;
using SFA.DAS.Payments.Reference.Commitments.Application.GetLastSeenEventIdQuery;
using SFA.DAS.Payments.Reference.Commitments.Infrastructure.Data;

namespace SFA.DAS.Payments.Reference.Commitments.UnitTests.Application.GetLastSeenEventIdQuery.GetLastSeenEventIdQueryHandler
{
    public class WhenHandlingRequest
    {
        private GetLastSeenEventIdQueryRequest _request;
        private Mock<IEventStreamPointerRepository> _eventStreamPointerRepository;
        private Mock<ILogger> _logger;
        private Commitments.Application.GetLastSeenEventIdQuery.GetLastSeenEventIdQueryHandler _handler;

        [SetUp]
        public void Arrange()
        {
            _request = new GetLastSeenEventIdQueryRequest();

            _eventStreamPointerRepository = new Mock<IEventStreamPointerRepository>();
            _eventStreamPointerRepository.Setup(r => r.GetLastEventId())
                .Returns(12345);

            _logger = new Mock<ILogger>();

            _handler = new Commitments.Application.GetLastSeenEventIdQuery.GetLastSeenEventIdQueryHandler(_eventStreamPointerRepository.Object, _logger.Object);
        }

        [Test]
        public void ThenItShouldReturnTheEventIdFromTheRepository()
        {
            // Act
            var actual = _handler.Handle(_request);

            // Assert
            Assert.IsNotNull(actual);
            Assert.AreEqual(12345, actual.EventId);
        }
    }
}
