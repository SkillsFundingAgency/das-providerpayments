using SFA.DAS.Payments.Calc.ProviderAdjustments.Infrastructure.Data.Entities;
using SFA.DAS.Payments.DCFS.Infrastructure.Data;

namespace SFA.DAS.Payments.Calc.ProviderAdjustments.Infrastructure.Data.Repositories
{
    public class DcfsAdjustmentRepository : DcfsRepository, IAdjustmentRepository
    {
        private const string CurrentAdjustmentSource = "Reference.ProviderAdjustmentsCurrent";
        private const string CurrentAdjustmentColumns = "Ukprn, "
                                                         + "SubmissionId, "
                                                         + "SubmissionCollectionPeriod, "
                                                         + "PaymentType, "
                                                         + "PaymentTypeName, "
                                                         + "Amount";
        private const string SelectCurrentProviderAdjustments = "SELECT " + CurrentAdjustmentColumns + " FROM " + CurrentAdjustmentSource + " WHERE Ukprn = @ukprn";

        private const string PreviousAdjustmentSource = "Reference.ProviderAdjustmentsHistory";
        private const string PreviousAdjustmentColumns = "Ukprn, "
                                                         + "SubmissionId, "
                                                         + "SubmissionCollectionPeriod, "
                                                         + "PaymentType, "
                                                         + "PaymentTypeName, "
                                                         + "Amount";
        private const string SelectPreviousProviderAdjustments = "SELECT " + PreviousAdjustmentColumns + " FROM " + PreviousAdjustmentSource + " WHERE Ukprn = @ukprn";

        public DcfsAdjustmentRepository(string connectionString)
            : base(connectionString)
        {
        }

        public AdjustmentEntity[] GetCurrentProviderAdjustments(long ukprn)
        {
            return Query<AdjustmentEntity>(SelectCurrentProviderAdjustments, new { ukprn });
        }

        public AdjustmentEntity[] GetPreviousProviderAdjustments(long ukprn)
        {
            return Query<AdjustmentEntity>(SelectPreviousProviderAdjustments, new { ukprn });
        }
    }
}