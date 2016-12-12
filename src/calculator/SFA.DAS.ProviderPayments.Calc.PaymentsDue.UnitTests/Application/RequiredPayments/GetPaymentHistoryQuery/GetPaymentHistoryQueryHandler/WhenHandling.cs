using System;
using Moq;
using NUnit.Framework;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Application.RequiredPayments.GetPaymentHistoryQuery;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data.Entities;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.UnitTests.Application.RequiredPayments.GetPaymentHistoryQuery.GetPaymentHistoryQueryHandler
{
    public class WhenHandling
    {
        private const long CommitmentId = 1;
        private const long Ukprn = 123456;
        private const string LearnRefNumber = "LEARNER-1";
        private const int AimSequenceNumber = 1;
        private const int DeliveryMonth = 8;
        private const int DeliveryYear = 2017;
        private const decimal AmountDue = 123.45m;

        private GetPaymentHistoryQueryRequest _request;
        private Mock<IRequiredPaymentRepository> _requiredPaymentRepository;
        private PaymentsDue.Application.RequiredPayments.GetPaymentHistoryQuery.GetPaymentHistoryQueryHandler _handler;

        [SetUp]
        public void Arrange()
        {
            //_request = new GetPaymentHistoryQueryRequest
            //{
            //    Ukprn = Ukprn,
            //    CommitmentId = CommitmentId
            //};

            //_requiredPaymentRepository = new Mock<IRequiredPaymentRepository>();
            //_requiredPaymentRepository.Setup(r => r.GetPreviousPaymentsForCommitment(Ukprn, CommitmentId))
            //    .Returns(new[]
            //    {
            //        new RequiredPaymentEntity
            //        {
            //            CommitmentId = CommitmentId,
            //            Ukprn = Ukprn,
            //            LearnRefNumber = LearnRefNumber,
            //            AimSeqNumber = AimSequenceNumber,
            //            DeliveryMonth = DeliveryMonth,
            //            DeliveryYear = DeliveryYear,
            //            AmountDue = AmountDue
            //        }
            //    });

            //_handler = new PaymentsDue.Application.RequiredPayments.GetPaymentHistoryQuery.GetPaymentHistoryQueryHandler(_requiredPaymentRepository.Object);
        }

        [Test]
        public void ThenItShouldReturnAValidResponse()
        {
            // Act
            var actual = _handler.Handle(_request);

            // Assert
            Assert.IsNotNull(actual);
            Assert.IsTrue(actual.IsValid);
        }

        [Test]
        public void ThenItShouldReturnItemsFromRepositoryWhenTheyAreAvailable()
        {
            // Act
            var actual = _handler.Handle(_request);

            // Assert
            Assert.IsNotNull(actual.Items);
            Assert.AreEqual(1, actual.Items.Length);
            Assert.AreEqual(CommitmentId, actual.Items[0].CommitmentId);
            Assert.AreEqual(Ukprn, actual.Items[0].Ukprn);
            Assert.AreEqual(LearnRefNumber, actual.Items[0].LearnerRefNumber);
            Assert.AreEqual(AimSequenceNumber, actual.Items[0].AimSequenceNumber);
            Assert.AreEqual(DeliveryMonth, actual.Items[0].DeliveryMonth);
            Assert.AreEqual(DeliveryYear, actual.Items[0].DeliveryYear);
            Assert.AreEqual(AmountDue, actual.Items[0].AmountDue);
        }

        [Test]
        public void ThenItShouldReturnAnEmptyArrayOfItemsWhenNoneInTheRepository()
        {
            //// Arrange
            //_requiredPaymentRepository.Setup(r => r.GetPreviousPaymentsForCommitment(Ukprn, CommitmentId))
            //    .Returns<RequiredPaymentEntity[]>(null);

            //// Act
            //var actual = _handler.Handle(_request);

            //// Assert
            //Assert.IsNotNull(actual.Items);
            //Assert.AreEqual(0, actual.Items.Length);
        }

        [Test]
        public void ThenItShouldReturnAnInvalidResponseWhenRepositoryErrors()
        {
            //// Arrange
            //_requiredPaymentRepository.Setup(r => r.GetPreviousPaymentsForCommitment(Ukprn, CommitmentId))
            //    .Throws<Exception>();

            //// Act
            //var actual = _handler.Handle(_request);

            //// Assert
            //Assert.IsNotNull(actual);
            //Assert.IsFalse(actual.IsValid);
            //Assert.IsNotNull(actual.Exception);
        }
    }
}
