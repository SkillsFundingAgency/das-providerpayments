using MediatR;
using Moq;
using NLog;
using NUnit.Framework;
using SFA.DAS.ProviderPayments.Calculator.LevyPayments.Application.Accounts;
using SFA.DAS.ProviderPayments.Calculator.LevyPayments.Application.Accounts.AllocateLevyCommand;
using SFA.DAS.ProviderPayments.Calculator.LevyPayments.Application.Accounts.GetNextAccountQuery;
using SFA.DAS.ProviderPayments.Calculator.LevyPayments.Application.Accounts.MarkAccountAsProcessedCommand;
using SFA.DAS.ProviderPayments.Calculator.LevyPayments.Application.Earnings;
using SFA.DAS.ProviderPayments.Calculator.LevyPayments.Application.Earnings.GetEarningForCommitmentQuery;
using SFA.DAS.ProviderPayments.Calculator.LevyPayments.Application.Payments;
using SFA.DAS.ProviderPayments.Calculator.LevyPayments.Application.Payments.ProcessPaymentCommand;

namespace SFA.DAS.ProviderPayments.Calculator.LevyPayments.UnitTests.LevyPaymentsProcessor.Process
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
                Commitments = new[]
                {
                    new Commitment { Id = "Commitment1" },
                    new Commitment { Id = "Commitment2" }
                }
            };

            _logger = new Mock<ILogger>();

            _mediator = new Mock<IMediator>();
            _mediator.Setup(m => m.Send(It.IsAny<GetNextAccountQueryRequest>()))
                .Returns(() =>
                {
                    _accountCounter++;
                    return _accountCounter <= 1 ? new GetNextAccountQueryResponse { Account = _account } : null;
                });
            _mediator.Setup(m => m.Send(It.IsAny<GetEarningForCommitmentQueryRequest>()))
                .Returns(new GetEarningForCommitmentQueryResponse
                {
                    Earning = new PeriodEarning
                    {
                        LearnerRefNumber = "Learner1",
                        AimSequenceNumber = 1,
                        Ukprn = 12345,
                        MonthlyInstallmentCapped = 123.45m
                    }
                });
            _mediator.Setup(m => m.Send(It.IsAny<AllocateLevyCommandRequest>()))
                .Returns(new AllocateLevyCommandResponse
                {
                    AmountAllocated = 123.45m
                });

            _processor = new LevyPayments.LevyPaymentsProcessor(_logger.Object, _mediator.Object);
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
        public void ThenItShouldGetEarningsForEveryCommitment()
        {
            // Act
            _processor.Process();

            // Assert
            _mediator.Verify(m => m.Send(It.Is<GetEarningForCommitmentQueryRequest>(r => r.CommitmentId == _account.Commitments[0].Id)), Times.Once);
            _mediator.Verify(m => m.Send(It.Is<GetEarningForCommitmentQueryRequest>(r => r.CommitmentId == _account.Commitments[1].Id)), Times.Once);
        }

        [Test]
        public void ThenItShouldAttemptToAllocateLevyIfCommitmentHasEarnings()
        {
            // Act
            _processor.Process();

            // Assert
            _mediator.Verify(m => m.Send(It.Is<AllocateLevyCommandRequest>(r => r.Account == _account && r.AmountRequested == 123.45m)), Times.Exactly(2));
        }

        [Test]
        public void ThenItShouldNotAttemptToAllocateLevyIfCommitmentHasNoEarnings()
        {
            // Arrange
            _mediator.Setup(m => m.Send(It.IsAny<GetEarningForCommitmentQueryRequest>()))
                .Returns(new GetEarningForCommitmentQueryResponse
                {
                    Earning = new PeriodEarning
                    {
                        MonthlyInstallmentCapped = 0m
                    }
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
            _mediator.Verify(m => m.Send(ItIsPaymentForCommitment(_account.Commitments[0], FundingSource.Levy, 123.45m)));
            _mediator.Verify(m => m.Send(ItIsPaymentForCommitment(_account.Commitments[1], FundingSource.Levy, 123.45m)));
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
            _mediator.Verify(m => m.Send(It.IsAny<GetEarningForCommitmentQueryRequest>()), Times.Never);
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
            _mediator.Verify(m => m.Send(It.IsAny<GetEarningForCommitmentQueryRequest>()), Times.Never);
            _mediator.Verify(m => m.Send(It.IsAny<AllocateLevyCommandRequest>()), Times.Never);
            _mediator.Verify(m => m.Send(It.IsAny<ProcessPaymentCommandRequest>()), Times.Never);
        }

        private ProcessPaymentCommandRequest ItIsPaymentForCommitment(Commitment commitment, FundingSource source, decimal amount)
        {
            return It.Is<ProcessPaymentCommandRequest>(r => r.Payment.CommitmentId == commitment.Id
                                                         && r.Payment.LearnerRefNumber == "Learner1"
                                                         && r.Payment.AimSequenceNumber == 1
                                                         && r.Payment.Ukprn == 12345
                                                         && r.Payment.Source == source
                                                         && r.Payment.TransactionType == TransactionType.Learning
                                                         && r.Payment.Amount == amount);
        }

    }
}
