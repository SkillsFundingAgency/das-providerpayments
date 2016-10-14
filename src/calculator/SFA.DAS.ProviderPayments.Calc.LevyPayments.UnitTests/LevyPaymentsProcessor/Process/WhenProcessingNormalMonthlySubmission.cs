using MediatR;
using Moq;
using NLog;
using NUnit.Framework;
using SFA.DAS.ProviderPayments.Calc.Common.Application;
using SFA.DAS.ProviderPayments.Calc.LevyPayments.Application.Accounts;
using SFA.DAS.ProviderPayments.Calc.LevyPayments.Application.Accounts.AllocateLevyCommand;
using SFA.DAS.ProviderPayments.Calc.LevyPayments.Application.Accounts.GetNextAccountQuery;
using SFA.DAS.ProviderPayments.Calc.LevyPayments.Application.Accounts.MarkAccountAsProcessedCommand;
using SFA.DAS.ProviderPayments.Calc.LevyPayments.Application.CollectionPeriods;
using SFA.DAS.ProviderPayments.Calc.LevyPayments.Application.CollectionPeriods.GetCurrentCollectionPeriodQuery;
using SFA.DAS.ProviderPayments.Calc.LevyPayments.Application.Payments;
using SFA.DAS.ProviderPayments.Calc.LevyPayments.Application.Payments.GetPaymentsDueForCommitmentQuery;
using SFA.DAS.ProviderPayments.Calc.LevyPayments.Application.Payments.ProcessPaymentCommand;

namespace SFA.DAS.ProviderPayments.Calc.LevyPayments.UnitTests.LevyPaymentsProcessor.Process
{
    public class WhenProcessingNormalMonthlySubmission
    {
        private int _accountCounter;
        private Account _account;

        private Mock<ILogger> _logger;
        private Mock<IMediator> _mediator;
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

