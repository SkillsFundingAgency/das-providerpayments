using System;
using System.Collections.Generic;
using System.Linq;
using CS.Common.External.Interfaces;
using MediatR;
using Moq;
using NLog;
using NUnit.Framework;
using SFA.DAS.ProviderPayments.Calc.Common.Context;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Application.CollectionPeriods;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Application.CollectionPeriods.GetCurrentCollectionPeriodQuery;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Application.Earnings;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Application.Earnings.GetProviderEarningsQuery;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Application.Providers;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Application.Providers.GetProvidersQuery;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Application.RequiredPayments;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Application.RequiredPayments.AddRequiredPaymentsCommand;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Application.RequiredPayments.GetPaymentHistoryQuery;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.UnitTests.PaymentsDueProcessor
{
    public class WhenProcessingValidScenario
    {
        private PaymentsDue.PaymentsDueProcessor _processor;
        private Mock<ILogger> _logger;
        private Mock<IMediator> _mediator;
        private PeriodEarning _periodEarning1;
        private PeriodEarning _periodEarning2;
        private Mock<IExternalContext> _externalContext;

        [SetUp]
        public void Arrange()
        {
            _logger = new Mock<ILogger>();

            _mediator = new Mock<IMediator>();
            ArrangeCurrentCollectionPeriod();
            ArrangeProviders();
            ArrangeProviderEarnings();
            ArrangePaymentHistory();
            _mediator.Setup(m => m.Send(It.IsAny<AddRequiredPaymentsCommandRequest>()))
                .Returns(new AddRequiredPaymentsCommandResponse { IsValid = true });

            _externalContext = new Mock<IExternalContext>();
            _externalContext.Setup(c => c.Properties)
                .Returns(new Dictionary<string, string>
                {
                    { ContextPropertyKeys.TransientDatabaseConnectionString, "" },
                    { ContextPropertyKeys.LogLevel, "DEBUG" },
                    { ContextPropertyKeys.YearOfCollection, "1718" }
                });

            _processor = new PaymentsDue.PaymentsDueProcessor(_logger.Object, _mediator.Object, new Payments.DCFS.Context.ContextWrapper(_externalContext.Object));
        }

        private void ArrangeCurrentCollectionPeriod()
        {
            _mediator
                .Setup(m => m.Send(It.IsAny<GetCurrentCollectionPeriodQueryRequest>()))
                .Returns(new GetCurrentCollectionPeriodQueryResponse
                {
                    IsValid = true,
                    Period = new CollectionPeriod { PeriodId = 1, Month = 9, Year = 2017, PeriodNumber = 1 }
                });
        }
        private void ArrangeProviders()
        {
            _mediator
                .Setup(m => m.Send(It.IsAny<GetProvidersQueryRequest>()))
                .Returns(new GetProvidersQueryResponse
                {
                    IsValid = true,
                    Items = new[] { new Provider { Ukprn = 10007459 } }
                });
        }
        private void ArrangeProviderEarnings()
        {
            _periodEarning1 = new PeriodEarning
            {
                CommitmentId = "COMMITMENT-1",
                Ukprn = 1,
                LearnerReferenceNumber = "LEARNER-1",
                AimSequenceNumber = 1,
                CollectionPeriodNumber = 1,
                CollectionAcademicYear = "1718",
                CalendarMonth = 8,
                CalendarYear = 2017,
                EarnedValue = 1000m,
                Type = Common.Application.TransactionType.Learning
            };
            _periodEarning2 = new PeriodEarning
            {
                CommitmentId = "COMMITMENT-1",
                Ukprn = 1,
                LearnerReferenceNumber = "LEARNER-1",
                AimSequenceNumber = 1,
                CollectionPeriodNumber = 2,
                CollectionAcademicYear = "1718",
                CalendarMonth = 9,
                CalendarYear = 2017,
                EarnedValue = 3000m,
                Type = Common.Application.TransactionType.Completion
            };
            _mediator
                .Setup(m => m.Send(It.IsAny<GetProviderEarningsQueryRequest>()))
                .Returns(new GetProviderEarningsQueryResponse
                {
                    IsValid = true,
                    Items = new[]
                    {
                        _periodEarning1,
                        _periodEarning2
                    }
                });
        }
        private void ArrangePaymentHistory()
        {
            _mediator.Setup(m => m.Send(It.IsAny<GetPaymentHistoryQueryRequest>()))
                .Returns(new GetPaymentHistoryQueryResponse
                {
                    IsValid = true,
                    Items = new RequiredPayment[0]
                });
        }


        [Test]
        public void ThenItShouldOutputPaymentsDueForEarningsThatHaveNoPaymentsFromPreviousRuns()
        {
            // Act
            _processor.Process();

            // Assert
            _mediator.Verify(m => m.Send(It.Is<AddRequiredPaymentsCommandRequest>(
                request => request.Payments.Length == 2)), Times.Once, "Expected only 2 payments");
            _mediator.Verify(m => m.Send(It.Is<AddRequiredPaymentsCommandRequest>(
                request => request.Payments.Any(p => PaymentForEarning(p, _periodEarning1, _periodEarning1.EarnedValue)))), Times.Once, "Expected a payment for earning 1");
            _mediator.Verify(m => m.Send(It.Is<AddRequiredPaymentsCommandRequest>(
                request => request.Payments.Any(p => PaymentForEarning(p, _periodEarning2, _periodEarning2.EarnedValue)))), Times.Once, "Expected a payment for earning 2");
        }

        [Test]
        public void ThenItShouldNotOuputPaymentDueWhenNothingHasBeenEarned()
        {
            // Arrange
            _mediator.Setup(m => m.Send(It.IsAny<GetProviderEarningsQueryRequest>()))
                .Returns(new GetProviderEarningsQueryResponse
                {
                    IsValid = true,
                    Items = new PeriodEarning[0]
                });

            // Act
            _processor.Process();

            // Assert
            _mediator.Verify(m => m.Send(It.IsAny<AddRequiredPaymentsCommandRequest>()), Times.Never);
        }

        [Test]
        public void ThenItShouldOnlyOuputPaymentsDueForEarningsUpToAndIncludingTheCurrentPeriod()
        {
            // Arrange
            _mediator
                .Setup(m => m.Send(It.IsAny<GetCurrentCollectionPeriodQueryRequest>()))
                .Returns(new GetCurrentCollectionPeriodQueryResponse
                {
                    IsValid = true,
                    Period = new CollectionPeriod { PeriodId = 1, Month = 8, Year = 2017 }
                });

            // Act
            _processor.Process();

            // Assert
            _mediator.Verify(m => m.Send(It.Is<AddRequiredPaymentsCommandRequest>(
                request => request.Payments.Length == 1)), Times.Once, "Expected only 1 payment");
            _mediator.Verify(m => m.Send(It.Is<AddRequiredPaymentsCommandRequest>(
                request => request.Payments.Any(p => PaymentForEarning(p, _periodEarning1, _periodEarning1.EarnedValue)))), Times.Once, "Expected a payment for earning 1");
            _mediator.Verify(m => m.Send(It.Is<AddRequiredPaymentsCommandRequest>(
                request => request.Payments.Any(p => PaymentForEarning(p, _periodEarning2, _periodEarning2.EarnedValue)))), Times.Never, "Expected not to have a payment for earning 2");
        }

        [TestCase(1, 8, 2017, 8, 2017)]
        [TestCase(2, 9, 2017, 8, 2017)]
        [TestCase(5, 4, 2018, 12, 2017)]
        [TestCase(5, 5, 2018, 1, 2018)]
        public void ThenItShouldAdjustPeriodWhenSendingGetProviderEarningsQueryRequest(int periodNumber, int collectionMonth, int collectionYear,
            int expectedPeriod1Month, int expectedPeriod1Year)
        {
            // Arrange
            _mediator
                .Setup(m => m.Send(It.IsAny<GetCurrentCollectionPeriodQueryRequest>()))
                .Returns(new GetCurrentCollectionPeriodQueryResponse
                {
                    IsValid = true,
                    Period = new CollectionPeriod { PeriodId = 1, Month = collectionMonth, Year = collectionYear, PeriodNumber = periodNumber }
                });

            // Act
            _processor.Process();

            // Assert
            _mediator.Verify(m => m.Send(It.Is<GetProviderEarningsQueryRequest>(r => r.Period1Month == expectedPeriod1Month && r.Period1Year == expectedPeriod1Year)), Times.Once);
        }

        [Test]
        public void ThenItShouldNotOuputPaymentsDueForEarningsThatHaveBeenFullyPaidInPreviousRuns()
        {
            // Arrange
            _mediator.Setup(m => m.Send(It.IsAny<GetPaymentHistoryQueryRequest>()))
                .Returns(new GetPaymentHistoryQueryResponse
                {
                    IsValid = true,
                    Items = new[]
                    {
                        new RequiredPayment
                        {
                            CommitmentId = _periodEarning1.CommitmentId,
                            Ukprn = _periodEarning1.Ukprn,
                            LearnerRefNumber = _periodEarning1.LearnerReferenceNumber,
                            AimSequenceNumber = _periodEarning1.AimSequenceNumber,
                            DeliveryMonth = _periodEarning1.CalendarMonth,
                            DeliveryYear = _periodEarning1.CalendarYear,
                            AmountDue = _periodEarning1.EarnedValue
                        }
                    }
                });

            // Act
            _processor.Process();

            // Assert
            _mediator.Verify(m => m.Send(It.Is<AddRequiredPaymentsCommandRequest>(
                request => request.Payments.Length == 1)), Times.Once, "Expected only 1 payment");
            _mediator.Verify(m => m.Send(It.Is<AddRequiredPaymentsCommandRequest>(
                request => request.Payments.Any(p => PaymentForEarning(p, _periodEarning1, _periodEarning1.EarnedValue)))), Times.Never, "Expected no payment for earning 1");
            _mediator.Verify(m => m.Send(It.Is<AddRequiredPaymentsCommandRequest>(
                request => request.Payments.Any(p => PaymentForEarning(p, _periodEarning2, _periodEarning2.EarnedValue)))), Times.Once, "Expected a payment for earning 2");
        }

        [Test]
        public void ThenItShouldOutputPartPaymentsDueForEarningsThatHavePaymentsThatDoNotCoverTheFullAmountInPreviousRuns()
        {
            // Arrange
            _mediator.Setup(m => m.Send(It.IsAny<GetPaymentHistoryQueryRequest>()))
                .Returns(new GetPaymentHistoryQueryResponse
                {
                    IsValid = true,
                    Items = new[]
                    {
                        new RequiredPayment
                        {
                            CommitmentId = _periodEarning1.CommitmentId,
                            Ukprn = _periodEarning1.Ukprn,
                            LearnerRefNumber = _periodEarning1.LearnerReferenceNumber,
                            AimSequenceNumber = _periodEarning1.AimSequenceNumber,
                            DeliveryMonth = _periodEarning1.CalendarMonth,
                            DeliveryYear = _periodEarning1.CalendarYear,
                            AmountDue = _periodEarning1.EarnedValue - 100
                        }
                    }
                });

            // Act
            _processor.Process();

            // Assert
            _mediator.Verify(m => m.Send(It.Is<AddRequiredPaymentsCommandRequest>(
                request => request.Payments.Length == 2)), Times.Once, "Expected only 2 payments");
            _mediator.Verify(m => m.Send(It.Is<AddRequiredPaymentsCommandRequest>(
                request => request.Payments.Any(p => PaymentForEarning(p, _periodEarning1, 100m)))), Times.Once, "Expected a payment for earning 1");
            _mediator.Verify(m => m.Send(It.Is<AddRequiredPaymentsCommandRequest>(
                request => request.Payments.Any(p => PaymentForEarning(p, _periodEarning2, _periodEarning2.EarnedValue)))), Times.Once, "Expected a payment for earning 2");
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

            _mediator
                .Setup(m => m.Send(It.IsAny<GetCurrentCollectionPeriodQueryRequest>()))
                .Returns(new GetCurrentCollectionPeriodQueryResponse
                {
                    IsValid = true,
                    Period = new CollectionPeriod { PeriodId = 1, Month = date.Month, Year = date.Year, PeriodNumber = periodNumber }
                });

            // Act
            _processor.Process();

            // Assert
            _mediator.Verify(m => m.Send(It.Is<GetProviderEarningsQueryRequest>(r => r.Period1Month == 8 && r.Period1Year == 2016)), Times.Once);
        }



        private bool PaymentForEarning(RequiredPayment payment, PeriodEarning earning, decimal expectedAmountDue)
        {
            if (payment.CommitmentId != earning.CommitmentId
                || payment.Ukprn != earning.Ukprn
                || payment.LearnerRefNumber != earning.LearnerReferenceNumber
                || payment.AimSequenceNumber != earning.AimSequenceNumber
                || payment.DeliveryMonth != earning.CalendarMonth
                || payment.DeliveryYear != earning.CalendarYear
                || payment.AmountDue != expectedAmountDue
                || (int)payment.TransactionType != (int)earning.Type)
            {
                return false;
            }
            return true;
        }
    }
}