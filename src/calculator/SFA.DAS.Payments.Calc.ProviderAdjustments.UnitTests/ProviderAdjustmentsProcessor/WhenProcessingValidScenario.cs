using System.Linq;
using MediatR;
using Moq;
using NLog;
using NUnit.Framework;
using SFA.DAS.Payments.Calc.ProviderAdjustments.Application.Adjustments;
using SFA.DAS.Payments.Calc.ProviderAdjustments.Application.Adjustments.GetCurrentAdjustmentsQuery;
using SFA.DAS.Payments.Calc.ProviderAdjustments.Application.Adjustments.GetPreviousAdjustmentsQuery;
using SFA.DAS.Payments.Calc.ProviderAdjustments.Application.CollectionPeriods;
using SFA.DAS.Payments.Calc.ProviderAdjustments.Application.CollectionPeriods.GetCurrentCollectionPeriodQuery;
using SFA.DAS.Payments.Calc.ProviderAdjustments.Application.Payments;
using SFA.DAS.Payments.Calc.ProviderAdjustments.Application.Payments.AddPaymentsCommand;
using SFA.DAS.Payments.Calc.ProviderAdjustments.Application.Providers;
using SFA.DAS.Payments.Calc.ProviderAdjustments.Application.Providers.GetProvidersQuery;

namespace SFA.DAS.Payments.Calc.ProviderAdjustments.UnitTests.ProviderAdjustmentsProcessor
{
    public class WhenProcessingValidScenario
    {
        private static readonly long Ukprn = 10007459;

        private static readonly CollectionPeriod CurrentPeriod = new CollectionPeriod
        {
            PeriodId = 1,
            Month = 4,
            Year = 2017,
            Name = "R09"
        };

        private static readonly object[] EmptyAdjustments =
        {
            new object[] { new Adjustment[0] },
            new object[] { null }
        };

        private static readonly Adjustment[] CurrentAdjustments =
        {
            new Adjustment
            {
                Ukprn = Ukprn,
                SubmissionId = "abc",
                SubmissionCollectionPeriod = 8,
                PaymentType = 1,
                PaymentTypeName = "adjustment",
                Amount = 150.00m
            },
            new Adjustment
            {
                Ukprn = Ukprn,
                SubmissionId = "abc",
                SubmissionCollectionPeriod = 9,
                PaymentType = 1,
                PaymentTypeName = "adjustment",
                Amount = -75.00m
            }
        };

        private static readonly Adjustment[] PreviousAdjustments =
        {
            new Adjustment
            {
                Ukprn = Ukprn,
                SubmissionId = "qwe",
                SubmissionCollectionPeriod = 8,
                PaymentType = 1,
                PaymentTypeName = "adjustment",
                Amount = 100.00m
            }
        };

        private Mock<ILogger> _logger;
        private Mock<IMediator> _mediator;
        private string _yearOfCollection = "1617";
        private ProviderAdjustments.ProviderAdjustmentsProcessor _processor;

        [SetUp]
        public void Arrange()
        {
            _logger = new Mock<ILogger>();
            _mediator = new Mock<IMediator>();

            _processor = new ProviderAdjustments.ProviderAdjustmentsProcessor(_logger.Object, _mediator.Object, _yearOfCollection);

            InitialMockSetup();
        }

        private void InitialMockSetup()
        {
            _mediator
               .Setup(m => m.Send(It.IsAny<GetCurrentCollectionPeriodQueryRequest>()))
               .Returns(new GetCurrentCollectionPeriodQueryResponse
               {
                   IsValid = true,
                   Period = CurrentPeriod
               });

            _mediator
                .Setup(m => m.Send(It.IsAny<GetProvidersQueryRequest>()))
                .Returns(new GetProvidersQueryResponse
                {
                    IsValid = true,
                    Items = new[] { new Provider { Ukprn = Ukprn } }
                });

            _mediator
                .Setup(m => m.Send(It.IsAny<GetCurrentAdjustmentsQueryRequest>()))
                .Returns(new GetCurrentAdjustmentsQueryResponse
                {
                    IsValid = true,
                    Items = CurrentAdjustments
                });

            _mediator
                .Setup(m => m.Send(It.IsAny<GetPreviousAdjustmentsQueryRequest>()))
                .Returns(new GetPreviousAdjustmentsQueryResponse
                {
                    IsValid = true,
                    Items = PreviousAdjustments
                });

            _mediator
                .Setup(m => m.Send(It.IsAny<AddPaymentsCommandRequest>()))
                .Returns(Unit.Value);
        }

        [Test]
        [TestCaseSource(nameof(EmptyAdjustments))]
        public void ThenNoPaymentsAreMareWhenNoCurrentOrPreviousAdjustmentsAreFound(Adjustment[] emptyAdjustments)
        {
            // Arrange
            _mediator
                .Setup(m => m.Send(It.IsAny<GetCurrentAdjustmentsQueryRequest>()))
                .Returns(new GetCurrentAdjustmentsQueryResponse
                {
                    IsValid = true,
                    Items = emptyAdjustments
                });

            _mediator
                .Setup(m => m.Send(It.IsAny<GetPreviousAdjustmentsQueryRequest>()))
                .Returns(new GetPreviousAdjustmentsQueryResponse
                {
                    IsValid = true,
                    Items = emptyAdjustments
                });

            // Act
            _processor.Process();

            // Assert
            _mediator.Verify(m => m.Send(
                It.Is<AddPaymentsCommandRequest>(r => r.Payments.Length == 0)), Times.Once, "Expecting no payments.");
        }

