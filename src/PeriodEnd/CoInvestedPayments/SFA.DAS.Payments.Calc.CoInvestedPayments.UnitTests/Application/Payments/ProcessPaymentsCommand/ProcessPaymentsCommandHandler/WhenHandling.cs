using System;
using Moq;
using NUnit.Framework;
using SFA.DAS.Payments.Calc.CoInvestedPayments.Application.Payments;
using SFA.DAS.Payments.Calc.CoInvestedPayments.Application.Payments.ProcessPaymentsCommand;
using SFA.DAS.Payments.Calc.CoInvestedPayments.Infrastructure.Data;
using SFA.DAS.Payments.Calc.CoInvestedPayments.Infrastructure.Data.Entities;
using SFA.DAS.Payments.DCFS.Domain;

namespace SFA.DAS.Payments.Calc.CoInvestedPayments.UnitTests.Application.Payments.ProcessPaymentsCommand.ProcessPaymentsCommandHandler
{
    public class WhenHandling
    {
        private static readonly Payment[] Payments =
        {
            new Payment
            {
                DeliveryMonth = 12,
                DeliveryYear = 2015,
                CollectionPeriodName = "R02",
                CollectionPeriodMonth = 9,
                CollectionPeriodYear = 2016,
                FundingSource = FundingSource.CoInvestedSfa,
                TransactionType = TransactionType.Learning,
                Amount = 900.00m
            },
            new Payment
            {
                DeliveryMonth = 12,
                DeliveryYear = 2015,
                CollectionPeriodName = "R02",
                CollectionPeriodMonth = 9,
                CollectionPeriodYear = 2016,
                FundingSource = FundingSource.CoInvestedEmployer,
                TransactionType = TransactionType.Learning,
                Amount = 100.00m
            }
        };

        private Mock<IPaymentRepository> _repository;
        private ProcessPaymentsCommandRequest _request;
        private CoInvestedPayments.Application.Payments.ProcessPaymentsCommand.ProcessPaymentsCommandHandler _handler;

        [SetUp]
        public void Arrange()
        {
            _request = new ProcessPaymentsCommandRequest
            {
                Payments = Payments
            };

            _repository = new Mock<IPaymentRepository>();

            _handler = new CoInvestedPayments.Application.Payments.ProcessPaymentsCommand.ProcessPaymentsCommandHandler(_repository.Object, null);
        }

        [Test]
        public void ThenValidProcessPaymentsCommandResponseReturnedForValidRepositoryResponse()
        {
            // Act
            var response = _handler.Handle(_request);

            // Assert
            Assert.IsNotNull(response);
            Assert.IsTrue(response.IsValid);
        }

        [Test]
        public void ThenItShouldWriteTheCoInvestedPaymentsToTheRepository()
        {
            // Act
            _handler.Handle(_request);

            // Assert
            _repository.Verify(r => r.AddPayments(It.Is<PaymentEntity[]>(p => PaymentBatchesMatch(p, Payments))), Times.Once);
        }

        [Test]
        public void ThenInvalidAddRequiredPaymentsCommandResponseReturnedForInvalidRepositoryResponse()
        {
            // Arrange
            _repository.Setup(r => r.AddPayments(It.IsAny<PaymentEntity[]>()))
                .Throws<Exception>();

            // Act
            var response = _handler.Handle(_request);

            // Assert
            Assert.IsNotNull(response);
            Assert.IsFalse(response.IsValid);
        }

        private bool PaymentBatchesMatch(PaymentEntity[] entities, Payment[] payments)
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

        private bool PaymentsMatch(PaymentEntity entity, Payment payment)
        {
            return entity.DeliveryMonth == payment.DeliveryMonth &&
                   entity.DeliveryYear == payment.DeliveryYear &&
                   entity.CollectionPeriodName == payment.CollectionPeriodName &&
                   entity.CollectionPeriodMonth == payment.CollectionPeriodMonth &&
                   entity.CollectionPeriodYear == payment.CollectionPeriodYear &&
                   entity.FundingSource == (int) payment.FundingSource &&
                   entity.TransactionType == (int) payment.TransactionType &&
                   entity.Amount == payment.Amount;
        }
    }
}