using NUnit.Framework;
using SFA.DAS.Events.Api.Types;
using SFA.DAS.Payments.Reference.Commitments.Infrastructure.Data;
using SFA.DAS.Payments.Reference.Commitments.Infrastructure.Data.Entities;
using SFA.DAS.Payments.Reference.Commitments.IntegrationTests.DataHelpers;
using System;

namespace SFA.DAS.Payments.Reference.Commitments.IntegrationTests.Infrastructure.Data.Dcfs.DcfsCommitmentRepository
{
    public class WhenInsertHistoryIsCalled
    {
        private ICommitmentRepository _repository;
        private CommitmentEntity _commitment;

        [SetUp]
        public void Arrange()
        {
            CommitmentDataHelper.Clean();

            _commitment = new CommitmentEntity
            {
                CommitmentId = 2,
                AccountId = 2,
                Uln = 258,
                Ukprn = 369,
                StartDate = new DateTime(2019, 9, 1),
                EndDate = new DateTime(2020, 10, 1),
                AgreedCost = 14523m,
                ProgrammeType = 965,
                FrameworkCode = 854,
                PathwayCode = 621,
                Priority = 1,
                PaymentStatus = (int)PaymentStatus.Withdrawn,
                PaymentStatusDescription = PaymentStatus.Withdrawn.ToString(),
                VersionId = "1-001",
                EffectiveFromDate = new DateTime(2019, 9, 1),
                TransferSendingEmployerAccountId = 123L,
                TransferApprovalDate = DateTime.Today,
                AccountLegalEntityPublicHashedId = "ABC123"
            };

            _repository = new Commitments.Infrastructure.Data.Dcfs.DcfsCommitmentRepository(GlobalTestContext.Instance.TransientConnectionString);
        }

        [Test]
        public void ThenItShouldCommitmentHistory()
        {
            // Act
            _repository.InsertHistory(_commitment);

            var commitment = CommitmentDataHelper.GetHistoryCommitment(_commitment.CommitmentId);

            // Assert
            Assert.True(commitment > 0);
        }
    }
}
