﻿using System;
using System.Linq;
using MediatR;
using SFA.DAS.Payments.Calc.CoInvestedPayments.Infrastructure.Data;


namespace SFA.DAS.Payments.Calc.CoInvestedPayments.Application.Earnings.GetProviderEarningsQuery
{
    public class GetProviderEarningsQueryHandler : IRequestHandler<GetProviderEarningsQueryRequest, GetProviderEarningsQueryResponse>
    {
        private readonly IEarningRepository _earningRepository;

        public GetProviderEarningsQueryHandler(IEarningRepository earningRepository)
        {
            _earningRepository = earningRepository;
        }

        public GetProviderEarningsQueryResponse Handle(GetProviderEarningsQueryRequest message)
        {
            try
            {
                var earningEntities = _earningRepository.GetProviderEarnings(message.Ukprn);

                return new GetProviderEarningsQueryResponse
                {
                    IsValid = true,
                    Items = earningEntities?.Select(e =>
                        new Earning
                        {
                            CommitmentId = e.CommitmentId,
                            LearnerRefNumber = e.LearnerRefNumber,
                            AimSequenceNumber = e.AimSequenceNumber,
                            Ukprn = e.Ukprn,
                            LearningStartDate = e.LearningStartDate,
                            LearningPlannedEndDate = e.LearningPlannedEndDate,
                            LearningActualEndDate = e.LearningActualEndDate,
                            MonthlyInstallment = e.MonthlyInstallment,
                            MonthlyInstallmentUncapped = e.MonthlyInstallmentUncapped,
                            CompletionPayment = e.CompletionPayment,
                            CompletionPaymentUncapped = e.CompletionPaymentUncapped
                        }).ToArray()
                };
            }
            catch (Exception ex)
            {
                return new GetProviderEarningsQueryResponse
                {
                    IsValid = false,
                    Exception = ex
                };
            }
        }
    }
}