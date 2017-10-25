using System.Collections.Generic;
using System.Linq;
using Moq;
using NUnit.Framework;
using SFA.DAS.ProviderPayments.Calc.ManualAdjustments.Application.ReversePaymentCommand;
using SFA.DAS.ProviderPayments.Calc.ManualAdjustments.Infrastructure.Entities;

namespace SFA.DAS.ProviderPayments.Calc.ManualAdjustments.UnitTests.Application.ReversePaymentCommand.ReversePaymentCommandHandler
{
    public class WhenHandlingValidRequest : WhenHandlingBase
    {
        [SetUp]
        public override void Arrange()
        {
            base.Arrange();
        }

        [Test]
        public void ThenItShouldReturnAValidResponse()
        {
            // Act
            var actual = Handler.Handle(Request);

            // Assert
            Assert.IsNotNull(actual);
            Assert.IsTrue(actual.IsValid);
        }

        [Test]
        public void ThenItShouldReturnAnIdForReversingRequiredPayment()
        {
            // Act
            var actual = Handler.Handle(Request);

            // Assert
            Assert.IsNotEmpty(actual.RequiredPaymentIdForReversal);
        }

        [Test]
        public void ThenItShouldCreateRequiredPaymentSameAsOrignalButWithInverseAmount()
        {
            // Act
            Handler.Handle(Request);

            // Assert
            RequiredPaymentRepository.Verify(r => r.CreateRequiredPayment(It.IsAny<RequiredPaymentEntity>()), Times.Once);
            RequiredPaymentRepository.Verify(r => r.CreateRequiredPayment(It.Is<RequiredPaymentEntity>(e => e.CommitmentId == OriginalRequiredPayment.CommitmentId)));
            RequiredPaymentRepository.Verify(r => r.CreateRequiredPayment(It.Is<RequiredPaymentEntity>(e => e.CommitmentVersionId == OriginalRequiredPayment.CommitmentVersionId)));
            RequiredPaymentRepository.Verify(r => r.CreateRequiredPayment(It.Is<RequiredPaymentEntity>(e => e.AccountId == OriginalRequiredPayment.AccountId)));
            RequiredPaymentRepository.Verify(r => r.CreateRequiredPayment(It.Is<RequiredPaymentEntity>(e => e.AccountVersionId == OriginalRequiredPayment.AccountVersionId)));
            RequiredPaymentRepository.Verify(r => r.CreateRequiredPayment(It.Is<RequiredPaymentEntity>(e => e.Uln == OriginalRequiredPayment.Uln)));
            RequiredPaymentRepository.Verify(r => r.CreateRequiredPayment(It.Is<RequiredPaymentEntity>(e => e.LearnRefNumber == OriginalRequiredPayment.LearnRefNumber)));
            RequiredPaymentRepository.Verify(r => r.CreateRequiredPayment(It.Is<RequiredPaymentEntity>(e => e.AimSeqNumber == OriginalRequiredPayment.AimSeqNumber)));
            RequiredPaymentRepository.Verify(r => r.CreateRequiredPayment(It.Is<RequiredPaymentEntity>(e => e.Ukprn == OriginalRequiredPayment.Ukprn)));
            RequiredPaymentRepository.Verify(r => r.CreateRequiredPayment(It.Is<RequiredPaymentEntity>(e => e.IlrSubmissionDateTime == OriginalRequiredPayment.IlrSubmissionDateTime)));
            RequiredPaymentRepository.Verify(r => r.CreateRequiredPayment(It.Is<RequiredPaymentEntity>(e => e.PriceEpisodeIdentifier == OriginalRequiredPayment.PriceEpisodeIdentifier)));
            RequiredPaymentRepository.Verify(r => r.CreateRequiredPayment(It.Is<RequiredPaymentEntity>(e => e.StandardCode == OriginalRequiredPayment.StandardCode)));
            RequiredPaymentRepository.Verify(r => r.CreateRequiredPayment(It.Is<RequiredPaymentEntity>(e => e.ProgrammeType == OriginalRequiredPayment.ProgrammeType)));
            RequiredPaymentRepository.Verify(r => r.CreateRequiredPayment(It.Is<RequiredPaymentEntity>(e => e.FrameworkCode == OriginalRequiredPayment.FrameworkCode)));
            RequiredPaymentRepository.Verify(r => r.CreateRequiredPayment(It.Is<RequiredPaymentEntity>(e => e.PathwayCode == OriginalRequiredPayment.PathwayCode)));
            RequiredPaymentRepository.Verify(r => r.CreateRequiredPayment(It.Is<RequiredPaymentEntity>(e => e.ApprenticeshipContractType == OriginalRequiredPayment.ApprenticeshipContractType)));
            RequiredPaymentRepository.Verify(r => r.CreateRequiredPayment(It.Is<RequiredPaymentEntity>(e => e.DeliveryMonth == OriginalRequiredPayment.DeliveryMonth)));
            RequiredPaymentRepository.Verify(r => r.CreateRequiredPayment(It.Is<RequiredPaymentEntity>(e => e.DeliveryYear == OriginalRequiredPayment.DeliveryYear)));
            RequiredPaymentRepository.Verify(r => r.CreateRequiredPayment(It.Is<RequiredPaymentEntity>(e => e.TransactionType == OriginalRequiredPayment.TransactionType)));
            RequiredPaymentRepository.Verify(r => r.CreateRequiredPayment(It.Is<RequiredPaymentEntity>(e => e.AmountDue == -OriginalRequiredPayment.AmountDue)));
            RequiredPaymentRepository.Verify(r => r.CreateRequiredPayment(It.Is<RequiredPaymentEntity>(e => e.SfaContributionPercentage == OriginalRequiredPayment.SfaContributionPercentage)));
            RequiredPaymentRepository.Verify(r => r.CreateRequiredPayment(It.Is<RequiredPaymentEntity>(e => e.FundingLineType == OriginalRequiredPayment.FundingLineType)));
            RequiredPaymentRepository.Verify(r => r.CreateRequiredPayment(It.Is<RequiredPaymentEntity>(e => e.UseLevyBalance == OriginalRequiredPayment.UseLevyBalance)));
            RequiredPaymentRepository.Verify(r => r.CreateRequiredPayment(It.Is<RequiredPaymentEntity>(e => e.CollectionPeriodName == "1617-R02")));
            RequiredPaymentRepository.Verify(r => r.CreateRequiredPayment(It.Is<RequiredPaymentEntity>(e => e.CollectionPeriodMonth == 9)));
            RequiredPaymentRepository.Verify(r => r.CreateRequiredPayment(It.Is<RequiredPaymentEntity>(e => e.CollectionPeriodYear == 2016)));
        }

