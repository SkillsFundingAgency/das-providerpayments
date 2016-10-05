using System;
using System.Linq;
using Moq;
using NUnit.Framework;
using SFA.DAS.ProviderPayments.Calc.Common.Application;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Application.Earnings.GetProviderEarningsQuery;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data.Entities;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.UnitTests.Application.Earnings.GetProviderEarningsQuery.GetProviderEarningsQueryHandler
{
    public class WhenHandling
    {
        private const long Ukprn = 10007459;

        private readonly EarningEntity[] EarningEntities = {
            new EarningEntity
            {
                CommitmentId = "C-001",
                LearnerRefNumber = "Lrn-001",
                AimSequenceNumber = 1,
                Ukprn = Ukprn,
                MonthlyInstallment = 1000.00m,
                CompletionPayment = 3000.00m,
                Period1 = 1000m,
                Period2 = 1000m,
                Period3 = 1000m,
                Period4 = 1000m,
                Period5 = 1000m,
                Period6 = 1000m,
                Period7 = 1000m,
                Period8 = 0,
                Period9 = 0,
                Period10 = 0,
                Period11 = 0,
                Period12 = 0
            },
            new EarningEntity
            {
                CommitmentId = "C-002",
                LearnerRefNumber = "Lrn-002",
                AimSequenceNumber = 1,
                Ukprn = Ukprn,
                MonthlyInstallment = 1000.00m,
                CompletionPayment = 3000.00m,
                Period1 = 1000m,
                Period2 = 1000m,
                Period3 = 1000m,
                Period4 = 1000m,
                Period5 = 1000m,
                Period6 = 1000m,
                Period7 = 1000m,
                Period8 = 1000m,
                Period9 = 1000m,
                Period10 = 0,
                Period11 = 0,
                Period12 = 0
            }
        };

        private Mock<IEarningRepository> _repository;
        private GetProviderEarningsQueryRequest _request;
        private PaymentsDue.Application.Earnings.GetProviderEarningsQuery.GetProviderEarningsQueryHandler _handler;

        [SetUp]
        public void Arrange()
        {
            _request = new GetProviderEarningsQueryRequest
            {
                Ukprn = Ukprn,
                AcademicYear = "1718"
            };

            _repository = new Mock<IEarningRepository>();
            _repository.Setup(r => r.GetProviderEarnings(Ukprn))
                .Returns(EarningEntities);

            _handler = new PaymentsDue.Application.Earnings.GetProviderEarningsQuery.GetProviderEarningsQueryHandler(_repository.Object);
        }

        [Test]
        public void ThenItShouldReturnAnEarningRowForEachPeriodThatHasAnEarning()
        {
            // Act
            var actual = _handler.Handle(_request);

            // Assert
            Assert.IsNotNull(actual);
            Assert.IsTrue(actual.IsValid);
            Assert.IsNotNull(actual.Items);
            Assert.AreEqual(16, actual.Items.Length);

            Assert.IsTrue(actual.Items.Any(e => e.CollectionPeriodNumber == 1 && e.CollectionAcademicYear == "1718" && e.CommitmentId == "C-001"));
            Assert.IsTrue(actual.Items.Any(e => e.CollectionPeriodNumber == 2 && e.CollectionAcademicYear == "1718" && e.CommitmentId == "C-001"));
            Assert.IsTrue(actual.Items.Any(e => e.CollectionPeriodNumber == 3 && e.CollectionAcademicYear == "1718" && e.CommitmentId == "C-001"));
            Assert.IsTrue(actual.Items.Any(e => e.CollectionPeriodNumber == 4 && e.CollectionAcademicYear == "1718" && e.CommitmentId == "C-001"));
            Assert.IsTrue(actual.Items.Any(e => e.CollectionPeriodNumber == 5 && e.CollectionAcademicYear == "1718" && e.CommitmentId == "C-001"));
            Assert.IsTrue(actual.Items.Any(e => e.CollectionPeriodNumber == 6 && e.CollectionAcademicYear == "1718" && e.CommitmentId == "C-001"));
            Assert.IsTrue(actual.Items.Any(e => e.CollectionPeriodNumber == 7 && e.CollectionAcademicYear == "1718" && e.CommitmentId == "C-001"));

            Assert.IsTrue(actual.Items.Any(e => e.CollectionPeriodNumber == 1 && e.CollectionAcademicYear == "1718" && e.CommitmentId == "C-002"));
            Assert.IsTrue(actual.Items.Any(e => e.CollectionPeriodNumber == 2 && e.CollectionAcademicYear == "1718" && e.CommitmentId == "C-002"));
            Assert.IsTrue(actual.Items.Any(e => e.CollectionPeriodNumber == 3 && e.CollectionAcademicYear == "1718" && e.CommitmentId == "C-002"));
            Assert.IsTrue(actual.Items.Any(e => e.CollectionPeriodNumber == 4 && e.CollectionAcademicYear == "1718" && e.CommitmentId == "C-002"));
            Assert.IsTrue(actual.Items.Any(e => e.CollectionPeriodNumber == 5 && e.CollectionAcademicYear == "1718" && e.CommitmentId == "C-002"));
            Assert.IsTrue(actual.Items.Any(e => e.CollectionPeriodNumber == 6 && e.CollectionAcademicYear == "1718" && e.CommitmentId == "C-002"));
            Assert.IsTrue(actual.Items.Any(e => e.CollectionPeriodNumber == 7 && e.CollectionAcademicYear == "1718" && e.CommitmentId == "C-002"));
            Assert.IsTrue(actual.Items.Any(e => e.CollectionPeriodNumber == 8 && e.CollectionAcademicYear == "1718" && e.CommitmentId == "C-002"));
            Assert.IsTrue(actual.Items.Any(e => e.CollectionPeriodNumber == 9 && e.CollectionAcademicYear == "1718" && e.CommitmentId == "C-002"));
        }

        [Test]
        public void ThenItShouldReturnALearningAndACompletionEarningWhenBothOccurInAPeriod()
        {
            // Arrange
            _repository.Setup(r => r.GetProviderEarnings(Ukprn))
                .Returns(new[]
                {
                    new EarningEntity
                    {
                        CommitmentId = "C-001",
                        LearnerRefNumber = "Lrn-001",
                        AimSequenceNumber = 1,
                        Ukprn = Ukprn,
                        MonthlyInstallment = 1000.00m,
                        CompletionPayment = 3000.00m,
                        Period1 = 1000m,
                        Period2 = 1000m,
                        Period3 = 1000m,
                        Period4 = 1000m,
                        Period5 = 1000m,
                        Period6 = 1000m,
                        Period7 = 4000m,
                        Period8 = 0,
                        Period9 = 0,
                        Period10 = 0,
                        Period11 = 0,
                        Period12 = 0
                    }
                });

            // Act
            var actual = _handler.Handle(_request);

            // Assert
            var period7Earnings = actual.Items.Where(e => e.CollectionPeriodNumber == 7).OrderBy(e => (int)e.Type).ToArray();
            Assert.AreEqual(2, period7Earnings.Length);

            Assert.AreEqual(EarningEntities[0].MonthlyInstallment, period7Earnings[0].EarnedValue);
            Assert.AreEqual(TransactionType.Learning, period7Earnings[0].Type);

            Assert.AreEqual(EarningEntities[0].CompletionPayment, period7Earnings[1].EarnedValue);
            Assert.AreEqual(TransactionType.Completion, period7Earnings[1].Type);
        }

        [Test]
        public void ThenItShouldReturnAResponseWithNoItemsIfNoEarningsInRepository()
        {
            _repository.Setup(r => r.GetProviderEarnings(Ukprn))
                .Returns<EarningEntity[]>(null);

            // Act
            var actual = _handler.Handle(_request);

            // Assert
            Assert.IsNotNull(actual);
            Assert.IsTrue(actual.IsValid);
            Assert.IsNotNull(actual.Items);
            Assert.AreEqual(0, actual.Items.Length);
        }

        [Test]
        public void ThenIsShouldReturnAnInvalidResponseWithErrorIfRepositoryThrowsException()
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