using NUnit.Framework;
using SFA.DAS.CollectionEarnings.DataLock.IntegrationTests.Tools;

namespace SFA.DAS.CollectionEarnings.DataLock.IntegrationTests.CopyToDeds
{
    public class WhenCleaningUpDuringAnIlrSubmission
    {
        private const long Ukprn = 10007459;

        [SetUp]
        public void Arrange()
        {
            TestDataHelper.Clean();
            TestDataHelper.AddValidProvider(Ukprn);
            TestDataHelper.AddCollectionPeriod();

            TestDataHelper.CopyReferenceData();
            TestDataHelper.ExecuteScript("SubmissionDataLockOutput.sql", true);
        }

        [Test]
        public void ThenExistingErrorsAreDeletedForTheCurrentUkprn()
        {
            // Act
            TestDataHelper.ExecuteScript(@"Deds Cleanup\Ilr.DataLock.Cleanup.Deds.DML.sql", true);

            // Assert
            var errors = TestDataHelper.GetValidationErrors(true);

            Assert.IsNotNull(errors);
            Assert.AreEqual(0, errors.Length);
        }

        [Test]
        public void ThenExistingMatchesAreDeletedForTheCurrentUkprn()
        {
            // Act
            TestDataHelper.ExecuteScript(@"Deds Cleanup\Ilr.DataLock.Cleanup.Deds.DML.sql", true);

            // Assert
            var matches = TestDataHelper.GetPriceEpisodeMatches(true);

            Assert.IsNotNull(matches);
            Assert.AreEqual(0, matches.Length);
        }
    }
}