using System;
using System.Linq;
using System.Net.Mime;
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
        private const string PriceEpisodeIdentifier = "Ep 1";

        [SetUp]
        public void SetUp()
        {
            //So there can't be a common setup, because the implementation is different depending on context
            //And SetUp() can't take the context

            //TestDataHelper.Clean();

            //TestDataHelper.SetCurrentPeriodEnd();
            //TestDataHelper.AddLearningProvider(Ukprn);
            //TestDataHelper.AddFileDetails(Ukprn);
            //TestDataHelper.AddCommitment(CommitmentId, Ukprn, LearnerRefNumber, passedDataLock: false);
            //TestDataHelper.AddIlrDataForCommitment(CommitmentId, LearnerRefNumber);

            //TestDataHelper.AddReferenceDataLockEvent(Ukprn, (int)EventStatus.Removed);
        }

        public void Arrange(Enums.TestFixtureContext context)
        {
            
        }

        [TestCase(Enums.TestFixtureContext.Submission)]
        //[TestCase(Enums.TestFixtureContext.PeriodEnd)]
        public void ThenAnEventShouldNotExistForThePriceEpisodeIdentifier(Enums.TestFixtureContext context)
        {
            //Arrange
            var helper = TestFixtureDataHelper.GetTestDataHelper(context);

            helper.Clean();
            helper.SetCurrentPeriodEnd();
            helper.AddLearningProvider(Ukprn);
            helper.AddFileDetails(Ukprn);
            helper.AddCommitment(CommitmentId, Ukprn, LearnerRefNumber, passedDataLock: false);
            helper.AddIlrDataForCommitment(CommitmentId, LearnerRefNumber);
            //helper.AddReferenceDataLockEvent(Ukprn, (int)EventStatus.Removed);

            helper.AddDataLockEvent(Ukprn, LearnerRefNumber, 1, "Ep 1", 0, 1, default(DateTime), default(DateTime), 15000M, default(DateTime), null, null, null, null, false, 3);


            helper.CopyReferenceData();

            //Act
            TaskRunner.RunTask(null,
                context == Enums.TestFixtureContext.PeriodEnd ? EventSource.PeriodEnd : EventSource.Submission);

            //Assert
            Assert.IsNull(helper.GetAllEvents()?.SingleOrDefault(e => e.PriceEpisodeIdentifier == PriceEpisodeIdentifier));
        }
    }
}

//Period end run arrange()
//// Arrange
//var ukprn = 10000534;
//var commitmentId = 1;

//TestDataHelper.PeriodEndAddLearningProvider(ukprn);
//            TestDataHelper.PeriodEndAddCommitment(commitmentId, ukprn, "Lrn-001", passedDataLock: false);
//            TestDataHelper.PeriodEndAddIlrDataForCommitment(commitmentId, "Lrn-001");

//            TestDataHelper.PeriodEndCopyReferenceData();


//submission arrange()
// Arrange
//var ukprn = 10000534;
//var commitmentId = 1;

//TestDataHelper.AddLearningProvider(ukprn);
//            TestDataHelper.AddFileDetails(ukprn);
//            TestDataHelper.AddCommitment(commitmentId, ukprn, "Lrn-001", passedDataLock: false);
//            TestDataHelper.AddIlrDataForCommitment(commitmentId, "Lrn-001");

//            TestDataHelper.SubmissionCopyReferenceData();