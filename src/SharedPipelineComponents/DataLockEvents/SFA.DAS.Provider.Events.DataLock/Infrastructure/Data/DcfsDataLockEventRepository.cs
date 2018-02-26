using System.Linq;
using SFA.DAS.Payments.DCFS.Infrastructure.Data;
using SFA.DAS.Provider.Events.DataLock.Domain.Data;
using SFA.DAS.Provider.Events.DataLock.Domain.Data.Entities;

namespace SFA.DAS.Provider.Events.DataLock.Infrastructure.Data
{
    public class DcfsDataLockEventRepository : DcfsRepository, IDataLockEventRepository
    {
        private const string SelectProviderLastSeenEvents = @"SELECT Id,
                                                                     DataLockEventId,
                                                                     ProcessDateTime,
                                                                     Status,
                                                                     IlrFileName,
                                                                     SubmittedDateTime,
                                                                     AcademicYear,
                                                                     UKPRN,
                                                                     ULN,
                                                                     LearnRefNumber,
                                                                     AimSeqNumber,
                                                                     PriceEpisodeIdentifier,
                                                                     CommitmentId,
                                                                     EmployerAccountId,
                                                                     EventSource,
                                                                     HasErrors,
                                                                     IlrStartDate,
                                                                     IlrStandardCode,
                                                                     IlrProgrammeType,
                                                                     IlrFrameworkCode,
                                                                     IlrPathwayCode,
                                                                     IlrTrainingPrice,
                                                                     IlrEndpointAssessorPrice,
                                                                     IlrPriceEffectiveFromDate,
                                                                     IlrPriceEffectiveToDate
                                                                FROM Reference.DataLockEvents
                                                                WHERE Ukprn = @ukprn";


        public DcfsDataLockEventRepository(string connectionString)
            : base(connectionString)
        {
        }

        public DataLockEventEntity[] GetProviderLastSeenEvents(long ukprn)
        {
            return Query<DataLockEventEntity>(SelectProviderLastSeenEvents, new { ukprn });
        }

        public void BulkWriteDataLockEvents(DataLockEventEntity[] events)
        {
            var columns = typeof(DataLockEventEntity).GetProperties().ToDictionary(p => p.Name, p =>
            {
                if (p.Name == "Ukprn" || p.Name == "Uln") return p.Name.ToUpperInvariant();
                return p.Name;
            });

            ExecuteBatch(events, "DataLockEvents.DataLockEvents", columns);
        }
    }
}