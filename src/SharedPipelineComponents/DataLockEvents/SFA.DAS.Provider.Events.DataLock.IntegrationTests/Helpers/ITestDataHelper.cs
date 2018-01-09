using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFA.DAS.Provider.Events.DataLock.Domain;

namespace SFA.DAS.Provider.Events.DataLock.IntegrationTests.Helpers
{
    public interface ITestDataHelper
    {
        void Clean();
        void CopyReferenceData();
        void AddLearningProvider(long ukprn);
        void SetCurrentPeriodEnd();
        void AddFileDetails(long ukprn, bool successful = true);

        void AddCommitment(long id,
            long ukprn,
            string learnerRefNumber,
            int aimSequenceNumber = 1,
            long uln = 0L,
            DateTime startDate = default(DateTime),
            DateTime endDate = default(DateTime),
            decimal agreedCost = 15000m,
            long? standardCode = null,
            int? programmeType = null,
            int? frameworkCode = null,
            int? pathwayCode = null,
            bool passedDataLock = true);

        void AddIlrDataForCommitment(long? commitmentId,
            string learnerRefNumber,
            int aimSequenceNumber = 1);

        void AddDataLockEvent(long ukprn,
            string learnerRefNumber,
            int aimSequenceNumber = 1,
            string priceEpisodeIdentifier = null,
            long uln = 0L,
            long commitmentId = 1,
            DateTime startDate = default(DateTime),
            DateTime endDate = default(DateTime),
            decimal agreedCost = 15000m,
            DateTime priceEffectiveFromDate = default(DateTime),
            long? standardCode = null,
            int? programmeType = null,
            int? frameworkCode = null,
            int? pathwayCode = null,
            bool passedDataLock = true,
            EventStatus status = EventStatus.New);

        DataLockEvent[] GetAllEvents();
        DataLockEventError[] GetAllEventErrors(Guid eventId);
        DataLockEventPeriod[] GetAllEventPeriods(Guid eventId);
        DataLockEventCommitmentVersion[] GetAllEventCommitmentVersions(Guid eventId);
    }
}
