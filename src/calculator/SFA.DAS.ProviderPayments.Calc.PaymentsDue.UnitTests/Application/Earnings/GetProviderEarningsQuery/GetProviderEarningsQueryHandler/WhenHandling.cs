using System;
using System.Linq;
using Moq;
using NUnit.Framework;
using SFA.DAS.ProviderPayments.Calc.Common.Application;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Application.Earnings;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Application.Earnings.GetProviderEarningsQuery;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data.Entities;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.UnitTests.Application.Earnings.GetProviderEarningsQuery.GetProviderEarningsQueryHandler
{
    public class WhenHandling
    {
        private const long Ukprn = 10007459;

        private static readonly EarningEntity[] EarningEntities = {
            new EarningEntity
            {
                CommitmentId = 1,
                CommitmentVersionId = "C1",
                AccountId = "1",
                AccountVersionId = "A1",
                Uln = 1,
                LearnerRefNumber = "Lrn-001",
                AimSequenceNumber = 1,
                Ukprn = Ukprn,
                EarningType = EarningTypes.Learning,
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
                Period12 = 0,
                StandardCode = 25
            },
            new EarningEntity
            {
                CommitmentId = 2,
                CommitmentVersionId = "C2",
                AccountId = "2",
                AccountVersionId = "A2",
                Uln = 2,
                LearnerRefNumber = "Lrn-002",
                AimSequenceNumber = 1,
                Ukprn = Ukprn,
                EarningType = EarningTypes.Learning,
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
                Period12 = 0,
                FrameworkCode = 550,
                ProgrammeType = 20,
                PathwayCode = 6
            }
        };

        private static readonly object[] EarningTypesWithExpectedTransactionTypes =
        {
            new object[] {EarningTypes.Learning, TransactionType.Learning},
            new object[] {EarningTypes.Completion, TransactionType.Completion},
            new object[] {EarningTypes.Balancing, TransactionType.Balancing}
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

            Assert.IsTrue(actual.Items.Any(e => e.CollectionPeriodNumber == 1 && e.CollectionAcademicYear == "1718" && e.CommitmentId == 1));
            Assert.IsTrue(actual.Items.Any(e => e.CollectionPeriodNumber == 2 && e.CollectionAcademicYear == "1718" && e.CommitmentId == 1));
            Assert.IsTrue(actual.Items.Any(e => e.CollectionPeriodNumber == 3 && e.CollectionAcademicYear == "1718" && e.CommitmentId == 1));
            Assert.IsTrue(actual.Items.Any(e => e.CollectionPeriodNumber == 4 && e.CollectionAcademicYear == "1718" && e.CommitmentId == 1));
            Assert.IsTrue(actual.Items.Any(e => e.CollectionPeriodNumber == 5 && e.CollectionAcademicYear == "1718" && e.CommitmentId == 1));
            Assert.IsTrue(actual.Items.Any(e => e.CollectionPeriodNumber == 6 && e.CollectionAcademicYear == "1718" && e.CommitmentId == 1));
            Assert.IsTrue(actual.Items.Any(e => e.CollectionPeriodNumber == 7 && e.CollectionAcademicYear == "1718" && e.CommitmentId == 1));

            Assert.IsTrue(actual.Items.Any(e => e.CollectionPeriodNumber == 1 && e.CollectionAcademicYear == "1718" && e.CommitmentId == 2));
            Assert.IsTrue(actual.Items.Any(e => e.CollectionPeriodNumber == 2 && e.CollectionAcademicYear == "1718" && e.CommitmentId == 2));
            Assert.IsTrue(actual.Items.Any(e => e.CollectionPeriodNumber == 3 && e.CollectionAcademicYear == "1718" && e.CommitmentId == 2));
            Assert.IsTrue(actual.Items.Any(e => e.CollectionPeriodNumber == 4 && e.CollectionAcademicYear == "1718" && e.CommitmentId == 2));
            Assert.IsTrue(actual.Items.Any(e => e.CollectionPeriodNumber == 5 && e.CollectionAcademicYear == "1718" && e.CommitmentId == 2));
            Assert.IsTrue(actual.Items.Any(e => e.CollectionPeriodNumber == 6 && e.CollectionAcademicYear == "1718" && e.CommitmentId == 2));
            Assert.IsTrue(actual.Items.Any(e => e.CollectionPeriodNumber == 7 && e.CollectionAcademicYear == "1718" && e.CommitmentId == 2));
            Assert.IsTrue(actual.Items.Any(e => e.CollectionPeriodNumber == 8 && e.CollectionAcademicYear == "1718" && e.CommitmentId == 2));
            Assert.IsTrue(actual.Items.Any(e => e.CollectionPeriodNumber == 9 && e.CollectionAcademicYear == "1718" && e.CommitmentId == 2));
        }

        [Test]
        [TestCaseSource(nameof(EarningTypesWithExpectedTransactionTypes))]
        public void ThenItShouldReturnEarningsWithTheCorrectTransactionType(string earningType, TransactionType transactionType)
        {
            // Arrange
            _repository.Setup(r => r.GetProviderEarnings(Ukprn))
                .Returns(new[]
                {
                    new EarningEntity
                    {
                        CommitmentId = 1,
                        CommitmentVersionId = "C1",
                        AccountId = "1",
                        AccountVersionId = "A1",
                        Uln = 1,
                        LearnerRefNumber = "Lrn-001",
                        AimSequenceNumber = 1,
                        Ukprn = Ukprn,
                        EarningType = earningType,
                        Period1 = 0,
                        Period2 = 0,
                        Period3 = 0,
                        Period4 = 0,
                        Period5 = 0,
                        Period6 = 1000.00m,
                        Period7 = 0,
                        Period8 = 0,
                        Period9 = 0,
                        Period10 = 0,
                        Period11 = 0,
                        Period12 = 0,
                        StandardCode = 25
                    }
                });

            // Act
            var actual = _handler.Handle(_request);

            // Assert
            var period6Earnings = actual.Items.Where(e => e.CollectionPeriodNumber == 6).OrderBy(e => (int)e.Type).ToArray();
            Assert.AreEqual(1, period6Earnings.Length);

            Assert.AreEqual(transactionType, period6Earnings[0].Type);
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

        [Test]
        public void ThenItShouldReturnAnInvalidResponseWithErrorIfAnInvalidEarningTypeIsFound()
        {
            // Arrange
            var invalidEarningType = "InvalidEarningType";

            _repository.Setup(r => r.GetProviderEarnings(Ukprn))
                .Returns(new[]
                {
                    new EarningEntity
                    {
                        CommitmentId = 1,
                        CommitmentVersionId = "C1",
                        AccountId = "1",
                        AccountVersionId = "A1",
                        Uln = 1,
                        LearnerRefNumber = "Lrn-001",
                        AimSequenceNumber = 1,
                        Ukprn = Ukprn,
                        EarningType = invalidEarningType,
                        Period1 = 0,
                        Period2 = 0,
                        Period3 = 0,
                        Period4 = 0,
                        Period5 = 0,
                        Period6 = 1000.00m,
                        Period7 = 0,
                        Period8 = 0,
                        Period9 = 0,
                        Period10 = 0,
                        Period11 = 0,
                        Period12 = 0,
                        StandardCode = 25
                    }
                });
            
            // Act
            var response = _handler.Handle(_request);

            // Assert
            Assert.IsFalse(response.IsValid);
            Assert.IsNull(response.Items);
            Assert.IsNotNull(response.Exception);
            Assert.AreEqual(typeof(InvalidEarningTypeException), response.Exception.GetType());
        }

        [Test]
        public void ThenItShouldReturnTheCorrectCourseInformation()
        {
            // Act
            var response = _handler.Handle(_request);

            // Assert
            Assert.IsNotNull(response);
            Assert.IsTrue(response.IsValid);
            Assert.IsNotNull(response.Items);
            Assert.AreEqual(16, response.Items.Length);

            Assert.AreEqual(7,
                response.Items.Count(
                    e =>
                        e.StandardCode == EarningEntities[0].StandardCode &&
                        e.FrameworkCode == EarningEntities[0].FrameworkCode &&
                        e.ProgrammeType == EarningEntities[0].ProgrammeType &&
                        e.PathwayCode == EarningEntities[0].PathwayCode &&
                        e.CommitmentId == EarningEntities[0].CommitmentId));

            Assert.AreEqual(9,
                response.Items.Count(
                    e =>
                        e.StandardCode == EarningEntities[1].StandardCode &&
                        e.FrameworkCode == EarningEntities[1].FrameworkCode &&
                        e.ProgrammeType == EarningEntities[1].ProgrammeType &&
                        e.PathwayCode == EarningEntities[1].PathwayCode &&
                        e.CommitmentId == EarningEntities[1].CommitmentId));
        }
    }
}