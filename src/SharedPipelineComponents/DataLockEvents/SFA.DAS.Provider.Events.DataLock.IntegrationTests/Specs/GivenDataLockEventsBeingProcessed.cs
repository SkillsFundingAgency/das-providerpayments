using System.Linq;
using NUnit.Framework;
using SFA.DAS.Provider.Events.DataLock.Domain;
using SFA.DAS.Provider.Events.DataLock.IntegrationTests.Execution;
using SFA.DAS.Provider.Events.DataLock.IntegrationTests.Helpers;

namespace SFA.DAS.Provider.Events.DataLock.IntegrationTests.Specs
{
    public class GivenDataLockEventsBeingProcessed_WhenAnEventThatHasAlreadyBeenRemovedIsProcessedAgain
    {
        private const long Ukprn = 10000534;
        private const int CommitmentId = 1;
        private const string LearnerRefNumber = "Lrn-001";
        private const string PriceEpisodeIdentifier = "1-1-1-2017-04-01";
        private const string Submission = "Submission";
        private const string PeriodEnd = "PeriodEnd";

        [SetUp]
        public void SetUp()
        {
            TestDataHelper.Clean();

            TestDataHelper.SetCurrentPeriodEnd();
            TestDataHelper.AddLearningProvider(Ukprn);
            TestDataHelper.AddFileDetails(Ukprn);
            TestDataHelper.AddCommitment(CommitmentId, Ukprn, LearnerRefNumber, passedDataLock: false);
            TestDataHelper.AddIlrDataForCommitment(CommitmentId, LearnerRefNumber);

            TestDataHelper.AddReferenceDataLockEvent(Ukprn, (int)EventStatus.Removed);
        }

        [TestCase(Submission)]
        [TestCase(PeriodEnd)]
        public void ThenAnEventShouldNotExistForThePriceEpisodeIdentifierForEachContext(string context)
        {
            if(context == Submission)
                TestDataHelper.SubmissionCopyReferenceData();
            else if(context == PeriodEnd)
                TestDataHelper.PeriodEndCopyReferenceData();

            TaskRunner.RunTask();

            Assert.IsNull(TestDataHelper.GetAllEvents()?.SingleOrDefault(e => e.PriceEpisodeIdentifier == PriceEpisodeIdentifier));
        }
    }
}