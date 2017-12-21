using System;
using System.Linq;
using NUnit.Framework;
using SFA.DAS.Provider.Events.DataLock.Domain;
using SFA.DAS.Provider.Events.DataLock.IntegrationTests.Execution;
using SFA.DAS.Provider.Events.DataLock.IntegrationTests.Helpers;

namespace SFA.DAS.Provider.Events.DataLock.IntegrationTests.Specs
{
    public class WhenANewEventIsGenerated
    {
        [SetUp]
        public void Arrange()
        {
            CommonTestDataHelper.Clean();
            CommonTestDataHelper.SetCurrentPeriodEnd();
        }

        [Test]
        public void ThenItShouldBeWrittenToTheDatabaseInASubmissionRun()
        {
            // Arrange
            var ukprn = 10000534;
            var commitmentId = 1;

            CommonTestDataHelper.AddLearningProvider(ukprn);
            CommonTestDataHelper.AddFileDetails(ukprn);
            CommonTestDataHelper.AddCommitment(commitmentId, ukprn, "Lrn-001", passedDataLock: false);
            CommonTestDataHelper.AddIlrDataForCommitment(commitmentId, "Lrn-001");

            CommonTestDataHelper.SubmissionCopyReferenceData();

            // Act
            TaskRunner.RunTask();

            // Assert
            var events = CommonTestDataHelper.GetAllEvents();

            Assert.IsNotNull(events);
            Assert.AreEqual(1, events.Length);

            var @event = events[0];

            //Assert.AreEqual(1, @event.Id);
            Assert.AreEqual(ukprn, @event.Ukprn);
            Assert.AreEqual(commitmentId, @event.CommitmentId);
            Assert.AreEqual(EventStatus.New, @event.Status);

            var eventErrors = CommonTestDataHelper.GetAllEventErrors(@event.DataLockEventId);
            var eventPeriods = CommonTestDataHelper.GetAllEventPeriods(@event.DataLockEventId);
            var eventCommitmentVersions = CommonTestDataHelper.GetAllEventCommitmentVersions(@event.DataLockEventId);

            Assert.IsNotNull(eventErrors);
            Assert.IsNotNull(eventPeriods);
            Assert.IsNotNull(eventCommitmentVersions);

            Assert.AreEqual(1, eventErrors.Length);
            Assert.AreEqual(36, eventPeriods.Length);
            Assert.AreEqual(1, eventCommitmentVersions.Length);
        }

        [Test]
        public void ThenItShouldBeWrittenToTheDatabaseInAPeriodEndRun()
        {
            // Arrange
            var ukprn = 10000534;
            var commitmentId = 1;

            CommonTestDataHelper.PeriodEndAddLearningProvider(ukprn);
            CommonTestDataHelper.PeriodEndAddCommitment(commitmentId, ukprn, "Lrn-001", passedDataLock: false);
            CommonTestDataHelper.PeriodEndAddIlrDataForCommitment(commitmentId, "Lrn-001");

            CommonTestDataHelper.PeriodEndCopyReferenceData();

            // Act
            TaskRunner.RunTask(eventsSource: EventSource.PeriodEnd);

            // Assert
            var events = CommonTestDataHelper.GetAllEvents(false);

            Assert.IsNotNull(events);
            Assert.AreEqual(1, events.Length);

            var @event = events[0];

            //Assert.AreEqual(1, @event.Id);
            Assert.AreEqual(ukprn, @event.Ukprn);
            Assert.AreEqual(commitmentId, @event.CommitmentId);
            Assert.AreEqual(EventStatus.New, @event.Status);

            var eventErrors = CommonTestDataHelper.GetAllEventErrors(@event.DataLockEventId, false);
            var eventPeriods = CommonTestDataHelper.GetAllEventPeriods(@event.DataLockEventId, false);
            var eventCommitmentVersions = CommonTestDataHelper.GetAllEventCommitmentVersions(@event.DataLockEventId, false);

            Assert.IsNotNull(eventErrors);
            Assert.IsNotNull(eventPeriods);
            Assert.IsNotNull(eventCommitmentVersions);

            Assert.AreEqual(1, eventErrors.Length);
            Assert.AreEqual(36, eventPeriods.Length);
            Assert.AreEqual(1, eventCommitmentVersions.Length);
        }

