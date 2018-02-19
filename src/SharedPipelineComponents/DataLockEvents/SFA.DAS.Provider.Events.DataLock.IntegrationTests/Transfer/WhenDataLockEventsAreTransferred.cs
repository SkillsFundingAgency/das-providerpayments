using NUnit.Framework;
using CS.Common.SqlBulkCopyCat;
using CS.Common.SqlBulkCopyCat.Model.Config.Builder;
using SFA.DAS.Provider.Events.DataLock.IntegrationTests.TestContext;

namespace SFA.DAS.Provider.Events.DataLock.IntegrationTests.Transfer
{
    class WhenDataLockEventsAreTransferred
    {
        /// <summary>
        /// This ignored test can be used to manually test the transer from transient db to Deds db of DataLockEvents and related tables.
        /// </summary>
        /// <remarks>
        /// Remove BeforeAllTests() in GlobalSetup before running, to stop your test data in the transient db from being deleted.
        /// Correct the mappingFilePath for your environment, or make it relative.
        /// </remarks>
        [Test]
        [Ignore("Useful for manual testing only (without further work)")]
        public void ThenItShouldTransfer()
        {
            const string mappingFilePath = @"C:\git\das-providerpayments\src\SharedPipelineComponents\DataLockEvents\DeployDataLock\copy mappings\DasDataLockEventsCopyToDedsMapping.xml";

            var config = new CopyCatConfigBuilder().FromXmlFile(mappingFilePath);
            config.SourceConnectionString = GlobalTestContext.Current.TransientSubmissionDatabaseConnectionString;
            config.DestinationConnectionString = GlobalTestContext.Current.DedsDatabaseConnectionString;

            var copyCat = new CopyCat(config);
            copyCat.Copy();
        }
    }
}
