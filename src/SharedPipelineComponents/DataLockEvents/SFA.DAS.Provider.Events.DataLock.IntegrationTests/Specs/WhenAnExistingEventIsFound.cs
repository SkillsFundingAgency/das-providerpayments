﻿using NUnit.Framework;
using SFA.DAS.Provider.Events.DataLock.Domain;
using SFA.DAS.Provider.Events.DataLock.IntegrationTests.Execution;
using SFA.DAS.Provider.Events.DataLock.IntegrationTests.Helpers;

namespace SFA.DAS.Provider.Events.DataLock.IntegrationTests.Specs
{
    public class WhenAnExistingEventIsFound
    {
        [SetUp]
        public void Arrange()
        {
            CommonTestDataHelper.Clean();
            CommonTestDataHelper.SetCurrentPeriodEnd();
        }

        [Test]
        public void ThenNoNewEventsShouldBeWrittenIfNothingChangedInASubmissionRun()
        {
            // Arrange
            var ukprn = 10000534;
            var commitmentId = 1;

            CommonTestDataHelper.AddLearningProvider(ukprn);
            CommonTestDataHelper.AddFileDetails(ukprn);
            CommonTestDataHelper.AddCommitment(commitmentId, ukprn, "Lrn-001", passedDataLock: false);
            CommonTestDataHelper.AddIlrDataForCommitment(commitmentId, "Lrn-001");

            CommonTestDataHelper.AddDataLockEvent(ukprn, "Lrn-001", passedDataLock: false);

            CommonTestDataHelper.SubmissionCopyReferenceData();

            // Act
            TaskRunner.RunTask();

            // Assert
            var events = CommonTestDataHelper.GetAllEvents();

            Assert.IsNotNull(events);
            Assert.AreEqual(0, events.Length);
        }

        [Test]
        public void ThenNoNewEventsShouldBeWrittenIfNothingChangedInAPeriodEndRun()
        {
            // Arrange
            var ukprn = 10000534;
            var commitmentId = 1;

            CommonTestDataHelper.PeriodEndAddLearningProvider(ukprn);
            CommonTestDataHelper.PeriodEndAddCommitment(commitmentId, ukprn, "Lrn-001", passedDataLock: false);
            CommonTestDataHelper.PeriodEndAddIlrDataForCommitment(commitmentId, "Lrn-001");

            CommonTestDataHelper.AddDataLockEvent(ukprn, "Lrn-001", passedDataLock: false);

            CommonTestDataHelper.PeriodEndCopyReferenceData();

            // Act
            TaskRunner.RunTask(eventsSource: EventSource.PeriodEnd);

            // Assert
            var events = CommonTestDataHelper.GetAllEvents(false);

            Assert.IsNotNull(events);
            Assert.AreEqual(0, events.Length);
        }

        [Test]
        public void ThenANewEventShouldBeWrittenIfSomethingChangedInASubmissionRun()
        {
            // Arrange
            var ukprn = 10000534;
            var commitmentId = 1;

            CommonTestDataHelper.AddLearningProvider(ukprn);
            CommonTestDataHelper.AddFileDetails(ukprn);
            CommonTestDataHelper.AddCommitment(commitmentId, ukprn, "Lrn-001", passedDataLock: false);
            CommonTestDataHelper.AddIlrDataForCommitment(commitmentId, "Lrn-001");

            CommonTestDataHelper.AddDataLockEvent(ukprn, "Lrn-001");

            CommonTestDataHelper.SubmissionCopyReferenceData();

            // Act
            TaskRunner.RunTask();

            // Assert
            var events = CommonTestDataHelper.GetAllEvents();

            Assert.IsNotNull(events);
            Assert.AreEqual(1, events.Length);

            var @event = events[0];

           // Assert.AreEqual(2, @event.Id);
            Assert.AreEqual(ukprn, @event.Ukprn);
            Assert.AreEqual(commitmentId, @event.CommitmentId);
            Assert.AreEqual(EventStatus.Updated, @event.Status);

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
        public void ThenANewEventShouldBeWrittenIfSomethingChangedInAPeriodEndRun()
        {
            // Arrange
            var ukprn = 10000534;
            var commitmentId = 1;

            CommonTestDataHelper.PeriodEndAddLearningProvider(ukprn);
            CommonTestDataHelper.PeriodEndAddCommitment(commitmentId, ukprn, "Lrn-001", passedDataLock: false);
            CommonTestDataHelper.PeriodEndAddIlrDataForCommitment(commitmentId, "Lrn-001");

            CommonTestDataHelper.AddDataLockEvent(ukprn, "Lrn-001");

            CommonTestDataHelper.PeriodEndCopyReferenceData();

            // Act
            TaskRunner.RunTask(eventsSource: EventSource.PeriodEnd);

            // Assert
            var events = CommonTestDataHelper.GetAllEvents(false);

            Assert.IsNotNull(events);
            Assert.AreEqual(1, events.Length);

            var @event = events[0];

            //Assert.AreEqual(2, @event.Id);
            Assert.AreEqual(ukprn, @event.Ukprn);
            Assert.AreEqual(commitmentId, @event.CommitmentId);
            Assert.AreEqual(EventStatus.Updated, @event.Status);

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
    }
}