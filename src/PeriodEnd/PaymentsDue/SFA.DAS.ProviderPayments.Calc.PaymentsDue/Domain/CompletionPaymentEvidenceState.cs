﻿namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.Domain
{
    public enum CompletionPaymentEvidenceState
    {
        Checkable,
        ErrorOnIlr,
        ExemptRedundancy,
        ExemptOwnDelivery,
        ExemptOtherReason

    }
}