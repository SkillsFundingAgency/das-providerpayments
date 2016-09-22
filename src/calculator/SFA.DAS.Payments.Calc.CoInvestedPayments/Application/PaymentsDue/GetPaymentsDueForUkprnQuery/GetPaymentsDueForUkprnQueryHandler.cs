﻿using System;
using System.Linq;
using MediatR;
using SFA.DAS.Payments.Calc.CoInvestedPayments.Infrastructure.Data;
using SFA.DAS.ProviderPayments.Calc.Common.Application;

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
                return new GetPaymentsDueForUkprnQueryResponse
                {
                    IsValid = false,
                    Exception = ex
                };
            }
        }
    }
}