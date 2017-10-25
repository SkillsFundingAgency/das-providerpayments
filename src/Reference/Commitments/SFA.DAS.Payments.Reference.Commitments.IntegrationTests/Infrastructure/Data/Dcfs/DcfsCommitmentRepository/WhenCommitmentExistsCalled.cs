using NUnit.Framework;
using SFA.DAS.Events.Api.Types;
using SFA.DAS.Payments.Reference.Commitments.Infrastructure.Data;
using SFA.DAS.Payments.Reference.Commitments.Infrastructure.Data.Entities;
using SFA.DAS.Payments.Reference.Commitments.IntegrationTests.DataHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFA.DAS.Payments.Reference.Commitments.IntegrationTests.Infrastructure.Data.Dcfs.DcfsCommitmentRepository
{
    public class WhenCommitmentExistsCalled
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
                StandardCode=0,
                ProgrammeType = 965,
                FrameworkCode = 854,
                PathwayCode = 621,
                Priority = 1,
                PaymentStatus = (int)PaymentStatus.Withdrawn,
                PaymentStatusDescription = PaymentStatus.Withdrawn.ToString(),
                VersionId = "1",
                EffectiveFromDate = new DateTime(2019, 9, 1),
                LegalEntityName="Legal Entity"
            };

            CommitmentDataHelper.AddCommitment(_commitment.CommitmentId, _commitment.AccountId
                   , _commitment.Uln, _commitment.Ukprn, _commitment.StartDate, _commitment.EndDate, _commitment.AgreedCost,
                   _commitment.StandardCode, _commitment.ProgrammeType, _commitment.FrameworkCode, _commitment.PathwayCode, _commitment.Priority,
                   _commitment.PaymentStatus, _commitment.PaymentStatusDescription,
                   _commitment.VersionId, _commitment.EffectiveFromDate,null,_commitment.LegalEntityName);

            _repository = new Commitments.Infrastructure.Data.Dcfs.DcfsCommitmentRepository(GlobalTestContext.Instance.TransientConnectionString);
        }

        [Test]
        public void ThenItShouldReturnCommitmentId()
        {

            // Act
            var result = _repository.CommitmentExists(_commitment);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result);


        }

        [Test]
        public void ThenItShouldNotReturnCommitmentId()
        {
            _commitment.EffectiveToDate = new DateTime(2020, 10, 10);
            // Act
            var result = _repository.CommitmentExists(_commitment);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsFalse(result);


        }
    }
}
