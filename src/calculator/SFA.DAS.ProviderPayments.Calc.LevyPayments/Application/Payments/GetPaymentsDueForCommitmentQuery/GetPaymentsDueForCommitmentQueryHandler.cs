using System;
using System.Linq;
using MediatR;
using SFA.DAS.ProviderPayments.Calc.Common.Application;
using SFA.DAS.ProviderPayments.Calc.LevyPayments.Infrastructure.Data;

namespace SFA.DAS.ProviderPayments.Calc.LevyPayments.Application.Payments.GetPaymentsDueForCommitmentQuery
{
    public class GetPaymentsDueForCommitmentQueryHandler : IRequestHandler<GetPaymentsDueForCommitmentQueryRequest, GetPaymentsDueForCommitmentQueryResponse>
    {
        private readonly IPaymentDueRepository _paymentDueRepository;

        public GetPaymentsDueForCommitmentQueryHandler(IPaymentDueRepository paymentDueRepository)
        {
            _paymentDueRepository = paymentDueRepository;
        }

        public GetPaymentsDueForCommitmentQueryResponse Handle(GetPaymentsDueForCommitmentQueryRequest message)
        {
            try
            {
                var paymentsDue = _paymentDueRepository.GetPaymentsDueForCommitment(message.CommitmentId);

                return new GetPaymentsDueForCommitmentQueryResponse
                {
                    IsValid = true,
                    Items = paymentsDue == null
                        ? null
                        : paymentsDue.Select(p => new PaymentDue
                        {
                            CommitmentId = p.CommitmentId,
                            LearnerRefNumber = p.LearnRefNumber,
                            AimSequenceNumber = p.AimSeqNumber,
                            Ukprn = p.Ukprn,
                            DeliveryMonth = p.DeliveryMonth,
                            DeliveryYear = p.DeliveryYear,
                            TransactionType = (TransactionType) p.TransactionType,
                            AmountDue = p.AmountDue
                        }).ToArray()
                };
            }
            catch (Exception ex)
            {
                return new GetPaymentsDueForCommitmentQueryResponse
                {
                    IsValid = false,
                    Exception = ex
                };
            }
        }
    }
}