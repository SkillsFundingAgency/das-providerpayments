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
        private const string SelectCurrentProviderAdjustments = "SELECT " + CurrentAdjustmentColumns + " FROM " + CurrentAdjustmentSource + " ";

        private const string PreviousAdjustmentSource = "Reference.ProviderAdjustmentsHistory";
        private const string PreviousAdjustmentColumns = "Ukprn, "
                                                         + "SubmissionId, "
                                                         + "SubmissionCollectionPeriod, "
                                                         + "PaymentType, "
                                                         + "PaymentTypeName, "
                                                         + "Amount";
        private const string SelectPreviousProviderAdjustments = "SELECT " + PreviousAdjustmentColumns + " FROM " + PreviousAdjustmentSource + " ";

        public DcfsAdjustmentRepository(string connectionString)
            : base(connectionString)
        {
        }

        public AdjustmentEntity[] GetCurrentProviderAdjustments()
        {
            return Query<AdjustmentEntity>(SelectCurrentProviderAdjustments);
        }

        public AdjustmentEntity[] GetPreviousProviderAdjustments()
        {
            return Query<AdjustmentEntity>(SelectPreviousProviderAdjustments);
        }
    }
}