        [Test]
        public void ThenItShouldCreatePaymentSameAsOriginalButWithInverseAmountForEachPaymentOnOriginalRequiredPayment()
        {
            // Arrange
            var createdPayments = new List<PaymentEntity>();
            var createdRequiredPayments = new List<PaymentEntity>();

            PaymentRepository.Setup(r => r.CreatePayment(It.IsAny<PaymentEntity>(), It.IsAny<RequiredPaymentEntity>()))
                .Callback((PaymentEntity entity,RequiredPaymentEntity rq) => createdPayments.Add(entity));

            // Act
            Handler.Handle(Request);

            // Assert
            PaymentRepository.Verify(r => r.CreatePayment(It.IsAny<PaymentEntity>(),It.IsAny<RequiredPaymentEntity>()), Times.Exactly(3));
            Assert.AreEqual(3, createdPayments.Count);

            var actualLevyPayment = createdPayments.SingleOrDefault(e => e.FundingSource == 1);
            Assert.IsNotNull(actualLevyPayment, "Could not find expected levy payment");
            Assert.AreEqual(OriginalPayment1.DeliveryMonth, actualLevyPayment.DeliveryMonth);
            Assert.AreEqual(OriginalPayment1.DeliveryYear, actualLevyPayment.DeliveryYear);
            Assert.AreEqual(OriginalPayment1.TransactionType, actualLevyPayment.TransactionType);
            Assert.AreEqual(OriginalPayment1.Amount, -actualLevyPayment.Amount);
            Assert.AreEqual(OriginalPayment1.CommitmentId, -actualLevyPayment.CommitmentId);
            Assert.AreEqual($"{YearOfCollection}-{OpenCollectionPeriod.Name}", actualLevyPayment.CollectionPeriodName);
            Assert.AreEqual(OpenCollectionPeriod.CalendarMonth, actualLevyPayment.CollectionPeriodMonth);
            Assert.AreEqual(OpenCollectionPeriod.CalendarYear, actualLevyPayment.CollectionPeriodYear);
        }

        [Test]
        public void ThenItShouldAdjustAccountLevyBalanceByAmountOfReversedLevyPayments()
        {
            // Act
            Handler.Handle(Request);

            // Assert
            AccountRepository.Verify(r => r.AdjustAccountBalance(OriginalRequiredPayment.AccountId, OriginalPayment1.Amount), Times.Once);
            AccountRepository.Verify(r => r.AdjustAccountBalance(It.IsAny<string>(), It.IsAny<decimal>()), Times.Once);
        }
    }
}