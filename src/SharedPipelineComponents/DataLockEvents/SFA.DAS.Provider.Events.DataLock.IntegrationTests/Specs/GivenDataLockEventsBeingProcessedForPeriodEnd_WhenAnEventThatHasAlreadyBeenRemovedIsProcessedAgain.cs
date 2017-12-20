using System.Linq;
using NUnit.Framework;
using SFA.DAS.Provider.Events.DataLock.Domain;
using SFA.DAS.Provider.Events.DataLock.IntegrationTests.Execution;
using SFA.DAS.Provider.Events.DataLock.IntegrationTests.Helpers;

namespace SFA.DAS.Provider.Events.DataLock.IntegrationTests.Specs
{
    public class GivenDataLockEventsBeingProcessedForPeriodEnd_WhenAnEventThatHasAlreadyBeenRemovedIsProcessedAgain
    {
        private DataLockEvent[] _actualEvents;
        private const long Ukprn = 10000534;
        private const int CommitmentId = 1;
        private const string LearnerRefNumber = "Lrn-001";
        private const string PriceEpisodeIdentifier = "1-1-1-2017-04-01";

        [SetUp]
        public void SetUp()
        {
            TestDataHelper.Clean();

            TestDataHelper.SetCurrentPeriodEnd();
            TestDataHelper.PeriodEndAddLearningProvider(Ukprn);
            TestDataHelper.PeriodEndAddCommitment(CommitmentId, Ukprn, LearnerRefNumber, passedDataLock: false);
            TestDataHelper.PeriodEndAddIlrDataForCommitment(CommitmentId, LearnerRefNumber);

            TestDataHelper.AddReferenceDataLockEvent(Ukprn, (int)EventStatus.Removed, inSubmission: false);
            TestDataHelper.PeriodEndCopyReferenceData();

            TaskRunner.RunTask(eventsSource: EventSource.PeriodEnd);

            _actualEvents = TestDataHelper.GetAllEvents(false);
        }

        [Test]
        public void ThenAnEventShouldNotExistForThePriceEpisodeIdentifier()
        {
            Assert.IsNull(_actualEvents.SingleOrDefault(e => e.PriceEpisodeIdentifier == PriceEpisodeIdentifier));
        }
    }
}