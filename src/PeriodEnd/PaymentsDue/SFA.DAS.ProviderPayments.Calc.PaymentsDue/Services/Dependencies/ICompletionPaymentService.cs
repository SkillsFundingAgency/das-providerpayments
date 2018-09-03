using System.Collections.Generic;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Domain;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data.Entities;
using SFA.DAS.ProviderPayments.Calc.Shared.Infrastructure.Data.Entities;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.Services.Dependencies
{
    public interface ICompletionPaymentService
    {
        CompletionPaymentEvidence CreateCompletionPaymentEvidence(List<LearnerSummaryPaymentEntity> learnerHistoricalPayments, List<RawEarning> learnerRawEarnings);
    }
}