using System;
using MediatR;
using Moq;
using NLog;
using NUnit.Framework;
using SFA.DAS.ProviderPayments.Calc.Common.Application;
using SFA.DAS.ProviderPayments.Calc.LevyPayments.Application.Accounts;
using SFA.DAS.ProviderPayments.Calc.LevyPayments.Application.Accounts.AllocateLevyCommand;
using SFA.DAS.ProviderPayments.Calc.LevyPayments.Application.Accounts.GetNextAccountQuery;
using SFA.DAS.ProviderPayments.Calc.LevyPayments.Application.CollectionPeriods;
using SFA.DAS.ProviderPayments.Calc.LevyPayments.Application.CollectionPeriods.GetCurrentCollectionPeriodQuery;
using SFA.DAS.ProviderPayments.Calc.LevyPayments.Application.Payments;
using SFA.DAS.ProviderPayments.Calc.LevyPayments.Application.Payments.GetPaymentsDueForCommitmentQuery;
using SFA.DAS.ProviderPayments.Calc.LevyPayments.Application.Payments.ProcessPaymentCommand;

namespace SFA.DAS.ProviderPayments.Calc.LevyPayments.UnitTests.LevyPaymentsProcessor.Process
{
    public class WhenProcessingInvalidScenario
    {
        private int _accountCounter;
        private Account _account;

        private Mock<ILogger> _logger;
        private Mock<IMediator> _mediator;
        private string _yearOfCollection = "1617";
        private LevyPayments.LevyPaymentsProcessor _processor;

        [SetUp]
        public void Arrange()
        {
            _accountCounter = 0;
            _account = new Account
            {
                Id = "ACC001",
                Commitments = new[]
                {
                    new Commitment { Id = 1 },
                    new Commitment { Id = 2 }
                }
            };

            _logger = new Mock<ILogger>();
            _mediator = new Mock<IMediator>();

            _processor = new LevyPayments.LevyPaymentsProcessor(_logger.Object, _mediator.Object, _yearOfCollection);

            InitialMockSetup();
        }

        private void InitialMockSetup()
        {
            _mediator
               .Setup(m => m.Send(It.IsAny<GetCurrentCollectionPeriodQueryRequest>()))
               .Returns(new GetCurrentCollectionPeriodQueryResponse
               {
                   IsValid = true,
                   Period = new CollectionPeriod { PeriodId = 1, Month = 9, Year = 2016 }
               });

            _mediator
                .Setup(m => m.Send(It.IsAny<GetNextAccountQueryRequest>()))
                .Returns(() =>
                {
                    _accountCounter++;
                    return _accountCounter <= 1 ? new GetNextAccountQueryResponse { Account = _account } : null;
                });

            _mediator
                .Setup(m => m.Send(It.IsAny<GetPaymentsDueForCommitmentQueryRequest>()))
                .Returns(new GetPaymentsDueForCommitmentQueryResponse
                {
                    IsValid = true,
                    Items = new[]
                    {
                        new PaymentDue
                        {
                            LearnerRefNumber = "Lrn-001",
                            AimSequenceNumber = 1,
                            Ukprn = 10007459,
                            DeliveryMonth = 9,
                            DeliveryYear = 2016,
                            TransactionType = TransactionType.Learning,
                            AmountDue = 1000.00m
                        },
                        new PaymentDue
                        {
                            LearnerRefNumber = "Lrn-001",
                            AimSequenceNumber = 1,
                            Ukprn = 10007459,
                            DeliveryMonth = 9,
                            DeliveryYear = 2016,
                            TransactionType = TransactionType.Completion,
                            AmountDue = 3000.00m
                        }
                    }
                });

            _mediator
                .Setup(m => m.Send(It.Is<AllocateLevyCommandRequest>(r => r.Account.Id == _account.Id && r.AmountRequested == 1000.00m)))
                .Returns(new AllocateLevyCommandResponse
                {
                    AmountAllocated = 1000.00m
                });

            _mediator
                .Setup(m => m.Send(It.Is<AllocateLevyCommandRequest>(r => r.Account.Id == _account.Id && r.AmountRequested == 3000.00m)))
                .Returns(new AllocateLevyCommandResponse
                {
                    AmountAllocated = 3000.00m
                });
        }

        [Test]
        public void ThenExpectingExceptionForGetCurrentCollectionPeriodQueryFailure()
        {
            // Arrange
            _mediator
                .Setup(m => m.Send(It.IsAny<GetCurrentCollectionPeriodQueryRequest>()))
                .Returns(new GetCurrentCollectionPeriodQueryResponse
                {
                    IsValid = false,
                    Exception = new Exception("Exception.")
                });

            // Assert
            var ex = Assert.Throws<LevyPaymentsProcessorException>(() => _processor.Process());
            Assert.IsTrue(ex.Message.Contains(LevyPaymentsProcessorException.ErrorReadingCollectionPeriodMessage));
        }

        [Test]
        public void ThenExpectingExceptionForGetNextAccountQueryFailure()
        {
            // Arrange
            _mediator
                .Setup(m => m.Send(It.IsAny<GetNextAccountQueryRequest>()))
                .Throws<Exception>();

            // Assert
            Assert.Throws<Exception>(() => _processor.Process());
        }

        [Test]
        public void ThenExpectingExceptionForGetPaymentsDueForCommitmentQueryFailure()
        {
            // Arrange
            _mediator
                .Setup(m => m.Send(It.IsAny<GetPaymentsDueForCommitmentQueryRequest>()))
                .Returns(new GetPaymentsDueForCommitmentQueryResponse
                {
                    IsValid = false,
                    Exception = new Exception("Exception.")
                });

            // Assert
            var ex = Assert.Throws<LevyPaymentsProcessorException>(() => _processor.Process());
            Assert.IsTrue(ex.Message.Contains(LevyPaymentsProcessorException.ErrorReadingPaymentsDueForCommitmentMessage));
        }

        [Test]
        public void ThenExpectingExceptionForAllocateLevyCommandFailure()
        {
            // Arrange
            _mediator
                .Setup(m => m.Send(It.IsAny<AllocateLevyCommandRequest>()))
                .Throws<Exception>();

            // Assert
            Assert.Throws<Exception>(() => _processor.Process());
        }

        [Test]
        public void ThenExpectingExceptionForProcessPaymentCommandFailure()
        {
            // Arrange
            _mediator
                .Setup(m => m.Send(It.IsAny<ProcessPaymentCommandRequest>()))
                .Throws<Exception>();

            // Assert
            Assert.Throws<Exception>(() => _processor.Process());
        }
    }
}