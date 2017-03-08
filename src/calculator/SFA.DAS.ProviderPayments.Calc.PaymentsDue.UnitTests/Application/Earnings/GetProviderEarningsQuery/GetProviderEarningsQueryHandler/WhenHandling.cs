﻿using System;
using System.Linq;
using Moq;
using NUnit.Framework;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Application.Earnings.GetProviderEarningsQuery;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data.Entities;
using SFA.DAS.Payments.DCFS.Domain;

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
                Period = 1,
                Amount = 1000m,
                TransactionType = (int)TransactionType.Learning,
                StandardCode = 25,
                PriceEpisodeEndDate = new DateTime(2018, 8, 1)
            },
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
                Period = 2,
                Amount = 1000m,
                TransactionType = (int)TransactionType.Learning,
                StandardCode = 25,
                PriceEpisodeEndDate = new DateTime(2018, 8, 1)
            },
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
                Period = 3,
                Amount = 1000m,
                TransactionType = (int)TransactionType.Learning,
                StandardCode = 25,
                PriceEpisodeEndDate = new DateTime(2018, 8, 1)
            },
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
                Period = 4,
                Amount = 1000m,
                TransactionType = (int)TransactionType.Learning,
                StandardCode = 25,
                PriceEpisodeEndDate = new DateTime(2018, 8, 1)
            },
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
                Period = 5,
                Amount = 1000m,
                TransactionType = (int)TransactionType.Learning,
                StandardCode = 25,
                PriceEpisodeEndDate = new DateTime(2018, 8, 1)
            },
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
                Period = 6,
                Amount = 1000m,
                TransactionType = (int)TransactionType.Learning,
                StandardCode = 25,
                PriceEpisodeEndDate = new DateTime(2018, 8, 1)
            },
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
                Period = 7,
                Amount = 1000m,
                TransactionType = (int)TransactionType.Learning,
                StandardCode = 25,
                PriceEpisodeEndDate = new DateTime(2018, 8, 1)
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
                Period = 1,
                Amount = 1000m,
                TransactionType = (int)TransactionType.Learning,
                FrameworkCode = 550,
                ProgrammeType = 20,
                PathwayCode = 6,
                PriceEpisodeEndDate = new DateTime(2018, 8, 1)
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
                Period = 2,
                Amount = 1000m,
                TransactionType = (int)TransactionType.Learning,
                FrameworkCode = 550,
                ProgrammeType = 20,
                PathwayCode = 6,
                PriceEpisodeEndDate = new DateTime(2018, 8, 1)
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
                Period = 3,
                Amount = 1000m,
                TransactionType = (int)TransactionType.Learning,
                FrameworkCode = 550,
                ProgrammeType = 20,
                PathwayCode = 6,
                PriceEpisodeEndDate = new DateTime(2018, 8, 1)
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
                Period = 4,
                Amount = 1000m,
                TransactionType = (int)TransactionType.Learning,
                FrameworkCode = 550,
                ProgrammeType = 20,
                PathwayCode = 6,
                PriceEpisodeEndDate = new DateTime(2018, 8, 1)
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
                Period = 5,
                Amount = 1000m,
                TransactionType = (int)TransactionType.Learning,
                FrameworkCode = 550,
                ProgrammeType = 20,
                PathwayCode = 6,
                PriceEpisodeEndDate = new DateTime(2018, 8, 1)
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
                Period = 6,
                Amount = 1000m,
                TransactionType = (int)TransactionType.Learning,
                FrameworkCode = 550,
                ProgrammeType = 20,
                PathwayCode = 6,
                PriceEpisodeEndDate = new DateTime(2018, 8, 1)
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
                Period = 7,
                Amount = 1000m,
                TransactionType = (int)TransactionType.Learning,
                FrameworkCode = 550,
                ProgrammeType = 20,
                PathwayCode = 6,
                PriceEpisodeEndDate = new DateTime(2018, 8, 1)
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
                Period = 8,
                Amount = 1000m,
                TransactionType = (int)TransactionType.Learning,
                FrameworkCode = 550,
                ProgrammeType = 20,
                PathwayCode = 6,
                PriceEpisodeEndDate = new DateTime(2018, 8, 1)
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
                Period = 9,
                Amount = 1000m,
                TransactionType = (int)TransactionType.Learning,
                FrameworkCode = 550,
                ProgrammeType = 20,
                PathwayCode = 6,
                PriceEpisodeEndDate = new DateTime(2018, 8, 1)
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
                AcademicYear = "1718",
                Period1Month = 8,
                Period1Year = 2017
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
        [TestCase(TransactionType.Learning)]
        [TestCase(TransactionType.Completion)]
        [TestCase(TransactionType.Balancing)]
        [TestCase(TransactionType.First16To18EmployerIncentive)]
        [TestCase(TransactionType.First16To18ProviderIncentive)]
        [TestCase(TransactionType.Second16To18EmployerIncentive)]
        [TestCase(TransactionType.Second16To18ProviderIncentive)]
        [TestCase(TransactionType.Balancing16To18FrameworkUplift)]
        [TestCase(TransactionType.Completion16To18FrameworkUplift)]
        [TestCase(TransactionType.OnProgramme16To18FrameworkUplift)]
        [TestCase(TransactionType.FirstDisadvantagePayment)]
        [TestCase(TransactionType.SecondDisadvantagePayment)]
        
        [TestCase(TransactionType.OnProgrammeMathsAndEnglish)]
        [TestCase(TransactionType.BalancingMathsAndEnglish)]
        [TestCase(TransactionType.LearningSupport)]
        public void ThenItShouldReturnEarningsWithTheCorrectTransactionType(TransactionType transactionType)
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
                        Period = 6,
                        PriceEpisodeEndDate=new DateTime(2019,10,10),
                        Amount = 1000m,
                        TransactionType = (int) transactionType,
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
            // Arrange
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
                        e.StandardCode == 25 &&
                        e.FrameworkCode == null &&
                        e.ProgrammeType == null &&
                        e.PathwayCode == null &&
                        e.CommitmentId == 1));

            Assert.AreEqual(9,
                response.Items.Count(
                    e =>
                        e.StandardCode == null &&
                        e.FrameworkCode == 550 &&
                        e.ProgrammeType == 20 &&
                        e.PathwayCode == 6 &&
                        e.CommitmentId == 2));
        }

        [Test]
        [TestCase(null, null, null, null, 2)]
        [TestCase(1L, "C1", "1", "A1", 1)]
        public void ThenItShouldReturnTheCorrectCommitmentAccountAndContractTypeInformation(long? commitmentId, string commitmentVersionId, string accountId, string accountVersionId, int apprenticeshipContractType)
        {
            // Arrange
            _repository.Setup(r => r.GetProviderEarnings(Ukprn))
                .Returns(new[]
                {
                    new EarningEntity
                    {
                        CommitmentId = commitmentId,
                        CommitmentVersionId = commitmentVersionId,
                        AccountId = accountId,
                        AccountVersionId = accountVersionId,
                        Uln = 1,
                        LearnerRefNumber = "Lrn-001",
                        AimSequenceNumber = 1,
                        Ukprn = Ukprn,
                        Period = 6,
                        Amount = 1000m,
                        TransactionType = (int)TransactionType.Completion,
                        StandardCode = 25,
                        ApprenticeshipContractType = apprenticeshipContractType,
                        PriceEpisodeEndDate = new DateTime(2018, 8, 1)
                    }
                });

            // Act
            var response = _handler.Handle(_request);

            // Assert
            Assert.AreEqual(1,
                response.Items.Count(
                    e =>
                        e.CommitmentId == commitmentId &&
                        e.CommitmentVersionId == commitmentVersionId &&
                        e.AccountId == accountId &&
                        e.AccountVersionId == accountVersionId &&
                        e.ApprenticeshipContractType == apprenticeshipContractType));
        }

        [Test]
        public void ThenItShouldReturnTheCorrectSfaContributionPercentage()
        {
            // Arrange
            _repository.Setup(r => r.GetProviderEarnings(Ukprn))
                .Returns(new[]
                {
                    new EarningEntity
                    {
                        CommitmentId = 1,
                        CommitmentVersionId = "1",
                        AccountId = "1",
                        AccountVersionId = "A1",
                        Uln = 1,
                        LearnerRefNumber = "Lrn-001",
                        AimSequenceNumber = 1,
                        Ukprn = Ukprn,
                        Period = 6,
                        Amount = 1000m,
                        TransactionType = (int)TransactionType.Completion,
                        StandardCode = 25,
                        ApprenticeshipContractType = 1,
                        PriceEpisodeSfaContribPct = 0.9m,
                        PriceEpisodeEndDate = new DateTime(2018, 8, 1)
                    }
                });

            // Act
            var response = _handler.Handle(_request);

            // Assert
            Assert.AreEqual(1, response.Items.Count(e => e.SfaContributionPercentage == 0.9m));
        }

        [Test]
        public void ThenItShouldReturnTheCorrectFundingLineType()
        {
            // Arrange
            _repository.Setup(r => r.GetProviderEarnings(Ukprn))
                .Returns(new[]
                {
                    new EarningEntity
                    {
                        CommitmentId = 1,
                        CommitmentVersionId = "1",
                        AccountId = "1",
                        AccountVersionId = "A1",
                        Uln = 1,
                        LearnerRefNumber = "Lrn-001",
                        AimSequenceNumber = 1,
                        Ukprn = Ukprn,
                        Period = 6,
                        Amount = 1000m,
                        TransactionType = (int)TransactionType.Completion,
                        StandardCode = 25,
                        ApprenticeshipContractType = 1,
                        PriceEpisodeSfaContribPct = 0.9m,
                        PriceEpisodeFundLineType = "Levy Funding Line Type",
                        PriceEpisodeEndDate = new DateTime(2018, 8, 1)
                    }
                });

            // Act
            var response = _handler.Handle(_request);

            // Assert
            Assert.AreEqual(1, response.Items.Count(e => e.FundingLineType == "Levy Funding Line Type"));
        }

        [Test]
        public void ThenItShouldNotReturnAnyEarningsForAPriceEpisodeThatEndedBeforeTheCurrentAcademicYear()
        {
            // Arrange
            _repository.Setup(r => r.GetProviderEarnings(Ukprn))
                .Returns(new[]
                {
                    new EarningEntity
                    {
                        CommitmentId = 1,
                        CommitmentVersionId = "1",
                        AccountId = "1",
                        AccountVersionId = "A1",
                        Uln = 1,
                        LearnerRefNumber = "Lrn-001",
                        AimSequenceNumber = 1,
                        Ukprn = Ukprn,
                        Period = 6,
                        Amount = 1000m,
                        TransactionType = (int)TransactionType.Learning,
                        StandardCode = 25,
                        ApprenticeshipContractType = 1,
                        PriceEpisodeSfaContribPct = 0.9m,
                        PriceEpisodeFundLineType = "Levy Funding Line Type",
                        PriceEpisodeEndDate = new DateTime(2017, 7, 31)
                    }
                });

            // Act
            var response = _handler.Handle(_request);

            // Assert
            Assert.AreEqual(0, response.Items.Length);
        }
    }
}