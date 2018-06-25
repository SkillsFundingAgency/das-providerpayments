using System.Collections.Generic;
using System.Linq;
using AutoFixture;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data.Entities;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Services;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.UnitTests.Utilities;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.UnitTests.ServiceTests.GivenADatalockValidationService
{
    [TestFixture]
    public class WithMissingCommitments
    {
        private static List<DatalockOutputEntity> RawDatalocks { get; set; }
        private static List<Commitment> Commitments { get; set; }
        private static List<DatalockValidationError> DatalockValidationErrors { get; set; }

        private static long _commitmentIdTwo;

        public WithMissingCommitments()
        {
            var fixture = new Fixture();
            var commitmentIdOne = fixture.Create<long>();
            _commitmentIdTwo = fixture.Create<long>();

            RawDatalocks = fixture.Build<DatalockOutputEntity>()
                .With(x => x.CommitmentId, commitmentIdOne)
                .With(x => x.Payable, true)
                .CreateMany(5)
                .ToList();
            RawDatalocks.AddRange(fixture.Build<DatalockOutputEntity>()
                .With(x => x.CommitmentId, _commitmentIdTwo)
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

            [Theory, PaymentsDueAutoData]
            public void ThenTheOutputShouldBeEmpty(DatalockValidationService sut)
            {
                var actual = sut.ProcessDatalocks(RawDatalocks, DatalockValidationErrors, Commitments);

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
                    .CreateMany(1)
                    .ToList();
            }

            [Theory, PaymentsDueAutoData]
            public void ThenThereShouldBeOutputForTheCommitments(DatalockValidationService sut)
            {
                var actual = sut.ProcessDatalocks(RawDatalocks, DatalockValidationErrors, Commitments);

                actual.Should().HaveCount(5);
            }
        }
    }
}
