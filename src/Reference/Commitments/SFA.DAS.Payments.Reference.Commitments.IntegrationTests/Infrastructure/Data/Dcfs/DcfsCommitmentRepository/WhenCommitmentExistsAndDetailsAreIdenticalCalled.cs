using NUnit.Framework;
using SFA.DAS.Events.Api.Types;
using SFA.DAS.Payments.Reference.Commitments.Infrastructure.Data;
using SFA.DAS.Payments.Reference.Commitments.Infrastructure.Data.Entities;
using SFA.DAS.Payments.Reference.Commitments.IntegrationTests.DataHelpers;
using System;

namespace SFA.DAS.Payments.Reference.Commitments.IntegrationTests.Infrastructure.Data.Dcfs.DcfsCommitmentRepository
{
    public class WhenCommitmentExistsAndDetailsAreIdenticalCalled
    {
        private ICommitmentRepository _repository;

        [SetUp]
        public void Arrange()
        {
            CommitmentDataHelper.Clean();

        }


        [TestCase(null, null, null, null, null)]
        [TestCase("2009-01-02",null, null, null, null)]
        [TestCase("2009-01-02", 1, "2010-09-08", null, null)]
        [TestCase(null, null, null, "2019-01-01", null)]
        [TestCase(null, null, null, null, "2017-01-01")]
        public void ThenItShouldReturnCommitmentId(string effectiveToDate, long? transferSendingEmployerAccountId, string transferApprovalDate, string pausedOnDate, string withdrawnOnDate)
        {
            var commitment = CreateCommitmentEntity();
            commitment.EffectiveToDate = effectiveToDate == null ? (DateTime?) null : DateTime.Parse(effectiveToDate);
            commitment.TransferSendingEmployerAccountId = transferSendingEmployerAccountId;
            commitment.TransferApprovalDate = transferApprovalDate == null ? (DateTime?) null : DateTime.Parse(transferApprovalDate);
            commitment.PausedOnDate = pausedOnDate == null ? (DateTime?) null : DateTime.Parse(pausedOnDate);
            commitment.WithdrawnOnDate = withdrawnOnDate == null ? (DateTime?) null : DateTime.Parse(withdrawnOnDate);

            CreateCommitmentInDatabase(commitment);

            // Act
            var result = _repository.CommitmentExistsAndDetailsAreIdentical(commitment);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result);
        }

        [Test]
        public void ThenItShouldNotReturnCommitmentId()
        {
            var commitment = CreateCommitmentEntity();
            CreateCommitmentInDatabase(commitment);

            commitment.EffectiveToDate = new DateTime(2020, 10, 10);
            // Act
            var result = _repository.CommitmentExistsAndDetailsAreIdentical(commitment);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsFalse(result);
        }

        private CommitmentEntity CreateCommitmentEntity()
        {
            return new CommitmentEntity
            {
                CommitmentId = 2,
                AccountId = 2,
                Uln = 258,
                Ukprn = 369,
                StartDate = new DateTime(2019, 9, 1),
                EndDate = new DateTime(2020, 10, 1),
                AgreedCost = 14523m,
                StandardCode = 0,
                ProgrammeType = 965,
                FrameworkCode = 854,
                PathwayCode = 621,
                Priority = 1,
                PaymentStatus = (int)PaymentStatus.Withdrawn,
                PaymentStatusDescription = PaymentStatus.Withdrawn.ToString(),
                VersionId = "1",
                EffectiveFromDate = new DateTime(2019, 9, 1),
                LegalEntityName = "Legal Entity"            };
        }


        private void CreateCommitmentInDatabase(CommitmentEntity commitment)
        {
            CommitmentDataHelper.AddCommitment(commitment.CommitmentId, commitment.AccountId, 
                commitment.Uln, commitment.Ukprn, commitment.StartDate, commitment.EndDate, commitment.AgreedCost,
                commitment.StandardCode, commitment.ProgrammeType, commitment.FrameworkCode, commitment.PathwayCode,
                commitment.Priority,
                commitment.PaymentStatus, commitment.PaymentStatusDescription,
                commitment.VersionId, commitment.EffectiveFromDate, commitment.EffectiveToDate, commitment.TransferSendingEmployerAccountId,
                commitment.TransferApprovalDate, commitment.PausedOnDate, commitment.WithdrawnOnDate, commitment.LegalEntityName);

            _repository =
                new Commitments.Infrastructure.Data.Dcfs.DcfsCommitmentRepository(GlobalTestContext.Instance
                    .TransientConnectionString);
        }
    }
}
