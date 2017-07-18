using System;
using System.Collections.Generic;
using CS.Common.External.Interfaces;
using MediatR;
using Moq;
using NLog;
using NUnit.Framework;
using SFA.DAS.Payments.DCFS.Context;
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
using SFA.DAS.Payments.DCFS.Domain;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Application.RequiredPayments.GetPaymentHistoryWhereNoEarningQuery;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.UnitTests.PaymentsDueProcessor
{
    public class WhenProcessingInvalidScenario
    {
        private PaymentsDue.PaymentsDueProcessor _processor;
        private Mock<ILogger> _logger;
        private Mock<IMediator> _mediator;
        private Mock<IExternalContext> _externalContext;

        [SetUp]
        public void Arrange()
        {
            _logger = new Mock<ILogger>();

            _mediator = new Mock<IMediator>();

            _externalContext = new Mock<IExternalContext>();
            _externalContext.Setup(c => c.Properties)
                .Returns(new Dictionary<string, string>
                {
                    { ContextPropertyKeys.TransientDatabaseConnectionString, "" },
                    { ContextPropertyKeys.LogLevel, "DEBUG" },
                    { PaymentsContextPropertyKeys.YearOfCollection, "1718" }
                });

            _processor = new PaymentsDue.PaymentsDueProcessor(_logger.Object, _mediator.Object, new ContextWrapper(_externalContext.Object));

            InitialMockSetup();
        }

        private void InitialMockSetup()
        {
            _mediator.Setup(m => m.Send(It.IsAny<GetPaymentHistoryQueryRequest>()))
                .Returns(new GetPaymentHistoryQueryResponse
                {
                    IsValid = true,
                    Items = new RequiredPayment[0]
                });
            _mediator
                .Setup(m => m.Send(It.IsAny<GetCurrentCollectionPeriodQueryRequest>()))
                .Returns(new GetCurrentCollectionPeriodQueryResponse
                {
                    IsValid = true,
                    Period = new CollectionPeriod { PeriodId = 1, Month = 9, Year = 2017 }
                });

            _mediator
                .Setup(m => m.Send(It.IsAny<GetProvidersQueryRequest>()))
                .Returns(new GetProvidersQueryResponse
                {
                    IsValid = true,
                    Items = new[] { new Provider { Ukprn = 10007459 } }
                });

            _mediator
                .Setup(m => m.Send(It.IsAny<GetProviderEarningsQueryRequest>()))
                .Returns(new GetProviderEarningsQueryResponse
                {
                    IsValid = true,
                    Items = new[]
                    {
                        new PeriodEarning
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
                            IsSuccess=true,
                            Payable=true,
                            ApprenticeshipContractType = 1
                        }
                    }
                });
            _mediator.Setup(m => m.Send(It.IsAny<GetPaymentHistoryWhereNoEarningQueryRequest>()))
                .Returns(new GetPaymentHistoryWhereNoEarningQueryResponse
                {
                    IsValid = true,
                    Items = new RequiredPayment[0]
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
        public void ThenExpectingExceptionForGetPaymentHistoryQueryFailure()
        {
            // Arrange
            _mediator
                .Setup(m => m.Send(It.IsAny<GetPaymentHistoryQueryRequest>()))
                .Returns(new GetPaymentHistoryQueryResponse
                {
                    IsValid = false,
                    Exception = new Exception("Exception.")
                });

            // Assert
            var ex = Assert.Throws<PaymentsDueProcessorException>(() => _processor.Process());
            Assert.IsTrue(ex.Message.Contains(PaymentsDueProcessorException.ErrorReadingPaymentHistoryMessage));
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

        [Test]
        public void ThenItShouldThrowExceptionIfGetPaymentHistoryWhereNoEarningQueryResponseIsInvalid()
        {
            // Arrange
            var innerException = new Exception("Some underlying error");
            _mediator.Setup(m => m.Send(It.IsAny<GetPaymentHistoryWhereNoEarningQueryRequest>()))
                .Returns(new GetPaymentHistoryWhereNoEarningQueryResponse
                {
                    IsValid = false,
                    Exception = innerException
                });

            // Act + Assert
            var ex = Assert.Throws<PaymentsDueProcessorException>(() => _processor.Process());
            Assert.AreEqual(PaymentsDueProcessorException.ErrorReadingPaymentHistoryWithoutEarningsMessage, ex.Message);
            Assert.AreSame(innerException, ex.InnerException);
        }
    }
}