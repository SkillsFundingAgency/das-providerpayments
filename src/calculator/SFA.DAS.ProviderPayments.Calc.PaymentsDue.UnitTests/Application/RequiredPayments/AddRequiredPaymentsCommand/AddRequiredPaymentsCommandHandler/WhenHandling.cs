using System;
using Moq;
using NUnit.Framework;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Application.RequiredPayments;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Application.RequiredPayments.AddRequiredPaymentsCommand;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data.Entities;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.UnitTests.Application.RequiredPayments.AddRequiredPaymentsCommand.AddRequiredPaymentsCommandHandler
{
    public class WhenHandling
    {
        private static readonly RequiredPayment[] Payments =
        {
            new RequiredPayment
            {
                CommitmentId = 1,
                LearnerRefNumber = "Lrn001",
                AimSequenceNumber = 1,
                Ukprn = 10007459,
                DeliveryMonth = 9,
                DeliveryYear = 2016,
                TransactionType = TransactionType.Learning,
                AmountDue = 1000.00m
            },
            new RequiredPayment
            {
                CommitmentId = 2,
                LearnerRefNumber = "Lrn002",
                AimSequenceNumber = 1,
                Ukprn = 10007459,
                DeliveryMonth = 9,
                DeliveryYear = 2016,
                TransactionType = TransactionType.Completion,
                AmountDue = 3000.00m
            }
        };

        private Mock<IRequiredPaymentRepository> _repository;
        private AddRequiredPaymentsCommandRequest _request;
        private PaymentsDue.Application.RequiredPayments.AddRequiredPaymentsCommand.AddRequiredPaymentsCommandHandler _handler;

        [SetUp]
        public void Arrange()
        {
            _request = new AddRequiredPaymentsCommandRequest
            {
                Payments = Payments
            };

            _repository = new Mock<IRequiredPaymentRepository>();

            _handler = new PaymentsDue.Application.RequiredPayments.AddRequiredPaymentsCommand.AddRequiredPaymentsCommandHandler(_repository.Object);
        }

        [Test]
        public void ThenValidAddRequiredPaymentsCommandResponseReturnedForValidRepositoryResponse()
        {
            // Act
            var response = _handler.Handle(_request);

            // Assert
            Assert.IsNotNull(response);
            Assert.IsTrue(response.IsValid);
        }

        [Test]
        public void ThenItShouldWriteTheRequiredPaymentsToTheRepository()
        {
            // Act
            _handler.Handle(_request);

            // Assert
            _repository.Verify(r => r.AddRequiredPayments(It.Is<RequiredPaymentEntity[]>(p => PaymentBatchesMatch(p, Payments))), Times.Once);
        }

        [Test]
        public void ThenInvalidAddRequiredPaymentsCommandResponseReturnedForInvalidRepositoryResponse()
        {
            // Arrange
            _repository.Setup(r => r.AddRequiredPayments(It.IsAny<RequiredPaymentEntity[]>()))
                .Throws<Exception>();

            // Act
            var response = _handler.Handle(_request);

            // Assert
            Assert.IsNotNull(response);
            Assert.IsFalse(response.IsValid);
        }

        private bool PaymentBatchesMatch(RequiredPaymentEntity[] entities, RequiredPayment[] payments)
        {
            if (entities.Length != payments.Length)
            {
                return false;
            }

            for (var x = 0; x < entities.Length; x++)
            {
                if (!PaymentsMatch(entities[x], payments[x]))
                {
                    return false;
                }
            }

            return true;
        }

        private bool PaymentsMatch(RequiredPaymentEntity entity, RequiredPayment payment)
        {
            return entity.CommitmentId == payment.CommitmentId &&
                   entity.LearnRefNumber == payment.LearnerRefNumber &&
                   entity.AimSeqNumber == payment.AimSequenceNumber &&
                   entity.Ukprn == payment.Ukprn &&
                   entity.DeliveryMonth == payment.DeliveryMonth &&
                   entity.DeliveryYear == payment.DeliveryYear &&
                   entity.TransactionType == (int) payment.TransactionType &&
                   entity.AmountDue == payment.AmountDue;
        }
    }
}