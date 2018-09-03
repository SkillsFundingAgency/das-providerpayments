using System;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.Domain
{
    public class CompletionPaymentEvidence {

        public CompletionPaymentEvidence()
        {
            // For Mocking reasons
        }
        public CompletionPaymentEvidence(decimal totalIlrEmployerPayment, CompletionPaymentEvidenceState state, decimal totalHistoricEmployerPayment)
        {
            IlrEvidenceEmployerPayment = totalIlrEmployerPayment;
            State = state;
            TotalHistoricEmployerPayment = totalHistoricEmployerPayment;
        }
        public decimal IlrEvidenceEmployerPayment { get; }
        public CompletionPaymentEvidenceState State { get; }
        public decimal TotalHistoricEmployerPayment { get; }
    }
}