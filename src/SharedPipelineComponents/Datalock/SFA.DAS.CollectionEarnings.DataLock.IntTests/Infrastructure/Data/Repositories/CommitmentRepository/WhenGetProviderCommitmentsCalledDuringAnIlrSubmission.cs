using System.Linq;
using NUnit.Framework;
using SFA.DAS.CollectionEarnings.DataLock.Infrastructure.Data;
using SFA.DAS.CollectionEarnings.DataLock.Infrastructure.Data.Entities;
using SFA.DAS.CollectionEarnings.DataLock.IntegrationTests.Utilities;
using SFA.DAS.CollectionEarnings.DataLock.UnitTests.Tools.Entities;

namespace SFA.DAS.CollectionEarnings.DataLock.IntegrationTests.Infrastructure.Data.Repositories.CommitmentRepository
{
    public class WhenGetProviderCommitmentsCalledDuringAnIlrSubmission
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
            
            _commitmentRepository = new DataLock.Infrastructure.Data.Repositories.CommitmentRepository(GlobalTestContext.Instance.SubmissionConnectionString);
        }

        [Test]
        public void ThenNoCommitmentsReturnedNoEntriesInTheDatabase()
        {
            // Act
            var commitments = _commitmentRepository.GetProviderCommitments(_ukprn);

            // Assert
            Assert.IsNotNull(commitments);
            Assert.AreEqual(0, commitments.Count());
        }

        [Test]
        public void ThenNoCommitmentsReturnedForAUkprnWithNoEntriesInTheDatabase()
        {
            // Arrange
            TestDataHelper.AddCommitment(_commitments[0]);
            TestDataHelper.AddCommitment(_commitments[1]);

            // Act
            var response = _commitmentRepository.GetProviderCommitments(_ukprnNoCommitments);

            // Assert
            Assert.IsNotNull(response);
            Assert.AreEqual(0, response.Count());
        }

        [Test]
        public void ThenCommitmentReturnedForOneEntryInTheDatabase()
        {
            // Arrange
            TestDataHelper.AddCommitment(_commitments[0]);
            TestDataHelper.AddValidProvider(_commitments[0].Ukprn);
            TestDataHelper.AddValidLearner(_commitments[0].Uln);
            TestDataHelper.CopyReferenceData();

            // Act
            var response = _commitmentRepository.GetProviderCommitments(_ukprn);

            // Assert
            Assert.IsNotNull(response);
            Assert.AreEqual(1, response.Count());
        }

        [Test]
        public void ThenCommitmentsReturnedForMultipleEntriesInTheDatabase()
        {
            // Arrange
            TestDataHelper.AddCommitment(_commitments[0]);
            TestDataHelper.AddCommitment(_commitments[1]);
            TestDataHelper.AddValidLearner( _commitments[0].Uln);
            TestDataHelper.AddValidProvider(_commitments[0].Ukprn);
            TestDataHelper.AddValidLearner(_commitments[1].Uln);
            TestDataHelper.CopyReferenceData();

            // Act
            var response = _commitmentRepository.GetProviderCommitments(_ukprn);

            // Assert
            Assert.IsNotNull(response);
            Assert.AreEqual(2, response.Count());
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

            TestDataHelper.AddCommitment(commitments[0]);
            TestDataHelper.AddCommitment(commitments[1]);
            TestDataHelper.AddCommitment(commitments[2]);
            TestDataHelper.AddCommitment(commitments[3]);

            TestDataHelper.AddValidLearner( commitments[0].Uln);
            TestDataHelper.AddValidProvider(commitments[3].Ukprn);
            
            TestDataHelper.CopyReferenceData();

            // Act
            var response = _commitmentRepository.GetProviderCommitments(_ukprn);

            // Assert
            Assert.IsNotNull(response);
            Assert.AreEqual(1, response.Count());
            Assert.AreEqual("1-004", response.First().VersionId);
        }
    }
}
