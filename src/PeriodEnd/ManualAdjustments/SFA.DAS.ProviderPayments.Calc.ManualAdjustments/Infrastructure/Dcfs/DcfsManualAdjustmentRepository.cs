using System;
using SFA.DAS.Payments.DCFS.Infrastructure.Data;

namespace SFA.DAS.ProviderPayments.Calc.ManualAdjustments.Infrastructure.Dcfs
{
    public class DcfsManualAdjustmentRepository : DcfsRepository, IManualAdjustmentRepository
    {
        private const string ManualAdjustmentsSource = "Adjustments.ManualAdjustments";

        public DcfsManualAdjustmentRepository(string connectionString)
            : base(connectionString)
        {
        }

        public Guid[] GetRequiredPaymentIdsToReverse()
        {
            return Query<Guid>($"SELECT RequiredPaymentIdToReverse FROM {ManualAdjustmentsSource} WHERE RequiredPaymentIdForReversal IS NULL");
        }

        public void SetRequiredPaymentIdAsReversed(string requiredPaymentIdToReverse, string requiredPaymentIdForReversal)
        {
            Execute($"UPDATE {ManualAdjustmentsSource} SET RequiredPaymentIdForReversal = @requiredPaymentIdForReversal WHERE RequiredPaymentIdToReverse = @requiredPaymentIdToReverse",
                new { requiredPaymentIdToReverse, requiredPaymentIdForReversal });
        }
    }
}
