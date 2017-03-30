using System;
using Moq;
using NUnit.Framework;
using SFA.DAS.Payments.DCFS.Domain;
using SFA.DAS.Payments.Calc.CoInvestedPayments.Infrastructure.Data.Entities;
using SFA.DAS.Payments.Calc.CoInvestedPayments.Infrastructure.Data;
using SFA.DAS.ProviderPayments.Calc.CoInvestedPayments.Application.Payments.GetCoInvestedPaymentsHistoryQuery;

namespace SFA.DAS.Payments.Calc.CoInvestedPayments.UnitTests.Application.Payments.GetCoInvestedPaymentsHistory.GetCoInvestedPaymentsHistoryQueryHandler
{
    public class WhenHandling
    {
        private const long CommitmentId = 1;

        private static readonly PaymentHistoryEntity[] PaymentHistoryEntities =
        {
            new PaymentHistoryEntity
            {
                RequiredPaymentId=Guid.NewGuid(),
                DeliveryMonth = 9,
                DeliveryYear = 2016,
                TransactionType = (int) TransactionType.Learning,
                Amount = 1000.00m,
                AimSequenceNumber = 1,
                FundingSource=2,
                Ukprn=1000,
                Uln=2000,
                StandardCode=23
            },
            new PaymentHistoryEntity
            {
                RequiredPaymentId=Guid.NewGuid(),
                DeliveryMonth = 9,
                DeliveryYear = 2016,
                TransactionType = (int) TransactionType.Learning,
                Amount = 1000.00m,
                AimSequenceNumber = 1,
                FundingSource=2,
                Ukprn=1000,
                Uln=2000,
                FrameworkCode=20,
                PathwayCode=100,
                ProgrammeType=1
            },
        };

        private static readonly object[] RepositoryResponses =
        {
            new object[] {PaymentHistoryEntities},
            new object[] {null}
        };

        private Mock<IPaymentRepository> _repository;
        private GetCoInvestedPaymentsHistoryQueryRequest _request;
        private ProviderPayments.Calc.CoInvestedPayments.Application.Payments.GetCoInvestedPaymentsHistoryQuery.GetCoInvestedPaymentsHistoryQueryHandler _handler;

        [SetUp]
        public void Arrange()
        {
            _request = new GetCoInvestedPaymentsHistoryQueryRequest
            {
                DeliveryMonth=9,
                DeliveryYear=2016,
                TransactionType=1,
                AimSequenceNumber=1,
                Ukprn=1000,
                Uln=2000,
                StandardCode=23
            };

            _repository = new Mock<IPaymentRepository>();

            _handler = new ProviderPayments.Calc.CoInvestedPayments.Application.Payments.GetCoInvestedPaymentsHistoryQuery.GetCoInvestedPaymentsHistoryQueryHandler(_repository.Object);
        }

        [Test]
        [TestCaseSource(nameof(RepositoryResponses))]
        public void ThenValidGetPaymentsHistoryResponseReturnedForValidRepositoryResponse(PaymentHistoryEntity[] payments)
        {
            // Arrange
            _repository.Setup(r => r.GetCoInvestedPaymentsHistory(9,2016,1,1,2000,1000,null,null,null,23))
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
            _repository.Setup(r => r.GetCoInvestedPaymentsHistory(9, 2016, 1, 1, 1000, 2000, null, null, null, 23))
                .Returns(PaymentHistoryEntities);

            // Act
            var response = _handler.Handle(_request);

            // Assert
            Assert.IsNotNull(response?.Items);
            Assert.AreEqual(PaymentHistoryEntities[0].AimSequenceNumber, response.Items[0].AimSequenceNumber);
            Assert.AreEqual(PaymentHistoryEntities[0].StandardCode, response.Items[0].StandardCode);
            Assert.AreEqual(PaymentHistoryEntities[0].DeliveryMonth, response.Items[0].DeliveryMonth);
            Assert.AreEqual(PaymentHistoryEntities[0].DeliveryYear, response.Items[0].DeliveryYear);
            Assert.AreEqual(PaymentHistoryEntities[0].TransactionType, (int) response.Items[0].TransactionType);
            Assert.AreEqual(PaymentHistoryEntities[0].FrameworkCode, response.Items[0].FrameworkCode);
            Assert.AreEqual(PaymentHistoryEntities[0].PathwayCode, response.Items[0].PathwayCode);
            Assert.AreEqual(PaymentHistoryEntities[0].ProgrammeType, response.Items[0].ProgrammeType);
            Assert.AreEqual(PaymentHistoryEntities[0].Ukprn, response.Items[0].Ukprn);
            Assert.AreEqual(PaymentHistoryEntities[0].Uln, response.Items[0].Uln);

        }


    }
}