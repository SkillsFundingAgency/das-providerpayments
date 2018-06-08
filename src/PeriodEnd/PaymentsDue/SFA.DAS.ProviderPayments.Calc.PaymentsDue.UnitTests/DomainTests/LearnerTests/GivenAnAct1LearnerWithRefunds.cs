using System.Collections.Generic;
using System.Linq;
using AutoFixture;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Domain;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data.Entities;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.UnitTests.DomainTests.LearnerTests
{
    [TestFixture]
    public class GivenAnAct1LearnerWithRefunds
    {
        private static readonly IFixture Fixture = new Fixture();

        private static readonly string PriceEpisode1 = Fixture.Create<string>();
        private static readonly string PriceEpisode2 = Fixture.Create<string>();

        private static readonly List<RawEarningForMathsOrEnglish> MathsAndEnglishEarnings =
            new List<RawEarningForMathsOrEnglish>();

        [TestFixture]
        public class WithASinglePriceEpisode
        {
            [TestFixture]
            public class WithNoDatalockFailures
            {
                private static readonly List<PriceEpisode> Datalocks = Fixture.Build<PriceEpisode>()
                    .With(x => x.PriceEpisodeIdentifier, PriceEpisode1)
                    .With(x => x.Payable, true)
                    .CreateMany(1)
                    .ToList();

                [TestFixture]
                public class WithPastPayments
                {
                    private static readonly List<RequiredPaymentEntity> pastPayments = 
                        Fixture.Build<RequiredPaymentEntity>()
                        .CreateMany(6)
                        .ToList();
                    
                    [TestFixture]
                    public class WithNoEarnings
                    {
                        private static readonly List<RawEarning> Earnings = new List<RawEarning>();
                        
                        [Test]
                        public void ThenThereAreNoNonPayments()
                        {
                            // 1 Price Episode 
                            // No Maths or English
                            // 0 earnings
                            // 6 past payments

                            var sut = new Learner(Earnings, MathsAndEnglishEarnings, Datalocks, pastPayments);
                            var actual = sut.CalculatePaymentsDue();

                            actual.NonPayableEarnings.Should().BeEmpty();
                        }

                        [Test]
                        public void ThenThePaymentsAreANegativeOfTheEarnings()
                        {
                            // 1 Price Episode 
                            // No Maths or English
                            // 0 earnings
                            // 6 past payments

                            var sut = new Learner(Earnings, MathsAndEnglishEarnings, Datalocks, pastPayments);
                            var actual = sut.CalculatePaymentsDue();

                            var expected = pastPayments.Sum(x => x.AmountDue) * -1;
                            actual.PayableEarnings.Sum(x => x.AmountDue).Should().Be(expected);
                        }
                    }
                }
            }

            [TestFixture]
            public class WithDatalockFailures
            {
                private static readonly List<PriceEpisode> Datalocks = Fixture.Build<PriceEpisode>()
                    .With(x => x.PriceEpisodeIdentifier, PriceEpisode1)
                    .With(x => x.Payable, false)
                    .CreateMany(1)
                    .ToList();

                private static readonly List<RequiredPaymentEntity> PastPayments =
                    new List<RequiredPaymentEntity>();

                [TestFixture]
                public class WithNoEarnings
                {
                    private static readonly List<RawEarning> Earnings = new List<RawEarning>();

                    [TestFixture]
                    public class WithPastPayments
                    {
                        private static readonly List<RequiredPaymentEntity> pastPayments =
                            Fixture.Build<RequiredPaymentEntity>()
                                .CreateMany(6)
                                .ToList();

                        [Test]
                        public void ThenThereAreNoPayments()
                        {
                            // 1 Price Episode 
                            // No Maths or English
                            // 0 earnings
                            // 6 past payments

                            var sut = new Learner(Earnings, MathsAndEnglishEarnings, Datalocks, pastPayments);
                            var actual = sut.CalculatePaymentsDue();

                            actual.NonPayableEarnings.Should().BeEmpty();
                        }
                    }
                }
            }
        }
    }
}
