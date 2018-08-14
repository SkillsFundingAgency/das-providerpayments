using System.Collections.Generic;
using System.Linq;
using SFA.DAS.Payments.DCFS.Domain;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Domain;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data.Entities;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Services.Dependencies;
using SFA.DAS.ProviderPayments.Calc.Shared.Infrastructure.Data.Entities;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.Services
{
    class CompletionPaymentService : ICompletionPaymentService
    {
        public CompletionPaymentEvidence CreateCompletionPaymentEvidence(List<PaymentEntity> learnerHistoricalPayments, List<RawEarning> learnerRawEarnings)
        {
            var iLrCompletionPayments = learnerRawEarnings.Where(x=>x.PriceEpisodeCumulativePmrs != 0).GroupBy(x => new CompletionPaymentGroup(x.PriceEpisodeCumulativePmrs, x.PriceEpisodeCompExemCode)).ToList();
            if (iLrCompletionPayments.Count > 1)
                return new CompletionPaymentEvidence(0, CompletionPaymentEvidenceState.ErrorOnIlr, 0);

            decimal totalEmployerPayments = learnerHistoricalPayments.Where(x =>
                    x.TransactionType == TransactionType.Learning &&
                    x.FundingSource == FundingSource.CoInvestedEmployer)
                .Sum(x => x.Amount);

            var completionPayment = iLrCompletionPayments.FirstOrDefault();

            if (completionPayment == null)
                return new CompletionPaymentEvidence(0, CompletionPaymentEvidenceState.Checkable, totalEmployerPayments);

            return new CompletionPaymentEvidence(completionPayment.Key.PriceEpisodeCumulativePmrs, CompletionPaymentEvidenceState.Checkable, totalEmployerPayments);
        }
    }
}



