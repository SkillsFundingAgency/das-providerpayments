using System;
using Moq;
using NUnit.Framework;
using SFA.DAS.ProviderPayments.Calculator.LevyPayments.Application.Earnings.GetEarningForCommitmentQuery;
using SFA.DAS.ProviderPayments.Calculator.LevyPayments.Infrastructure.Data;
using SFA.DAS.ProviderPayments.Calculator.LevyPayments.Infrastructure.Data.Entities;

namespace SFA.DAS.ProviderPayments.Calculator.LevyPayments.UnitTests.Application.Earnings.GetEarningForCommitmentQuery.GetEarningForCommitmentQueryHandler
{
    public class WhenHandlingValidRequest
    {
        private const string CommitmentId = "Commitment1";
        private readonly EarningEntity _earningEntity = new EarningEntity
        {
            CommitmentId = CommitmentId,
            LearnerRefNumber = "LEARNER1",
            AimSequenceNumber = 99,
            Ukprn = 12345,
            LearningStartDate = new DateTime(2017, 5, 14),
            LearningPlannedEndDate = new DateTime(2018, 5, 21),
            CurrentPeriod = 1,
            TotalNumberOfPeriods = 12,
            MonthlyInstallment = 1100,
            MonthlyInstallmentCapped = 1000,
            CompletionPayment = 3300,
            CompletionPaymentCapped = 3000
        };
        private GetEarningForCommitmentQueryRequest _request;
        private Mock<IEarningRepository> _earningRepository;
        private LevyPayments.Application.Earnings.GetEarningForCommitmentQuery.GetEarningForCommitmentQueryHandler _handler;

        [SetUp]
        public void Arrange()
        {
            _request = new GetEarningForCommitmentQueryRequest
            {
                CommitmentId = CommitmentId
            };

            _earningRepository = new Mock<IEarningRepository>();
            _earningRepository.Setup(r => r.GetEarningForCommitment(CommitmentId))
                .Returns(_earningEntity);

            _handler = new LevyPayments.Application.Earnings.GetEarningForCommitmentQuery.GetEarningForCommitmentQueryHandler(_earningRepository.Object);
        }

        [Test]
        public void ThenItShouldReturnAnInstanceOfGetEarningForCommitmentQueryResponse()
        {
            // Act
            var actual = _handler.Handle(_request);

            // Assert
            Assert.IsNotNull(actual);
        }

        [Test]
        public void ThenItShouldHaveTheEarningForTheCommitment()
        {
            // Act
            var actual = _handler.Handle(_request);

            // Assert
            Assert.IsNotNull(actual?.Earning);
            Assert.AreEqual(_earningEntity.CommitmentId, actual.Earning.CommitmentId);
            Assert.AreEqual(_earningEntity.LearnerRefNumber, actual.Earning.LearnerRefNumber);
            Assert.AreEqual(_earningEntity.AimSequenceNumber, actual.Earning.AimSequenceNumber);
            Assert.AreEqual(_earningEntity.Ukprn, actual.Earning.Ukprn);
            Assert.AreEqual(_earningEntity.LearningStartDate, actual.Earning.LearningStartDate);
            Assert.AreEqual(_earningEntity.LearningPlannedEndDate, actual.Earning.LearningPlannedEndDate);
            Assert.AreEqual(_earningEntity.LearningActualEndDate, actual.Earning.LearningActualEndDate);
            Assert.AreEqual(_earningEntity.CurrentPeriod, actual.Earning.CurrentPeriod);
            Assert.AreEqual(_earningEntity.TotalNumberOfPeriods, actual.Earning.TotalNumberOfPeriods);
            Assert.AreEqual(_earningEntity.MonthlyInstallment, actual.Earning.MonthlyInstallment);
            Assert.AreEqual(_earningEntity.MonthlyInstallmentCapped, actual.Earning.MonthlyInstallmentCapped);
            Assert.AreEqual(_earningEntity.CompletionPayment, actual.Earning.CompletionPayment);
            Assert.AreEqual(_earningEntity.CompletionPaymentCapped, actual.Earning.CompletionPaymentCapped);
        }

    }
}
