using System.Collections.Generic;
using SFA.DAS.Payments.DCFS.Infrastructure.Data;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data.Entities;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data.Repositories
{
    public class CommitmentRepository : DcfsRepository, ICommitmentRepository
    {
        public CommitmentRepository(string connectionString)
            : base(connectionString)
        {
        }

        public IList<Commitment> GetProviderCommitments(long ukprn)
        {
            const string sql = @"
                    SELECT  
                        ULN,
                        Ukprn, 
                        CommitmentId, 
                        C.VersionId [CommitmentVersionId],
                        C.AccountId, 
                        A.VersionId [AccountVersionId],
                        StartDate,
                        EndDate,
                        AgreedCost,
                        StandardCode,
                        ProgrammeType,
                        FrameworkCode,
                        PathwayCode,
                        PaymentStatus,
                        Priority,
                        EffectiveFrom,
                        EffectiveTo,
                        LegalEntityName,
                        TransferSendingEmployerAccountId,
                        TransferApprovalDate,
                        A.IsLevyPayer
                        
                    FROM Reference.DasCommitments C
                    LEFT JOIN Reference.DasAccounts A
                    ON C.AccountId = A.AccountId
                    WHERE Ukprn = @ukprn
                ";

            return Query<Commitment>(sql, new {ukprn});
        }
    }

    public interface ICommitmentRepository
    {
        IList<Commitment> GetProviderCommitments(long ukprn);
    }
}