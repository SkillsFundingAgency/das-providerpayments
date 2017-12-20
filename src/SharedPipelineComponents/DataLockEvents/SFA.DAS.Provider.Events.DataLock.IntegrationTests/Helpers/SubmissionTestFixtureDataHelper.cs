using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using SFA.DAS.Payments.DCFS.Domain;
using SFA.DAS.Payments.DCFS.Extensions;
using SFA.DAS.Provider.Events.DataLock.Domain;
using SFA.DAS.Provider.Events.DataLock.IntegrationTests.TestContext;

namespace SFA.DAS.Provider.Events.DataLock.IntegrationTests.Helpers
{
    public class SubmissionTestFixtureDataHelper : ITestFixtureDataHelper
    {
        public void Clean()
        {
            TestDataHelper.Clean();
        }

        public void CopyReferenceData()
        {
            TestDataHelper.SubmissionCopyReferenceData();
        }

        public void AddLearningProvider(long ukprn)
        {
            TestDataHelper.AddLearningProvider(ukprn);
        }

        public void SetCurrentPeriodEnd()
        {
            TestDataHelper.SetCurrentPeriodEnd();
        }

        public void AddFileDetails(long ukprn, bool successful = true)
        {
            TestDataHelper.AddFileDetails(ukprn, successful);
        }

        public void AddCommitment(long id, long ukprn, string learnerRefNumber, int aimSequenceNumber = 1, long uln = 0,
            DateTime startDate = new DateTime(), DateTime endDate = new DateTime(), decimal agreedCost = 15000,
            long? standardCode = null, int? programmeType = null, int? frameworkCode = null, int? pathwayCode = null,
            bool passedDataLock = true)
        {
            TestDataHelper.AddCommitment(id, ukprn, learnerRefNumber, aimSequenceNumber, uln, startDate, endDate, agreedCost, standardCode, programmeType, frameworkCode, pathwayCode, passedDataLock);
        }

        public void AddIlrDataForCommitment(long? commitmentId, string learnerRefNumber, int aimSequenceNumber = 1)
        {
            TestDataHelper.AddIlrDataForCommitment(commitmentId, learnerRefNumber, aimSequenceNumber);
        }

        public void AddDataLockEvent(long ukprn, string learnerRefNumber, int aimSequenceNumber = 1,
            string priceEpisodeIdentifier = null, long uln = 0, long commitmentId = 1, DateTime startDate = new DateTime(),
            DateTime endDate = new DateTime(), decimal agreedCost = 15000, DateTime priceEffectiveFromDate = new DateTime(),
            long? standardCode = null, int? programmeType = null, int? frameworkCode = null, int? pathwayCode = null,
            bool passedDataLock = true, int status=1)
        {
            TestDataHelper.AddDataLockEvent(ukprn, learnerRefNumber, aimSequenceNumber,priceEpisodeIdentifier,uln,commitmentId, startDate, endDate, agreedCost, priceEffectiveFromDate, standardCode, programmeType, frameworkCode, pathwayCode, passedDataLock, status);
        }

        public void AddReferenceDataLockEvent(long ukprn, int status = 1)
        {
            TestDataHelper.AddReferenceDataLockEvent(ukprn, status);
        }

        public DataLockEvent[] GetAllEvents()
        {
            return TestDataHelper.GetAllEvents(true);
        }

        public DataLockEventError[] GetAllEventErrors(Guid eventId)
        {
            return TestDataHelper.GetAllEventErrors(eventId, true);
        }

        public DataLockEventPeriod[] GetAllEventPeriods(Guid eventId)
        {
            return TestDataHelper.GetAllEventPeriods(eventId, true);
        }

        public DataLockEventCommitmentVersion[] GetAllEventCommitmentVersions(Guid eventId)
        {
            return TestDataHelper.GetAllEventCommitmentVersions(eventId, true);
        }
    }
}
