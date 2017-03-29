using System;
using Moq;
using NUnit.Framework;
using SFA.DAS.ProviderPayments.Calc.LevyPayments.Application.Payments.GetPaymentsDueForCommitmentQuery;
using SFA.DAS.ProviderPayments.Calc.LevyPayments.Infrastructure.Data;
using SFA.DAS.ProviderPayments.Calc.LevyPayments.Infrastructure.Data.Entities;
using SFA.DAS.Payments.DCFS.Domain;
using SFA.DAS.ProviderPayments.Calc.LevyPayments.Application.Payments.GetLevyPaymentsHistoryQuery;

namespace SFA.DAS.ProviderPayments.Calc.LevyPayments.UnitTests.Application.Payments.GetLevyPaymentsHistory.GetLevyPaymentsHistoryQueryHandler
{
    public class WhenHandling
    {
        private const long CommitmentId = 1;

        private static readonly PaymentHistoryEntity[] PaymentHistoryEntities =
        {
            new PaymentHistoryEntity
            {
                RequiredPaymentId=Guid.NewGuid(),
                CommitmentId = CommitmentId,
                DeliveryMonth = 9,
                DeliveryYear = 2016,
                TransactionType = (int) TransactionType.Learning,
                Amount = 1000.00m
            },
            new PaymentHistoryEntity
            {
                RequiredPaymentId=Guid.NewGuid(),
                CommitmentId = CommitmentId,
                DeliveryMonth = 10,
                DeliveryYear = 2016,
                TransactionType = (int) TransactionType.Completion,
                Amount = 1000.00m
            },
        };

        private static readonly object[] RepositoryResponses =
        {
            new object[] {PaymentHistoryEntities},
            new object[] {null}
        };

        private Mock<IPaymentRepository> _repository;
        private GetLevyPaymentsHistoryQueryRequest _request;
        private LevyPayments.Application.Payments.GetLevyPaymentsHistoryQuery.GetLevyPaymentsHistoryQueryHandler _handler;

        [SetUp]
        public void Arrange()
        {
            _request = new GetLevyPaymentsHistoryQueryRequest
            {
                CommitmentId = CommitmentId,
                DeliveryMonth=9,
                DeliveryYear=2016,
                TransactionType=1
            };

            _repository = new Mock<IPaymentRepository>();

            _handler = new LevyPayments.Application.Payments.GetLevyPaymentsHistoryQuery.GetLevyPaymentsHistoryQueryHandler(_repository.Object);
        }

        [Test]
        [TestCaseSource(nameof(RepositoryResponses))]
        public void ThenValidGetPaymentsHistoryResponseReturnedForValidRepositoryResponse(PaymentHistoryEntity[] payments)
        {
            // Arrange
            _repository.Setup(r => r.GetLevyPaymentsHistory(9,2016,1,CommitmentId))
                .Returns(payments);

            // Act
            var response = _handler.Handle(_request);

            // Assert
            Assert.IsNotNull(response);
            Assert.IsTrue(response.IsValid);
        }

        [Test]
        public void ThenPaymentsHistoryShouldBeInTheGetPaymentsHistoryQueryResponse()
        {
            // Arrange
            _repository.Setup(r => r.GetLevyPaymentsHistory(9, 2016, 1, CommitmentId))
                .Returns(PaymentHistoryEntities);

            // Act
            var response = _handler.Handle(_request);

            // Assert
            Assert.IsNotNull(response?.Items);
            Assert.AreEqual(PaymentHistoryEntities[0].CommitmentId, response.Items[0].CommitmentId);

            Assert.AreEqual(PaymentHistoryEntities[0].CommitmentId, response.Items[0].CommitmentId);
            Assert.AreEqual(PaymentHistoryEntities[0].DeliveryMonth, response.Items[0].DeliveryMonth);
            Assert.AreEqual(PaymentHistoryEntities[0].DeliveryYear, response.Items[0].DeliveryYear);
            Assert.AreEqual(PaymentHistoryEntities[0].TransactionType, (int) response.Items[0].TransactionType);
            Assert.AreEqual(PaymentHistoryEntities[0].Amount, response.Items[0].Amount);
        }

        
    }
}