using System.Collections.Generic;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Dto;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Services.Dependencies;
using SFA.DAS.ProviderPayments.Calc.Shared.Infrastructure.Data.Entities;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.Services
{
    class RemoveCompletionPaymentsWithoutEvidence : IFilterOutCompletionPaymentsWithoutEvidence
    {
        public FilteredEarningsResult Process(List<LearnerSummaryPaymentEntity> employerPayments, List<RawEarning> earnings)
        {
            return new FilteredEarningsResult {RawEarnings = earnings};
        }
    }
}
