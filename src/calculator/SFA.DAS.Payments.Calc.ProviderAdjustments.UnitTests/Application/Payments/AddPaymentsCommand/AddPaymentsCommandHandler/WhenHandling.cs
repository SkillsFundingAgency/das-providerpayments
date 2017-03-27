using System;
using Moq;
using NUnit.Framework;
using SFA.DAS.Payments.Calc.ProviderAdjustments.Application.Payments;
using SFA.DAS.Payments.Calc.ProviderAdjustments.Application.Payments.AddPaymentsCommand;
using SFA.DAS.Payments.Calc.ProviderAdjustments.Infrastructure.Data;
using SFA.DAS.Payments.Calc.ProviderAdjustments.Infrastructure.Data.Entities;

namespace SFA.DAS.Payments.Calc.ProviderAdjustments.UnitTests.Application.Payments.AddPaymentsCommand.AddPaymentsCommandHandler
{
    public class WhenHandling
    {
        private static readonly long Ukprn = 10007459;
        private static readonly Guid SubmissionId = Guid.NewGuid();

        private static readonly Payment[] Payments =
        {
            new Payment
            {
                Ukprn = Ukprn,
                SubmissionId = SubmissionId,
                SubmissionCollectionPeriod = 1,
                SubmissionAcademicYear = 1617,
                PaymentType = 57,
                PaymentTypeName = "type",
                Amount = 123.45m,
                CollectionPeriodName = "1617-R12",
                CollectionPeriodMonth = 7,
                CollectionPeriodYear = 2017
            },
            new Payment
            {
                Ukprn = Ukprn,
                SubmissionId = SubmissionId,
                SubmissionCollectionPeriod = 5,
                SubmissionAcademicYear = 1617,
                PaymentType = 77,
                PaymentTypeName = "type",
                Amount = -75.12m,
                CollectionPeriodName = "1617-R12",
                CollectionPeriodMonth = 7,
                CollectionPeriodYear = 2017
            }
        };

        private Mock<IPaymentRepository> _repository;
        private AddPaymentsCommandRequest _request;
        private ProviderAdjustments.Application.Payments.AddPaymentsCommand.AddPaymentsCommandHandler _handler;

        [SetUp]
        public void Arrange()
        {
            _request = new AddPaymentsCommandRequest
            {
                Payments = Payments
            };

            _repository = new Mock<IPaymentRepository>();

            _handler = new ProviderAdjustments.Application.Payments.AddPaymentsCommand.AddPaymentsCommandHandler(_repository.Object);
        }

        [Test]
        public void ThenItShouldWriteThePaymentsToTheRepository()
        {
            // Act
            _handler.Handle(_request);

            // Assert
            _repository.Verify(r => r.AddProviderAdjustments(It.Is<PaymentEntity[]>(p => PaymentBatchesMatch(p, Payments))), Times.Once);
        }



        [Test]
        public void ThenItShouldThrowAnExceptionForInvalidRepositoryResponse()
        {
            // Arrange
            _repository.Setup(r => r.AddProviderAdjustments(It.IsAny<PaymentEntity[]>()))
                .Throws<Exception>();

            // Assert
            Assert.Throws<Exception>(() => _handler.Handle(_request));
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
            return entity.Ukprn == payment.Ukprn
                   && entity.SubmissionId == payment.SubmissionId
                   && entity.SubmissionCollectionPeriod == payment.SubmissionCollectionPeriod
                   && entity.SubmissionAcademicYear == payment.SubmissionAcademicYear
                   && entity.PaymentType == payment.PaymentType
                   && entity.PaymentTypeName == payment.PaymentTypeName
                   && entity.Amount == payment.Amount
                   && entity.CollectionPeriodName == payment.CollectionPeriodName
                   && entity.CollectionPeriodMonth == payment.CollectionPeriodMonth
                   && entity.CollectionPeriodYear == payment.CollectionPeriodYear;
        }
    }
}