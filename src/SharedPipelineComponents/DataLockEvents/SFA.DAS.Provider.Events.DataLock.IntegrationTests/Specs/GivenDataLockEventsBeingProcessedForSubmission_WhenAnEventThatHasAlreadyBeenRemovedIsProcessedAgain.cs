using System.Linq;
using NUnit.Framework;
using SFA.DAS.Provider.Events.DataLock.Domain;
using SFA.DAS.Provider.Events.DataLock.IntegrationTests.Execution;
using SFA.DAS.Provider.Events.DataLock.IntegrationTests.Helpers;

namespace SFA.DAS.Provider.Events.DataLock.IntegrationTests.Specs
{
    public class GivenDataLockEventsBeingProcessedForSubmission_WhenAnEventThatHasAlreadyBeenRemovedIsProcessedAgain
    {
        private DataLockEvent[] _actualEvents;
        private const long Ukprn = 10000534;
        private const string LearnerRefNumber = "Lrn-001";
        private const string PriceEpisodeIdentifier = "Ep 1";

        [SetUp]
        public void SetUp()
        {
            TestDataHelper.Clean();

            TestDataHelper.SetCurrentPeriodEnd();
            TestDataHelper.AddLearningProvider(Ukprn);
            TestDataHelper.AddFileDetails(Ukprn);

            TestDataHelper.AddDataLockEvent(Ukprn, LearnerRefNumber, priceEpisodeIdentifier: PriceEpisodeIdentifier, status: EventStatus.New);
            TestDataHelper.AddDataLockEvent(Ukprn, LearnerRefNumber, priceEpisodeIdentifier: PriceEpisodeIdentifier, status: EventStatus.Removed);

            TestDataHelper.SubmissionCopyReferenceData();

            TaskRunner.RunTask();

            _actualEvents = TestDataHelper.GetAllEvents();
        }

        [Test]
        public void ThenAnEventShouldNotExistForThePriceEpisodeIdentifier()
        {
            Assert.IsNull(_actualEvents.SingleOrDefault(e => e.PriceEpisodeIdentifier == PriceEpisodeIdentifier));
        }
    }
}