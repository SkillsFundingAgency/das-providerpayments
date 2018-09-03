using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Domain;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.UnitTests.Utilities.Helpers
{
    public static class CompletionPaymentsEvidenceHelper
    {
        public static CompletionPaymentEvidence CreateCanPayEvidence()
        {
            return new CompletionPaymentEvidence(0, CompletionPaymentEvidenceState.Checkable, 0);
        }
    }
}
