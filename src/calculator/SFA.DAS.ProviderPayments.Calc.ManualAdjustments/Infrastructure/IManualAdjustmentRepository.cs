namespace SFA.DAS.ProviderPayments.Calc.ManualAdjustments.Infrastructure
{
    public interface IManualAdjustmentRepository
    {
        string[] GetRequiredPaymentIdsToReverse();

        void SetRequiredPaymentIdAsReversed(string requiredPaymentIdToReverse, string requiredPaymentIdForReversal);
    }
}