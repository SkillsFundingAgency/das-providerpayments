using SFA.DAS.CollectionEarnings.DataLock.Infrastructure.Data.Entities;
using SFA.DAS.Payments.DCFS.Infrastructure.Data;

namespace SFA.DAS.CollectionEarnings.DataLock.Infrastructure.Data.Repositories
{
    public class CommitmentRepository : DcfsRepository, ICommitmentRepository
    {
        private const string CommitmentSource = "DataLock.vw_Commitments";
        private const string CommitmentColumns = "CommitmentId," +
                                                 "VersionId," +
                                                 "Uln," +
                                                 "Ukprn," +
                                                 "ProviderUkprn," +
                                                 "AccountId," +
                                                 "StartDate," +
                                                 "EndDate," +
                                                 "AgreedCost," +
                                                 "StandardCode," +
                                                 "ProgrammeType," +
                                                 "FrameworkCode," +
                                                 "PathwayCode," +
                                                 "PaymentStatus," +
                                                 "PaymentStatusDescription," +
                                                 "Priority," +
                                                 "EffectiveFrom," +
                                                 "EffectiveTo";
        private const string SelectCommitments = "SELECT " + CommitmentColumns + " FROM " + CommitmentSource;
        private const string SelectProviderCommitments = SelectCommitments + " WHERE ProviderUkprn = @ukprn";

        public CommitmentRepository(string connectionString)
            : base(connectionString)
        {
        }

        public CommitmentEntity[] GetProviderCommitments(long ukprn)
        {
            return Query<CommitmentEntity>(SelectProviderCommitments, new { ukprn });
        }
    }
}