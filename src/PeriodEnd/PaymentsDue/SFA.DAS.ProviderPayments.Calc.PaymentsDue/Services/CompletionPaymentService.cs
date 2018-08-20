using System;
using System.Collections.Generic;
using System.Linq;
using SFA.DAS.Payments.DCFS.Domain;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Domain;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data.Entities;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Services.Dependencies;
using SFA.DAS.ProviderPayments.Calc.Shared.Infrastructure.Data.Entities;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.Services
{
    public class CompletionPaymentService : ICompletionPaymentService
    {
        public CompletionPaymentEvidence CreateCompletionPaymentEvidence(
            List<LearnerSummaryPaymentEntity> learnerHistoricalPayments, List<RawEarning> learnerRawEarnings)
        {
            if (learnerHistoricalPayments == null) throw new ArgumentException(nameof(learnerHistoricalPayments));
            if (learnerRawEarnings == null) throw new ArgumentException(nameof(learnerRawEarnings));

            var iLrCompletionPayments = learnerRawEarnings.Where(x=> x.PriceEpisodeCumulativePmrs != 0 || x.PriceEpisodeCompExemCode != 0).GroupBy(x => new CompletionPaymentGroup(x.PriceEpisodeCumulativePmrs, x.PriceEpisodeCompExemCode)).ToList();
            if (iLrCompletionPayments.Count > 1)
                return new CompletionPaymentEvidence(0, CompletionPaymentEvidenceState.ErrorOnIlr, 0);

            decimal totalEmployerPayments = learnerHistoricalPayments.Where(x =>
                    x.TransactionType == TransactionType.Learning)
                .Sum(x => x.Amount);

            var completionPayment = iLrCompletionPayments.FirstOrDefault();

            if (completionPayment == null)
                return new CompletionPaymentEvidence(0, CompletionPaymentEvidenceState.Checkable, totalEmployerPayments);

            return new CompletionPaymentEvidence(completionPayment.Key.PriceEpisodeCumulativePmrs,
                MapReasonToState(completionPayment.Key.PriceEpisodeCompExemCode), totalEmployerPayments);
        }

        private CompletionPaymentEvidenceState MapReasonToState(int priceEpisodeCompExemCode)
        {
            switch (priceEpisodeCompExemCode)
            {
                case 0:
                    return CompletionPaymentEvidenceState.Checkable;
                case 1:
                    return CompletionPaymentEvidenceState.ExemptRedundancy;
                case 2:
                    return CompletionPaymentEvidenceState.ExemptOwnDelivery;
                case 3:
                    return CompletionPaymentEvidenceState.ExemptOtherReason;
                default:
                    return CompletionPaymentEvidenceState.ErrorOnIlr;
            }
        }

    }
}



