using MediatR;
using Moq;
using NLog;
using NUnit.Framework;
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
                    new Commitment { Id = "C-001" },
                    new Commitment { Id = "C-002" }
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
                            TransactionType = TransactionType.Completion,
                            AmountDue = 3000.00m
                        }
                    }
                });

            _mediator
                .Setup(m => m.Send(It.Is<AllocateLevyCommandRequest>(r => r.Account.Id == _account.Id && r.AmountRequested == 3000.00m)))
                .Returns(new AllocateLevyCommandResponse
                {
                    AmountAllocated = 3000.00m
                });
        }

        [Test]
        public void ThenItShouldProcessALevyPaymentForTheCompletionPayment()
        {
            // Act
            _processor.Process();

            // Assert
            _mediator.Verify(m => m.Send(ItIsPaymentForCommitment(_account.Commitments[0], FundingSource.Levy, TransactionType.Completion, 3000.00m)), Times.Once);
            _mediator.Verify(m => m.Send(ItIsPaymentForCommitment(_account.Commitments[1], FundingSource.Levy, TransactionType.Completion, 3000.00m)), Times.Once);
        }

        private ProcessPaymentCommandRequest ItIsPaymentForCommitment(Commitment commitment, FundingSource source, TransactionType transactionType, decimal amount)
        {
            return It.Is<ProcessPaymentCommandRequest>(r => IsCorrectPayment(r, commitment, source, transactionType, amount));
        }

        private bool IsCorrectPayment(ProcessPaymentCommandRequest request, Commitment commitment, FundingSource source, TransactionType transactionType, decimal amount)
        {
            return request.Payment.CommitmentId == commitment.Id
                && request.Payment.LearnerRefNumber == "Lrn-001"
                && request.Payment.AimSequenceNumber == 1
                && request.Payment.Ukprn == 10007459
                && request.Payment.DeliveryMonth == 8
                && request.Payment.DeliveryYear == 2015
                && request.Payment.CollectionPeriodMonth == 9
                && request.Payment.CollectionPeriodYear == 2016
                && request.Payment.Source == source
                && request.Payment.TransactionType == transactionType
                && request.Payment.Amount == amount;
        }
    }
}