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
    public class GivenAnAct2LearnerWithPartialRefund
    {
        private static readonly IFixture Fixture = new Fixture();

        private static readonly string PriceEpisode1 = Fixture.Create<string>();

        private static readonly int ProgrammeType = Fixture.Create<int>();
        private static readonly int StandardCode = Fixture.Create<int>();
        private static readonly int PathwayCode = Fixture.Create<int>();
        private static readonly int FrameworkCode = Fixture.Create<int>();

        private static readonly List<RawEarningForMathsOrEnglish> MathsAndEnglishEarnings =
            new List<RawEarningForMathsOrEnglish>();

        private static readonly List<PriceEpisode> Datalocks = new List<PriceEpisode>();

        [TestFixture]
        public class PriceChangeFrom500To750InR03
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

            private static readonly List<RequiredPaymentEntity> PastPayments =
                Fixture.Build<RequiredPaymentEntity>()
                    .With(x => x.PriceEpisodeIdentifier, PriceEpisode1)
                    .With(x => x.ApprenticeshipContractType, 2)
                    .With(x => x.StandardCode, StandardCode)
                    .With(x => x.ProgrammeType, ProgrammeType)
                    .With(x => x.PathwayCode, PathwayCode)
                    .With(x => x.FrameworkCode, FrameworkCode)
                    .CreateMany(6)
                    .ToList();

            

            [SetUp]
            public void Setup()
            {
                for (var i = 0; i < 6; i++)
                {
                    Earnings[i].TransactionType01 = 500;
                    Earnings[i].TransactionType02 = 0;
                    Earnings[i].TransactionType03 = 0;
                    Earnings[i].TransactionType04 = 0;
                    Earnings[i].TransactionType05 = 0;
                    Earnings[i].TransactionType06 = 0;
                    Earnings[i].TransactionType07 = 0;
                    Earnings[i].TransactionType08 = 0;
                    Earnings[i].TransactionType09 = 0;
                    Earnings[i].TransactionType10 = 0;
                    Earnings[i].TransactionType11 = 0;
                    Earnings[i].TransactionType12 = 0;
                    Earnings[i].TransactionType13 = 0;
                    Earnings[i].TransactionType14 = 0;
                    Earnings[i].TransactionType15 = 0;
                    
                    PastPayments[i].DeliveryMonth = Earnings[i].DeliveryMonth;
                    PastPayments[i].DeliveryYear = Earnings[i].DeliveryYear;
                    PastPayments[i].AmountDue = Earnings[i].TransactionType01;
                   
                    PastPayments[i].AimSeqNumber = Earnings[i].AimSeqNumber;
                    PastPayments[i].ApprenticeshipContractType = Earnings[i].ApprenticeshipContractType;
                    PastPayments[i].FundingLineType = Earnings[i].FundingLineType;
                    PastPayments[i].LearnAimRef = Earnings[i].LearnAimRef;
                    PastPayments[i].SfaContributionPercentage = Earnings[i].SfaContributionPercentage;
                    PastPayments[i].TransactionType = 1;

                    PastPayments[i].AccountId = null;
                    PastPayments[i].AccountVersionId = null;
                    PastPayments[i].CommitmentId = null;
                    PastPayments[i].CommitmentVersionId = null;
                }
            }

            [Test]
            public void ThereArePaymentsForR01Of500()
            {
                Earnings[0].TransactionType01 = 500;

                PastPayments[0].AmountDue = 500;
                

                var sut = new Learner(Earnings.Take(1), MathsAndEnglishEarnings, Datalocks, PastPayments.Take(0));
                var actual = sut.CalculatePaymentsDue();

                var expected = 500;
                actual.PayableEarnings.Sum(x => x.AmountDue).Should().Be(expected);
            }

            [Test]
            public void ThereArePaymentsForR02Of500()
            {
                Earnings[0].TransactionType01 = 500;
                Earnings[1].TransactionType01 = 500;

                PastPayments[0].AmountDue = 500;
                PastPayments[1].AmountDue = 500;
                

                var sut = new Learner(Earnings.Take(2), MathsAndEnglishEarnings, Datalocks, PastPayments.Take(1));
                var actual = sut.CalculatePaymentsDue();

                var expected = 500;
                actual.PayableEarnings.Sum(x => x.AmountDue).Should().Be(expected);
            }

            [Test]
            public void WithAPriceIncreaseTo750_ThereAreCorrectPaymentsForR03()
            {
                Earnings[0].TransactionType01 = 750;
                Earnings[1].TransactionType01 = 750;
                Earnings[2].TransactionType01 = 750;

                PastPayments[0].AmountDue = 500;
                PastPayments[1].AmountDue = 500;
                PastPayments[2].AmountDue = 500;
                

                var sut = new Learner(Earnings.Take(3), MathsAndEnglishEarnings, Datalocks, PastPayments.Take(2));
                var actual = sut.CalculatePaymentsDue();

                var expected = 1250;
                actual.PayableEarnings.Sum(x => x.AmountDue).Should().Be(expected);

                actual.PayableEarnings.Should().HaveCount(3);
                actual.PayableEarnings[0].AmountDue.Should().Be(250);
                actual.PayableEarnings[1].AmountDue.Should().Be(250);
                actual.PayableEarnings[2].AmountDue.Should().Be(750);
            }

            [Test]
            public void ThereArePaymentsForR04Of750()
            {
                Earnings[0].TransactionType01 = 750;
                Earnings[1].TransactionType01 = 750;
                Earnings[2].TransactionType01 = 750;
                Earnings[3].TransactionType01 = 750;

                PastPayments[0].AmountDue = 500;
                PastPayments[1].AmountDue = 500;
                PastPayments[2].AmountDue = 750;
                PastPayments[3].AmountDue = 250;
                PastPayments[4].AmountDue = 250;

                CopySignificantProperties(PastPayments[0], PastPayments[3]);
                CopySignificantProperties(PastPayments[1], PastPayments[4]);

                var sut = new Learner(Earnings.Take(4), MathsAndEnglishEarnings, Datalocks, PastPayments.Take(5));
                var actual = sut.CalculatePaymentsDue();

                var expected = 750;
                actual.PayableEarnings.Sum(x => x.AmountDue).Should().Be(expected);
            }

            void CopySignificantProperties(RequiredPaymentEntity from, RequiredPaymentEntity to)
            {
                to.DeliveryMonth = from.DeliveryMonth;
                to.DeliveryYear = from.DeliveryYear;
                to.AccountId = from.AccountId;
                to.AccountVersionId = from.AccountVersionId;
                to.CommitmentId = from.CommitmentId;
                to.TransactionType = from.TransactionType;
                to.AimSeqNumber = from.AimSeqNumber;
                to.ApprenticeshipContractType = from.ApprenticeshipContractType;
                to.FrameworkCode = from.FrameworkCode;
                to.StandardCode = from.StandardCode;
                to.ProgrammeType = from.ProgrammeType;
                to.PathwayCode = from.PathwayCode;
                to.FundingLineType = from.FundingLineType;
                to.LearnAimRef = from.LearnAimRef;
                to.PriceEpisodeIdentifier = from.PriceEpisodeIdentifier;
            }
        }
    }
}
