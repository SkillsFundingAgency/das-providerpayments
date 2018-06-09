using System.Collections.Generic;
using System.Linq;
using AutoFixture;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Domain;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data.Entities;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.UnitTests.DomainTests.DatalockTests
{
    [TestFixture]
    public class GivenADatalockValidator
    {
        private static readonly IFixture Fixture = new Fixture();

        private static readonly string PriceEpisode1 = Fixture.Create<string>();

        private static readonly List<Commitment> Commitments = Fixture.Build<Commitment>()
            .CreateMany()
            .ToList();

        [TestFixture]
        public class WithAMismatchedCommitment
        {
            private static readonly List<DatalockOutput> Datalocks =
                Fixture.Build<DatalockOutput>()
                    .With(x => x.CommitmentId, Commitments[2].CommitmentId)
                    .With(x => x.PriceEpisodeIdentifier, PriceEpisode1)
                    .With(x => x.Payable, true)
                    .CreateMany()
                    .ToList();

            [Test]
            public void ThenAllPriceEpisodesAreNotPayable()
            {
                var sut = new IShouldBeInTheDatalockComponent();
                var actual = sut.ValidatePriceEpisodes(Commitments.Take(1).ToList(), Datalocks);

                foreach (var priceEpisode in actual)
                {
                    priceEpisode.Payable.Should().BeFalse();
                }
            }

            [Test]
            public void ThenThereShouldBePriceEpisodes()
            {
                var sut = new IShouldBeInTheDatalockComponent();
                var actual = sut.ValidatePriceEpisodes(Commitments.Take(1).ToList(), Datalocks);

                actual.Should().NotBeEmpty();
            }

            [Test]
            public void ThenThePriceEpisodesShouldHaveTheCorrectId()
            {
                var sut = new IShouldBeInTheDatalockComponent();
                var actual = sut.ValidatePriceEpisodes(Commitments.Take(1).ToList(), Datalocks);

                foreach (var priceEpisode in actual)
                {
                    priceEpisode.PriceEpisodeIdentifier.Should().Be(PriceEpisode1);
                }
            }

            [Test]
            public void ThenThePriceEpisodesShouldHaveTheCorrectAccountId()
            {
                var sut = new IShouldBeInTheDatalockComponent();
                var actual = sut.ValidatePriceEpisodes(Commitments.Take(1).ToList(), Datalocks);

                foreach (var priceEpisode in actual)
                {
                    priceEpisode.AccountId.Should().Be(0);
                }
            }
        }

        [TestFixture]
        public class WithNoMatchingCommitments
        {

        }

    }
}
