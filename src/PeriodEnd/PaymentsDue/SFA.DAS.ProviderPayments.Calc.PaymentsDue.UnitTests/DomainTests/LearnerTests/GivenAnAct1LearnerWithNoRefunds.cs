using System.Collections.Generic;
using System.Linq;
using AutoFixture;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Domain;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data.Entities;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.UnitTests.Utilities.Extensions;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.UnitTests.DomainTests.LearnerTests
{
    [TestFixture]
    public class GivenAnAct1LearnerWithNoRefunds
    {
        private static readonly IFixture Fixture = new Fixture();

        private static readonly string PriceEpisode1 = Fixture.Create<string>();
        private static readonly string PriceEpisode2 = Fixture.Create<string>();

        private static readonly List<RawEarning> Earnings = Fixture.Build<RawEarning>()
            .With(x => x.PriceEpisodeIdentifier, PriceEpisode1)
            .With(x => x.ApprenticeshipContractType, 1)
            .CreateMany(6)
            .ToList();
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

                [Test]
                public void ThenThereAreNoNonPaymentsForARefund()
                {
                    // 1 Price Episode 
                    // No Maths or English
                    // 0 earnings
                    // 6 past payments
                    
                    var pastPayments = Fixture.Build<RequiredPaymentEntity>()
                        .CreateMany(6)
                        .ToList();

                    var sut = new Learner(Earnings, MathsAndEnglishEarnings, Datalocks, pastPayments);
                    var actual = sut.CalculatePaymentsDue();

                    actual.NonPayableEarnings.Should().BeEmpty();
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


                [Test]
                public void ThenThereAreNoPayments()
                {
                    // 1 Price Episode (non payable)
                    // No Maths or English
                    // 6 earnings
                    // 0 past payments

                    var sut = new Learner(Earnings, MathsAndEnglishEarnings, Datalocks, PastPayments);
                    var actual = sut.CalculatePaymentsDue();

                    actual.PayableEarnings.Should().BeEmpty();
                }

                [Test]
                public void ThenThereAreTheCorrectNumberOfNonPayments()
                {
                    // 1 Price Episode (non payable)
                    // No Maths or English
                    // 6 earnings
                    // 0 past payments

                    var sut = new Learner(Earnings, MathsAndEnglishEarnings, Datalocks, PastPayments);
                    var actual = sut.CalculatePaymentsDue();

                    var expected = Earnings.NumberOfNonZeroTransactions();
                    actual.NonPayableEarnings.Should().HaveCount(expected);
                }
            }
        }
    }
}
