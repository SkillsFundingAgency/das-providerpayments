using System;
using MediatR;
using Moq;
using NLog;
using NUnit.Framework;
using SFA.DAS.ProviderPayments.Calculator.LevyPayments.Application.Accounts;
using SFA.DAS.ProviderPayments.Calculator.LevyPayments.Application.Accounts.AllocateLevyCommand;
using SFA.DAS.ProviderPayments.Calculator.LevyPayments.Application.Accounts.GetNextAccountQuery;
using SFA.DAS.ProviderPayments.Calculator.LevyPayments.Application.Earnings;
using SFA.DAS.ProviderPayments.Calculator.LevyPayments.Application.Earnings.GetEarningForCommitmentQuery;
using SFA.DAS.ProviderPayments.Calculator.LevyPayments.Application.Payments;
using SFA.DAS.ProviderPayments.Calculator.LevyPayments.Application.Payments.ProcessPaymentCommand;

namespace SFA.DAS.ProviderPayments.Calculator.LevyPayments.UnitTests.LevyPaymentsProcessor.Process
{
    public class WhenProcessingCompletionSubmission
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
                        MonthlyInstallmentCapped = 123.45m,
                        CompletionPaymentCapped = 987.65m,
                        LearningActualEndDate = new DateTime(2018, 5, 1)
                    }
                });
            _mediator.Setup(m => m.Send(It.Is<AllocateLevyCommandRequest>(r => r.Account.Id == _account.Id && r.AmountRequested == 987.65m)))
                .Returns(new AllocateLevyCommandResponse
                {
                    AmountAllocated = 987.65m
                });
            _mediator.Setup(m => m.Send(It.Is<AllocateLevyCommandRequest>(r => r.Account.Id == _account.Id && r.AmountRequested == 123.45m)))
                .Returns(new AllocateLevyCommandResponse
                {
                    AmountAllocated = 123.45m
                });

            _processor = new LevyPayments.LevyPaymentsProcessor(_logger.Object, _mediator.Object);
        }

        [Test]
        public void ThenItShouldProcessALevyPaymentForTheCompletionPayment()
        {
            // Act
            _processor.Process();

            // Assert
            _mediator.Verify(m => m.Send(ItIsPaymentForCommitment(_account.Commitments[0], FundingSource.Levy, TransactionType.Completion, 987.65m)), Times.Once);
            _mediator.Verify(m => m.Send(ItIsPaymentForCommitment(_account.Commitments[1], FundingSource.Levy, TransactionType.Completion, 987.65m)), Times.Once);
        }

        [Test]
        public void ThenItShouldProcessLevyPaymentsForCompletetionAndLearningIfCompletingOnCensusDate()
        {
            // Arrange
            _mediator.Setup(m => m.Send(It.IsAny<GetEarningForCommitmentQueryRequest>()))
                .Returns(new GetEarningForCommitmentQueryResponse
                {
                    Earning = new PeriodEarning
                    {
                        LearnerRefNumber = "Learner1",
                        AimSequenceNumber = 1,
                        Ukprn = 12345,
                        MonthlyInstallmentCapped = 123.45m,
                        CompletionPaymentCapped = 987.65m,
                        LearningActualEndDate = new DateTime(2018, 5, 31)
                    }
                });

            // Act
            _processor.Process();

            // Assert
            _mediator.Verify(m => m.Send(ItIsPaymentForCommitment(_account.Commitments[0], FundingSource.Levy, TransactionType.Learning, 123.45m)), Times.Once);
            _mediator.Verify(m => m.Send(ItIsPaymentForCommitment(_account.Commitments[1], FundingSource.Levy, TransactionType.Learning, 123.45m)), Times.Once);

            _mediator.Verify(m => m.Send(ItIsPaymentForCommitment(_account.Commitments[0], FundingSource.Levy, TransactionType.Completion, 987.65m)), Times.Once);
            _mediator.Verify(m => m.Send(ItIsPaymentForCommitment(_account.Commitments[1], FundingSource.Levy, TransactionType.Completion, 987.65m)), Times.Once);
        }


        private ProcessPaymentCommandRequest ItIsPaymentForCommitment(Commitment commitment, FundingSource source, TransactionType transactionType, decimal amount)
        {
            return It.Is<ProcessPaymentCommandRequest>(r => IsCorrectPayment(r, commitment, source, transactionType, amount));
        }
        private bool IsCorrectPayment(ProcessPaymentCommandRequest request, Commitment commitment, FundingSource source, TransactionType transactionType, decimal amount)
        {
            return request.Payment.CommitmentId == commitment.Id
                && request.Payment.LearnerRefNumber == "Learner1"
                && request.Payment.AimSequenceNumber == 1
                && request.Payment.Ukprn == 12345
                && request.Payment.Source == source
                && request.Payment.TransactionType == transactionType
                && request.Payment.Amount == amount;
        }
    }
}
