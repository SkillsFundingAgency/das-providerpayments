using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.ProviderPayments.Calc.Shared.Infrastructure.Data.Entities;
using SFA.DAS.ProviderPayments.Calc.Shared.Infrastructure.Data.Repositories;
using SFA.DAS.ProviderPayments.Calc.Shared.IntegrationTests.Attributes;

namespace SFA.DAS.ProviderPayments.Calc.Shared.IntegrationTests.Tests.Infrastructure
{
    [TestFixture, SetupUkprn]
    public class GivenACommitmentsRepository
    {
        private CommitmentRepository _sut;

        [OneTimeSetUp]
        public void Setup()
        {
            _sut = new CommitmentRepository(GlobalTestContext.Instance.TransientConnectionString);
        }

        [TestFixture]
        public class AndThereAreNoCommitments : GivenACommitmentsRepository
        {
            [TestFixture]
            public class WhenCallingGetAllForProvider : AndThereAreNoCommitments
            {
                [Test]
                public void ThenItReturnsAnEmptyList()
                {
                    Setup();
                    var result = _sut.GetProviderCommitments(SharedTestContext.Ukprn);
                    result.Should().BeEmpty();
                }
            }
        }

        [TestFixture, SetupCommitments]
        public class AndThereAreSomeCommitments : GivenACommitmentsRepository
        {
            [TestFixture]
            public class WhenCallingGetAllForProvider : AndThereAreSomeCommitments
            {
                private List<CommitmentEntity> _actualCommitments;
                private List<CommitmentEntity> _expectedCommitments;

                [SetUp]
                public new void Setup()
                {
                    base.Setup();
                    _actualCommitments = _sut
                        .GetProviderCommitments(SharedTestContext.Ukprn)
                        .ToList();

                    _expectedCommitments = SharedTestContext.Commitments
                        .Where(x => x.Ukprn == SharedTestContext.Ukprn)
                        .ToList();
                }

                [Test]
                public void ThenItRetrievesExpectedCount()
                {
                    if (_expectedCommitments.Count < 1)
                        Assert.Fail("Not enough earnings to test");

                    _actualCommitments.Count.Should().Be(_expectedCommitments.Count);
                }

                [Test]
                public void ThenCommitmentIdIsSetCorrectly() =>
                    _actualCommitments[0].CommitmentId.Should().Be(_expectedCommitments[0].CommitmentId);

                [Test]
                public void ThenVersionidIsSetCorrectly() =>
                    _actualCommitments[0].VersionId.Should().Be(_expectedCommitments[0].VersionId);

                [Test]
                public void ThenUlnIsSetCorrectly() =>
                    _actualCommitments[0].Uln.Should().Be(_expectedCommitments[0].Uln);

                [Test]
                public void ThenAccountIdIsSetCorrectly() =>
                    _actualCommitments[0].AccountId.Should().Be(_expectedCommitments[0].AccountId);

                [Test]
                public void ThenStartDateIsSetCorrectly() =>
                    _actualCommitments[0].StartDate.Date.Should().Be(_expectedCommitments[0].StartDate.Date);

                [Test]
                public void ThenEndDateIsSetCorrectly() =>
                    _actualCommitments[0].EndDate.Date.Should().Be(_expectedCommitments[0].EndDate.Date);

                [Test]
                public void ThenAgreedCostIsSetCorrectly() =>
                    _actualCommitments[0].AgreedCost.Should().Be(_expectedCommitments[0].AgreedCost);

                [Test]
                public void ThenStandardCodeIsSetCorrectly() =>
                    _actualCommitments[0].StandardCode.Should().Be(_expectedCommitments[0].StandardCode);

                [Test]
                public void ThenProgrammeTypeIsSetCorrectly() =>
                    _actualCommitments[0].ProgrammeType.Should().Be(_expectedCommitments[0].ProgrammeType);

                [Test]
                public void ThenFrameworkCodeIsSetCorrectly() =>
                    _actualCommitments[0].FrameworkCode.Should().Be(_expectedCommitments[0].FrameworkCode);

                [Test]
                public void ThenPathwayCodeIsSetCorrectly() =>
                    _actualCommitments[0].PathwayCode.Should().Be(_expectedCommitments[0].PathwayCode);

                [Test]
                public void ThenPaymentStatusIsSetCorrectly() =>
                    _actualCommitments[0].PaymentStatus.Should().Be(_expectedCommitments[0].PaymentStatus);

                [Test]
                public void ThenPaymentStatusDescriptionIsSetCorrectly() =>
                    _actualCommitments[0].PaymentStatusDescription.Should().Be(_expectedCommitments[0].PaymentStatusDescription);

                [Test]
                public void ThenPriorityIsSetCorrectly() =>
                    _actualCommitments[0].Priority.Should().Be(_expectedCommitments[0].Priority);

                [Test]
                public void ThenEffectiveFromIsSetCorrectly() =>
                    _actualCommitments[0].EffectiveFrom.Date.Should().Be(_expectedCommitments[0].EffectiveFrom.Date);

                [Test]
                public void ThenEffectiveToIsSetCorrectly() =>
                    _actualCommitments[0].EffectiveTo?.Date.Should().Be(_expectedCommitments[0].EffectiveTo?.Date??DateTime.MaxValue);

                [Test]
                public void ThenTransferSendingEmployerAccountIdIsSetCorrectly() =>
                    _actualCommitments[0].TransferSendingEmployerAccountId.Should().Be(_expectedCommitments[0].TransferSendingEmployerAccountId);

                [Test]
                public void ThenTransferApprovalDateIsSetCorrectly() =>
                    _actualCommitments[0].TransferApprovalDate?.Date.Should().Be(_expectedCommitments[0].TransferApprovalDate?.Date??DateTime.MaxValue);
            }
        }
    }
}
