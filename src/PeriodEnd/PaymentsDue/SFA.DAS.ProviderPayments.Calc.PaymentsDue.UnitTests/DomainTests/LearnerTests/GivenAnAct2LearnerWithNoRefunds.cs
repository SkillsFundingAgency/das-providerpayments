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
    [TestFixture(Ignore="Temp")]
    public class GivenAnAct2LearnerWithNoRefunds
    {
        private static readonly IFixture Fixture = new Fixture();

        private static readonly string PriceEpisode1 = Fixture.Create<string>();

        private static readonly List<PriceEpisode> Datalocks = new List<PriceEpisode>();

        private static readonly int ProgrammeType = Fixture.Create<int>();
        private static readonly int StandardCode = Fixture.Create<int>();
        private static readonly int PathwayCode = Fixture.Create<int>();
        private static readonly int FrameworkCode = Fixture.Create<int>();


        [TestFixture(Ignore = "Temp")]
        public class WithNoPastPayments
        {
            private static readonly List<RequiredPaymentEntity> PastPayments = new List<RequiredPaymentEntity>();

            [TestFixture(Ignore = "Temp")]
            public class WithSixEarnings
            {
                private static readonly List<RawEarning> Earnings = Fixture.Build<RawEarning>()
                    .With(x => x.PriceEpisodeIdentifier, PriceEpisode1)
                    .With(x => x.ApprenticeshipContractType, 2)
                    .With(x => x.StandardCode, StandardCode)
                    .With(x => x.ProgrammeType, ProgrammeType)
                    .With(x => x.PathwayCode, PathwayCode)
                    .With(x => x.FrameworkCode, FrameworkCode)
                    .CreateMany(6)
                    .ToList();

                [TestFixture(Ignore = "Temp")]
                public class WithNoMathsAndEnglishEarnings
                {
                    private static readonly List<RawEarningForMathsOrEnglish> MathsAndEnglishEarnings =
                        new List<RawEarningForMathsOrEnglish>();

                    [Test]
                    public void ThenThereAreNoNonPayablePayments()
                    {
                        // No datalock price episodes
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
                        // No datalock price episodes
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
                        // No datalock price episodes
                        // No maths and english
                        // 6 earnings
                        // No past payments

                        var sut = new Learner(Earnings, MathsAndEnglishEarnings, Datalocks, PastPayments);
                        var actual = sut.CalculatePaymentsDue();

                        var expected = Earnings.TotalAmount();
                        actual.PayableEarnings.Sum(x => x.AmountDue).Should().Be(expected);
                    }
                }

                [TestFixture(Ignore = "Temp")]
                public class WithMathsAndEnglishEarnings
                {
                    private static readonly List<RawEarningForMathsOrEnglish> MathsAndEnglishEarnings =
                        Fixture.Build<RawEarningForMathsOrEnglish>()
                            .With(x => x.PriceEpisodeIdentifier, PriceEpisode1)
                            .With(x => x.ApprenticeshipContractType, 2)
                            .With(x => x.StandardCode, StandardCode)
                            .With(x => x.ProgrammeType, ProgrammeType)
                            .With(x => x.PathwayCode, PathwayCode)
                            .With(x => x.FrameworkCode, FrameworkCode)
                            .CreateMany(6)
                            .ToList();

                    [Test]
                    public void ThenThereAreNoNonPayablePayments()
                    {
                        // No datalock price episodes
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
                        // No datalock price episodes
                        // No maths and english
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
                        // No datalock price episodes
                        // No maths and english
                        // 6 earnings
                        // No past payments

                        var sut = new Learner(Earnings, MathsAndEnglishEarnings, Datalocks, PastPayments);
                        var actual = sut.CalculatePaymentsDue();

                        var expected = Earnings.TotalAmount() + MathsAndEnglishEarnings.TotalAmount();
                        actual.PayableEarnings.Sum(x => x.AmountDue).Should().Be(expected);
                    }
                }
            }
        }
    }
}
