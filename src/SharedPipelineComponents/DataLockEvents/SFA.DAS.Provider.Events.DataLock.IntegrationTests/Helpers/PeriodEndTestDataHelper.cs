using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFA.DAS.Provider.Events.DataLock.Domain;

namespace SFA.DAS.Provider.Events.DataLock.IntegrationTests.Helpers
{
    public class PeriodEndTestDataHelper : ITestDataHelper
    {
        public void Clean()
        {
            CommonTestDataHelper.Clean();
        }

        public void CopyReferenceData()
        {
            CommonTestDataHelper.PeriodEndCopyReferenceData();
        }

        public void AddLearningProvider(long ukprn)
        {
            CommonTestDataHelper.PeriodEndAddLearningProvider(ukprn);
        }

        public void SetCurrentPeriodEnd()
        {
            CommonTestDataHelper.SetCurrentPeriodEnd();
        }

        public void AddFileDetails(long ukprn, bool successful = true)
        {
            CommonTestDataHelper.AddFileDetails(ukprn, successful);
        }

        public void AddCommitment(long id, long ukprn, string learnerRefNumber, int aimSequenceNumber = 1, long uln = 0,
            DateTime startDate = new DateTime(), DateTime endDate = new DateTime(), decimal agreedCost = 15000,
            long? standardCode = null, int? programmeType = null, int? frameworkCode = null, int? pathwayCode = null,
            bool passedDataLock = true)
        {
            CommonTestDataHelper.PeriodEndAddCommitment(id, ukprn, learnerRefNumber, aimSequenceNumber, uln, startDate, endDate, agreedCost, standardCode, programmeType, frameworkCode, pathwayCode, passedDataLock);
        }

        public void AddIlrDataForCommitment(long? commitmentId, string learnerRefNumber, int aimSequenceNumber = 1)
        {
            CommonTestDataHelper.PeriodEndAddIlrDataForCommitment(commitmentId, learnerRefNumber, aimSequenceNumber);
        }

        public void AddDataLockEvent(long ukprn, string learnerRefNumber, int aimSequenceNumber = 1,
            string priceEpisodeIdentifier = null, long uln = 0, long commitmentId = 1, DateTime startDate = new DateTime(),
            DateTime endDate = new DateTime(), decimal agreedCost = 15000, DateTime priceEffectiveFromDate = new DateTime(),
            long? standardCode = null, int? programmeType = null, int? frameworkCode = null, int? pathwayCode = null,
            bool passedDataLock = true, EventStatus status = EventStatus.New)
        {
            CommonTestDataHelper.AddDataLockEvent(ukprn, learnerRefNumber, aimSequenceNumber, priceEpisodeIdentifier, uln, commitmentId, startDate, endDate, agreedCost, priceEffectiveFromDate, standardCode, programmeType, frameworkCode, pathwayCode, passedDataLock, status);
        }

        public DataLockEvent[] GetAllEvents()
        {
            return CommonTestDataHelper.GetAllEvents(false);
        }

        public DataLockEventError[] GetAllEventErrors(Guid eventId)
        {
            return CommonTestDataHelper.GetAllEventErrors(eventId, false);
        }

        public DataLockEventPeriod[] GetAllEventPeriods(Guid eventId)
        {
            return CommonTestDataHelper.GetAllEventPeriods(eventId, false);
        }

        public DataLockEventCommitmentVersion[] GetAllEventCommitmentVersions(Guid eventId)
        {
            return CommonTestDataHelper.GetAllEventCommitmentVersions(eventId, false);
        }
    }
}
