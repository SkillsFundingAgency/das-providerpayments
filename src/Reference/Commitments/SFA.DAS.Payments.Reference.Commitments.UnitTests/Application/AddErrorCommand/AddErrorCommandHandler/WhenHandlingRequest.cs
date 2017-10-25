using System;
using Moq;
using NLog;
using NUnit.Framework;
using SFA.DAS.Payments.Reference.Commitments.Application.AddErrorCommand;
using SFA.DAS.Payments.Reference.Commitments.Infrastructure.Data;

namespace SFA.DAS.Payments.Reference.Commitments.UnitTests.Application.AddErrorCommand.AddErrorCommandHandler
{
    public class WhenHandlingRequest
    {
        private Exception _exception;
        private AddErrorCommandRequest _request;
        private Mock<IProcessErrorRepository> _processErrorRepository;
        private Mock<ILogger> _logger;
        private Commitments.Application.AddErrorCommand.AddErrorCommandHandler _handler;

        [SetUp]
        public void Arrange()
        {
            _exception = new Exception("Testing");
            _request = new AddErrorCommandRequest
            {
                Error = _exception
            };

            _processErrorRepository = new Mock<IProcessErrorRepository>();

            _logger = new Mock<ILogger>();

            _handler = new Commitments.Application.AddErrorCommand.AddErrorCommandHandler(_processErrorRepository.Object, _logger.Object);
        }

        [Test]
        public void ThenItShouldWriteTheExceptionIncludingStackTraceToTheRepository()
        {
            // Act
            _handler.Handle(_request);

            // Assert
            _processErrorRepository.Verify(r => r.WriteError(_exception.ToString()), Times.Once);
        }
    }
}
