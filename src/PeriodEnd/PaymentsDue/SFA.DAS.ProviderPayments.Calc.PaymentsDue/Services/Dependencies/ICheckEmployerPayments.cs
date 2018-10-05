using System.Collections.Generic;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Domain;
using SFA.DAS.ProviderPayments.Calc.Shared.Infrastructure.Data.Entities;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.Services.Dependencies
{
    public interface ICheckEmployerPayments
    {
        CompletionPaymentEvidence CreateCompletionPaymentEvidence(
            List<LearnerSummaryPaymentEntity> employerPayments, 
            RawEarning rawEarning);
    }
}