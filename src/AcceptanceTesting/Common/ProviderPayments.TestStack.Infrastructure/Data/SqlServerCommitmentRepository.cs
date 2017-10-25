using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProviderPayments.TestStack.Domain.Data;
using ProviderPayments.TestStack.Domain.Data.Entities;

namespace ProviderPayments.TestStack.Infrastructure.Data
{
    public class SqlServerCommitmentRepository : SqlServerRepository, ICommitmentRepository
    {
        public SqlServerCommitmentRepository()
            : base("DedsConnectionString")
        {
        }

        public async Task<IEnumerable<CommitmentEntity>> All()
        {
            return await Query<CommitmentEntity>(@"SELECT CommitmentId Id
                                                         ,Uln
                                                         ,Ukprn
                                                         ,AccountId
                                                         ,StartDate
                                                         ,EndDate
                                                         ,AgreedCost Cost
                                                         ,StandardCode
                                                         ,ProgrammeType
                                                         ,FrameworkCode
                                                         ,PathwayCode
                                                         ,Priority
                                                         ,VersionId Version
                                                         ,PaymentStatus
                                                         ,PaymentStatusDescription
                                                         ,EffectiveFromDate EffectiveFrom
                                                   FROM dbo.DasCommitments
                                                   ORDER BY Ukprn, Priority");
        }

        public async Task<CommitmentEntity> Single(CommitmentEntityId id)
        {
            return (await Query<CommitmentEntity>(@"SELECT CommitmentId Id
                                                          ,Uln
                                                          ,Ukprn
                                                          ,AccountId
                                                          ,StartDate
                                                          ,EndDate
                                                          ,AgreedCost Cost
                                                          ,StandardCode
                                                          ,ProgrammeType
                                                          ,FrameworkCode
                                                          ,PathwayCode
                                                          ,Priority
                                                          ,VersionId Version
                                                          ,PaymentStatus
                                                          ,PaymentStatusDescription
                                                          ,EffectiveFromDate EffectiveFrom
                                                    FROM dbo.DasCommitments
                                                    WHERE CommitmentId = @id AND VersionId = @version", new { id = id.Id, version = id.Version })).SingleOrDefault();
        }

        public async Task Create(CommitmentEntity entity)
        {
            await Execute(@"INSERT INTO dbo.DasCommitments
                            (CommitmentId,Uln,Ukprn,AccountId,StartDate,EndDate,AgreedCost,StandardCode,ProgrammeType,FrameworkCode,PathwayCode,Priority,VersionId,PaymentStatus,PaymentStatusDescription,EffectiveFromDate)
                            VALUES
                            (@Id,@Uln,@Ukprn,@AccountId,@StartDate,@EndDate,@Cost,@StandardCode,@ProgrammeType,@FrameworkCode,@PathwayCode,@Priority,@Version,@PaymentStatus,@PaymentStatusDescription,@EffectiveFrom)",
                            entity);
        }

        public async Task Update(CommitmentEntity entity)
        {
            await Execute(@"UPDATE dbo.DasCommitments
                            SET Uln = @Uln,
                                Ukprn = @Ukprn,
                                AccountId = @AccountId,
                                StartDate = @StartDate,
                                EndDate = @EndDate,
                                AgreedCost = @Cost,
                                StandardCode = @StandardCode,
                                ProgrammeType = @ProgrammeType,
                                FrameworkCode = @FrameworkCode,
                                PathwayCode = @PathwayCode,
                                Priority = @Priority,
                                EffectiveFromDate = @EffectiveFrom,
                                PaymentStatus = @PaymentStatus,
                                PaymentStatusDescription = @PaymentStatusDescription
                            WHERE CommitmentId = @Id
                                AND VersionId = @Version",
                          entity);
        }

        public async Task Delete(CommitmentEntityId id)
        {
            await Execute("DELETE FROM dbo.DasCommitments WHERE CommitmentId = @id AND VersionId = @version", new { id = id.Id, version = id.Version });
        }

        public async Task UpdateEventStreamPointer()
        {
            await Execute("INSERT INTO EventStreamPointer SELECT ISNULL(MAX(EventId),0) + 1, GETDATE() FROM EventStreamPointer");
        }
    }
}
