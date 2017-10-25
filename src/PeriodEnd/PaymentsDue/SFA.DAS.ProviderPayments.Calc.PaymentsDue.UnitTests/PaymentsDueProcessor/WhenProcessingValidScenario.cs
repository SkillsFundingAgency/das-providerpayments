using System;
using System.Linq;
using Moq;
using NUnit.Framework;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Application.CollectionPeriods;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Application.CollectionPeriods.GetCurrentCollectionPeriodQuery;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Application.Earnings;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Application.Earnings.GetProviderEarningsQuery;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Application.RequiredPayments;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Application.RequiredPayments.AddRequiredPaymentsCommand;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Application.RequiredPayments.GetPaymentHistoryQuery;
using SFA.DAS.Payments.DCFS.Domain;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Application.RequiredPayments.GetPaymentHistoryWhereNoEarningQuery;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.UnitTests.PaymentsDueProcessor
{
    public class WhenProcessingValidScenario : WhenProcessingValidScenarioBase
    {
        protected override void ArrangeProviderEarnings()
        {
            PeriodEarning1 = new PeriodEarning
            {
                CommitmentId = 1,
                Ukprn = 1,
                LearnerReferenceNumber = "LEARNER-1",
                AimSequenceNumber = 1,
                CollectionPeriodNumber = 1,
                CollectionAcademicYear = "1718",
                CalendarMonth = 8,
                CalendarYear = 2017,
                EarnedValue = 1000m,
                Type = TransactionType.Learning,
                StandardCode = 25,
                ApprenticeshipContractType = 1,
                SfaContributionPercentage = 0.9m,
                FundingLineType = "Levy Funding Line Type",
                IsSuccess = true,
                Payable = true
            };
            PeriodEarning2 = new PeriodEarning
            {
                CommitmentId = 1,
                Ukprn = 1,
                LearnerReferenceNumber = "LEARNER-1",
                AimSequenceNumber = 1,
                CollectionPeriodNumber = 2,
                CollectionAcademicYear = "1718",
                CalendarMonth = 9,
                CalendarYear = 2017,
                EarnedValue = 3000m,
                Type = TransactionType.Completion,
                StandardCode = 25,
                ApprenticeshipContractType = 1,
                SfaContributionPercentage = 0.75m,
                FundingLineType = "Levy Funding Line Type",
                IsSuccess = true,
                Payable = true
            };
            PeriodEarning3 = new PeriodEarning
            {
                CommitmentId = 1,
                Ukprn = 1,
                LearnerReferenceNumber = "LEARNER-1",
                AimSequenceNumber = 1,
                CollectionPeriodNumber = 2,
                CollectionAcademicYear = "1718",
                CalendarMonth = 9,
                CalendarYear = 2017,
                EarnedValue = 2000m,
                Type = TransactionType.Balancing,
                StandardCode = 25,
                ApprenticeshipContractType = 1,
                SfaContributionPercentage = 0.9m,
                FundingLineType = "Levy Funding Line Type",
                IsSuccess = true,
                Payable = true
            };
            Mediator
                .Setup(m => m.Send(It.IsAny<GetProviderEarningsQueryRequest>()))
                .Returns(new GetProviderEarningsQueryResponse
                {
                    IsValid = true,
                    Items = new[]
                    {
                        PeriodEarning1,
                        PeriodEarning2,
                        PeriodEarning3
                    }
                });
        }

        [Test]
        public void ThenItShouldOutputPaymentsDueForEarningsThatHaveNoPaymentsFromPreviousRuns()
        {
            // Act
            Processor.Process();

            // Assert
            Mediator.Verify(m => m.Send(It.Is<AddRequiredPaymentsCommandRequest>(
                request => request.Payments.Length == 3)), Times.Once, "Expected only 3 payments");
            Mediator.Verify(m => m.Send(It.Is<AddRequiredPaymentsCommandRequest>(
                request => request.Payments.Any(p => PaymentForEarning(p, PeriodEarning1, PeriodEarning1.EarnedValue)))), Times.Once, "Expected a payment for earning 1");
            Mediator.Verify(m => m.Send(It.Is<AddRequiredPaymentsCommandRequest>(
                request => request.Payments.Any(p => PaymentForEarning(p, PeriodEarning2, PeriodEarning2.EarnedValue)))), Times.Once, "Expected a payment for earning 2");
            Mediator.Verify(m => m.Send(It.Is<AddRequiredPaymentsCommandRequest>(
                request => request.Payments.Any(p => PaymentForEarning(p, PeriodEarning3, PeriodEarning3.EarnedValue)))), Times.Once, "Expected a payment for earning 3");
        }

        [Test]
        public void ThenItShouldNotOuputPaymentDueWhenNothingHasBeenEarned()
        {
            // Arrange
            Mediator.Setup(m => m.Send(It.IsAny<GetProviderEarningsQueryRequest>()))
                .Returns(new GetProviderEarningsQueryResponse
                {
                    IsValid = true,
                    Items = new PeriodEarning[0]
                });

            // Act
            Processor.Process();

            // Assert
            Mediator.Verify(m => m.Send(It.IsAny<AddRequiredPaymentsCommandRequest>()), Times.Never);
        }

        [Test]
        public void ThenItShouldOnlyOuputPaymentsDueForEarningsUpToAndIncludingTheCurrentPeriod()
        {
            // Arrange
            Mediator
                .Setup(m => m.Send(It.IsAny<GetCurrentCollectionPeriodQueryRequest>()))
                .Returns(new GetCurrentCollectionPeriodQueryResponse
                {
                    IsValid = true,
                    Period = new CollectionPeriod { PeriodId = 1, Month = 8, Year = 2017 }
                });

            // Act
            Processor.Process();

            // Assert
            Mediator.Verify(m => m.Send(It.Is<AddRequiredPaymentsCommandRequest>(
                request => request.Payments.Length == 1)), Times.Once, "Expected only 1 payment");
            Mediator.Verify(m => m.Send(It.Is<AddRequiredPaymentsCommandRequest>(
                request => request.Payments.Any(p => PaymentForEarning(p, PeriodEarning1, PeriodEarning1.EarnedValue)))), Times.Once, "Expected a payment for earning 1");
            Mediator.Verify(m => m.Send(It.Is<AddRequiredPaymentsCommandRequest>(
                request => request.Payments.Any(p => PaymentForEarning(p, PeriodEarning2, PeriodEarning2.EarnedValue)))), Times.Never, "Expected not to have a payment for earning 2");
            Mediator.Verify(m => m.Send(It.Is<AddRequiredPaymentsCommandRequest>(
                request => request.Payments.Any(p => PaymentForEarning(p, PeriodEarning3, PeriodEarning3.EarnedValue)))), Times.Never, "Expected not to have a payment for earning 3");
        }

        [TestCase(1, 8, 2017, 8, 2017)]
        [TestCase(2, 9, 2017, 8, 2017)]
        [TestCase(5, 4, 2018, 12, 2017)]
        [TestCase(5, 5, 2018, 1, 2018)]
        public void ThenItShouldAdjustPeriodWhenSendingGetProviderEarningsQueryRequest(int periodNumber, int collectionMonth, int collectionYear,
            int expectedPeriod1Month, int expectedPeriod1Year)
        {
            // Arrange
            Mediator
                .Setup(m => m.Send(It.IsAny<GetCurrentCollectionPeriodQueryRequest>()))
                .Returns(new GetCurrentCollectionPeriodQueryResponse
                {
                    IsValid = true,
                    Period = new CollectionPeriod { PeriodId = 1, Month = collectionMonth, Year = collectionYear, PeriodNumber = periodNumber }
                });

            // Act
            Processor.Process();

            // Assert
            Mediator.Verify(m => m.Send(It.Is<GetProviderEarningsQueryRequest>(r => r.AcademicYear == expectedPeriod1Year.ToString())), Times.Never);
        }

        [Test]
        public void ThenItShouldNotOuputPaymentsDueForEarningsThatHaveBeenFullyPaidInPreviousRuns()
        {
            // Arrange
            Mediator.Setup(m => m.Send(It.IsAny<GetPaymentHistoryQueryRequest>()))
                .Returns(new GetPaymentHistoryQueryResponse
                {
                    IsValid = true,
                    Items = new[]
                    {
                        new RequiredPayment
                        {
                            CommitmentId = PeriodEarning1.CommitmentId,
                            Ukprn = PeriodEarning1.Ukprn,
                            LearnerRefNumber = PeriodEarning1.LearnerReferenceNumber,
                            AimSequenceNumber = PeriodEarning1.AimSequenceNumber,
                            DeliveryMonth = PeriodEarning1.CalendarMonth,
                            DeliveryYear = PeriodEarning1.CalendarYear,
                            AmountDue = PeriodEarning1.EarnedValue,
                            TransactionType = PeriodEarning1.Type,
                            Uln = PeriodEarning1.Uln,
                            StandardCode = PeriodEarning1.StandardCode,
                            FrameworkCode = PeriodEarning1.FrameworkCode,
                            ProgrammeType = PeriodEarning1.ProgrammeType,
                            PathwayCode = PeriodEarning1.PathwayCode,
                            ApprenticeshipContractType = PeriodEarning1.ApprenticeshipContractType
                        }
                    }
                });

            // Act
            Processor.Process();

            // Assert
            Mediator.Verify(m => m.Send(It.Is<AddRequiredPaymentsCommandRequest>(
                request => request.Payments.Length == 2)), Times.Once, "Expected only 2 payments");
            Mediator.Verify(m => m.Send(It.Is<AddRequiredPaymentsCommandRequest>(
                request => request.Payments.Any(p => PaymentForEarning(p, PeriodEarning1, PeriodEarning1.EarnedValue)))), Times.Never, "Expected no payment for earning 1");
            Mediator.Verify(m => m.Send(It.Is<AddRequiredPaymentsCommandRequest>(
                request => request.Payments.Any(p => PaymentForEarning(p, PeriodEarning2, PeriodEarning2.EarnedValue)))), Times.Once, "Expected a payment for earning 2");
            Mediator.Verify(m => m.Send(It.Is<AddRequiredPaymentsCommandRequest>(
                request => request.Payments.Any(p => PaymentForEarning(p, PeriodEarning3, PeriodEarning3.EarnedValue)))), Times.Once, "Expected a payment for earning 3");
        }

        [Test]
        public void ThenItShouldOutputPartPaymentsDueForEarningsThatHavePaymentsThatDoNotCoverTheFullAmountInPreviousRuns()
        {
            // Arrange
            Mediator.Setup(m => m.Send(It.IsAny<GetPaymentHistoryQueryRequest>()))
                .Returns(new GetPaymentHistoryQueryResponse
                {
                    IsValid = true,
                    Items = new[]
                    {
                        new RequiredPayment
                        {
                            CommitmentId = PeriodEarning1.CommitmentId,
                            Ukprn = PeriodEarning1.Ukprn,
                            LearnerRefNumber = PeriodEarning1.LearnerReferenceNumber,
                            AimSequenceNumber = PeriodEarning1.AimSequenceNumber,
                            DeliveryMonth = PeriodEarning1.CalendarMonth,
                            DeliveryYear = PeriodEarning1.CalendarYear,
                            AmountDue = PeriodEarning1.EarnedValue - 100,
                            TransactionType = PeriodEarning1.Type,
                            Uln = PeriodEarning1.Uln,
                            StandardCode = PeriodEarning1.StandardCode,
                            FrameworkCode = PeriodEarning1.FrameworkCode,
                            ProgrammeType = PeriodEarning1.ProgrammeType,
                            PathwayCode = PeriodEarning1.PathwayCode,
                            ApprenticeshipContractType = PeriodEarning1.ApprenticeshipContractType
                        }
                    }
                });

            // Act
            Processor.Process();

            // Assert
            Mediator.Verify(m => m.Send(It.Is<AddRequiredPaymentsCommandRequest>(
                request => request.Payments.Length == 3)), Times.Once, "Expected only 3 payments");
            Mediator.Verify(m => m.Send(It.Is<AddRequiredPaymentsCommandRequest>(
                request => request.Payments.Any(p => PaymentForEarning(p, PeriodEarning1, 100m)))), Times.Once, "Expected a payment for earning 1");
            Mediator.Verify(m => m.Send(It.Is<AddRequiredPaymentsCommandRequest>(
                request => request.Payments.Any(p => PaymentForEarning(p, PeriodEarning2, PeriodEarning2.EarnedValue)))), Times.Once, "Expected a payment for earning 2");
            Mediator.Verify(m => m.Send(It.Is<AddRequiredPaymentsCommandRequest>(
                request => request.Payments.Any(p => PaymentForEarning(p, PeriodEarning3, PeriodEarning3.EarnedValue)))), Times.Once, "Expected a payment for earning 3");
        }

        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        [TestCase(10)]
        [TestCase(11)]
        [TestCase(12)]
        [TestCase(13)]
        [TestCase(14)]
        public void ThenItShouldCorrectCalculateTheFirstPeriod(int periodNumber)
        {
            //Arrange
            var date = (new DateTime(2016, 8, 1)).AddMonths((periodNumber <= 12 ? periodNumber : 12) - 1);

            Mediator
                .Setup(m => m.Send(It.IsAny<GetCurrentCollectionPeriodQueryRequest>()))
                .Returns(new GetCurrentCollectionPeriodQueryResponse
                {
                    IsValid = true,
                    Period = new CollectionPeriod { PeriodId = 1, Month = date.Month, Year = date.Year, PeriodNumber = periodNumber }
                });

            // Act
            Processor.Process();

            // Assert
            Mediator.Verify(m => m.Send(It.Is<GetProviderEarningsQueryRequest>(r => r.AcademicYear == string.Empty)), Times.Never);
        }

        [Test]
        public void ThenItShouldWriteTheCorrectRequestedPaymentForAPeriodEarningWithNoPaymentHistory()
        {
            // Arrange
            var periodEarning = new PeriodEarning
            {
                CommitmentId = 1,
                CommitmentVersionId = "1",
                AccountId = "1",
                AccountVersionId = "A1",
                Ukprn = 1,
                Uln = 123456,
                LearnerReferenceNumber = "Lrn-001",
                AimSequenceNumber = 1,
                CollectionPeriodNumber = 1,
                CollectionAcademicYear = "1718",
                CalendarMonth = 8,
                CalendarYear = 2017,
                EarnedValue = 1000m,
                Type = TransactionType.Learning,
                StandardCode = 25,
                ApprenticeshipContractType = 1,
                PriceEpisodeIdentifier = "25-25-01/08/2017",
                SfaContributionPercentage = 0.9m,
                FundingLineType = "Levy Funding Line",
                IsSuccess = true,
                Payable = true
            };

            Mediator
                .Setup(m => m.Send(It.IsAny<GetProviderEarningsQueryRequest>()))
                .Returns(new GetProviderEarningsQueryResponse
                {
                    IsValid = true,
                    Items = new[]
                    {
                        periodEarning
                    }
                });

            // Act
            Processor.Process();

            // Assert
            Mediator.Verify(m => m.Send(It.Is<AddRequiredPaymentsCommandRequest>(
                request => request.Payments.Length == 1)), Times.Once, "Expected only 1 required payment");
            Mediator.Verify(m => m.Send(It.Is<AddRequiredPaymentsCommandRequest>(
                request => request.Payments.Any(p => PaymentForEarning(p, periodEarning, periodEarning.EarnedValue)))), Times.Once);
        }

        [Test]
        public void ThenItShouldOutputPaymentsDueForHistoricalPaymentsThatNoLongerHaveEarnings()
        {
            // Arrange
            Mediator.Setup(m => m.Send(It.IsAny<GetProviderEarningsQueryRequest>()))
                .Returns(new GetProviderEarningsQueryResponse
                {
                    IsValid = true,
                    Items = new PeriodEarning[0]
                });
            Mediator.Setup(m => m.Send(It.IsAny<GetPaymentHistoryWhereNoEarningQueryRequest>()))
                .Returns(new GetPaymentHistoryWhereNoEarningQueryResponse
                {
                    IsValid = true,
                    Items = new[]
                    {
                        new RequiredPayment
                        {
                            CommitmentId = 2,
                            CommitmentVersionId = "2",
                            AccountId = "2",
                            AccountVersionId = "B2",
                            Ukprn = 2,
                            Uln = 987654321,
                            LearnerRefNumber = "LRN-002",
                            AimSequenceNumber = 1,
                            AmountDue = 1000,
                            DeliveryMonth = 8,
                            DeliveryYear = 2016
                        }
                    }
                });

            // Act
            Processor.Process();

            // Assert
            Mediator.Verify(m => m.Send(It.Is<AddRequiredPaymentsCommandRequest>(
                request => request.Payments.Length == 1)), Times.Once, "Expected only 1 required payment");
            Mediator.Verify(m => m.Send(It.Is<AddRequiredPaymentsCommandRequest>(
                request => request.Payments[0].AmountDue == -1000)), Times.Once, "Expected payment to be for -1000");
            Mediator.Verify(m => m.Send(It.Is<AddRequiredPaymentsCommandRequest>(
                request => request.Payments[0].DeliveryMonth == 8)), Times.Once, "Expected payment to be for delivery month 8");
            Mediator.Verify(m => m.Send(It.Is<AddRequiredPaymentsCommandRequest>(
                request => request.Payments[0].DeliveryYear == 2016)), Times.Once, "Expected payment to be for delivery year 2016");
        }
    }
}