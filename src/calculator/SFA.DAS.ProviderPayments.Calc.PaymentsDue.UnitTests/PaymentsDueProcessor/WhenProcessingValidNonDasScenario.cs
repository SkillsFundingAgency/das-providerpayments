﻿using System.Linq;
using Moq;
using NUnit.Framework;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Application.CollectionPeriods;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Application.CollectionPeriods.GetCurrentCollectionPeriodQuery;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Application.Earnings;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Application.Earnings.GetProviderEarningsQuery;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Application.RequiredPayments;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Application.RequiredPayments.AddRequiredPaymentsCommand;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Application.RequiredPayments.GetPaymentHistoryQuery;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.UnitTests.PaymentsDueProcessor
{
    public class WhenProcessingValidNonDasScenario : WhenProcessingValidScenarioBase
    {
        protected override void ArrangeProviderEarnings()
        {
            PeriodEarning1 = new PeriodEarning
            {
                Ukprn = 1,
                LearnerReferenceNumber = "LEARNER-1",
                AimSequenceNumber = 1,
                CollectionPeriodNumber = 1,
                CollectionAcademicYear = "1718",
                CalendarMonth = 8,
                CalendarYear = 2017,
                EarnedValue = 1000m,
                Type = Common.Application.TransactionType.Learning,
                StandardCode = 25,
                ApprenticeshipContractType = 2
            };
            PeriodEarning2 = new PeriodEarning
            {
                Ukprn = 1,
                LearnerReferenceNumber = "LEARNER-1",
                AimSequenceNumber = 1,
                CollectionPeriodNumber = 2,
                CollectionAcademicYear = "1718",
                CalendarMonth = 9,
                CalendarYear = 2017,
                EarnedValue = 3000m,
                Type = Common.Application.TransactionType.Completion,
                StandardCode = 25,
                ApprenticeshipContractType = 2
            };
            PeriodEarning3 = new PeriodEarning
            {
                Ukprn = 1,
                LearnerReferenceNumber = "LEARNER-1",
                AimSequenceNumber = 1,
                CollectionPeriodNumber = 2,
                CollectionAcademicYear = "1718",
                CalendarMonth = 9,
                CalendarYear = 2017,
                EarnedValue = 2000m,
                Type = Common.Application.TransactionType.Balancing,
                StandardCode = 25,
                ApprenticeshipContractType = 2
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
    }
}