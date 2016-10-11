using System;
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
            var accountId = Guid.NewGuid().ToString();
            TestDataHelper.AddAccount(accountId);

            var commitmentId1 = Guid.NewGuid().ToString();
            var commitmentId2 = Guid.NewGuid().ToString();

            TestDataHelper.AddCommitment(commitmentId1, accountId, priority: 5);
            TestDataHelper.AddCommitment(commitmentId2, accountId, priority: 2);

            // Act
            var commitments = _repository.GetCommitmentsForAccount(accountId);

            // Assert
            Assert.IsNotNull(commitments);
            Assert.AreEqual(2, commitments.Length);

            Assert.AreEqual(commitmentId2, commitments[0].Id);
            Assert.AreEqual(commitmentId1, commitments[1].Id);
        }
    }
}