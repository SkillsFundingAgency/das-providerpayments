﻿using System;
using System.Collections.Generic;
using System.Linq;
using AutoFixture;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Domain;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data.Entities;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.UnitTests.Utilities.Extensions;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.UnitTests.ProductionScenarios
{
    [TestFixture]
    public class GivenALearnerWithTwoCommitments
    {
        private static readonly IFixture Fixture = new Fixture();
        private static readonly long CommitmentOne = Fixture.Create<long>();
        private static readonly long CommitmentTwo = Fixture.Create<long>();
        private static readonly string PriceEpisodeIdentifierForThisYear = "25-80-01/08/2017";
        
        private static readonly string LearnAimRef = Fixture.Create<string>();
        private static readonly long AccountId = Fixture.Create<long>();
        private static readonly string FundingLineType = Fixture.Create<string>();

        private static readonly List<RequiredPaymentEntity> PastPayments = Fixture.Build<RequiredPaymentEntity>()
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

        private static readonly List<DatalockOutput> Datalocks = new List<DatalockOutput>();

        private static readonly List<Commitment> Commitments = new List<Commitment>
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

        private static readonly List<RawEarning> Earnings = Fixture.Build<RawEarning>()
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


            var datalockForNextYearFirstCommitment = new List<DatalockOutputEntity>();
            var datalockForThisYearFirstCommitment = Fixture.Build<DatalockOutputEntity>()
                .With(x => x.PriceEpisodeIdentifier, PriceEpisodeIdentifierForThisYear)
                .With(x => x.CommitmentId, CommitmentOne)
                .With(x => x.TransactionTypesFlag, 1)
                .With(x => x.Payable, false)
                .CreateMany(12)
                .ToList();
            var datalockForNextYearSecondCommitment = new List<DatalockOutputEntity>();
            var datalockForThisYearSecondCommitment = Fixture.Build<DatalockOutputEntity>()
                .With(x => x.PriceEpisodeIdentifier, PriceEpisodeIdentifierForThisYear)
                .With(x => x.CommitmentId, CommitmentTwo)
                .With(x => x.TransactionTypesFlag, 1)
                .With(x => x.Payable, true)
                .CreateMany(12)
                .ToList();


            for (var i = 0; i < 10; i++)
            {
                PastPayments[i].DeliveryYear = (i + 1).DeliveryYearFromPeriod();
                PastPayments[i].DeliveryMonth = (i + 1).DeliveryMonthFromPeriod();

                Earnings[i].Period = i + 1;
                Earnings[i].DeliveryYear = (i + 1).DeliveryYearFromPeriod();
                Earnings[i].DeliveryMonth = (i + 1).DeliveryMonthFromPeriod();

                datalockForThisYearFirstCommitment[i].Period = i + 1;
                datalockForThisYearSecondCommitment[i].Period = i + 1;
            }
            for (var i = 10; i < 12; i++)
            {
                datalockForThisYearFirstCommitment[i].Period = i + 1;
                datalockForThisYearSecondCommitment[i].Period = i + 1;
            }

            Datalocks.AddRange(datalockForNextYearFirstCommitment.Select(x => new DatalockOutput(x)));
            Datalocks.AddRange(datalockForNextYearSecondCommitment.Select(x => new DatalockOutput(x)));
            Datalocks.AddRange(datalockForThisYearFirstCommitment.Select(x => new DatalockOutput(x)));
            Datalocks.AddRange(datalockForThisYearSecondCommitment.Select(x => new DatalockOutput(x)));
        }

        [Test]
        public void ThereShouldBeNoRefunds()
        {
            var datalockComponent = new IShouldBeInTheDatalockComponent();
            var datalockResult = datalockComponent.ValidatePriceEpisodes(
                Commitments,
                Datalocks,
                new List<DatalockValidationError>(),
                Earnings,
                new List<RawEarningForMathsOrEnglish>(), 
                new DateTime(2017, 08, 01));

            var sut = new Learner(datalockResult.Earnings, datalockResult.PeriodsToIgnore, PastPayments);
            var actual = sut.CalculatePaymentsDue();

            actual.Should().NotContain(x => x.AmountDue < 0);
        }
    }
}
