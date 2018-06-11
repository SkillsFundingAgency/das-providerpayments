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
    public class GivenAnAct1LearnerChangesPriceAndADatalockOccurs
    {
        private static readonly IFixture Fixture = new Fixture();

        private static readonly string PriceEpisode1 = Fixture.Create<string>();

        private static readonly int ProgrammeType = Fixture.Create<int>();
        private static readonly int StandardCode = Fixture.Create<int>();
        private static readonly int PathwayCode = Fixture.Create<int>();
        private static readonly int FrameworkCode = Fixture.Create<int>();

        private static readonly List<RawEarningForMathsOrEnglish> MathsAndEnglishEarnings =
            new List<RawEarningForMathsOrEnglish>();

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
            Fixture.Build<RequiredPaymentEntity>()
                .With(x => x.PriceEpisodeIdentifier, PriceEpisode1)
                .With(x => x.ApprenticeshipContractType, 1)
                .With(x => x.StandardCode, StandardCode)
                .With(x => x.ProgrammeType, ProgrammeType)
                .With(x => x.PathwayCode, PathwayCode)
                .With(x => x.FrameworkCode, FrameworkCode)
                .CreateMany(6)
                .ToList();

        private static readonly List<PriceEpisode> Datalocks = Fixture.Build<PriceEpisode>()
            .With(x => x.PriceEpisodeIdentifier, PriceEpisode1)
            .With(x => x.Payable, true)
            .CreateMany(1)
            .ToList();

        [SetUp]
        public void Setup()
        {
            for (var i = 0; i < 6; i++)
            {
                Earnings[i].TransactionType01 = 100;
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
                PastPayments[i].AccountId = Datalocks[0].AccountId;
                PastPayments[i].AccountVersionId = Datalocks[0].AccountVersionId;
                PastPayments[i].CommitmentId = Datalocks[0].CommitmentId;
                PastPayments[i].CommitmentVersionId = Datalocks[0].CommitmentVersionId;

                PastPayments[i].AimSeqNumber = Earnings[i].AimSeqNumber;
                PastPayments[i].ApprenticeshipContractType = Earnings[i].ApprenticeshipContractType;
                PastPayments[i].FundingLineType = Earnings[i].FundingLineType;
                PastPayments[i].LearnAimRef = Earnings[i].LearnAimRef;
                PastPayments[i].SfaContributionPercentage = Earnings[i].SfaContributionPercentage;
                PastPayments[i].TransactionType = 1;
                PastPayments[i].UseLevyBalance = Datalocks[0].Payable;
            }
        }

        [Test]
        public void WithPassingDatalock_ThereArePaymentsForR01()
        {
            Datalocks[0].Payable = true;

            var sut = new Learner(Earnings.Take(1), MathsAndEnglishEarnings, Datalocks, PastPayments.Take(0));
            var actual = sut.CalculatePaymentsDue();

            var expected = Earnings.Skip(0).Take(1).TotalAmount();
            actual.PayableEarnings.Sum(x => x.AmountDue).Should().Be(expected);
        }

        [Test]
        public void WithFailingDatalockAndAnIncreaseInPrice_ThereAreNoPaymentsForR02()
        {
            Datalocks[0].Payable = false;
            Earnings.ForEach(x=>x.TransactionType01 = 300);

            var sut = new Learner(Earnings.Take(2), MathsAndEnglishEarnings, Datalocks, PastPayments.Take(1));
            var actual = sut.CalculatePaymentsDue();

            var expected = 0; 
            actual.PayableEarnings.Sum(x => x.AmountDue).Should().Be(expected);
        }

        [Test]
        public void WithFailingDatalockAndADecreaseInPrice_ThereAreNoPaymentsForR02()
        {
            Datalocks[0].Payable = false;
            Earnings.ForEach(x => x.TransactionType01 = 50);

            var sut = new Learner(Earnings.Take(2), MathsAndEnglishEarnings, Datalocks, PastPayments.Take(1));
            var actual = sut.CalculatePaymentsDue();

            var expected = 0; 
            actual.PayableEarnings.Sum(x => x.AmountDue).Should().Be(expected);
        }

        [Test]
        public void WithFailingDatalockAndNoChangeInPrice_ThereAreNoPaymentsForR02()
        {
            Datalocks[0].Payable = false;

            var sut = new Learner(Earnings.Take(2), MathsAndEnglishEarnings, Datalocks, PastPayments.Take(1));
            var actual = sut.CalculatePaymentsDue();

            var expected = 0;
            actual.PayableEarnings.Sum(x => x.AmountDue).Should().Be(expected);
        }

        [Test]
        public void WithPassingDatalockInR03ButHavingMadeNoPaymentsInR02AndAPriceIncrease_ThereArePaymentsForR03()
        {
            Datalocks[0].Payable = true;
            Earnings.ForEach(x => x.TransactionType01 = 150);

            var sut = new Learner(Earnings.Take(3), MathsAndEnglishEarnings, Datalocks, PastPayments.Take(1));
            var actual = sut.CalculatePaymentsDue();

            var expected = 350;
            actual.PayableEarnings.Sum(x => x.AmountDue).Should().Be(expected);
        }



    }
}
