using System;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.Domain
{
    public class CompletionPaymentEvidence {

        public CompletionPaymentEvidence(decimal totalIlrEmployerPayment, CompletionPaymentEvidenceState state, decimal totalHistoricEmployerPayment)
        {
            TotalIlrEmployerPayment = totalIlrEmployerPayment;
            State = state;
            TotalHistoricEmployerPayment = totalHistoricEmployerPayment;
        }
        public decimal TotalIlrEmployerPayment { get; }
        public CompletionPaymentEvidenceState State { get; }
        public decimal TotalHistoricEmployerPayment { get; }
        public bool HasEnoughEmployerPayments => (Decimal.Round(TotalIlrEmployerPayment) >= Decimal.Round(TotalHistoricEmployerPayment));
    }
}