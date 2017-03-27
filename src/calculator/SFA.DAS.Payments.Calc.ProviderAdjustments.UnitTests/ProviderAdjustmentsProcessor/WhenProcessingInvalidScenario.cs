using System;
using MediatR;
using Moq;
using NLog;
using NUnit.Framework;
using SFA.DAS.Payments.Calc.ProviderAdjustments.Application.Adjustments;
using SFA.DAS.Payments.Calc.ProviderAdjustments.Application.Adjustments.GetCurrentAdjustmentsQuery;
using SFA.DAS.Payments.Calc.ProviderAdjustments.Application.Adjustments.GetPreviousAdjustmentsQuery;
using SFA.DAS.Payments.Calc.ProviderAdjustments.Application.CollectionPeriods;
using SFA.DAS.Payments.Calc.ProviderAdjustments.Application.CollectionPeriods.GetCurrentCollectionPeriodQuery;
using SFA.DAS.Payments.Calc.ProviderAdjustments.Application.Payments.AddPaymentsCommand;
using SFA.DAS.Payments.Calc.ProviderAdjustments.Application.Providers;
using SFA.DAS.Payments.Calc.ProviderAdjustments.Application.Providers.GetProvidersQuery;

namespace SFA.DAS.Payments.Calc.ProviderAdjustments.UnitTests.ProviderAdjustmentsProcessor
{
    public class WhenProcessingInvalidScenario
    {
        private static readonly long Ukprn = 10007459;

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
                   Period = new CollectionPeriod { PeriodId = 1, Month = 4, Year = 2017, Name = "R09" }
               });

            _mediator
                .Setup(m => m.Send(It.IsAny<GetProvidersQueryRequest>()))
                .Returns(new GetProvidersQueryResponse
                {
                    IsValid = true,
                    Items = new[] { new Provider { Ukprn =  Ukprn } }
                });

            _mediator
                .Setup(m => m.Send(It.IsAny<GetCurrentAdjustmentsQueryRequest>()))
                .Returns(new GetCurrentAdjustmentsQueryResponse
                {
                    IsValid = true,
                    Items = new []
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
                    }
                });

            _mediator
                .Setup(m => m.Send(It.IsAny<GetPreviousAdjustmentsQueryRequest>()))
                .Returns(new GetPreviousAdjustmentsQueryResponse
                {
                    IsValid = true,
                    Items = new []
                    {
                        new Adjustment
                        {
                            Ukprn = Ukprn,
                            SubmissionId = "abc",
                            SubmissionCollectionPeriod = 8,
                            PaymentType = 1,
                            PaymentTypeName = "adjustment",
                            Amount = 100.00m
                        }
                    }
                });

            _mediator
                .Setup(m => m.Send(It.IsAny<AddPaymentsCommandRequest>()))
                .Returns(Unit.Value);
        }

        [Test]
        public void ThenExpectingExceptionForGetCurrentCollectionPeriodQueryFailure()
        {
            // Arrange
            _mediator
                .Setup(m => m.Send(It.IsAny<GetCurrentCollectionPeriodQueryRequest>()))
                .Returns(new GetCurrentCollectionPeriodQueryResponse()
                {
                    IsValid = false,
                    Exception = new Exception("Exception.")
                });

            // Assert
            var ex = Assert.Throws<ProviderAdjustmentsProcessorException>(() => _processor.Process());
            Assert.IsTrue(ex.Message.Contains(ProviderAdjustmentsProcessorException.ErrorReadingCollectionPeriodMessage));
        }

        [Test]
        public void ThenExpectingExceptionForGetCurrentCollectionPeriodQueryReturningNoPeriod()
        {
            // Arrange
            _mediator
                .Setup(m => m.Send(It.IsAny<GetCurrentCollectionPeriodQueryRequest>()))
                .Returns(new GetCurrentCollectionPeriodQueryResponse()
                {
                    IsValid = true,
                    Period = null
                });

            // Assert
            var ex = Assert.Throws<ProviderAdjustmentsProcessorException>(() => _processor.Process());
            Assert.IsTrue(ex.Message.Contains(ProviderAdjustmentsProcessorException.ErrorNoCollectionPeriodMessage));
        }

        [Test]
        public void ThenExpectingExceptionForGetProvidersQueryFailure()
        {
            // Arrange
            _mediator
                .Setup(m => m.Send(It.IsAny<GetProvidersQueryRequest>()))
                .Returns(new GetProvidersQueryResponse
                {
                    IsValid = false,
                    Exception = new Exception("Exception.")
                });

            // Assert
            var ex = Assert.Throws<ProviderAdjustmentsProcessorException>(() => _processor.Process());
            Assert.IsTrue(ex.Message.Contains(ProviderAdjustmentsProcessorException.ErrorReadingProvidersMessage));
        }

        [Test]
        public void ThenExpectingExceptionForGetCurrentAdjustmentsQueryFailure()
        {
            // Arrange
            _mediator
                .Setup(m => m.Send(It.IsAny<GetCurrentAdjustmentsQueryRequest>()))
                .Returns(new GetCurrentAdjustmentsQueryResponse
                {
                    IsValid = false,
                    Exception = new Exception("Exception.")
                });

            // Assert
            var ex = Assert.Throws<ProviderAdjustmentsProcessorException>(() => _processor.Process());
            Assert.IsTrue(ex.Message.Contains(ProviderAdjustmentsProcessorException.ErrorReadingCurrentAdjustmentsMessage));
        }

        [Test]
        public void ThenExpectingExceptionForGetPreviousAdjustmentsQueryFailure()
        {
            // Arrange
            _mediator
                .Setup(m => m.Send(It.IsAny<GetPreviousAdjustmentsQueryRequest>()))
                .Returns(new GetPreviousAdjustmentsQueryResponse
                {
                    IsValid = false,
                    Exception = new Exception("Exception.")
                });

            // Assert
            var ex = Assert.Throws<ProviderAdjustmentsProcessorException>(() => _processor.Process());
            Assert.IsTrue(ex.Message.Contains(ProviderAdjustmentsProcessorException.ErrorReadingPreviousAdjustmentsMessage));
        }

        [Test]
        public void ThenExpectingExceptionForAddPaymentsCommandFailure()
        {
            // Arrange
            _mediator
                .Setup(m => m.Send(It.IsAny<AddPaymentsCommandRequest>()))
                .Throws<Exception>();

            // Assert
            var ex = Assert.Throws<ProviderAdjustmentsProcessorException>(() => _processor.Process());
            Assert.IsTrue(ex.Message.Contains(ProviderAdjustmentsProcessorException.ErrorWritingAdjustmentsMessage));
        }
    }
}