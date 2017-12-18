using System.Linq;
using NUnit.Framework;
using SFA.DAS.Provider.Events.DataLock.Domain;
using SFA.DAS.Provider.Events.DataLock.IntegrationTests.Execution;
using SFA.DAS.Provider.Events.DataLock.IntegrationTests.Helpers;

namespace SFA.DAS.Provider.Events.DataLock.IntegrationTests.Specs
{
    public class GivenDataLockEventsBeingProcessed_WhenAnEventThatHasAlreadyBeenRemovedIsProcessedAgain
    {
        private DataLockEvent _actualEvent;
        private const long Ukprn = 10000534;
        private const int CommitmentId = 1;
        private const string LearnerRefNumber = "Lrn-001";
        private const string PriceEpisodeIdentifier = "1-1-1-2017-04-01";

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

            TestDataHelper.SubmissionCopyReferenceData();

            TaskRunner.RunTask();

            var events = TestDataHelper.GetAllEvents();
            _actualEvent = events?.SingleOrDefault(e => e.PriceEpisodeIdentifier == PriceEpisodeIdentifier);
        }

        [Test]
        public void ThenAnEventShouldNotExistForThePriceEpisodeIdentifier()
        {
            Assert.IsNull(_actualEvent);
        }
    }
}