            _processor = new LevyPayments.LevyPaymentsProcessor(_logger.Object, _mediator.Object);

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
                            DeliveryMonth = 8,
                            DeliveryYear = 2015,
                            TransactionType = TransactionType.Learning,
                            AmountDue = 1000.00m
                        }
                    }
                });

            _mediator
                .Setup(m => m.Send(It.Is<AllocateLevyCommandRequest>(r => r.Account.Id == _account.Id && r.AmountRequested == 1000.00m)))
                .Returns(new AllocateLevyCommandResponse
                {
                    AmountAllocated = 1000.00m
                });
        }

        [Test]
        public void ThenItShouldGetCurrentCollectionPeriod()
        {
            // Act
            _processor.Process();

            // Assert
            _mediator.Verify(m => m.Send(It.IsAny<GetCurrentCollectionPeriodQueryRequest>()), Times.Once);
        }

        [Test]
        public void ThenItShouldExecuteGetNextAccountQueryUntilNoAccountsAreReturned()
        {
            // Arrange
            var counter = 0;
            _mediator.Setup(m => m.Send(It.IsAny<GetNextAccountQueryRequest>()))
                .Returns(() =>
                {
                    counter++;
                    return new GetNextAccountQueryResponse
                    {
                        Account = counter >= 3 ? null : new Account { Commitments = new Commitment[0] }
                    };
                });

            // Act
            _processor.Process();

            // Assert
            _mediator.Verify(m => m.Send(It.IsAny<GetNextAccountQueryRequest>()), Times.Exactly(3));
        }

        [Test]
        public void ThenItShouldGetPaymentsDueForEveryCommitment()
        {
            // Act
            _processor.Process();

            // Assert
            _mediator.Verify(m => m.Send(It.Is<GetPaymentsDueForCommitmentQueryRequest>(r => r.CommitmentId == _account.Commitments[0].Id)), Times.Once);
            _mediator.Verify(m => m.Send(It.Is<GetPaymentsDueForCommitmentQueryRequest>(r => r.CommitmentId == _account.Commitments[1].Id)), Times.Once);
        }

        [Test]
        public void ThenItShouldAttemptToAllocateLevyIfCommitmentHasPaymentsDue()
        {
            // Act
            _processor.Process();

            // Assert
            _mediator.Verify(m => m.Send(It.Is<AllocateLevyCommandRequest>(r => r.Account == _account && r.AmountRequested == 1000.00m)), Times.Exactly(2));
        }

        [Test]
        public void ThenItShouldNotAttemptToAllocateLevyIfCommitmentHasNoPaymentsDue()
        {
            // Arrange
            _mediator
                .Setup(m => m.Send(It.IsAny<GetPaymentsDueForCommitmentQueryRequest>()))
                .Returns(new GetPaymentsDueForCommitmentQueryResponse
                {
                    IsValid = true,
                    Items = new PaymentDue[] {}
                });

            // Act
            _processor.Process();

            // Assert
            _mediator.Verify(m => m.Send(It.IsAny<AllocateLevyCommandRequest>()), Times.Never);
        }

        [Test]
        public void ThenItShouldProcessALevyPayment()
        {
            // Act
            _processor.Process();

            // Assert
            _mediator.Verify(m => m.Send(ItIsPaymentForCommitment(_account.Commitments[0], FundingSource.Levy, 1000.00m)));
            _mediator.Verify(m => m.Send(ItIsPaymentForCommitment(_account.Commitments[1], FundingSource.Levy, 1000.00m)));
        }

        [Test]
        public void ThenItShouldMarkAccountAsProcessed()
        {
            // Act
            _processor.Process();

            // Assert
            _mediator.Verify(m => m.Send(It.Is<MarkAccountAsProcessedCommandRequest>(r => r.AccountId == _account.Id)), Times.Once);
        }

        [Test]
        public void ThenItShouldNotDoAnythingIfThereAreNoAccounts()
        {
            // Arrange
            _mediator.Setup(m => m.Send(It.IsAny<GetNextAccountQueryRequest>()))
                .Returns<GetNextAccountQueryResponse>(null);

            // Act
            _processor.Process();

            // Assert
            _mediator.Verify(m => m.Send(It.IsAny<GetNextAccountQueryRequest>()), Times.Once);
            _mediator.Verify(m => m.Send(It.IsAny<GetPaymentsDueForCommitmentQueryRequest>()), Times.Never);
            _mediator.Verify(m => m.Send(It.IsAny<AllocateLevyCommandRequest>()), Times.Never);
            _mediator.Verify(m => m.Send(It.IsAny<ProcessPaymentCommandRequest>()), Times.Never);
            _mediator.Verify(m => m.Send(It.IsAny<MarkAccountAsProcessedCommandRequest>()), Times.Never);
        }

        [Test]
        public void ThenItShouldNotMakePaymentsIfAccountHasNoCommitments()
        {
            // Arrange
            _account.Commitments = new Commitment[0];

            // Act
            _processor.Process();

            // Assert
            _mediator.Verify(m => m.Send(It.IsAny<GetPaymentsDueForCommitmentQueryRequest>()), Times.Never);
            _mediator.Verify(m => m.Send(It.IsAny<AllocateLevyCommandRequest>()), Times.Never);
            _mediator.Verify(m => m.Send(It.IsAny<ProcessPaymentCommandRequest>()), Times.Never);
        }

        private ProcessPaymentCommandRequest ItIsPaymentForCommitment(Commitment commitment, FundingSource fundingSource, decimal amount)
        {
            return It.Is<ProcessPaymentCommandRequest>(r => r.Payment.CommitmentId == commitment.Id
                                                         && r.Payment.LearnerRefNumber == "Lrn-001"
                                                         && r.Payment.AimSequenceNumber == 1
                                                         && r.Payment.Ukprn == 10007459
                                                         && r.Payment.DeliveryMonth == 8
                                                         && r.Payment.DeliveryYear == 2015
                                                         && r.Payment.CollectionPeriodMonth == 9
                                                         && r.Payment.CollectionPeriodYear == 2016
                                                         && r.Payment.FundingSource == fundingSource
                                                         && r.Payment.TransactionType == TransactionType.Learning
                                                         && r.Payment.Amount == amount);
        }
    }
}
