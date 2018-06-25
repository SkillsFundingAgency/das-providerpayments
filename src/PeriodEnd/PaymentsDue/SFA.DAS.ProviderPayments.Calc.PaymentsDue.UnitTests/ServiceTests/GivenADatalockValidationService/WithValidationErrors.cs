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
    class WithValidationErrors
    {
        private static List<DatalockOutputEntity> RawDatalocks { get; set; }
        private static List<Commitment> Commitments { get; set; }
        private static List<DatalockValidationError> DatalockValidationErrors { get; set; }


        public WithValidationErrors()
        {
            var fixture = new Fixture();
            var commitmentIdOne = fixture.Create<long>();
            var priceEpisodeId = fixture.Create<string>();

            RawDatalocks = fixture.Build<DatalockOutputEntity>()
                .With(x => x.CommitmentId, commitmentIdOne)
                .With(x => x.Payable, true)
                .With(x => x.PriceEpisodeIdentifier, priceEpisodeId)
                .CreateMany(10)
                .ToList();

            DatalockValidationErrors = fixture.Build<DatalockValidationError>()
                .With(x => x.PriceEpisodeIdentifier, priceEpisodeId)
                .CreateMany(1)
                .ToList();

            Commitments = fixture.Build<Commitment>()
                .With(x => x.CommitmentId, commitmentIdOne)
                .CreateMany(1)
                .ToList();
        }

        [Test, PaymentsDueAutoData]
        public void ThenTheOutputShouldBeEmpty(DatalockValidationService sut)
        {
            var actual = sut.ProcessDatalocks(RawDatalocks, DatalockValidationErrors, Commitments);

            actual.Should().HaveCount(0);
        }
    }
}
