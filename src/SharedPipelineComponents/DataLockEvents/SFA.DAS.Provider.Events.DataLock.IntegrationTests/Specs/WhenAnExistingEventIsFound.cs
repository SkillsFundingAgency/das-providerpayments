using NUnit.Framework;
using SFA.DAS.Provider.Events.DataLock.Domain;
using SFA.DAS.Provider.Events.DataLock.IntegrationTests.Execution;
using SFA.DAS.Provider.Events.DataLock.IntegrationTests.Helpers;

namespace SFA.DAS.Provider.Events.DataLock.IntegrationTests.Specs
{
    public class WhenAnExistingEventIsFound
    {
        [TestCase(TestFixtureContext.Submission)]
        [TestCase(TestFixtureContext.PeriodEnd)]
        public void ThenNoNewEventsShouldBeWrittenIfNothingChanged(TestFixtureContext context)
        {
            //Arrange
            var helper = TestDataHelper.Get(context);

            var ukprn = 10000534;
            var commitmentId = 1;

            helper.Clean();
            helper.SetCurrentPeriodEnd();

            helper.AddLearningProvider(ukprn);

            helper.AddFileDetails(ukprn);
            helper.AddCommitment(commitmentId, ukprn, "Lrn-001", passedDataLock: false);
            helper.AddIlrDataForCommitment(commitmentId, "Lrn-001");

            helper.AddDataLockEvent(ukprn, "Lrn-001", passedDataLock: false);

            helper.CopyReferenceData();

            //Act
            TaskRunner.RunTask(eventsSource:
                context == TestFixtureContext.PeriodEnd
                ? EventSource.PeriodEnd
                : EventSource.Submission);

            // Assert
            var events = helper.GetAllEvents();

            Assert.IsNotNull(events);
            Assert.AreEqual(0, events.Length);
        }


        [TestCase(TestFixtureContext.Submission)]
        [TestCase(TestFixtureContext.PeriodEnd)]
        public void ThenANewEventShouldBeWrittenIfSomethingChanged(TestFixtureContext context)
        {
            //Arrange
            var helper = TestDataHelper.Get(context);

            helper.Clean();
            helper.SetCurrentPeriodEnd();

            var ukprn = 10000534;
            var commitmentId = 1;

            helper.AddLearningProvider(ukprn);
            helper.AddFileDetails(ukprn);
            helper.AddCommitment(commitmentId, ukprn, "Lrn-001", passedDataLock: false);
            helper.AddIlrDataForCommitment(commitmentId, "Lrn-001");

            helper.AddDataLockEvent(ukprn, "Lrn-001");

            helper.CopyReferenceData();

            //Act
            TaskRunner.RunTask(eventsSource:
                context == TestFixtureContext.PeriodEnd
                ? EventSource.PeriodEnd
                : EventSource.Submission);

            // Assert
            var events = helper.GetAllEvents();

            Assert.IsNotNull(events);
            Assert.AreEqual(1, events.Length);

            var @event = events[0];
            Assert.AreEqual(ukprn, @event.Ukprn);
            Assert.AreEqual(commitmentId, @event.CommitmentId);
            Assert.AreEqual(EventStatus.Updated, @event.Status);

            var eventErrors = helper.GetAllEventErrors(@event.DataLockEventId);
            var eventPeriods = helper.GetAllEventPeriods(@event.DataLockEventId);
            var eventCommitmentVersions = helper.GetAllEventCommitmentVersions(@event.DataLockEventId);

            Assert.IsNotNull(eventErrors);
            Assert.IsNotNull(eventPeriods);
            Assert.IsNotNull(eventCommitmentVersions);

            Assert.AreEqual(1, eventErrors.Length);
            Assert.AreEqual(36, eventPeriods.Length);
            Assert.AreEqual(1, eventCommitmentVersions.Length);
        }

        
    }
}