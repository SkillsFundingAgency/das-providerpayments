using System.Collections.Generic;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Domain;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Dto;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data.Entities;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Services.Dependencies;
using SFA.DAS.ProviderPayments.Calc.Shared.Infrastructure.Data.Entities;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.Services
{
    class RemoveCompletionPaymentsWithoutEvidence : IFilterOutCompletionPaymentsWithoutEvidence
    {
        private readonly ICheckEmployerPayments _employerPaymentsChecker;

        public RemoveCompletionPaymentsWithoutEvidence(
            ICheckEmployerPayments employerPaymentsChecker)
        {
            _employerPaymentsChecker = employerPaymentsChecker;
        }

        public FilteredEarningsResult Process(List<LearnerSummaryPaymentEntity> employerPayments, List<RawEarning> earnings)
        {
            var nonPayableEarnings = new List<NonPayableEarning>();

            foreach (var rawEarning in earnings)
            {
                if (rawEarning.TransactionType02 == 0)
                {
                    continue;
                }

                var checkResult = _employerPaymentsChecker.CreateCompletionPaymentEvidence(
                    employerPayments, 
                    rawEarning);

                var reasonToHoldBack = "";
                var holdBack = false;
                if (checkResult != null)
                {
                    holdBack = HoldBackCompletionPayment(checkResult, out reasonToHoldBack);
                }

                if (holdBack)
                {
                    var heldBackCompletionPayment = new NonPayableEarning(rawEarning);
                    heldBackCompletionPayment.PaymentFailureMessage = reasonToHoldBack;
                    heldBackCompletionPayment.PaymentFailureReason = PaymentFailureType.HeldBackCompletionPayment;
                    
                    nonPayableEarnings.Add(heldBackCompletionPayment);
                }
            }

            return new FilteredEarningsResult {RawEarnings = earnings, NonPayableEarnings = nonPayableEarnings};
        }

        private bool HoldBackCompletionPayment(CompletionPaymentEvidence completionPaymentEvidence, out string reason)
        {
            if (completionPaymentEvidence.State == CompletionPaymentEvidenceState.ErrorOnIlr)
            {
                reason = "Error on PMR records in ILR";
                return true;
            }

            if (completionPaymentEvidence.State == CompletionPaymentEvidenceState.ExemptRedundancy ||
                completionPaymentEvidence.State == CompletionPaymentEvidenceState.ExemptOwnDelivery ||
                completionPaymentEvidence.State == CompletionPaymentEvidenceState.ExemptOtherReason)
            {
                reason = "";
                return false;
            }

            if (decimal.Round(completionPaymentEvidence.IlrEvidenceEmployerPayment) <
                decimal.Floor(completionPaymentEvidence.TotalHistoricEmployerPayment))
            {
                reason = "Historic Evidence does not show enough employer payments were made";
                return true;
            }

            reason = "";
            return false;
        }
    }
}