        [Test]
        public void ThenItShouldWriteMultipleEventsWhenErrorsForMultipleCommitmentsOnTheSamePriceEpisodeAreProduced()
        {
            // Arrange
            var ukprn = 10000534;
            var commitmentId1 = 1;
            var commitmentId2 = 2;

            CommonTestDataHelper.AddLearningProvider(ukprn);
            CommonTestDataHelper.AddFileDetails(ukprn);
            CommonTestDataHelper.AddCommitment(commitmentId1, ukprn, "Lrn-001", passedDataLock: false);
            CommonTestDataHelper.AddCommitment(commitmentId2, ukprn, "Lrn-001", passedDataLock: false);
            CommonTestDataHelper.AddIlrDataForCommitment(commitmentId1, "Lrn-001");

            CommonTestDataHelper.SubmissionCopyReferenceData();

            // Act
            TaskRunner.RunTask();

            // Assert
            var events = CommonTestDataHelper.GetAllEvents();

            Assert.IsNotNull(events);
            Assert.AreEqual(2, events.Length);
            Assert.AreEqual(1, events.Count(x => x.CommitmentId == commitmentId1), "More than 1 event for commitment 1");
            Assert.AreEqual(1, events.Count(x => x.CommitmentId == commitmentId2), "More than 1 event for commitment 2");
        }

        [Test, Explicit]
        public void ThenItShouldCompleteInAnAcceptableTimeInASubmissionRun()
        {
            // Arrange
            var ukprn = 10000534;
            var numberOfLearners = 1200;

            CommonTestDataHelper.AddLearningProvider(ukprn);
            CommonTestDataHelper.AddFileDetails(ukprn);
            for (var i = 1; i <= numberOfLearners; i++)
            {
                var learnRefNumber = $"Lrn-{i:0000}";
                CommonTestDataHelper.AddCommitment(i, ukprn, learnRefNumber, passedDataLock: false);
                CommonTestDataHelper.AddIlrDataForCommitment(i, learnRefNumber);
            }
            CommonTestDataHelper.SubmissionCopyReferenceData();

            // Act
            var startTime = DateTime.Now;
            TaskRunner.RunTask();

            // Assert
            var duration = DateTime.Now - startTime;
            Console.WriteLine($"Execution took {duration.TotalSeconds:0.0}");
            Assert.IsTrue(duration.TotalSeconds < 30, $"Expected to complete in less than 30 seconds but took {duration.TotalSeconds:0.0}");
        }

        [Test, Explicit]
        public void ThenItShouldCompleteInAnAcceptableTimeInAPeriodEndRun()
        {
            // Arrange
            var ukprn = 10000534;
            var numberOfLearners = 20000;

            CommonTestDataHelper.PeriodEndAddLearningProvider(ukprn);
            for (var i = 1; i <= numberOfLearners; i++)
            {
                var learnRefNumber = $"Lrn-{i:0000}";
                CommonTestDataHelper.PeriodEndAddCommitment(i, ukprn, learnRefNumber, passedDataLock: false);
                CommonTestDataHelper.PeriodEndAddIlrDataForCommitment(i, learnRefNumber);
            }

            CommonTestDataHelper.PeriodEndCopyReferenceData();

            // Act
            var startTime = DateTime.Now;
            TaskRunner.RunTask(eventsSource: EventSource.PeriodEnd);

            // Assert
            var duration = DateTime.Now - startTime;
            Console.WriteLine($"Execution took {duration.TotalSeconds:0.0}");
            Assert.IsTrue(duration.TotalSeconds < 600, $"Expected to complete in less than 300 seconds but took {duration.TotalSeconds:0.0}");
        }

    }
}