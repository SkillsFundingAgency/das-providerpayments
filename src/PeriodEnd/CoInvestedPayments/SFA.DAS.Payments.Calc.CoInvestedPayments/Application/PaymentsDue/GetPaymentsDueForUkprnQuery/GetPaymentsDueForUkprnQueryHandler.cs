using System;
using System.Linq;
using MediatR;
using SFA.DAS.Payments.Calc.CoInvestedPayments.Infrastructure.Data;
using SFA.DAS.Payments.DCFS.Domain;

namespace SFA.DAS.Payments.Calc.CoInvestedPayments.Application.PaymentsDue.GetPaymentsDueForUkprnQuery
{
    public class GetPaymentsDueForCommitmentQueryHandler : IRequestHandler<GetPaymentsDueForUkprnQueryRequest, GetPaymentsDueForUkprnQueryResponse>
    {
        private readonly IPaymentDueRepository _paymentDueRepository;

        public GetPaymentsDueForCommitmentQueryHandler(IPaymentDueRepository paymentDueRepository)
        {
            _paymentDueRepository = paymentDueRepository;
        }

        public GetPaymentsDueForUkprnQueryResponse Handle(GetPaymentsDueForUkprnQueryRequest message)
        {
            try
            {
                var paymentsDue = _paymentDueRepository.GetPaymentsDueByUkprn(message.Ukprn);

                return new GetPaymentsDueForUkprnQueryResponse
                {
                    IsValid = true,
                    Items = paymentsDue?.Select(p => new PaymentDue
                    {
                        Id = p.Id,
                        Ukprn = p.Ukprn,
                        DeliveryMonth = p.DeliveryMonth,
                        DeliveryYear = p.DeliveryYear,
                        TransactionType = (TransactionType) p.TransactionType,
                        AmountDue = p.AmountDue,
                        SfaContributionPercentage = p.SfaContributionPercentage,
                        AimSequenceNumber = p.AimSequenceNumber,
                        FrameworkCode = p.FrameworkCode,
                        PathwayCode = p.PathwayCode,
                        ProgrammeType = p.ProgrammeType,
                        StandardCode = p.StandardCode,
                        Uln = p.Uln,
                        LearnRefNumber = p.LearnRefNumber
                    }).ToArray()
                };
            }
            catch (Exception ex)
            {
                return new GetPaymentsDueForUkprnQueryResponse
                {
                    IsValid = false,
                    Exception = ex
                };
            }
        }
    }
}