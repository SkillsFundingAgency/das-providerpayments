using NUnit.Framework;
using SFA.DAS.ProviderPayments.Calc.LevyPayments.Infrastructure.Data;
using SFA.DAS.ProviderPayments.Calc.LevyPayments.IntegrationTests.Tools;

namespace SFA.DAS.ProviderPayments.Calc.LevyPayments.IntegrationTests.Infrastructure.Data.Repositories.DcfsCommitmentRepository
{
    public class WhenGettingCommitmentsForAccount
    {
        private ICommitmentRepository _repository;

        [SetUp]
        public void Arrange()
        {
            TestDataHelper.Clean();

            _repository = new LevyPayments.Infrastructure.Data.Repositories.DcfsCommitmentRepository(GlobalTestContext.Instance.ConnectionString);
        }

        [Test]
        public void ThenTheCommitmentsAreOrderedByPriority()
        {
            // Arrange
            var accountId = 1;
            TestDataHelper.AddAccount(accountId);

            var commitmentId1 = 1L;
            var commitmentId2 = 2L;

            TestDataHelper.AddCommitment(commitmentId1, accountId, priority: 6);
            TestDataHelper.AddCommitment(commitmentId1, accountId, versionId: "7", priority: 4);
            TestDataHelper.AddCommitment(commitmentId1, accountId, versionId: "15", priority: 5);
            TestDataHelper.AddCommitment(commitmentId2, accountId, priority: 7);
            TestDataHelper.AddCommitment(commitmentId2, accountId, versionId: "3", priority: 2);

            TestDataHelper.CopyReferenceData();

            // Act
            var commitments = _repository.GetCommitmentsForAccount(accountId);

            // Assert
            Assert.IsNotNull(commitments);
            Assert.AreEqual(5, commitments.Length);

            Assert.AreEqual(commitmentId2, commitments[0].Id);
            Assert.AreEqual(commitmentId1, commitments[1].Id);
            Assert.AreEqual(commitmentId1, commitments[2].Id);
            Assert.AreEqual(commitmentId1, commitments[3].Id);
            Assert.AreEqual(commitmentId2, commitments[4].Id);
        }
    }
}