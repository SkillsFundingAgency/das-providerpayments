using NUnit.Framework;
using SFA.DAS.CollectionEarnings.DataLock.Infrastructure.Data;
using SFA.DAS.CollectionEarnings.DataLock.Infrastructure.Data.Entities;
using SFA.DAS.CollectionEarnings.DataLock.IntegrationTests.Tools;
using SFA.DAS.CollectionEarnings.DataLock.UnitTests.Tools.Entities;

namespace SFA.DAS.CollectionEarnings.DataLock.IntegrationTests.Infrastructure.Data.Repositories.CommitmentRepository
{
    public class WhenGetProviderCommitmentsCalledDuringAnIlrPeriodEnd
    {
        private readonly long _ukprn = 10007459;
        private readonly long _ukprnNoCommitments = 10007458;

        private readonly CommitmentEntity[] _commitments =
        {
            new CommitmentEntityBuilder().Build(),
            new CommitmentEntityBuilder().WithCommitmentId(2).WithUln(1000000027).WithAgreedCost(30000.00m).Build()
        };

        private ICommitmentRepository _commitmentRepository;

        [SetUp]
        public void Arrange()
        {
            TestDataHelper.Clean();
            TestDataHelper.PeriodEndAddProvider(_ukprn);

            _commitmentRepository = new DataLock.Infrastructure.Data.Repositories.CommitmentRepository(GlobalTestContext.Instance.PeriodEndConnectionString);
        }

        [Test]
        public void ThenNoCommitmentsReturnedNoEntriesInTheDatabase()
        {
            // Act
            var commitments = _commitmentRepository.GetProviderCommitments(_ukprn);

            // Assert
            Assert.IsNotNull(commitments);
            Assert.AreEqual(0, commitments.Length);
        }

        [Test]
        public void ThenNoCommitmentsReturnedForAUkprnWithNoEntriesInTheDatabase()
        {
            // Arrange
            TestDataHelper.PeriodEndAddCommitment(_commitments[0]);
            TestDataHelper.PeriodEndAddCommitment(_commitments[1]);

            // Act
            var response = _commitmentRepository.GetProviderCommitments(_ukprnNoCommitments);

            // Assert
            Assert.IsNotNull(response);
            Assert.AreEqual(0, response.Length);
        }

        [Test]
        public void ThenCommitmentReturnedForOneEntryInTheDatabase()
        {
            // Arrange
            TestDataHelper.PeriodEndAddCommitment(_commitments[0]);
            TestDataHelper.PeriodEndExecuteScript("PeriodEndCommitmentMatchingData.sql");
            TestDataHelper.PeriodEndCopyReferenceData();

            // Act
            var response = _commitmentRepository.GetProviderCommitments(_ukprn);

            // Assert
            Assert.IsNotNull(response);
            Assert.AreEqual(1, response.Length);
        }

        [Test]
        public void ThenCommitmentsReturnedForMultipleEntriesInTheDatabase()
        {
            // Arrange
            TestDataHelper.PeriodEndAddCommitment(_commitments[0]);
            TestDataHelper.PeriodEndAddCommitment(_commitments[1]);
           
            TestDataHelper.PeriodEndExecuteScript("PeriodEndCommitmentMatchingData.sql");

            TestDataHelper.PeriodEndCopyReferenceData();

            // Act
            var response = _commitmentRepository.GetProviderCommitments(_ukprn);

            // Assert
            Assert.IsNotNull(response);
            Assert.AreEqual(2, response.Length);
        }

        [Test]
        public void ThenLatestVersionOfACommitmentIsReturnedForACommitmentWithMultipleIdenticalVersions()
        {
            // Arrange
            var commitments = new[]
            {
                new CommitmentEntityBuilder().Build(),
                new CommitmentEntityBuilder().WithVersionId("1-002").Build(),
                new CommitmentEntityBuilder().WithVersionId("1-003").Build(),
                new CommitmentEntityBuilder().WithVersionId("1-004").Build()
            };

            TestDataHelper.PeriodEndAddCommitment(commitments[0]);
            TestDataHelper.PeriodEndAddCommitment(commitments[1]);
            TestDataHelper.PeriodEndAddCommitment(commitments[2]);
            TestDataHelper.PeriodEndAddCommitment(commitments[3]);

            //TestDataHelper.PeriodEndAddValidLearner(_commitments[0].Ukprn, _commitments[0].Uln);
            TestDataHelper.PeriodEndExecuteScript("PeriodEndCommitmentMatchingData.sql");

            TestDataHelper.PeriodEndCopyReferenceData();

            // Act
            var response = _commitmentRepository.GetProviderCommitments(_ukprn);

            // Assert
            Assert.IsNotNull(response);
            Assert.AreEqual(1, response.Length);
            Assert.AreEqual("1-004", response[0].VersionId);
        }
    }
}