using System.Collections.Generic;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Dto;
using SFA.DAS.ProviderPayments.Calc.Shared.Infrastructure.Data.Entities;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.Services.Dependencies
{
    public interface IFilterOutCompletionPaymentsWithoutEvidence
    {
        FilteredEarningsResult Process(List<LearnerSummaryPaymentEntity> employerPayments, List<RawEarning> earnings);
    }
}
