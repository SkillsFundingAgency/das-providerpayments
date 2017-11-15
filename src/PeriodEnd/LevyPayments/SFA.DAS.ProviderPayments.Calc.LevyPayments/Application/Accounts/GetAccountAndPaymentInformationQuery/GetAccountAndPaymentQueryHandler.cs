using System.Collections.Generic;
using System.Linq;
using MediatR;
using SFA.DAS.Payments.DCFS.Domain;
using SFA.DAS.ProviderPayments.Calc.LevyPayments.Application.Accounts.GetNextAccountQuery;
using SFA.DAS.ProviderPayments.Calc.LevyPayments.Application.Payments;
using SFA.DAS.ProviderPayments.Calc.LevyPayments.Infrastructure.Data;
using SFA.DAS.ProviderPayments.Calc.LevyPayments.Infrastructure.Data.Entities;

namespace SFA.DAS.ProviderPayments.Calc.LevyPayments.Application.Accounts.GetAccountAndPaymentInformationQuery
{
    public class GetAccountAndPaymentQueryHandler : IRequestHandler<GetAccountAndPaymentQueryRequest, GetAccountAndPaymentQueryResponse>
    {
        private readonly IAccountRepository _accountRepository;

        public GetAccountAndPaymentQueryHandler(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public GetAccountAndPaymentQueryResponse Handle(GetAccountAndPaymentQueryRequest message)
        {
            var accountEntity = _accountRepository.GetAccountAndPaymentInformationForProcessing();
            if (accountEntity == null || !accountEntity.Any())
            {
                return new GetAccountAndPaymentQueryResponse();
            }

            var account = new Account
            {
                Id = accountEntity.First().AccountId,
                Name = accountEntity.First().AccountName,
                Commitments = new Commitment[0],
                Payments = new List<PaymentDue>(),
                Refunds = new List<PaymentDue>()
            };

            var refunds = accountEntity.Where(o => o.IsRefund)
                .Select(paymentEntity => new PaymentDue
                {
                    CommitmentId = paymentEntity.CommitmentId,
                    Id = paymentEntity.Id,
                    AimSequenceNumber = paymentEntity.AimSeqNumber,
                    AmountDue = paymentEntity.AmountDue,
                    DeliveryMonth = paymentEntity.DeliveryMonth,
                    DeliveryYear = paymentEntity.DeliveryYear,
                    LearnerRefNumber = paymentEntity.LearnRefNumber,
                    TransactionType = (TransactionType) paymentEntity.TransactionType,
                    Ukprn = paymentEntity.Ukprn
                })
                .ToList();
            account.Refunds = refunds;

            var payments = accountEntity.Where(o => o.IsRefund == false)
                .Select(paymentEntity => new PaymentDue
                {
                    CommitmentId = paymentEntity.CommitmentId,
                    Id = paymentEntity.Id,
                    AimSequenceNumber = paymentEntity.AimSeqNumber,
                    AmountDue = paymentEntity.AmountDue,
                    DeliveryMonth = paymentEntity.DeliveryMonth,
                    DeliveryYear = paymentEntity.DeliveryYear,
                    LearnerRefNumber = paymentEntity.LearnRefNumber,
                    TransactionType = (TransactionType)paymentEntity.TransactionType,
                    Ukprn = paymentEntity.Ukprn
                })
                .ToList();

            account.Payments = payments;

            return new GetAccountAndPaymentQueryResponse
            {
                Account = account
            };
        }
    }
}
