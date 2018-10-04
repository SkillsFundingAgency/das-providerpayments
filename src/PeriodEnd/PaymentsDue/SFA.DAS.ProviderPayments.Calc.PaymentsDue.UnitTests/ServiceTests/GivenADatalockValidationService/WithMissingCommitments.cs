using System.Collections.Generic;
using System.Linq;
using AutoFixture;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data.Entities;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Services;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.UnitTests.Utilities;
using SFA.DAS.ProviderPayments.Calc.Shared.Infrastructure.Data.Entities;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.UnitTests.ServiceTests.GivenADatalockValidationService
{
    [TestFixture]
    public class WithMissingCommitments
    {
        private static List<DatalockOutputEntity> RawDatalocks { get; set; }
        private static List<Commitment> Commitments { get; set; }
        private static List<DatalockValidationError> DatalockValidationErrors { get; set; }

        private static long _commitmentIdTwo;
        private static string _commitmentVersionIdTwo;

        public WithMissingCommitments()
        {
            var fixture = new Fixture();
            var commitmentIdOne = fixture.Create<long>();
            var commitmentVersionIdOne = fixture.Create<string>();
            
            _commitmentIdTwo = fixture.Create<long>();
            _commitmentVersionIdTwo = fixture.Create<string>();

            RawDatalocks = fixture.Build<DatalockOutputEntity>()
                .With(x => x.CommitmentId, commitmentIdOne)
                .With(x => x.VersionId, commitmentVersionIdOne)
                .With(x => x.Payable, true)
                .CreateMany(5)
                .ToList();
            RawDatalocks.AddRange(fixture.Build<DatalockOutputEntity>()
                .With(x => x.CommitmentId, _commitmentIdTwo)
                .With(x => x.VersionId, _commitmentVersionIdTwo)
                .With(x => x.Payable, true)
                .CreateMany(5)
            );

            DatalockValidationErrors = new List<DatalockValidationError>();
        }
        
        [TestFixture]
        public class WithNoCommitments : WithMissingCommitments
        {
            public WithNoCommitments()
            {
                Commitments = new List<Commitment>();
            }

            [Test, PaymentsDueAutoData]
            public void ThenTheOutputShouldBeEmpty(DatalockValidationService sut)
            {
                var actual = sut.GetSuccessfulDatalocks(RawDatalocks, DatalockValidationErrors, Commitments);

                actual.Should().HaveCount(0);
            }
        }

        [TestFixture]
        public class WithSomeCommitments : WithMissingCommitments
        {
            public WithSomeCommitments()
            {
                Commitments = new Fixture().Build<Commitment>()
                    .With(x => x.CommitmentId, _commitmentIdTwo)
                    .With(x => x.CommitmentVersionId, _commitmentVersionIdTwo)
                    .CreateMany(1)
                    .ToList();
            }

            [Test, PaymentsDueAutoData]
            public void ThenThereShouldBeOutputForTheCommitments(DatalockValidationService sut)
            {
                var actual = sut.GetSuccessfulDatalocks(RawDatalocks, DatalockValidationErrors, Commitments);

                actual.Should().HaveCount(5);
            }
        }
    }
}
