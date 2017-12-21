using System.Linq;
using NUnit.Framework;
using SFA.DAS.Provider.Events.DataLock.Domain;
using SFA.DAS.Provider.Events.DataLock.IntegrationTests.Execution;
using SFA.DAS.Provider.Events.DataLock.IntegrationTests.Helpers;

namespace SFA.DAS.Provider.Events.DataLock.IntegrationTests.Specs
{
    public class WhenAnExistingEventIsNonLongerFound
    {
        private const long Ukprn = 10000534;
        private const int CommitmentId = 1;
        private const string LearnerRefNumber = "Lrn-001";
        private const string PriceEpisodeIdentifier = "1-1-1-2017-04-01";

        [SetUp]
        public void Arrange()
        {
            CommonTestDataHelper.Clean();

            CommonTestDataHelper.SetCurrentPeriodEnd();
            CommonTestDataHelper.AddLearningProvider(Ukprn);
            CommonTestDataHelper.AddFileDetails(Ukprn);
            CommonTestDataHelper.AddCommitment(CommitmentId, Ukprn, LearnerRefNumber, passedDataLock: false);
            CommonTestDataHelper.AddIlrDataForCommitment(CommitmentId, LearnerRefNumber);

            CommonTestDataHelper.AddDataLockEvent(Ukprn, LearnerRefNumber, passedDataLock: false, priceEpisodeIdentifier: PriceEpisodeIdentifier);
        }

        [Test]
        public void ThenItShouldWriteADeletionEventDuringSubmission()
        {
            // Arrange
            CommonTestDataHelper.SubmissionCopyReferenceData();
            
            // Act
            TaskRunner.RunTask();

            // Assert
            var events = CommonTestDataHelper.GetAllEvents();
            var actualEvent = events?.SingleOrDefault(e => e.PriceEpisodeIdentifier == PriceEpisodeIdentifier);

            Assert.IsNotNull(actualEvent);
            Assert.AreEqual(EventStatus.Removed, actualEvent.Status);
            Assert.AreEqual(PriceEpisodeIdentifier, actualEvent.PriceEpisodeIdentifier);
            Assert.AreEqual(LearnerRefNumber, actualEvent.LearnRefnumber);
        }

        [Test]
        public void ThenItShouldNotWriteADeletionEventIfTheLastSeenStatusIsRemoved()
        {
            CommonTestDataHelper.SubmissionCopyReferenceData();

            // Act
            TaskRunner.RunTask();

            // Assert
            var events = CommonTestDataHelper.GetAllEvents();
            var actualEvent = events?.SingleOrDefault(e => e.PriceEpisodeIdentifier == PriceEpisodeIdentifier);

            Assert.IsNotNull(actualEvent);
            Assert.AreEqual(EventStatus.Removed, actualEvent.Status);
            Assert.AreEqual(PriceEpisodeIdentifier, actualEvent.PriceEpisodeIdentifier);
            Assert.AreEqual(LearnerRefNumber, actualEvent.LearnRefnumber);
        }
    }
}
