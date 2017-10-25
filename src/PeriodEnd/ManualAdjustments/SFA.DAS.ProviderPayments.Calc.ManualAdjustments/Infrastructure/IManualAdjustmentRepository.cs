using System;

namespace SFA.DAS.ProviderPayments.Calc.ManualAdjustments.Infrastructure
{
    public interface IManualAdjustmentRepository
    {
        Guid[] GetRequiredPaymentIdsToReverse();

        void SetRequiredPaymentIdAsReversed(string requiredPaymentIdToReverse, string requiredPaymentIdForReversal);
    }
}