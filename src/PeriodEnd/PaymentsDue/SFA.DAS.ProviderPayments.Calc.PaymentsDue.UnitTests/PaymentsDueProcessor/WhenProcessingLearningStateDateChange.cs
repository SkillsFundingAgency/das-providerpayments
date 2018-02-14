using System;
using System.Linq;
using Moq;
using NUnit.Framework;
using SFA.DAS.Payments.DCFS.Domain;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Application.Earnings;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Application.Earnings.GetProviderEarningsQuery;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Application.RequiredPayments;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Application.RequiredPayments.AddRequiredPaymentsCommand;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Application.RequiredPayments.GetPaymentHistoryQuery;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.UnitTests.PaymentsDueProcessor
{
    public class WhenProcessingLearningStateDateChange : WhenProcessingValidScenarioBase
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
                Payable = true,
                LearningStartDate = new DateTime(2017, 08, 15)
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
                Payable = true,
                LearningStartDate = new DateTime(2017, 08, 15)
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
                Payable = true,
                LearningStartDate = new DateTime(2017, 08, 15)
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
        public void ThenItShouldNotOuputPaymentsDueForEarningsThatHaveBeenFullyPaidInPreviousRunsIfTheLearningStateDateIsDifferentButInTheSameMonth()
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
                            ApprenticeshipContractType = PeriodEarning1.ApprenticeshipContractType,
                            LearningStartDate = new DateTime(2017, 08, 05)
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
        public void ThenItShouldNotOuputPaymentsDueForEarningsThatHaveBeenFullyPaidInPreviousRunsIfTheLearningStateDateIsDifferentButInTheSameYear()
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
                            ApprenticeshipContractType = PeriodEarning1.ApprenticeshipContractType,
                            LearningStartDate = new DateTime(2017, 09, 15)
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
        public void ThenItShouldNotOuputPaymentsDueForEarningsThatHaveBeenFullyPaidInPreviousRunsIfTheLearningStateDateIsDifferentAcrossYears()
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
                            ApprenticeshipContractType = PeriodEarning1.ApprenticeshipContractType,
                            LearningStartDate = new DateTime(2018, 08, 15)
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
    }
}
