using System;
using System.Diagnostics;
using System.Linq;
using NUnit.Framework;
using SFA.DAS.Provider.Events.DataLock.Domain;
using SFA.DAS.Provider.Events.DataLock.IntegrationTests.Execution;
using SFA.DAS.Provider.Events.DataLock.IntegrationTests.Helpers;

namespace SFA.DAS.Provider.Events.DataLock.IntegrationTests.Specs
{
    public class WhenANewEventIsGenerated
    {
        private ITestDataHelper _helper;

        private void Setup(TestFixtureContext context)
        {
            _helper = TestDataHelper.Get(context);
            _helper.Clean();
            _helper.SetCurrentPeriodEnd();
        }

        [TestCase(TestFixtureContext.Submission)]
        [TestCase(TestFixtureContext.PeriodEnd)]
        public void ThenItShouldBeWrittenToTheDatabase(TestFixtureContext context)
        {
            // Arrange
            Setup(context);

            var ukprn = 10000534;
            var commitmentId = 1;

            _helper.AddLearningProvider(ukprn);
            _helper.AddFileDetails(ukprn);
            _helper.AddCommitment(commitmentId, ukprn, "Lrn-001", passedDataLock: false);
            _helper.AddIlrDataForCommitment(commitmentId, "Lrn-001");

            _helper.CopyReferenceData();

            //Act
            TaskRunner.RunTask(eventsSource:
                context == TestFixtureContext.PeriodEnd
                ? EventSource.PeriodEnd
                : EventSource.Submission);


            // Assert
            var events = _helper.GetAllEvents();

            Assert.IsNotNull(events);
            Assert.AreEqual(1, events.Length);

            var @event = events[0];

            //Assert.AreEqual(1, @event.Id);
            Assert.AreEqual(ukprn, @event.Ukprn);
            Assert.AreEqual(commitmentId, @event.CommitmentId);
            Assert.AreEqual(EventStatus.New, @event.Status);

            var eventErrors = _helper.GetAllEventErrors(@event.DataLockEventId);
            var eventPeriods = _helper.GetAllEventPeriods(@event.DataLockEventId);
            var eventCommitmentVersions = _helper.GetAllEventCommitmentVersions(@event.DataLockEventId);

            Assert.IsNotNull(eventErrors);
            Assert.IsNotNull(eventPeriods);
            Assert.IsNotNull(eventCommitmentVersions);

            Assert.AreEqual(1, eventErrors.Length);
            Assert.AreEqual(48, eventPeriods.Length);
            Assert.AreEqual(1, eventCommitmentVersions.Length);
        }

        [TestCase(TestFixtureContext.Submission)]
        [TestCase(TestFixtureContext.PeriodEnd)]
        public void ThenItShouldWriteMultipleEventsWhenErrorsForMultipleCommitmentsOnTheSamePriceEpisodeAreProduced(TestFixtureContext context)
        {
            // Arrange
            Setup(context);

            var ukprn = 10000534;
            var commitmentId1 = 1;
            var commitmentId2 = 2;

            _helper.AddLearningProvider(ukprn);
            _helper.AddFileDetails(ukprn);
            _helper.AddCommitment(commitmentId1, ukprn, "Lrn-001", passedDataLock: false);
            _helper.AddCommitment(commitmentId2, ukprn, "Lrn-001", passedDataLock: false);
            _helper.AddIlrDataForCommitment(commitmentId1, "Lrn-001");

            _helper.CopyReferenceData();

            // Act
            TaskRunner.RunTask(eventsSource:
                context == TestFixtureContext.PeriodEnd
                ? EventSource.PeriodEnd
                : EventSource.Submission);

            // Assert
            var events = _helper.GetAllEvents();

            Assert.IsNotNull(events);
            Assert.AreEqual(2, events.Length);
            Assert.AreEqual(1, events.Count(x => x.CommitmentId == commitmentId1), "More than 1 event for commitment 1");
            Assert.AreEqual(1, events.Count(x => x.CommitmentId == commitmentId2), "More than 1 event for commitment 2");
        }

        [Explicit]
        [TestCase(TestFixtureContext.Submission, 1200, 30)]
        [TestCase(TestFixtureContext.PeriodEnd, 20000, 300)]
        public void ThenItShouldCompleteInAnAcceptableTime(TestFixtureContext context,  int numberOfLearners, int expectedMaxElapsed)
        {
            // Arrange
            Setup(TestFixtureContext.Submission);

            var ukprn = 10000534;

            _helper.AddLearningProvider(ukprn);
            _helper.AddFileDetails(ukprn);
            for (var i = 1; i <= numberOfLearners; i++)
            {
                var learnRefNumber = $"Lrn-{i:0000}";
                _helper.AddCommitment(i, ukprn, learnRefNumber, passedDataLock: false);
                _helper.AddIlrDataForCommitment(i, learnRefNumber);
            }
            _helper.CopyReferenceData();

            // Act
            var stopwatch = Stopwatch.StartNew();
            TaskRunner.RunTask(eventsSource:
                context == TestFixtureContext.PeriodEnd
                    ? EventSource.PeriodEnd
                    : EventSource.Submission);
            stopwatch.Stop();

            // Assert
            Console.WriteLine($"Execution took {stopwatch.Elapsed.TotalSeconds:0.0}");
            Assert.IsTrue(stopwatch.Elapsed.TotalSeconds < expectedMaxElapsed, $"Expected to complete in less than {expectedMaxElapsed} seconds but took {stopwatch.Elapsed.TotalSeconds:0.0}");
        }

    }
}