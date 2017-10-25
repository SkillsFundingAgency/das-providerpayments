using NUnit.Framework;
using SFA.DAS.CollectionEarnings.DataLock.IntegrationTests.Tools;

namespace SFA.DAS.CollectionEarnings.DataLock.IntegrationTests.CopyToDeds
{
    public class WhenCleaningUpDuringAnIlrPeriodEnd
    {
        private const long Ukprn = 10007459;

        [SetUp]
        public void Arrange()
        {
            TestDataHelper.Clean();
            TestDataHelper.PeriodEndAddProvider(Ukprn);
            TestDataHelper.PeriodEndAddCollectionPeriod();

            TestDataHelper.PeriodEndCopyReferenceData();
            TestDataHelper.PeriodEndExecuteScript("PeriodEndDataLockOutput.sql", true);
        }

        [Test]
        public void ThenExistingErrorsAreDeletedForTheCurrentUkprn()
        {
            // Act
            TestDataHelper.PeriodEndExecuteScript(@"Deds Cleanup\PeriodEnd.DataLock.Cleanup.Deds.DML.sql", true);

            // Assert
            var errors = TestDataHelper.PeriodEndGetValidationErrors(true);

            Assert.IsNotNull(errors);
            Assert.AreEqual(0, errors.Length);
        }

        [Test]
        public void ThenExistingMatchesAreDeletedForTheCurrentUkprn()
        {
            // Act
            TestDataHelper.PeriodEndExecuteScript(@"Deds Cleanup\PeriodEnd.DataLock.Cleanup.Deds.DML.sql", true);

            // Assert
            var matches = TestDataHelper.PeriodEndGetPriceEpisodeMatches(true);

            Assert.IsNotNull(matches);
            Assert.AreEqual(0, matches.Length);
        }
    }
}