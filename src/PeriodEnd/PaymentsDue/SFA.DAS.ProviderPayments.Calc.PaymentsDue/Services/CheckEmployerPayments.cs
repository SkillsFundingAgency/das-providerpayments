using System;
using System.Collections.Generic;
using System.Linq;
using SFA.DAS.Payments.DCFS.Domain;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Domain;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Services.Dependencies;
using SFA.DAS.ProviderPayments.Calc.Shared.Infrastructure.Data.Entities;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.Services
{
    public class CheckEmployerPayments : ICheckEmployerPayments
    {
        public CompletionPaymentEvidence CreateCompletionPaymentEvidence(
            List<LearnerSummaryPaymentEntity> employerPayments, 
            RawEarning rawEarning)
        {
            if (employerPayments == null) throw new ArgumentException(nameof(employerPayments));
            if (rawEarning == null) throw new ArgumentException(nameof(rawEarning));

            var totalEmployerPayments = employerPayments.Where(x =>
                    x.TransactionType == TransactionType.Learning &&
                    x.StandardCode == rawEarning.StandardCode &&
                    x.ProgrammeType == rawEarning.ProgrammeType &&
                    x.FrameworkCode == rawEarning.FrameworkCode &&
                    x.PathwayCode == rawEarning.PathwayCode &&
                    x.ApprenticeshipContractType == rawEarning.ApprenticeshipContractType &&
                    x.SfaContributionPercentage == rawEarning.SfaContributionPercentage &&
                    x.FundingLineType == rawEarning.FundingLineType)
                .Sum(x => x.Amount);

            return new CompletionPaymentEvidence(rawEarning.CumulativePmrs,
                MapReasonToState(rawEarning.ExemptionCodeForCompletionHoldback), 
                totalEmployerPayments);
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


