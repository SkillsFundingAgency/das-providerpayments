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

        private static readonly int ProgrammeType = Fixture.Create<int>();
        private static readonly int StandardCode = Fixture.Create<int>();
        private static readonly int PathwayCode = Fixture.Create<int>();
        private static readonly int FrameworkCode = Fixture.Create<int>();
        
        private static readonly List<RawEarning> Earnings = Fixture.Build<RawEarning>()
            .With(x => x.PriceEpisodeIdentifier, PriceEpisode1)
            .With(x => x.ApprenticeshipContractType, 1)
            .With(x => x.StandardCode, StandardCode)
            .With(x => x.ProgrammeType, ProgrammeType)
            .With(x => x.PathwayCode, PathwayCode)
            .With(x => x.FrameworkCode, FrameworkCode)
            .CreateMany(6)
            .ToList();

        private static readonly List<RequiredPaymentEntity> PastPayments =
            new List<RequiredPaymentEntity>();


        [TestFixture]
        public class WithNoMathsAndEnglishEarnings
        {
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
                    public void ThenThereAreNoNonPayablePayments()
                    {
                        // 1 datalock price episodes
                        // No maths and english
                        // 6 earnings
                        // No past payments

                        var sut = new Learner(Earnings, MathsAndEnglishEarnings, Datalocks, PastPayments);
                        var actual = sut.CalculatePaymentsDue();

                        actual.NonPayableEarnings.Should().BeEmpty();
                    }

                    [Test]
                    public void ThenThereAreTheCorrectNumberOfPayments()
                    {
                        // 1 datalock price episodes
                        // No maths and english
                        // 6 earnings
                        // No past payments

                        var sut = new Learner(Earnings, MathsAndEnglishEarnings, Datalocks, PastPayments);
                        var actual = sut.CalculatePaymentsDue();

                        var expected = Earnings.NumberOfNonZeroTransactions();
                        actual.PayableEarnings.Should().HaveCount(expected);
                    }

                    [Test]
                    public void ThenThePaymentsHaveTheCorrectAmount()
                    {
                        // 1 datalock price episodes
                        // No maths and english
                        // 6 earnings
                        // No past payments

                        var sut = new Learner(Earnings, MathsAndEnglishEarnings, Datalocks, PastPayments);
                        var actual = sut.CalculatePaymentsDue();

                        var expected = Earnings.TotalAmount();
                        actual.PayableEarnings.Sum(x => x.AmountDue).Should().Be(expected);
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

        [TestFixture]
        public class WithMathsAndEnglishPayments
        {
            private static readonly List<RawEarningForMathsOrEnglish> MathsAndEnglishEarnings =
                Fixture.Build<RawEarningForMathsOrEnglish>()
                    .With(x => x.PriceEpisodeIdentifier, PriceEpisode1)
                    .With(x => x.ApprenticeshipContractType, 1)
                    .With(x => x.StandardCode, StandardCode)
                    .With(x => x.ProgrammeType, ProgrammeType)
                    .With(x => x.PathwayCode, PathwayCode)
                    .With(x => x.FrameworkCode, FrameworkCode)
                    .CreateMany(6)
                    .ToList();

            [TestFixture]
            public class WithNoDatalockFailures
            {
                private static readonly List<PriceEpisode> Datalocks = Fixture.Build<PriceEpisode>()
                    .With(x => x.PriceEpisodeIdentifier, PriceEpisode1)
                    .With(x => x.Payable, true)
                    .CreateMany(1)
                    .ToList();

                [Test]
                public void ThenThereAreNoNonPayablePayments()
                {
                    // 1 datalock price episodes
                    // 6 maths and english
                    // 6 earnings
                    // No past payments

                    var sut = new Learner(Earnings, MathsAndEnglishEarnings, Datalocks, PastPayments);
                    var actual = sut.CalculatePaymentsDue();

                    actual.NonPayableEarnings.Should().BeEmpty();
                }

                [Test]
                public void ThenThereAreTheCorrectNumberOfPayments()
                {
                    // 1 datalock price episodes
                    // 6 maths and english
                    // 6 earnings
                    // No past payments

                    var sut = new Learner(Earnings, MathsAndEnglishEarnings, Datalocks, PastPayments);
                    var actual = sut.CalculatePaymentsDue();

                    var expected = Earnings.NumberOfNonZeroTransactions() +
                                   MathsAndEnglishEarnings.NumberOfNonZeroTransactions();

                    actual.PayableEarnings.Should().HaveCount(expected);
                }

                [Test]
                public void ThenThePaymentsHaveTheCorrectAmount()
                {
                    // 1 datalock price episodes
                    // 6 maths and english
                    // 6 earnings
                    // No past payments

                    var sut = new Learner(Earnings, MathsAndEnglishEarnings, Datalocks, PastPayments);
                    var actual = sut.CalculatePaymentsDue();

                    var expected = Earnings.TotalAmount() + MathsAndEnglishEarnings.TotalAmount();

                    actual.PayableEarnings.Sum(x => x.AmountDue).Should().Be(expected);
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
                    // 6 Maths or English
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
                    // 6 Maths or English
                    // 6 earnings
                    // 0 past payments

                    var sut = new Learner(Earnings, MathsAndEnglishEarnings, Datalocks, PastPayments);
                    var actual = sut.CalculatePaymentsDue();

                    var expected = Earnings.NumberOfNonZeroTransactions() +
                                   MathsAndEnglishEarnings.NumberOfNonZeroTransactions();

                    actual.NonPayableEarnings.Should().HaveCount(expected);
                }
            }
        }
    }
}
