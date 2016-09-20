using System;
using Moq;
using NUnit.Framework;
using SFA.DAS.Payments.Calc.CoInvestedPayments.Application.CollectionPeriods.GetCurrentCollectionPeriodQuery;
using SFA.DAS.Payments.Calc.CoInvestedPayments.Application.Earnings.GetProviderEarningsQuery;
using SFA.DAS.Payments.Calc.CoInvestedPayments.Infrastructure.Data;
using SFA.DAS.Payments.Calc.CoInvestedPayments.Infrastructure.Data.Entities;

namespace SFA.DAS.Payments.Calc.CoInvestedPayments.UnitTests.Application.Earnings.GetProviderEarningsQuery.GetProviderEarningsQueryHandler
{
    public class WhenHandling
    {
        private const long Ukprn = 10007459;

        private static readonly EarningEntity[] EarningEntities = {
            new EarningEntity
            {
                CommitmentId = "C-001",
                LearnerRefNumber = "Lrn-001",
                AimSequenceNumber = 1,
                Ukprn = Ukprn,
                LearningStartDate = new DateTime(2016, 8, 1),
                LearningPlannedEndDate = new DateTime(2017, 7, 31),
                LearningActualEndDate = null,
                MonthlyInstallment = 1000.00m,
                MonthlyInstallmentUncapped = 1000.00m,
                CompletionPayment = 3000.00m,
                CompletionPaymentUncapped = 3000.00m
            },
            new EarningEntity
            {
                CommitmentId = "C-002",
                LearnerRefNumber = "Lrn-002",
                AimSequenceNumber = 1,
                Ukprn = Ukprn,
                LearningStartDate = new DateTime(2016, 8, 1),
                LearningPlannedEndDate = new DateTime(2017, 7, 31),
                LearningActualEndDate = null,
                MonthlyInstallment = 1000.00m,
                MonthlyInstallmentUncapped = 1000.00m,
                CompletionPayment = 3000.00m,
                CompletionPaymentUncapped = 3000.00m
            }
        };

        private static readonly object[] RepositoryResponses =
        {
            new object[] {EarningEntities},
            new object[] {null}
        };

        private Mock<IEarningRepository> _repository;
        private GetProviderEarningsQueryRequest _request;
        private CoInvestedPayments.Application.Earnings.GetProviderEarningsQuery.GetProviderEarningsQueryHandler _handler;

        [SetUp]
        public void Arrange()
        {
            _request = new GetProviderEarningsQueryRequest
            {
                Ukprn = Ukprn
            };

            _repository = new Mock<IEarningRepository>();

            _handler = new CoInvestedPayments.Application.Earnings.GetProviderEarningsQuery.GetProviderEarningsQueryHandler(_repository.Object);
        }

        [Test]
        [TestCaseSource(nameof(RepositoryResponses))]
        public void ThenValidGetProviderEarningsQueryResponseReturnedForValidRepositoryResponse(EarningEntity[] earnings)
        {
            // Arrange
            _repository.Setup(r => r.GetProviderEarnings(Ukprn))
                .Returns(earnings);

            // Act
            var response = _handler.Handle(_request);

            // Assert
            Assert.IsNotNull(response);
            Assert.IsTrue(response.IsValid);
        }

        [Test]
        public void ThenEarningsShouldBeInTheGetProviderEarningsQueryResponse()
        {
            // Arrange
            _repository.Setup(r => r.GetProviderEarnings(Ukprn))
                .Returns(EarningEntities);

            // Act
            var response = _handler.Handle(_request);

            // Assert
            Assert.IsNotNull(response?.Items);
            Assert.AreEqual(EarningEntities[0].CommitmentId, response.Items[0].CommitmentId);
            Assert.AreEqual(EarningEntities[0].LearnerRefNumber, response.Items[0].LearnerRefNumber);
            Assert.AreEqual(EarningEntities[0].AimSequenceNumber, response.Items[0].AimSequenceNumber);
            Assert.AreEqual(EarningEntities[0].Ukprn, response.Items[0].Ukprn);
            Assert.AreEqual(EarningEntities[0].LearningStartDate, response.Items[0].LearningStartDate);
            Assert.AreEqual(EarningEntities[0].LearningPlannedEndDate, response.Items[0].LearningPlannedEndDate);
            Assert.AreEqual(EarningEntities[0].LearningActualEndDate, response.Items[0].LearningActualEndDate);
            Assert.AreEqual(EarningEntities[0].MonthlyInstallment, response.Items[0].MonthlyInstallment);
            Assert.AreEqual(EarningEntities[0].MonthlyInstallmentUncapped, response.Items[0].MonthlyInstallmentUncapped);
            Assert.AreEqual(EarningEntities[0].CompletionPayment, response.Items[0].CompletionPayment);
            Assert.AreEqual(EarningEntities[0].CompletionPaymentUncapped, response.Items[0].CompletionPaymentUncapped);
            Assert.AreEqual(EarningEntities[1].CommitmentId, response.Items[1].CommitmentId);
            Assert.AreEqual(EarningEntities[1].LearnerRefNumber, response.Items[1].LearnerRefNumber);
            Assert.AreEqual(EarningEntities[1].AimSequenceNumber, response.Items[1].AimSequenceNumber);
            Assert.AreEqual(EarningEntities[1].Ukprn, response.Items[1].Ukprn);
            Assert.AreEqual(EarningEntities[1].LearningStartDate, response.Items[1].LearningStartDate);
            Assert.AreEqual(EarningEntities[1].LearningPlannedEndDate, response.Items[1].LearningPlannedEndDate);
            Assert.AreEqual(EarningEntities[1].LearningActualEndDate, response.Items[1].LearningActualEndDate);
            Assert.AreEqual(EarningEntities[1].MonthlyInstallment, response.Items[1].MonthlyInstallment);
            Assert.AreEqual(EarningEntities[1].MonthlyInstallmentUncapped, response.Items[1].MonthlyInstallmentUncapped);
            Assert.AreEqual(EarningEntities[1].CompletionPayment, response.Items[1].CompletionPayment);
            Assert.AreEqual(EarningEntities[1].CompletionPaymentUncapped, response.Items[1].CompletionPaymentUncapped);
        }

        [Test]
        public void ThenInvalidGetProviderEarningsQueryResponseReturnedForInvalidRepositoryResponse()
        {
            // Arrange
            _repository.Setup(r => r.GetProviderEarnings(Ukprn))
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