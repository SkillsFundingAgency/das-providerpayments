using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using AutoFixture;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Domain;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data.Entities;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.UnitTests.ProductionScenarios
{
    [TestFixture]
    public class GivenALearnerWithTwoCommitments
    {
        private static IFixture Fixture = new Fixture();
        private static long CommitmentOne = Fixture.Create<long>();
        private static long CommitmentTwo = Fixture.Create<long>();
        private static string PriceEpisodeIdentifierForThisYear = "25-80-01/08/2017";
        private static string PriceEpisodeIdentifierForNextYear = "25-80-01/08/2018";

        private static string LearnAimRef = Fixture.Create<string>();
        private static long AccountId = Fixture.Create<long>();
        private static string FundingLineType = Fixture.Create<string>();

        private static List<RequiredPaymentEntity> PastPayments = Fixture.Build<RequiredPaymentEntity>()
            .With(x => x.CommitmentId, CommitmentOne)
            .With(x => x.AccountId, AccountId)
            .With(x => x.TransactionType, 1)
            .With(x => x.AmountDue, 1200)
            .With(x => x.StandardCode, 80)
            .With(x => x.ProgrammeType, 25)
            .With(x => x.FrameworkCode, 0)
            .With(x => x.PathwayCode, 0)
            .With(x => x.PriceEpisodeIdentifier, PriceEpisodeIdentifierForThisYear)
            .With(x => x.LearnAimRef, LearnAimRef)
            .With(x => x.ApprenticeshipContractType, 1)
            .With(x => x.SfaContributionPercentage, 0.9m)
            .With(x => x.FundingLineType, FundingLineType)
            .CreateMany(9)
            .ToList();

        private static List<DatalockOutput> Datalocks = new List<DatalockOutput>();

        private static List<Commitment> Commitments = new List<Commitment>
        {
            new Commitment
            {
                AccountId = AccountId,
                CommitmentId = CommitmentOne,
                StandardCode = 80,
                ProgrammeType = 25,
                FrameworkCode = 0,
                PathwayCode = 0,
                PaymentStatus = 3,
                IsLevyPayer = true,
            },
            new Commitment
            {
                AccountId = AccountId,
                CommitmentId = CommitmentTwo,
                StandardCode = 80,
                ProgrammeType = 25,
                FrameworkCode = 0,
                PathwayCode = 0,
                PaymentStatus = 1,
                IsLevyPayer = true,
            }
        };

        private static List<RawEarning> Earnings = Fixture.Build<RawEarning>()
            .With(x => x.PriceEpisodeIdentifier, PriceEpisodeIdentifierForThisYear)
            .With(x => x.ProgrammeType, 25)
            .With(x => x.StandardCode, 80)
            .With(x => x.FrameworkCode, 0)
            .With(x => x.PathwayCode, 0)
            .With(x => x.SfaContributionPercentage, 0.9m)
            .With(x => x.FundingLineType, FundingLineType)
            .With(x => x.LearnAimRef, LearnAimRef)
            .With(x => x.TransactionType01, 1200)
            .With(x => x.TransactionType02, 0)
            .With(x => x.TransactionType03, 0)
            .With(x => x.TransactionType04, 0)
            .With(x => x.TransactionType05, 0)
            .With(x => x.TransactionType06, 0)
            .With(x => x.TransactionType07, 0)
            .With(x => x.TransactionType08, 0)
            .With(x => x.TransactionType09, 0)
            .With(x => x.TransactionType10, 0)
            .With(x => x.TransactionType11, 0)
            .With(x => x.TransactionType12, 0)
            .With(x => x.TransactionType13, 0)
            .With(x => x.TransactionType14, 0)
            .With(x => x.TransactionType15, 0)
            .With(x => x.ApprenticeshipContractType, 1)
            .CreateMany(10)
            .ToList();

        static int DeliveryYearFromPeriod(int period)
        {
            if (period < 6)
            {
                return 2017;
            }

            return 2018;
        }

        static int DeliveryMonthFromPeriod(int period)
        {
            if (period < 6)
            {
                return period + 7;
            }

            return period - 6;
        }

        [SetUp]
        public void Setup()
        {
            var secondSetOfPastPayments = Fixture.Build<RequiredPaymentEntity>()
                .With(x => x.CommitmentId, CommitmentTwo)
                .With(x => x.AccountId, AccountId)
                .With(x => x.TransactionType, 1)
                .With(x => x.AmountDue, 1200)
                .With(x => x.StandardCode, 80)
                .With(x => x.ProgrammeType, 25)
                .With(x => x.FrameworkCode, 0)
                .With(x => x.PathwayCode, 0)
                .With(x => x.PriceEpisodeIdentifier, PriceEpisodeIdentifierForThisYear)
                .With(x => x.LearnAimRef, LearnAimRef)
                .With(x => x.ApprenticeshipContractType, 1)
                .With(x => x.SfaContributionPercentage, 0.9m)
                .With(x => x.FundingLineType, FundingLineType)
                .CreateMany(1)
                .ToList();
            PastPayments.AddRange(secondSetOfPastPayments);


            var datalockForNextYearFirstCommitment = Fixture.Build<DatalockOutput>()
                .With(x => x.PriceEpisodeIdentifier, PriceEpisodeIdentifierForNextYear)
                .With(x => x.CommitmentId, CommitmentOne)
                .With(x => x.Payable, false)
                .CreateMany(12)
                .ToList();
            var datalockForThisYearFirstCommitment = Fixture.Build<DatalockOutput>()
                .With(x => x.PriceEpisodeIdentifier, PriceEpisodeIdentifierForThisYear)
                .With(x => x.CommitmentId, CommitmentOne)
                .With(x => x.Payable, false)
                .CreateMany(12)
                .ToList();
            var datalockForNextYearSecondommitment = Fixture.Build<DatalockOutput>()
                .With(x => x.PriceEpisodeIdentifier, PriceEpisodeIdentifierForNextYear)
                .With(x => x.CommitmentId, CommitmentTwo)
                .With(x => x.Payable, true)
                .CreateMany(12)
                .ToList();
            var datalockForThisYearSecondCommitment = Fixture.Build<DatalockOutput>()
                .With(x => x.PriceEpisodeIdentifier, PriceEpisodeIdentifierForThisYear)
                .With(x => x.CommitmentId, CommitmentTwo)
                .With(x => x.Payable, true)
                .CreateMany(12)
                .ToList();


            for (var i = 0; i < 10; i++)
            {
                PastPayments[i].DeliveryYear = DeliveryYearFromPeriod(i + 1);
                PastPayments[i].DeliveryMonth = DeliveryMonthFromPeriod(i + 1);

                Earnings[i].Period = i + 1;
                Earnings[i].DeliveryYear = DeliveryYearFromPeriod(i + 1);
                Earnings[i].DeliveryMonth = DeliveryMonthFromPeriod(i + 1);

                datalockForNextYearFirstCommitment[i].Period = i + 1;
                datalockForNextYearSecondommitment[i].Period = i + 1;
                datalockForThisYearFirstCommitment[i].Period = i + 1;
                datalockForThisYearSecondCommitment[i].Period = i + 1;
            }
            for (var i = 10; i < 12; i++)
            {
                datalockForNextYearFirstCommitment[i].Period = i + 1;
                datalockForNextYearSecondommitment[i].Period = i + 1;
                datalockForThisYearFirstCommitment[i].Period = i + 1;
                datalockForThisYearSecondCommitment[i].Period = i + 1;
            }

            Datalocks.AddRange(datalockForNextYearFirstCommitment);
            Datalocks.AddRange(datalockForNextYearSecondommitment);
            Datalocks.AddRange(datalockForThisYearFirstCommitment);
            Datalocks.AddRange(datalockForThisYearSecondCommitment);
        }

        [Test]
        public void ThereShouldBeNoRefunds()
        {
            var datalockComponent = new IShouldBeInTheDatalockComponent();
            var datalockResult = datalockComponent.ValidatePriceEpisodes(Commitments, Datalocks, 2017);

            var sut = new Learner(Earnings, new List<RawEarningForMathsOrEnglish>(), datalockResult, PastPayments);
            var actual = sut.CalculatePaymentsDue();

            actual.PayableEarnings.Should().NotContain(x => x.AmountDue < 0);
        }
    }
}
