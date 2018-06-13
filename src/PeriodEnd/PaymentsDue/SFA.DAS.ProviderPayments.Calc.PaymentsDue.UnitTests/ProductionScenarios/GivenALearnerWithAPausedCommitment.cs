using System;
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
    public class GivenALearnerWithAPausedCommitment
    {
        private static IFixture Fixture = new Fixture();
        private static long CommitmentOne = Fixture.Create<long>();
        private static string PriceEpisodeIdentifierForThisYear = "2-444-1-14/09/2017";
        
        private static string LearnAimRefForZprog = Fixture.Create<string>();
        private static string LearnAimRefForMaths = Fixture.Create<string>();
        private static string LearnAimRefForEnglish = Fixture.Create<string>();
        private static long AccountId = Fixture.Create<long>();
        private static string FundingLineType = Fixture.Create<string>();

        private static List<RequiredPaymentEntity> PastPayments = new List<RequiredPaymentEntity>();
        private static List<RawEarningForMathsOrEnglish> MathsAndEnglishEarnings = new List<RawEarningForMathsOrEnglish>();

        private static List<DatalockOutput> Datalocks = new List<DatalockOutput>();

        private static List<Commitment> Commitments = new List<Commitment>
        {
            new Commitment
            {
                AccountId = AccountId,
                CommitmentId = CommitmentOne,
                StandardCode = 0,
                ProgrammeType = 2,
                FrameworkCode = 444,
                PathwayCode = 1,
                PaymentStatus = 2,
                IsLevyPayer = true,
            },
        };

        private static List<RawEarning> Earnings = Fixture.Build<RawEarning>()
            .With(x => x.PriceEpisodeIdentifier, PriceEpisodeIdentifierForThisYear)
            .With(x => x.ProgrammeType, 2)
            .With(x => x.StandardCode, 0)
            .With(x => x.FrameworkCode, 444)
            .With(x => x.PathwayCode, 1)
            .With(x => x.SfaContributionPercentage, 0.9m)
            .With(x => x.FundingLineType, FundingLineType)
            .With(x => x.LearnAimRef, LearnAimRefForZprog)
            .With(x => x.TransactionType01, 70.58824m)
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
            .CreateMany(4)
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
            var pastPayments = Fixture.Build<RequiredPaymentEntity>()
                .With(x => x.CommitmentId, CommitmentOne)
                .With(x => x.AccountId, AccountId)
                .With(x => x.TransactionType, 1)
                .With(x => x.AmountDue, 70.58824m)
                .With(x => x.StandardCode, 0)
                .With(x => x.ProgrammeType, 2)
                .With(x => x.FrameworkCode, 444)
                .With(x => x.PathwayCode, 1)
                .With(x => x.PriceEpisodeIdentifier, PriceEpisodeIdentifierForThisYear)
                .With(x => x.LearnAimRef, LearnAimRefForZprog)
                .With(x => x.ApprenticeshipContractType, 1)
                .With(x => x.SfaContributionPercentage, 0.9m)
                .With(x => x.FundingLineType, FundingLineType)
                .CreateMany(4)
                .ToList();
            PastPayments.AddRange(pastPayments);
            pastPayments = Fixture.Build<RequiredPaymentEntity>()
                .With(x => x.CommitmentId, CommitmentOne)
                .With(x => x.AccountId, AccountId)
                .With(x => x.TransactionType, 13)
                .With(x => x.AmountDue, 27.70588m)
                .With(x => x.StandardCode, 0)
                .With(x => x.ProgrammeType, 2)
                .With(x => x.FrameworkCode, 444)
                .With(x => x.PathwayCode, 1)
                .With(x => x.PriceEpisodeIdentifier, PriceEpisodeIdentifierForThisYear)
                .With(x => x.LearnAimRef, LearnAimRefForMaths)
                .With(x => x.ApprenticeshipContractType, 1)
                .With(x => x.SfaContributionPercentage, 0.9m)
                .With(x => x.FundingLineType, FundingLineType)
                .CreateMany(4)
                .ToList();
            PastPayments.AddRange(pastPayments);
            pastPayments = Fixture.Build<RequiredPaymentEntity>()
                .With(x => x.CommitmentId, CommitmentOne)
                .With(x => x.AccountId, AccountId)
                .With(x => x.TransactionType, 13)
                .With(x => x.AmountDue, 27.70588m)
                .With(x => x.StandardCode, 0)
                .With(x => x.ProgrammeType, 2)
                .With(x => x.FrameworkCode, 444)
                .With(x => x.PathwayCode, 1)
                .With(x => x.PriceEpisodeIdentifier, PriceEpisodeIdentifierForThisYear)
                .With(x => x.LearnAimRef, LearnAimRefForEnglish)
                .With(x => x.ApprenticeshipContractType, 1)
                .With(x => x.SfaContributionPercentage, 0.9m)
                .With(x => x.FundingLineType, FundingLineType)
                .CreateMany(4)
                .ToList();
            PastPayments.AddRange(pastPayments);

            var mathsEarnings = Fixture.Build<RawEarningForMathsOrEnglish>()
                .With(x => x.PriceEpisodeIdentifier, PriceEpisodeIdentifierForThisYear)
                .With(x => x.ProgrammeType, 2)
                .With(x => x.StandardCode, 0)
                .With(x => x.FrameworkCode, 444)
                .With(x => x.PathwayCode, 1)
                .With(x => x.SfaContributionPercentage, 0.9m)
                .With(x => x.FundingLineType, FundingLineType)
                .With(x => x.LearnAimRef, LearnAimRefForMaths)
                .With(x => x.TransactionType01, 0)
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
                .With(x => x.TransactionType13, 27.70588m)
                .With(x => x.TransactionType14, 0)
                .With(x => x.TransactionType15, 0)
                .With(x => x.ApprenticeshipContractType, 1)
                .CreateMany(4)
                .ToList();
            MathsAndEnglishEarnings.AddRange(mathsEarnings);

            var englishEarnings = Fixture.Build<RawEarningForMathsOrEnglish>()
                .With(x => x.PriceEpisodeIdentifier, PriceEpisodeIdentifierForThisYear)
                .With(x => x.ProgrammeType, 2)
                .With(x => x.StandardCode, 0)
                .With(x => x.FrameworkCode, 444)
                .With(x => x.PathwayCode, 1)
                .With(x => x.SfaContributionPercentage, 0.9m)
                .With(x => x.FundingLineType, FundingLineType)
                .With(x => x.LearnAimRef, LearnAimRefForEnglish)
                .With(x => x.TransactionType01, 0)
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
                .With(x => x.TransactionType13, 27.70588m)
                .With(x => x.TransactionType14, 0)
                .With(x => x.TransactionType15, 0)
                .With(x => x.ApprenticeshipContractType, 1)
                .CreateMany(4)
                .ToList();
            MathsAndEnglishEarnings.AddRange(englishEarnings);


            var datalockForNextYearFirstCommitment = Fixture.Build<DatalockOutput>()
                .With(x => x.PriceEpisodeIdentifier, PriceEpisodeIdentifierForThisYear)
                .With(x => x.CommitmentId, CommitmentOne)
                .With(x => x.Payable, false)
                .CreateMany(5)
                .ToList();
            
            for (var i = 0; i < 4; i++)
            {
                PastPayments[i].DeliveryYear = DeliveryYearFromPeriod(i + 2);
                PastPayments[i].DeliveryMonth = DeliveryMonthFromPeriod(i + 2);

                PastPayments[4 + i].DeliveryYear = DeliveryYearFromPeriod(i + 2);
                PastPayments[4 + i].DeliveryMonth = DeliveryMonthFromPeriod(i + 2);

                PastPayments[8 + i].DeliveryYear = DeliveryYearFromPeriod(i + 2);
                PastPayments[8 + i].DeliveryMonth = DeliveryMonthFromPeriod(i + 2);

                Earnings[i].Period = i + 2;
                Earnings[i].DeliveryYear = DeliveryYearFromPeriod(i + 2);
                Earnings[i].DeliveryMonth = DeliveryMonthFromPeriod(i + 2);

                MathsAndEnglishEarnings[i].Period = i + 2;
                MathsAndEnglishEarnings[i].DeliveryYear = DeliveryYearFromPeriod(i + 2);
                MathsAndEnglishEarnings[i].DeliveryMonth = DeliveryMonthFromPeriod(i + 2);

                MathsAndEnglishEarnings[4 + i].Period = i + 2;
                MathsAndEnglishEarnings[4 + i].DeliveryYear = DeliveryYearFromPeriod(i + 2);
                MathsAndEnglishEarnings[4 + i].DeliveryMonth = DeliveryMonthFromPeriod(i + 2);

                datalockForNextYearFirstCommitment[i].Period = i + 2;
            }

            datalockForNextYearFirstCommitment[4].Period = 6;

            Datalocks.AddRange(datalockForNextYearFirstCommitment);
        }

        [Test]
        public void ThereShouldBeNoRefunds()
        {
            var datalockComponent = new IShouldBeInTheDatalockComponent();
            var datalockResult = datalockComponent.ValidatePriceEpisodes(Commitments, Datalocks, new DateTime(2018, 07, 31));

            var sut = new Learner(Earnings, MathsAndEnglishEarnings, datalockResult, PastPayments);
            var actual = sut.CalculatePaymentsDue();

            actual.PayableEarnings.Should().NotContain(x => x.AmountDue < 0);
        }
    }
}
