using System;
using Moq;
using NUnit.Framework;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Application.RequiredPayments;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Application.RequiredPayments.AddRequiredPaymentsCommand;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data.Entities;
using SFA.DAS.Payments.DCFS.Domain;

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
                AmountDue = 1000.00m,
                StandardCode = 25,
                SfaContributionPercentage = 0.9m,
                FundingLineType = "Levy Funding Line"
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
                AmountDue = 3000.00m,
                FrameworkCode = 550,
                ProgrammeType = 20,
                PathwayCode = 6,
                SfaContributionPercentage = 0.9m,
                FundingLineType = "Levy Funding Line"
            },
            new RequiredPayment
            {
                CommitmentId = 2,
                LearnerRefNumber = "Lrn002",
                AimSequenceNumber = 1,
                Ukprn = 10007459,
                DeliveryMonth = 9,
                DeliveryYear = 2016,
                TransactionType = TransactionType.First16To18EmployerIncentive,
                AmountDue = 500.00m,
                FrameworkCode = 550,
                ProgrammeType = 20,
                PathwayCode = 6,
                SfaContributionPercentage = 0.9m,
                FundingLineType = "Levy Funding Line"
            },
            new RequiredPayment
            {
                CommitmentId = 2,
                LearnerRefNumber = "Lrn002",
                AimSequenceNumber = 1,
                Ukprn = 10007459,
                DeliveryMonth = 9,
                DeliveryYear = 2016,
                TransactionType = TransactionType.Second16To18EmployerIncentive,
                AmountDue = 500.00m,
                FrameworkCode = 550,
                ProgrammeType = 20,
                PathwayCode = 6,
                SfaContributionPercentage = 0.9m,
                FundingLineType = "Levy Funding Line"
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
            return entity.CommitmentId == payment.CommitmentId
                   && entity.CommitmentVersionId == payment.CommitmentVersionId
                   && entity.AccountId == payment.AccountVersionId
                   && entity.AccountVersionId == payment.AccountVersionId
                   && entity.Uln == payment.Uln
                   && entity.LearnRefNumber == payment.LearnerRefNumber
                   && entity.AimSeqNumber == payment.AimSequenceNumber
                   && entity.Ukprn == payment.Ukprn
                   && entity.IlrSubmissionDateTime == payment.IlrSubmissionDateTime
                   && entity.DeliveryMonth == payment.DeliveryMonth
                   && entity.DeliveryYear == payment.DeliveryYear
                   && entity.TransactionType == (int) payment.TransactionType
                   && entity.AmountDue == payment.AmountDue
                   && entity.StandardCode == payment.StandardCode
                   && entity.FrameworkCode == payment.FrameworkCode
                   && entity.ProgrammeType == payment.ProgrammeType
                   && entity.PathwayCode == payment.PathwayCode
                   && entity.PriceEpisodeIdentifier == payment.PriceEpisodeIdentifier
                   && entity.ApprenticeshipContractType == payment.ApprenticeshipContractType
                   && entity.SfaContributionPercentage == payment.SfaContributionPercentage
                   && entity.FundingLineType == payment.FundingLineType;
        }
    }
}