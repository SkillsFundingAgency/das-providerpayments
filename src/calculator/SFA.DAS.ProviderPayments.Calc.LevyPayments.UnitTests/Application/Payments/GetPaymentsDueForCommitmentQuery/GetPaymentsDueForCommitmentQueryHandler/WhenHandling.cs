using System;
using Moq;
using NUnit.Framework;
using SFA.DAS.ProviderPayments.Calc.LevyPayments.Application.Payments;
using SFA.DAS.ProviderPayments.Calc.LevyPayments.Application.Payments.GetPaymentsDueForCommitmentQuery;
using SFA.DAS.ProviderPayments.Calc.LevyPayments.Infrastructure.Data;
using SFA.DAS.ProviderPayments.Calc.LevyPayments.Infrastructure.Data.Entities;

namespace SFA.DAS.ProviderPayments.Calc.LevyPayments.UnitTests.Application.Payments.GetPaymentsDueForCommitmentQuery.GetPaymentsDueForCommitmentQueryHandler
{
    public class WhenHandling
    {
        private const string CommitmentId = "C-001";

        private static readonly PaymentDueEntity[] PaymentDueEntities = 
        {
            new PaymentDueEntity
            {
                CommitmentId = CommitmentId,
                LearnRefNumber = "Lrn-001",
                AimSeqNumber = 1,
                Ukprn = 10007459,
                DeliveryMonth = 9,
                DeliveryYear = 2016,
                TransactionType = (int) TransactionType.Learning,
                AmountDue = 1000.00m
            },
            new PaymentDueEntity
            {
                CommitmentId = CommitmentId,
                LearnRefNumber = "Lrn-001",
                AimSeqNumber = 1,
                Ukprn = 10007459,
                DeliveryMonth = 9,
                DeliveryYear = 2016,
                TransactionType = (int) TransactionType.Completion,
                AmountDue = 3000.00m
            }
        };

        private static readonly object[] RepositoryResponses =
        {
            new object[] {PaymentDueEntities},
            new object[] {null}
        };

        private Mock<IPaymentDueRepository> _repository;
        private GetPaymentsDueForCommitmentQueryRequest _request;
        private LevyPayments.Application.Payments.GetPaymentsDueForCommitmentQuery.GetPaymentsDueForCommitmentQueryHandler _handler;

        [SetUp]
        public void Arrange()
        {
            _request = new GetPaymentsDueForCommitmentQueryRequest
            {
                CommitmentId = CommitmentId
            };

            _repository = new Mock<IPaymentDueRepository>();

            _handler = new LevyPayments.Application.Payments.GetPaymentsDueForCommitmentQuery.GetPaymentsDueForCommitmentQueryHandler(_repository.Object);
        }

        [Test]
        [TestCaseSource(nameof(RepositoryResponses))]
        public void ThenValidGetPaymentsDueForCommitmentQueryResponseReturnedForValidRepositoryResponse(PaymentDueEntity[] paymentsDue)
        {
            // Arrange
            _repository.Setup(r => r.GetPaymentsDueForCommitment(CommitmentId))
                .Returns(paymentsDue);

            // Act
            var response = _handler.Handle(_request);

            // Assert
            Assert.IsNotNull(response);
            Assert.IsTrue(response.IsValid);
        }

        [Test]
        public void ThenPaymentsDueShouldBeInTheGetPaymentsDueForCommitmentQueryResponse()
        {
            // Arrange
            _repository.Setup(r => r.GetPaymentsDueForCommitment(CommitmentId))
                .Returns(PaymentDueEntities);

            // Act
            var response = _handler.Handle(_request);

            // Assert
            Assert.IsNotNull(response?.Items);
            Assert.AreEqual(PaymentDueEntities[0].CommitmentId, response.Items[0].CommitmentId);
            Assert.AreEqual(PaymentDueEntities[0].LearnRefNumber, response.Items[0].LearnerRefNumber);
            Assert.AreEqual(PaymentDueEntities[0].AimSeqNumber, response.Items[0].AimSequenceNumber);
            Assert.AreEqual(PaymentDueEntities[0].Ukprn, response.Items[0].Ukprn);
            Assert.AreEqual(PaymentDueEntities[0].DeliveryMonth, response.Items[0].DeliveryMonth);
            Assert.AreEqual(PaymentDueEntities[0].DeliveryYear, response.Items[0].DeliveryYear);
            Assert.AreEqual(PaymentDueEntities[0].TransactionType, (int) response.Items[0].TransactionType);
            Assert.AreEqual(PaymentDueEntities[0].AmountDue, response.Items[0].AmountDue);
        }

        [Test]
        public void ThenInvalidGetPaymentsDueForCommitmentQueryResponseReturnedForInvalidRepositoryResponse()
        {
            // Arrange
            _repository.Setup(r => r.GetPaymentsDueForCommitment(CommitmentId))
                .Throws<Exception>();

            // Act
            var response = _handler.Handle(_request);

            // Assert
            Assert.IsFalse(response.IsValid);
            Assert.IsNull(response.Items);
            Assert.IsNotNull(response.Exception);
        }
    }
}