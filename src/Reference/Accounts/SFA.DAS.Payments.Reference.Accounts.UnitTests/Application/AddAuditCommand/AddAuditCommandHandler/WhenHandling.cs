using System;
using Moq;
using NUnit.Framework;
using SFA.DAS.Payments.Reference.Accounts.Infrastructure.Data;

namespace SFA.DAS.Payments.Reference.Accounts.UnitTests.Application.AddAuditCommand.AddAuditCommandHandler
{
    class WhenHandling
    {
        private Mock<IAuditRepository> _auditRepository;
        private Accounts.Application.AddAuditCommand.AddAuditCommandHandler _handler;

        [SetUp]
        public void Arrange()
        {
            _auditRepository = new Mock<IAuditRepository>();

            _handler = new Accounts.Application.AddAuditCommand.AddAuditCommandHandler(_auditRepository.Object);
        }

        [Test]
        public void ThenItShouldCreateAuditInRepository()
        {
            // Act
            var request = new Accounts.Application.AddAuditCommand.AddAuditCommandRequest
            {
                CorrelationDate = new DateTime(2017, 4, 1),
                AccountRead = 1234,
                CompletedSuccessfully = true
            };
            _handler.Handle(request);

            // Assert
            _auditRepository.Verify(r => r.CreateAudit(request.CorrelationDate, request.AccountRead, request.CompletedSuccessfully));
        }
    }
}
