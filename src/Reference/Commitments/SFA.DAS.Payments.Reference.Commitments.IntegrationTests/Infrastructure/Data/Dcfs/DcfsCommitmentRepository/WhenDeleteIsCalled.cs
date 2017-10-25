using System;
using NUnit.Framework;
using SFA.DAS.Events.Api.Types;
using SFA.DAS.Payments.Reference.Commitments.Infrastructure.Data;
using SFA.DAS.Payments.Reference.Commitments.Infrastructure.Data.Entities;
using SFA.DAS.Payments.Reference.Commitments.IntegrationTests.DataHelpers;

namespace SFA.DAS.Payments.Reference.Commitments.IntegrationTests.Infrastructure.Data.Dcfs.DcfsCommitmentRepository
{
    public class WhenDeleteIsCalled
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
                EffectiveFromDate = new DateTime(2019, 9, 1)
            };

            CommitmentDataHelper.AddCommitment(_commitment.CommitmentId, _commitment.AccountId
                   , _commitment.Uln, _commitment.Ukprn, _commitment.StartDate, _commitment.EndDate, _commitment.AgreedCost,
                   _commitment.StandardCode, _commitment.ProgrammeType, _commitment.FrameworkCode, _commitment.PathwayCode, _commitment.Priority,
                   _commitment.PaymentStatus, _commitment.PaymentStatusDescription,
                   _commitment.VersionId, _commitment.EffectiveFromDate);



            _repository = new Commitments.Infrastructure.Data.Dcfs.DcfsCommitmentRepository(GlobalTestContext.Instance.TransientConnectionString);
        }

        [Test]
        public void ThenItShouldDeleteCommitment()
        {
            // Arrange
            CommitmentDataHelper.AddCommitment(_commitment.CommitmentId, _commitment.AccountId
                      , _commitment.Uln, _commitment.Ukprn, _commitment.StartDate, _commitment.EndDate, _commitment.AgreedCost,
                      _commitment.StandardCode, _commitment.ProgrammeType, _commitment.FrameworkCode, _commitment.PathwayCode, _commitment.Priority,
                      _commitment.PaymentStatus, _commitment.PaymentStatusDescription,
                      "1-002", new DateTime(2017, 10, 10));

            // Act
             _repository.Delete(_commitment.CommitmentId);

            var commitment = _repository.GetById(_commitment.CommitmentId);

            // Assert
            Assert.IsNull(commitment);
            

        }
    }
}
