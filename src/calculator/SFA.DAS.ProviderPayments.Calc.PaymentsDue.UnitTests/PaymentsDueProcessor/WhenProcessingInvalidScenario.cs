﻿using System;
using MediatR;
using Moq;
using NLog;
using NUnit.Framework;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Application.CollectionPeriods;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Application.CollectionPeriods.GetCurrentCollectionPeriodQuery;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Application.Earnings;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Application.Earnings.GetProviderEarningsQuery;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Application.Providers;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Application.Providers.GetProvidersQuery;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Application.RequiredPayments.AddRequiredPaymentsCommand;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.UnitTests.PaymentsDueProcessor
{
    public class WhenProcessingInvalidScenario
    {
        private PaymentsDue.PaymentsDueProcessor _processor;
        private Mock<ILogger> _logger;
        private Mock<IMediator> _mediator;

        [SetUp]
        public void Arrange()
        {
            _logger = new Mock<ILogger>();
            _mediator = new Mock<IMediator>();

            _processor = new PaymentsDue.PaymentsDueProcessor(_logger.Object, _mediator.Object);

            InitialMockSetup();
        }

        private void InitialMockSetup()
        {
            _mediator
                .Setup(m => m.Send(It.IsAny<GetCurrentCollectionPeriodQueryRequest>()))
                .Returns(new GetCurrentCollectionPeriodQueryResponse
                {
                    IsValid = true,
                    Period = new CollectionPeriod {PeriodId = 1, Month = 9, Year = 2016}
                });

            _mediator
                .Setup(m => m.Send(It.IsAny<GetProvidersQueryRequest>()))
                .Returns(new GetProvidersQueryResponse
                {
                    IsValid = true,
                    Items = new[] {new Provider {Ukprn = 10007459}}
                });

            _mediator
                .Setup(m => m.Send(It.IsAny<GetProviderEarningsQueryRequest>()))
                .Returns(new GetProviderEarningsQueryResponse
                {
                    IsValid = true,
                    Items = new[]
                    {
                        new Earning
                        {
                            CommitmentId = "C-001",
                            LearnerRefNumber = "Lrn-001",
                            AimSequenceNumber = 1,
                            Ukprn = 10007459,
                            LearningStartDate = new DateTime(2016, 8, 1),
                            LearningPlannedEndDate = new DateTime(2017, 7, 31),
                            LearningActualEndDate = new DateTime(2017, 7, 31),
                            MonthlyInstallment = 1000.00m,
                            MonthlyInstallmentUncapped = 1000.00m,
                            CompletionPayment = 3000.00m,
                            CompletionPaymentUncapped = 3000.00m
                        }
                    }
                });
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
            var ex = Assert.Throws<PaymentsDueProcessorException>(() => _processor.Process());
            Assert.IsTrue(ex.Message.Contains(PaymentsDueProcessorException.ErrorReadingCollectionPeriodMessage));
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
            var ex = Assert.Throws<PaymentsDueProcessorException>(() => _processor.Process());
            Assert.IsTrue(ex.Message.Contains(PaymentsDueProcessorException.ErrorNoCollectionPeriodMessage));
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
            var ex = Assert.Throws<PaymentsDueProcessorException>(() => _processor.Process());
            Assert.IsTrue(ex.Message.Contains(PaymentsDueProcessorException.ErrorReadingProvidersMessage));
        }

        [Test]
        public void ThenExpectingExceptionForGetProviderEarningsQueryFailure()
        {
            // Arrange
            _mediator
                .Setup(m => m.Send(It.IsAny<GetProviderEarningsQueryRequest>()))
                .Returns(new GetProviderEarningsQueryResponse
                {
                    IsValid = false,
                    Exception = new Exception("Exception.")
                });

            // Assert
            var ex = Assert.Throws<PaymentsDueProcessorException>(() => _processor.Process());
            Assert.IsTrue(ex.Message.Contains(PaymentsDueProcessorException.ErrorReadingProviderEarningsMessage));
        }

        [Test]
        public void ThenExpectingExceptionForAddRequiredPaymentsCommandFailure()
        {
            // Arrange
            _mediator
                .Setup(m => m.Send(It.IsAny<AddRequiredPaymentsCommandRequest>()))
                .Returns(new AddRequiredPaymentsCommandResponse
                {
                    IsValid = false,
                    Exception = new Exception("Exception.")
                });

            // Assert
            var ex = Assert.Throws<PaymentsDueProcessorException>(() => _processor.Process());
            Assert.IsTrue(ex.Message.Contains(PaymentsDueProcessorException.ErrorWritingRequiredProviderPaymentsMessage));
        }
    }
}