        [Test]
        [TestCaseSource(nameof(EmptyAdjustments))]
        public void ThenNoPaymentsAreMareWhenNoPreviousAdjustmentsAreFound(Adjustment[] emptyAdjustments)
        {
            // Arrange
            _mediator
                .Setup(m => m.Send(It.IsAny<GetCurrentAdjustmentsQueryRequest>()))
                .Returns(new GetCurrentAdjustmentsQueryResponse
                {
                    IsValid = true,
                    Items = emptyAdjustments
                });

            // Act
            _processor.Process();

            // Assert
            _mediator.Verify(m => m.Send(
                It.Is<AddPaymentsCommandRequest>(r => r.Payments.Length == 0)), Times.Once, "Expecting no payments.");
        }

        [Test]
        [TestCaseSource(nameof(EmptyAdjustments))]
        public void ThenItShouldOutputFullPaymentsForAdjustmentsWithNoHistory(Adjustment[] emptyAdjustments)
        {
            // Arrange
            _mediator
                .Setup(m => m.Send(It.IsAny<GetPreviousAdjustmentsQueryRequest>()))
                .Returns(new GetPreviousAdjustmentsQueryResponse
                {
                    IsValid = true,
                    Items = emptyAdjustments
                });

            // Act
            _processor.Process();

            // Assert
            _mediator.Verify(m => m.Send(
                It.Is<AddPaymentsCommandRequest>(r => r.Payments.Length == 2)), Times.Once, "Expecting 2 payments.");
            _mediator.Verify(m => m.Send(
                It.Is<AddPaymentsCommandRequest>(
                    r =>
                        r.Payments.Any(p => PaymentForAdjustment(p, CurrentAdjustments[0], CurrentAdjustments[0].Amount)))),
                Times.Once, "Expecting full payment for the first adjustment");
            _mediator.Verify(m => m.Send(
                It.Is<AddPaymentsCommandRequest>(
                    r =>
                        r.Payments.Any(p => PaymentForAdjustment(p, CurrentAdjustments[1], CurrentAdjustments[1].Amount)))),
                Times.Once, "Expecting full payment for the second adjustment");
        }

        [Test]
        public void ThenItShouldOutputAdjustedPaymentsForAdjustmentsWithHistory()
        {
            // Arrange
            _mediator
                .Setup(m => m.Send(It.IsAny<GetCurrentAdjustmentsQueryRequest>()))
                .Returns(new GetCurrentAdjustmentsQueryResponse
                {
                    IsValid = true,
                    Items = new [] { CurrentAdjustments[0] }
                });

            // Act
            _processor.Process();

            // Assert
            _mediator.Verify(m => m.Send(
                It.Is<AddPaymentsCommandRequest>(r => r.Payments.Length == 1)), Times.Once, "Expecting 1 payment.");
            _mediator.Verify(m => m.Send(
                It.Is<AddPaymentsCommandRequest>(
                    r =>
                        r.Payments.Any(p => PaymentForAdjustment(p, CurrentAdjustments[0], CurrentAdjustments[0].Amount - PreviousAdjustments[0].Amount)))),
                Times.Once, "Expecting adjusted payment");
        }

        [Test]
        public void ThenItShouldOutputNegativePaymentsForAdjustmentsThatHavePreviouslyBeenOverpaid()
        {
            // Arrange
            _mediator
                .Setup(m => m.Send(It.IsAny<GetCurrentAdjustmentsQueryRequest>()))
                .Returns(new GetCurrentAdjustmentsQueryResponse
                {
                    IsValid = true,
                    Items = new[] { CurrentAdjustments[0] }
                });

            var previousAdjustment = PreviousAdjustments[0];
            previousAdjustment.Amount = 275.00m;

            _mediator
                .Setup(m => m.Send(It.IsAny<GetPreviousAdjustmentsQueryRequest>()))
                .Returns(new GetPreviousAdjustmentsQueryResponse
                {
                    IsValid = true,
                    Items = new [] { previousAdjustment }
                });


            // Act
            _processor.Process();

            // Assert
            _mediator.Verify(m => m.Send(
                It.Is<AddPaymentsCommandRequest>(r => r.Payments.Length == 1)), Times.Once, "Expecting 1 payment.");
            _mediator.Verify(m => m.Send(
                It.Is<AddPaymentsCommandRequest>(
                    r =>
                        r.Payments.Any(p => PaymentForAdjustment(p, CurrentAdjustments[0], -125.00m)))),
                Times.Once, "Expecting adjusted negative payment");
        }

        private bool PaymentForAdjustment(Payment payment, Adjustment adjustment, decimal amount)
        {
            return payment.Ukprn == adjustment.Ukprn
                   && payment.SubmissionId == adjustment.SubmissionId
                   && payment.SubmissionCollectionPeriod == adjustment.SubmissionCollectionPeriod
                   && payment.SubmissionAcademicYear == int.Parse(_yearOfCollection)
                   && payment.PaymentType == adjustment.PaymentType
                   && payment.PaymentTypeName == adjustment.PaymentTypeName
                   && payment.Amount == amount
                   && payment.CollectionPeriodName == $"{_yearOfCollection}-{CurrentPeriod.Name}"
                   && payment.CollectionPeriodMonth == CurrentPeriod.Month
                   && payment.CollectionPeriodYear == CurrentPeriod.Year;
        }
    }
}