using NUnit.Framework;
using CS.Common.SqlBulkCopyCat;
using CS.Common.SqlBulkCopyCat.Model.Config.Builder;
using SFA.DAS.Provider.Events.DataLock.IntegrationTests.Helpers;
using SFA.DAS.Provider.Events.DataLock.IntegrationTests.TestContext;

// this ignored test can be used to manually test the transer from transient db to Deds db of DataLockEvents and related tables

namespace SFA.DAS.Provider.Events.DataLock.IntegrationTests.Transfer
{
    class WhenDataLockEventsAreTransferred
    {
        //// comment out in GlobalSetup...
        //[OneTimeSetUp]
        //public void BeforeAllTests()
        //{
        //    //SetupDedsDatabase();
        //    //SetupSubmissionTransientDatabase();
        //    //SetupPeriodEndTransientDatabase();
        //}

        [Test]
        [Ignore("Useful for manual testing only (without further work)")]
        public void ThenItShouldTransfer()
        {
            //TestDataHelper.SubmissionCopyReferenceData();

            const string mappingFilePath = @"C:\git\das-providerpayments\src\SharedPipelineComponents\DataLockEvents\DeployDataLock\copy mappings\DasDataLockEventsCopyToDedsMapping.xml";

            var config = new CopyCatConfigBuilder().FromXmlFile(mappingFilePath);
            config.SourceConnectionString = GlobalTestContext.Current.TransientSubmissionDatabaseConnectionString;
            config.DestinationConnectionString = GlobalTestContext.Current.DedsDatabaseConnectionString;

            var copyCat = new CopyCat(config);
            copyCat.Copy();
        }
    }
}
