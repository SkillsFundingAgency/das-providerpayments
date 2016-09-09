using Moq;
using NUnit.Framework;
using SFA.DAS.ProviderPayments.Calculator.LevyPayments.Application.Payments;
using SFA.DAS.ProviderPayments.Calculator.LevyPayments.Application.Payments.ProcessPaymentCommand;
using SFA.DAS.ProviderPayments.Calculator.LevyPayments.Infrastructure.Data;
using SFA.DAS.ProviderPayments.Calculator.LevyPayments.Infrastructure.Data.Entities;

namespace SFA.DAS.ProviderPayments.Calculator.LevyPayments.UnitTests.Application.Payments.ProcessPaymentCommand.ProcessPaymentCommandHandler.Handle
{
    public class WhenHandlingValidRequest
    {
        private ProcessPaymentCommandRequest _request;
        private Mock<IPaymentRepository> _paymentRepository;
        private LevyPayments.Application.Payments.ProcessPaymentCommand.ProcessPaymentCommandHandler _handler;

        [SetUp]
        public void Arrange()
        {
            _request = new ProcessPaymentCommandRequest
            {
                Payment = new Payment
                {
                    CommitmentId = "Commitment1",
                    LearnerRefNumber = "LEARNER1",
                    AimSequenceNumber = 99,
                    Ukprn = 12345,
                    Source = FundingSource.Levy,
                    TransactionType = TransactionType.Learning,
                    Amount = 987.65m
                }
            };

            _paymentRepository = new Mock<IPaymentRepository>();

            _handler = new LevyPayments.Application.Payments.ProcessPaymentCommand.ProcessPaymentCommandHandler(_paymentRepository.Object);
        }

        [Test]
        public void ThenItShouldWriteThePaymentToTheRepository()
        {
            // Act
            _handler.Handle(_request);

            // Assert
            _paymentRepository.Verify(r => r.AddPayment(It.Is<PaymentEntity>(e => PaymentsAreSame(e, _request.Payment))), Times.Once);
        }

        private bool PaymentsAreSame(PaymentEntity entity, Payment payment)
        {
            return entity.CommitmentId == payment.CommitmentId
                && entity.LearnerRefNumber == payment.LearnerRefNumber
                && entity.AimSequenceNumber == payment.AimSequenceNumber
                && entity.Ukprn == payment.Ukprn
                && entity.Source == (int)payment.Source
                && entity.TransactionType == (int)payment.TransactionType
                && entity.Amount == payment.Amount;
        }
    }
}
