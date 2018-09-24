﻿using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using AutoFixture;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.CollectionEarnings.DataLock.Application.DataLock.Matcher;
using SFA.DAS.CollectionEarnings.DataLock.Domain;
using SFA.DAS.CollectionEarnings.DataLock.Infrastructure.Data.Entities;
using SFA.DAS.CollectionEarnings.DataLock.Services;
using SFA.DAS.CollectionEarnings.DataLock.UnitTests.Utilities.Attributes;
using SFA.DAS.CollectionEarnings.DataLock.UnitTests.Utilities.Extensions;
using SFA.DAS.Payments.DCFS.Domain;
using SFA.DAS.ProviderPayments.Calc.Common.Domain;
using SFA.DAS.ProviderPayments.Calc.Shared.Infrastructure.Data.Entities;

namespace SFA.DAS.CollectionEarnings.DataLock.UnitTests.Tests.ServiceTests.GivenADatalockValidationService
{
    public static class EpisodeDateExtensions
    {
        public static string EpisodeIdentifier(this DateTime source)
        {
            if (source.Month < 8)
            {
                return $"10-10-01/08/{source.Year - 1}";
            }

            return $"10-10-01/08/{source.Year}";
        }

        public static void AddPriceEpisodeIdentifiers(this List<RawEarning> source)
        {
            foreach (var rawEarning in source)
            {
                rawEarning.PriceEpisodeIdentifier = rawEarning.EpisodeStartDate?.EpisodeIdentifier();
            }
        }

        public static DateTime FirstDayOfAcademicYear(this DateTime source)
        {
            if (source.Month < 8)
            {
                return new DateTime(source.Year - 1, 8, 1);
            }
            return new DateTime(source.Year, 8, 1);
        }
    }

    [TestFixture]
    public class WhenCallingValidateDatalocksForProvider
    {
        static DateTime RandomStartDateWithin1819AcademicYear(DateTime source)
        {
            var fixture = new Fixture();
            var date = source;
            while (date > new DateTime(2019, 08, 01) ||
                   date <= new DateTime(2018, 08, 01))
            {
                date = fixture.Create<DateTime>();
            }

            return date;
        }

        static void AssociateEarningsWithCommitment(List<RawEarning> earnings, CommitmentEntity commitment)
        {
            commitment.StartDate = RandomStartDateWithin1819AcademicYear(commitment.StartDate);
            commitment.StartDate = commitment.StartDate.FirstDayOfAcademicYear();
            var learnRefNumber = earnings.First().LearnRefNumber;
            var aimSequenceNumber = earnings.First().AimSeqNumber;
            foreach (var earning in earnings)
            {
                earning.ProgrammeType = commitment.ProgrammeType??0;
                earning.StandardCode = (int)commitment.StandardCode;
                earning.PathwayCode = commitment.PathwayCode ?? 0;
                earning.FrameworkCode = commitment.FrameworkCode ?? 0;
                earning.AgreedPrice = commitment.AgreedCost;
                earning.Uln = commitment.Uln;
                earning.Ukprn = commitment.Ukprn;
                earning.FirstIncentiveCensusDate = null;
                earning.SecondIncentiveCensusDate = null;
                earning.LearnRefNumber = learnRefNumber;
                earning.AimSeqNumber = aimSequenceNumber;
                
                earning.EpisodeStartDate = commitment.StartDate.AddDays(5);
                earning.EpisodeEffectiveTnpStartDate = earning.EpisodeStartDate;
                earning.PriceEpisodeIdentifier = earning.EpisodeStartDate.Value.EpisodeIdentifier();
                earning.EndDate = null;

                commitment.EndDate = commitment.StartDate.AddYears(2);
                commitment.WithdrawnOnDate = null;
                commitment.PausedOnDate = null;
                commitment.PaymentStatus = (int)PaymentStatus.Active;
                commitment.ProviderUkprn = commitment.Ukprn;
            }
        }

        static ImmutableHashSet<long> CreateNonPayableAccountsList(params long[] accountsToInclude)
        {
            return ImmutableHashSet.Create(accountsToInclude);
        }

        [TestFixture]
        public class WithOneRawEarning
        {
            [TestFixture]
            public class WithAValidCommitment
            {
                [Test, AutoMoqData]
                public void ThenThereIsAPayablePeriodMatch(
                    DatalockValidationService sut,
                    RawEarning earning,
                    CommitmentEntity commitment
                    )
                {
                    var earnings = new List<RawEarning>{earning};
                    AssociateEarningsWithCommitment(earnings, commitment);
                    var accounts = CreateNonPayableAccountsList();
                    var commitments = new List<CommitmentEntity> {commitment};
                    var providerCommitments = new ProviderCommitments(commitments);

                    var actual = sut.ValidateDatalockForProvider(providerCommitments, earnings, accounts);

                    actual.PriceEpisodePeriodMatches.Should().Contain(x => x.Payable);
                }

                [Test, AutoMoqData]
                public void ShouldNotContainAFirst16To18Incentive(
                    DatalockValidationService sut,
                    RawEarning earning,
                    CommitmentEntity commitment
                )
                {
                    var earnings = new List<RawEarning> { earning };
                    AssociateEarningsWithCommitment(earnings, commitment);
                    var accounts = CreateNonPayableAccountsList();
                    var commitments = new List<CommitmentEntity> { commitment };
                    var providerCommitments = new ProviderCommitments(commitments);

                    var actual = sut.ValidateDatalockForProvider(providerCommitments, earnings, accounts);

                    actual.PriceEpisodePeriodMatches.Should().NotContain(x => x.Payable && x.TransactionTypesFlag == TransactionTypesFlag.FirstEmployerProviderIncentives);
                }

                [Test, AutoMoqData]
                public void ShouldNotContainASecond16To18Incentive(
                    DatalockValidationService sut,
                    RawEarning earning,
                    CommitmentEntity commitment
                )
                {
                    var earnings = new List<RawEarning> { earning };
                    AssociateEarningsWithCommitment(earnings, commitment);
                    var accounts = CreateNonPayableAccountsList();
                    var commitments = new List<CommitmentEntity> { commitment };
                    var providerCommitments = new ProviderCommitments(commitments);

                    var actual = sut.ValidateDatalockForProvider(providerCommitments, earnings, accounts);

                    actual.PriceEpisodePeriodMatches.Should().NotContain(x => x.Payable && x.TransactionTypesFlag == TransactionTypesFlag.SecondEmployerProviderIncentives);
                }

                [Test, AutoMoqData]
                public void WithAFirst16To18IncentiveThenThereIsAPayablePeriodMatch(
                    DatalockValidationService sut,
                    RawEarning earning,
                    CommitmentEntity commitment
                )
                {
                    var earnings = new List<RawEarning> { earning };
                    AssociateEarningsWithCommitment(earnings, commitment);
                    earning.FirstIncentiveCensusDate = commitment.StartDate.AddDays(15);
                    var accounts = CreateNonPayableAccountsList();
                    var commitments = new List<CommitmentEntity> { commitment };
                    var providerCommitments = new ProviderCommitments(commitments);

                    var actual = sut.ValidateDatalockForProvider(providerCommitments, earnings, accounts);

                    actual.PriceEpisodePeriodMatches.Should().Contain(x => x.Payable && x.TransactionTypesFlag == TransactionTypesFlag.FirstEmployerProviderIncentives);
                }

                [Test, AutoMoqData]
                public void WithASecond16To18IncentiveThenThereIsAPayablePeriodMatch(
                    DatalockValidationService sut,
                    RawEarning earning,
                    CommitmentEntity commitment
                )
                {
                    var earnings = new List<RawEarning> { earning };
                    AssociateEarningsWithCommitment(earnings, commitment);
                    earning.SecondIncentiveCensusDate = commitment.StartDate.AddDays(15);
                    var accounts = CreateNonPayableAccountsList();
                    var commitments = new List<CommitmentEntity> { commitment };
                    var providerCommitments = new ProviderCommitments(commitments);

                    var actual = sut.ValidateDatalockForProvider(providerCommitments, earnings, accounts);

                    actual.PriceEpisodePeriodMatches.Should().Contain(x => x.Payable && x.TransactionTypesFlag == TransactionTypesFlag.SecondEmployerProviderIncentives);
                }

                [Test, AutoMoqData]
                public void ThenThereAreNoValidationErrors(
                    DatalockValidationService sut,
                    RawEarning earning,
                    CommitmentEntity commitment
                )
                {
                    var earnings = new List<RawEarning> { earning };
                    AssociateEarningsWithCommitment(earnings, commitment);
                    var accounts = CreateNonPayableAccountsList();
                    var commitments = new List<CommitmentEntity> { commitment };
                    var providerCommitments = new ProviderCommitments(commitments);

                    var actual = sut.ValidateDatalockForProvider(providerCommitments, earnings, accounts);

                    actual.ValidationErrors.Should().BeEmpty();
                }

                [Test, AutoMoqData]
                public void ThenThereIsAMatch(
                    DatalockValidationService sut,
                    RawEarning earning,
                    CommitmentEntity commitment
                )
                {
                    var earnings = new List<RawEarning> { earning };
                    AssociateEarningsWithCommitment(earnings, commitment);
                    var accounts = CreateNonPayableAccountsList();
                    var commitments = new List<CommitmentEntity> { commitment };
                    var providerCommitments = new ProviderCommitments(commitments);

                    var actual = sut.ValidateDatalockForProvider(providerCommitments, earnings, accounts);

                    actual.PriceEpisodeMatches.Should().Contain(x => x.IsSuccess);
                }

                [TestFixture]
                public class AndANonLevyAccount
                {
                    [Test, AutoMoqData]
                    public void ThenThereIsADLOCK_11(
                        DatalockValidationService sut,
                        RawEarning earning,
                        CommitmentEntity commitment
                    )
                    {
                        var earnings = new List<RawEarning> { earning };
                        AssociateEarningsWithCommitment(earnings, commitment);
                        var accounts = CreateNonPayableAccountsList(commitment.AccountId);
                        var commitments = new List<CommitmentEntity> { commitment };
                        var providerCommitments = new ProviderCommitments(commitments);

                        var actual = sut.ValidateDatalockForProvider(providerCommitments, earnings, accounts);

                        actual.ValidationErrors.Should().Contain(x => x.RuleId == DataLockErrorCodes.NotLevyPayer);
                    }
                }
            }

            [TestFixture]
            public class WithNoCommitments
            {
                [Test, AutoMoqData]
                public void ThenThereAreNoPeriodMatches(
                    DatalockValidationService sut,
                    RawEarning earning)
                {
                    var earnings = new List<RawEarning> { earning };
                    earnings.AddPriceEpisodeIdentifiers();

                    var accounts = CreateNonPayableAccountsList();
                    var providerCommitments = new ProviderCommitments(new List<CommitmentEntity>());

                    var actual = sut.ValidateDatalockForProvider(providerCommitments, earnings, accounts);

                    actual.PriceEpisodePeriodMatches.Should().BeEmpty();
                }

                [Test, AutoMoqData]
                public void ThenThereIsADLOCK_02ValidationError(
                    DatalockValidationService sut,
                    RawEarning earning)
                {
                    var earnings = new List<RawEarning> { earning };
                    earning.EpisodeStartDate = RandomStartDateWithin1819AcademicYear(earning.EpisodeStartDate ?? new DateTime(2018, 1, 1));
                    earnings.AddPriceEpisodeIdentifiers();

                    var accounts = CreateNonPayableAccountsList();
                    var providerCommitments = new ProviderCommitments(new List<CommitmentEntity>());

                    var actual = sut.ValidateDatalockForProvider(providerCommitments, earnings, accounts);

                    actual.ValidationErrors.Should().HaveCount(1);
                    actual.ValidationErrors.Should().OnlyContain(x => x.RuleId == DataLockErrorCodes.MismatchingUln);
                }

                [Test, AutoMoqData]
                public void ThenThereAreNoMatches(
                    DatalockValidationService sut,
                    RawEarning earning)
                {
                    var earnings = new List<RawEarning> { earning };
                    earnings.AddPriceEpisodeIdentifiers();
                    
                    var accounts = CreateNonPayableAccountsList();
                    var providerCommitments = new ProviderCommitments(new List<CommitmentEntity>());

                    var actual = sut.ValidateDatalockForProvider(providerCommitments, earnings, accounts);

                    actual.PriceEpisodeMatches.Should().BeEmpty();
                }
            }

            [TestFixture]
            public class WithAnInvalidCommitment
            {
                [Test, AutoMoqData]
                public void MismatchedUkprnReturnsDLOCK_01(
                    DatalockValidationService sut,
                    RawEarning earning,
                    CommitmentEntity commitment)
                {
                    var earnings = new List<RawEarning> { earning };
                    AssociateEarningsWithCommitment(earnings, commitment);
                    var accounts = CreateNonPayableAccountsList();
                    var commitments = new List<CommitmentEntity> { commitment };
                    commitment.ProviderUkprn += 1;
                    var providerCommitments = new ProviderCommitments(commitments);

                    var actual = sut.ValidateDatalockForProvider(providerCommitments, earnings, accounts);

                    actual.ValidationErrors.Should().Contain(x => x.RuleId == DataLockErrorCodes.MismatchingUkprn);
                }

                [Test, AutoMoqData]
                public void MismatchedProgrammeTypeReturnsDLOCK_05(
                    DatalockValidationService sut,
                    RawEarning earning,
                    CommitmentEntity commitment
                )
                {
                    var earnings = new List<RawEarning> { earning };
                    AssociateEarningsWithCommitment(earnings, commitment);
                    var accounts = CreateNonPayableAccountsList();
                    var commitments = new List<CommitmentEntity> { commitment };
                    commitment.ProgrammeType += 1;
                    var providerCommitments = new ProviderCommitments(commitments);
                    
                    var actual = sut.ValidateDatalockForProvider(providerCommitments, earnings, accounts);

                    actual.ValidationErrors.Should().Contain(x => x.RuleId == DataLockErrorCodes.MismatchingProgramme);
                }

                [Test, AutoMoqData]
                public void MismatchedStandardCodeReturnsDLOCK_03(
                    DatalockValidationService sut,
                    RawEarning earning,
                    CommitmentEntity commitment
                )
                {
                    var earnings = new List<RawEarning> { earning };
                    AssociateEarningsWithCommitment(earnings, commitment);
                    var accounts = CreateNonPayableAccountsList();
                    var commitments = new List<CommitmentEntity> { commitment };
                    commitment.StandardCode += 1;
                    var providerCommitments = new ProviderCommitments(commitments);

                    var actual = sut.ValidateDatalockForProvider(providerCommitments, earnings, accounts);

                    actual.ValidationErrors.Should().Contain(x => x.RuleId == DataLockErrorCodes.MismatchingStandard);
                }

                [Test, AutoMoqData]
                public void MismatchedFrameworkCodeReturnsDLOCK_04(
                    DatalockValidationService sut,
                    RawEarning earning,
                    CommitmentEntity commitment
                )
                {
                    var earnings = new List<RawEarning> { earning };
                    AssociateEarningsWithCommitment(earnings, commitment);
                    var accounts = CreateNonPayableAccountsList();
                    var commitments = new List<CommitmentEntity> { commitment };
                    commitment.FrameworkCode += 1;
                    var providerCommitments = new ProviderCommitments(commitments);

                    var actual = sut.ValidateDatalockForProvider(providerCommitments, earnings, accounts);

                    actual.ValidationErrors.Should().Contain(x => x.RuleId == DataLockErrorCodes.MismatchingFramework);
                }

                [Test, AutoMoqData]
                public void MismatchedPathwayReturnsDLOCK_06(
                    DatalockValidationService sut,
                    RawEarning earning,
                    CommitmentEntity commitment
                )
                {
                    var earnings = new List<RawEarning> { earning };
                    AssociateEarningsWithCommitment(earnings, commitment);
                    var accounts = CreateNonPayableAccountsList();
                    var commitments = new List<CommitmentEntity> { commitment };
                    commitment.PathwayCode += 1;
                    var providerCommitments = new ProviderCommitments(commitments);

                    var actual = sut.ValidateDatalockForProvider(providerCommitments, earnings, accounts);

                    actual.ValidationErrors.Should().Contain(x => x.RuleId == DataLockErrorCodes.MismatchingPathway);
                }

                [Test, AutoMoqData]
                public void MismatchedPriceReturnsDLOCK_07(
                    DatalockValidationService sut,
                    RawEarning earning,
                    CommitmentEntity commitment
                )
                {
                    var earnings = new List<RawEarning> { earning };
                    AssociateEarningsWithCommitment(earnings, commitment);
                    var accounts = CreateNonPayableAccountsList();
                    var commitments = new List<CommitmentEntity> { commitment };
                    commitment.AgreedCost += 1;
                    var providerCommitments = new ProviderCommitments(commitments);

                    var actual = sut.ValidateDatalockForProvider(providerCommitments, earnings, accounts);

                    actual.ValidationErrors.Should().Contain(x => x.RuleId == DataLockErrorCodes.MismatchingPrice);
                }

                [Test, AutoMoqData]
                public void OverlappingCommitmentsReturnsDLOCK_08(
                    DatalockValidationService sut,
                    RawEarning earning,
                    CommitmentEntity commitment1
                )
                {
                    var earnings = new List<RawEarning> { earning };
                    AssociateEarningsWithCommitment(earnings, commitment1);
                    var accounts = CreateNonPayableAccountsList();
                    var commitment2 = commitment1.Clone();
                    commitment2.CommitmentId += 1;
                    var commitments = new List<CommitmentEntity> { commitment1, commitment2 };
                    var providerCommitments = new ProviderCommitments(commitments);

                    var actual = sut.ValidateDatalockForProvider(providerCommitments, earnings, accounts);

                    actual.ValidationErrors.Should().Contain(x => x.RuleId == DataLockErrorCodes.MultipleMatches);
                }

                [Test, AutoMoqData]
                public void CommitmentStartDateAfterEarningStartDateReturnsDLOCK_09(
                    DatalockValidationService sut,
                    RawEarning earning,
                    CommitmentEntity commitment1
                )
                {
                    var earnings = new List<RawEarning> { earning };
                    AssociateEarningsWithCommitment(earnings, commitment1);
                    var accounts = CreateNonPayableAccountsList();
                    var commitments = new List<CommitmentEntity> { commitment1 };
                    earning.Period = 1;
                    commitment1.StartDate = commitment1.StartDate.AddMonths(2);
                    var providerCommitments = new ProviderCommitments(commitments);

                    var actual = sut.ValidateDatalockForProvider(providerCommitments, earnings, accounts);

                    actual.ValidationErrors.Should().Contain(x => x.RuleId == DataLockErrorCodes.EarlierStartDate);
                }
            }

            [TestFixture]
            public class WithAWithdrawnCommitment
            {
                [TestFixture]
                public class ThatWasWithdrawnBeforeTheOnProgCensusDate
                {
                    [TestFixture]
                    public class AndAfterTheCourseEndDate
                    {
                        [Test, AutoMoqData]
                        public void ThenThereIsAPeriodMatchForTheCompletionPayment(
                            DatalockValidationService sut,
                            RawEarning earning,
                            CommitmentEntity commitment
                        )
                        {
                            var earnings = new List<RawEarning> { earning };
                            AssociateEarningsWithCommitment(earnings, commitment);
                            commitment.WithdrawnOnDate = commitment.StartDate.AddDays(2);
                            commitment.PaymentStatus = (int)PaymentStatus.Cancelled;
                            earning.EndDate = commitment.StartDate;
                            earning.ResetEarnings();
                            earning.TransactionType02 = new Fixture().Create<decimal>();

                            var accounts = CreateNonPayableAccountsList();
                            var commitments = new List<CommitmentEntity> { commitment };
                            var providerCommitments = new ProviderCommitments(commitments);

                            var actual = sut.ValidateDatalockForProvider(providerCommitments, earnings, accounts);

                            actual.PriceEpisodePeriodMatches.Where(x => x.Payable).Should().HaveCount(1);
                        }
                    }

                    [TestFixture]
                    public class AndBeforeTheCourseEndDate
                    {
                        [Test, AutoMoqData]
                        public void ThenThereIsNoPeriodMatchForTheCompletionPayment(
                            DatalockValidationService sut,
                            RawEarning earning,
                            CommitmentEntity commitment
                        )
                        {
                            var earnings = new List<RawEarning> { earning };
                            AssociateEarningsWithCommitment(earnings, commitment);
                            commitment.WithdrawnOnDate = commitment.StartDate.AddDays(2);
                            commitment.PaymentStatus = (int)PaymentStatus.Cancelled;
                            earning.EndDate = commitment.StartDate.AddDays(10);
                            earning.ResetEarnings();
                            earning.TransactionType02 = new Fixture().Create<decimal>();

                            var accounts = CreateNonPayableAccountsList();
                            var commitments = new List<CommitmentEntity> { commitment };
                            var providerCommitments = new ProviderCommitments(commitments);

                            var actual = sut.ValidateDatalockForProvider(providerCommitments, earnings, accounts);

                            actual.PriceEpisodePeriodMatches.Where(x => x.Payable).Should().HaveCount(0);
                        }

                        [Test, AutoMoqData]
                        public void ThenThereIsADLOCK_10(
                            DatalockValidationService sut,
                            RawEarning earning,
                            CommitmentEntity commitment
                        )
                        {
                            var earnings = new List<RawEarning> { earning };
                            AssociateEarningsWithCommitment(earnings, commitment);
                            commitment.WithdrawnOnDate = commitment.StartDate.AddDays(2);
                            commitment.PaymentStatus = (int)PaymentStatus.Cancelled;
                            earning.EndDate = commitment.StartDate.AddDays(10);
                            earning.ResetEarnings();
                            earning.TransactionType02 = new Fixture().Create<decimal>();

                            var accounts = CreateNonPayableAccountsList();
                            var commitments = new List<CommitmentEntity> { commitment };
                            var providerCommitments = new ProviderCommitments(commitments);

                            var actual = sut.ValidateDatalockForProvider(providerCommitments, earnings, accounts);

                            actual.ValidationErrors
                                .Where(x => x.RuleId == DataLockErrorCodes.EmployerStopped)
                                .Should().HaveCount(1);
                        }
                    }
                }
                [TestFixture]
                public class ThatWasWithdrawnBeforeTheEarning
                {
                    [Test, AutoMoqData]
                    public void ThenThereAreNoPeriodMatches(
                        DatalockValidationService sut,
                        RawEarning earning,
                        CommitmentEntity commitment
                    )
                    {
                        var earnings = new List<RawEarning> { earning };
                        AssociateEarningsWithCommitment(earnings, commitment);
                        commitment.WithdrawnOnDate = commitment.StartDate.AddDays(2);
                        commitment.PaymentStatus = (int)PaymentStatus.Cancelled;
                        var accounts = CreateNonPayableAccountsList();
                        var commitments = new List<CommitmentEntity> { commitment };
                        var providerCommitments = new ProviderCommitments(commitments);

                        var actual = sut.ValidateDatalockForProvider(providerCommitments, earnings, accounts);

                        actual.PriceEpisodePeriodMatches.Where(x => x.Payable).Should().BeEmpty();
                    }

                    [Test, AutoMoqData]
                    public void ThenThereIsOneValidationErrors(
                        DatalockValidationService sut,
                        RawEarning earning,
                        CommitmentEntity commitment
                    )
                    {
                        var earnings = new List<RawEarning> { earning };
                        AssociateEarningsWithCommitment(earnings, commitment);
                        commitment.WithdrawnOnDate = commitment.StartDate.AddDays(2);
                        commitment.PaymentStatus = (int)PaymentStatus.Cancelled;
                        var accounts = CreateNonPayableAccountsList();
                        var commitments = new List<CommitmentEntity> { commitment };
                        var providerCommitments = new ProviderCommitments(commitments);

                        var actual = sut.ValidateDatalockForProvider(providerCommitments, earnings, accounts);

                        actual.ValidationErrors.Where(x => x.RuleId == DataLockErrorCodes.EmployerStopped).Should()
                            .HaveCount(1);
                    }

                    [Test, AutoMoqData]
                    public void ThenThereAreNoMatches(
                        DatalockValidationService sut,
                        RawEarning earning,
                        CommitmentEntity commitment
                    )
                    {
                        var earnings = new List<RawEarning> { earning };
                        AssociateEarningsWithCommitment(earnings, commitment);
                        commitment.WithdrawnOnDate = commitment.StartDate.AddDays(2);
                        commitment.PaymentStatus = (int)PaymentStatus.Cancelled;
                        var accounts = CreateNonPayableAccountsList();
                        var commitments = new List<CommitmentEntity> { commitment };
                        var providerCommitments = new ProviderCommitments(commitments);

                        var actual = sut.ValidateDatalockForProvider(providerCommitments, earnings, accounts);

                        actual.PriceEpisodeMatches.Where(x => x.IsSuccess).Should().BeEmpty();
                    }
                }

                [TestFixture]
                public class ThatWasWithdrawnAfterTheEarning
                {
                    [Test, AutoMoqData]
                    public void ThenThereIsAPeriodMatches(
                        DatalockValidationService sut,
                        RawEarning earning,
                        CommitmentEntity commitment
                    )
                    {
                        var earnings = new List<RawEarning> { earning };
                        AssociateEarningsWithCommitment(earnings, commitment);
                        commitment.WithdrawnOnDate = commitment.StartDate.AddMonths(1);
                        commitment.PaymentStatus = (int)PaymentStatus.Cancelled;
                        earning.Period = 1;

                        var accounts = CreateNonPayableAccountsList();
                        var commitments = new List<CommitmentEntity> { commitment };
                        var providerCommitments = new ProviderCommitments(commitments);

                        var actual = sut.ValidateDatalockForProvider(providerCommitments, earnings, accounts);

                        actual.PriceEpisodePeriodMatches.Should().Contain(x => x.Payable);
                    }

                    [Test, AutoMoqData]
                    public void ThenThereAreNoValidationErrors(
                        DatalockValidationService sut,
                        RawEarning earning,
                        CommitmentEntity commitment
                    )
                    {
                        var earnings = new List<RawEarning> { earning };
                        AssociateEarningsWithCommitment(earnings, commitment);
                        commitment.WithdrawnOnDate = commitment.StartDate.AddMonths(1);
                        commitment.PaymentStatus = (int)PaymentStatus.Cancelled;
                        earning.Period = 1;

                        var accounts = CreateNonPayableAccountsList();
                        var commitments = new List<CommitmentEntity> { commitment };
                        var providerCommitments = new ProviderCommitments(commitments);

                        var actual = sut.ValidateDatalockForProvider(providerCommitments, earnings, accounts);

                        actual.ValidationErrors.Should().BeEmpty();
                    }

                    [Test, AutoMoqData]
                    public void ThenThereIsAMatch(
                        DatalockValidationService sut,
                        RawEarning earning,
                        CommitmentEntity commitment
                    )
                    {
                        var earnings = new List<RawEarning> { earning };
                        AssociateEarningsWithCommitment(earnings, commitment);
                        commitment.WithdrawnOnDate = commitment.StartDate.AddMonths(1);
                        commitment.PaymentStatus = (int)PaymentStatus.Cancelled;
                        earning.Period = 1;

                        var accounts = CreateNonPayableAccountsList();
                        var commitments = new List<CommitmentEntity> { commitment };
                        var providerCommitments = new ProviderCommitments(commitments);


                        var actual = sut.ValidateDatalockForProvider(providerCommitments, earnings, accounts);

                        actual.PriceEpisodeMatches.Should().Contain(x => x.IsSuccess);
                    }
                }
            }
        }

        [TestFixture]
        public class WithMultipleEarnings
        {
            [TestFixture]
            public class WithAValidCommitment
            {
                [Test, AutoMoqData]
                public void ThenThereAreThreePayablePeriodMatches(
                    DatalockValidationService sut,
                    List<RawEarning> earnings,
                    CommitmentEntity commitment
                    )
                {
                    earnings[0].Period = 1;
                    earnings[1].Period = 2;
                    earnings[2].Period = 3;
                    AssociateEarningsWithCommitment(earnings, commitment);
                    var accounts = CreateNonPayableAccountsList();
                    var commitments = new List<CommitmentEntity> { commitment };
                    var providerCommitments = new ProviderCommitments(commitments);

                    var actual = sut.ValidateDatalockForProvider(providerCommitments, earnings, accounts);

                    actual.PriceEpisodePeriodMatches.Should().OnlyContain(x => x.Payable);
                    actual.PriceEpisodePeriodMatches.Should().HaveCount(3);
                }

                [Test, AutoMoqData]
                public void ThenThereAreNoValidationErrors(
                    DatalockValidationService sut,
                    List<RawEarning> earnings,
                    CommitmentEntity commitment
                )
                {
                    earnings[0].Period = 1;
                    earnings[1].Period = 2;
                    earnings[2].Period = 3;
                    AssociateEarningsWithCommitment(earnings, commitment);
                    var accounts = CreateNonPayableAccountsList();
                    var commitments = new List<CommitmentEntity> { commitment };
                    var providerCommitments = new ProviderCommitments(commitments);

                    var actual = sut.ValidateDatalockForProvider(providerCommitments, earnings, accounts);

                    actual.ValidationErrors.Should().BeEmpty();
                }

                [Test, AutoMoqData]
                public void ThenThereAreThreeMatches(
                    DatalockValidationService sut,
                    List<RawEarning> earnings,
                    CommitmentEntity commitment
                )
                {
                    earnings[0].Period = 1;
                    earnings[1].Period = 2;
                    earnings[2].Period = 3;
                    AssociateEarningsWithCommitment(earnings, commitment);
                    var accounts = CreateNonPayableAccountsList();
                    var commitments = new List<CommitmentEntity> { commitment };
                    var providerCommitments = new ProviderCommitments(commitments);

                    var actual = sut.ValidateDatalockForProvider(providerCommitments, earnings, accounts);

                    actual.PriceEpisodeMatches.Should().OnlyContain(x => x.IsSuccess);
                    actual.PriceEpisodePeriodMatches.Should().HaveCount(3);
                }
            }

            [TestFixture]
            public class WithNoCommitments
            {
                [Test, AutoMoqData]
                public void ThenThereAreNoPeriodMatches(
                    DatalockValidationService sut,
                    List<RawEarning> earnings
                )
                {
                    earnings[0].Period = 1;
                    earnings[1].Period = 2;
                    earnings[2].Period = 3;
                    earnings.AddPriceEpisodeIdentifiers();

                    var accounts = CreateNonPayableAccountsList();
                    var providerCommitments = new ProviderCommitments(new List<CommitmentEntity>());

                    var actual = sut.ValidateDatalockForProvider(providerCommitments, earnings, accounts);

                    actual.PriceEpisodePeriodMatches.Should().BeEmpty();
                }

                [Test, AutoMoqData]
                public void ThenThereAreNoValidationErrors(
                    DatalockValidationService sut,
                    List<RawEarning> earnings
                )
                {
                    earnings[0].Period = 1;
                    earnings[1].Period = 2;
                    earnings[2].Period = 3;
                    earnings[0].EpisodeStartDate = RandomStartDateWithin1819AcademicYear(earnings[0].EpisodeStartDate ?? new DateTime(2018, 8, 1));
                    earnings[1].EpisodeStartDate = RandomStartDateWithin1819AcademicYear(earnings[0].EpisodeStartDate ?? new DateTime(2018, 8, 1));
                    earnings[2].EpisodeStartDate = RandomStartDateWithin1819AcademicYear(earnings[0].EpisodeStartDate ?? new DateTime(2018, 8, 1));
                    earnings.AddPriceEpisodeIdentifiers();

                    var accounts = CreateNonPayableAccountsList();
                    var providerCommitments = new ProviderCommitments(new List<CommitmentEntity>());

                    var actual = sut.ValidateDatalockForProvider(providerCommitments, earnings, accounts);

                    actual.ValidationErrors.Should().HaveCount(3);
                }

                [Test, AutoMoqData]
                public void ThenThereAreNoMatches(
                    DatalockValidationService sut,
                    List<RawEarning> earnings
                )
                {
                    earnings[0].Period = 1;
                    earnings[1].Period = 2;
                    earnings[2].Period = 3;
                    earnings.AddPriceEpisodeIdentifiers();
                    var accounts = CreateNonPayableAccountsList();
                    var providerCommitments = new ProviderCommitments(new List<CommitmentEntity>());

                    var actual = sut.ValidateDatalockForProvider(providerCommitments, earnings, accounts);

                    actual.PriceEpisodeMatches.Should().BeEmpty();
                }
            }

            [TestFixture]
            public class WithAWithdrawnCommitment
            {
                [TestFixture]
                public class ThatWasWithdrawnBeforeThirdEarning
                {
                    [Test, AutoMoqData]
                    public void ThenThereAreTwoPeriodMatches(
                        DatalockValidationService sut,
                        List<RawEarning> earnings,
                        CommitmentEntity commitment
                    )
                    {
                        earnings[0].Period = 1;
                        earnings[1].Period = 2;
                        earnings[2].Period = 3;
                        AssociateEarningsWithCommitment(earnings, commitment);
                        commitment.WithdrawnOnDate = commitment.StartDate.AddMonths(2);
                        commitment.PaymentStatus = (int)PaymentStatus.Cancelled;
                        var accounts = CreateNonPayableAccountsList();
                        var commitments = new List<CommitmentEntity> { commitment };
                        var providerCommitments = new ProviderCommitments(commitments);

                        var actual = sut.ValidateDatalockForProvider(providerCommitments, earnings, accounts);

                        actual.PriceEpisodePeriodMatches.Where(x => x.Payable).Should().HaveCount(2);
                    }

                    [Test, AutoMoqData]
                    public void ThenThereIsOneValidationErrors(
                        DatalockValidationService sut,
                        List<RawEarning> earnings,
                        CommitmentEntity commitment
                    )
                    {
                        earnings[0].Period = 1;
                        earnings[1].Period = 2;
                        earnings[2].Period = 3;
                        AssociateEarningsWithCommitment(earnings, commitment);
                        commitment.WithdrawnOnDate = commitment.StartDate.AddMonths(2);
                        commitment.PaymentStatus = (int)PaymentStatus.Cancelled;
                        var accounts = CreateNonPayableAccountsList();
                        var commitments = new List<CommitmentEntity> { commitment };
                        var providerCommitments = new ProviderCommitments(commitments);

                        var actual = sut.ValidateDatalockForProvider(providerCommitments, earnings, accounts);

                        actual.ValidationErrors.Should().HaveCount(1);
                        actual.ValidationErrors.Should().Contain(x => x.RuleId == DataLockErrorCodes.EmployerStopped);
                    }

                    [Test, AutoMoqData]
                    public void ThenThereAreTwoMatches(
                        DatalockValidationService sut,
                        List<RawEarning> earnings,
                        CommitmentEntity commitment
                    )
                    {
                        earnings[0].Period = 1;
                        earnings[1].Period = 2;
                        earnings[2].Period = 3;
                        AssociateEarningsWithCommitment(earnings, commitment);
                        commitment.WithdrawnOnDate = commitment.StartDate.AddMonths(2);
                        commitment.PaymentStatus = (int)PaymentStatus.Cancelled;
                        var accounts = CreateNonPayableAccountsList();
                        var commitments = new List<CommitmentEntity> { commitment };
                        var providerCommitments = new ProviderCommitments(commitments);

                        var actual = sut.ValidateDatalockForProvider(providerCommitments, earnings, accounts);

                        actual.PriceEpisodeMatches.Where(x => x.IsSuccess).Should().HaveCount(1);
                    }
                }

                [TestFixture]
                public class ThatWasWithdrawnAfterTheEarnings
                {
                    [Test, AutoMoqData]
                    public void ThenThereIsAPeriodMatch(
                        DatalockValidationService sut,
                        List<RawEarning> earnings,
                        CommitmentEntity commitment
                    )
                    {
                        earnings[0].Period = 1;
                        earnings[1].Period = 2;
                        earnings[2].Period = 3;
                        AssociateEarningsWithCommitment(earnings, commitment);
                        commitment.WithdrawnOnDate = commitment.StartDate.AddMonths(3);
                        commitment.PaymentStatus = (int)PaymentStatus.Cancelled;
                        var accounts = CreateNonPayableAccountsList();
                        var commitments = new List<CommitmentEntity> { commitment };
                        var providerCommitments = new ProviderCommitments(commitments);

                        var actual = sut.ValidateDatalockForProvider(providerCommitments, earnings, accounts);

                        actual.PriceEpisodePeriodMatches.Should().OnlyContain(x => x.Payable);
                        actual.PriceEpisodePeriodMatches.Should().HaveCount(3);
                    }

                    [Test, AutoMoqData]
                    public void ThenThereAreNoValidationErrors(
                        DatalockValidationService sut,
                        List<RawEarning> earnings,
                        CommitmentEntity commitment
                    )
                    {
                        earnings[0].Period = 1;
                        earnings[1].Period = 2;
                        earnings[2].Period = 3;
                        AssociateEarningsWithCommitment(earnings, commitment);
                        commitment.WithdrawnOnDate = commitment.StartDate.AddMonths(3);
                        commitment.PaymentStatus = (int)PaymentStatus.Cancelled;
                        var accounts = CreateNonPayableAccountsList();
                        var commitments = new List<CommitmentEntity> { commitment };
                        var providerCommitments = new ProviderCommitments(commitments);

                        var actual = sut.ValidateDatalockForProvider(providerCommitments, earnings, accounts);

                        actual.ValidationErrors.Should().BeEmpty();
                    }

                    [Test, AutoMoqData]
                    public void ThenThereIsAMatch(
                        DatalockValidationService sut,
                        List<RawEarning> earnings,
                        CommitmentEntity commitment
                    )
                    {
                        earnings[0].Period = 1;
                        earnings[1].Period = 2;
                        earnings[2].Period = 3;
                        AssociateEarningsWithCommitment(earnings, commitment);
                        commitment.WithdrawnOnDate = commitment.StartDate.AddMonths(3);
                        commitment.PaymentStatus = (int)PaymentStatus.Cancelled;
                        var accounts = CreateNonPayableAccountsList();
                        var commitments = new List<CommitmentEntity> { commitment };
                        var providerCommitments = new ProviderCommitments(commitments);

                        var actual = sut.ValidateDatalockForProvider(providerCommitments, earnings, accounts);

                        actual.PriceEpisodeMatches.Should().OnlyContain(x => x.IsSuccess);
                        actual.PriceEpisodeMatches.Should().HaveCount(1);
                    }
                }
            }

            [TestFixture]
            public class WithAPausedCommitment
            {
                [Test, AutoMoqData]
                public void ThenThereAreNoPeriodMatches(
                        DatalockValidationService sut,
                        List<RawEarning> earnings,
                        CommitmentEntity commitment
                    )
                {
                    earnings[0].Period = 1;
                    earnings[1].Period = 2;
                    earnings[2].Period = 3;
                    AssociateEarningsWithCommitment(earnings, commitment);
                    commitment.PausedOnDate = commitment.StartDate.AddMonths(2);
                    commitment.PaymentStatus = (int)PaymentStatus.Paused;
                    var accounts = CreateNonPayableAccountsList();
                    var commitments = new List<CommitmentEntity> { commitment };
                    var providerCommitments = new ProviderCommitments(commitments);

                    var actual = sut.ValidateDatalockForProvider(providerCommitments, earnings, accounts);

                    actual.PriceEpisodePeriodMatches.Where(x => x.Payable).Should().BeEmpty();
                }

                [Test, AutoMoqData]
                public void ThenThereIsOneValidationError(
                    DatalockValidationService sut,
                    List<RawEarning> earnings,
                    CommitmentEntity commitment
                )
                {
                    earnings[0].Period = 1;
                    earnings[1].Period = 2;
                    earnings[2].Period = 3;
                    AssociateEarningsWithCommitment(earnings, commitment);
                    commitment.PausedOnDate = commitment.StartDate.AddMonths(2);
                    commitment.PaymentStatus = (int)PaymentStatus.Paused;
                    var accounts = CreateNonPayableAccountsList();
                    var commitments = new List<CommitmentEntity> { commitment };
                    var providerCommitments = new ProviderCommitments(commitments);

                    var actual = sut.ValidateDatalockForProvider(providerCommitments, earnings, accounts);

                    actual.ValidationErrors.Should().HaveCount(1);
                    actual.ValidationErrors.Should().Contain(x => x.RuleId == DataLockErrorCodes.EmployerPaused);
                }

                [Test, AutoMoqData]
                public void ThenThereAreNoMatches(
                    DatalockValidationService sut,
                    List<RawEarning> earnings,
                    CommitmentEntity commitment
                )
                {
                    earnings[0].Period = 1;
                    earnings[1].Period = 2;
                    earnings[2].Period = 3;
                    AssociateEarningsWithCommitment(earnings, commitment);
                    commitment.PausedOnDate = commitment.StartDate.AddMonths(2);
                    commitment.PaymentStatus = (int)PaymentStatus.Paused;
                    var accounts = CreateNonPayableAccountsList();
                    var commitments = new List<CommitmentEntity> { commitment };
                    var providerCommitments = new ProviderCommitments(commitments);

                    var actual = sut.ValidateDatalockForProvider(providerCommitments, earnings, accounts);

                    actual.PriceEpisodeMatches.Where(x => x.IsSuccess).Should().BeEmpty();
                }
            }
        }

        [TestFixture]
        public class WithMultipleCommitments
        {
            /// <summary>
            /// 6 earnings for these tests, split evenly between the commitments
            /// </summary>
            [TestFixture]
            public class AndMultipleEarnings
            {
                protected void SetupEarningForCommitment(CommitmentEntity commitment, IEnumerable<RawEarning> earnings, DateTime commitmentStartDate)
                {
                    commitment.StartDate = commitmentStartDate;
                    var learnRefNumber = earnings.First().LearnRefNumber;
                    var aimSequenceNumber = earnings.First().AimSeqNumber;
                    foreach (var earning in earnings)
                    {
                        earning.ProgrammeType = commitment.ProgrammeType ?? 0;
                        earning.StandardCode = (int)commitment.StandardCode;
                        earning.PathwayCode = commitment.PathwayCode ?? 0;
                        earning.FrameworkCode = commitment.FrameworkCode ?? 0;
                        earning.AgreedPrice = commitment.AgreedCost;
                        earning.Uln = commitment.Uln;
                        earning.Ukprn = commitment.Ukprn;
                        earning.FirstIncentiveCensusDate = null;
                        earning.SecondIncentiveCensusDate = null;
                        earning.LearnRefNumber = learnRefNumber;
                        earning.AimSeqNumber = aimSequenceNumber;

                        earning.EpisodeStartDate = commitment.StartDate.AddDays(5);
                        earning.EpisodeEffectiveTnpStartDate = earning.EpisodeStartDate;
                        earning.PriceEpisodeIdentifier = earning.EpisodeStartDate.Value.EpisodeIdentifier();
                        earning.EndDate = null;

                        commitment.EndDate = commitment.StartDate.AddYears(2);
                        commitment.WithdrawnOnDate = null;
                        commitment.PausedOnDate = null;
                        commitment.PaymentStatus = (int)PaymentStatus.Active;
                        commitment.ProviderUkprn = commitment.Ukprn;
                    }
                }

                [TestFixture]
                public class WithTwoCommitmentVersions : AndMultipleEarnings
                {
                    List<CommitmentEntity> Setup(CommitmentEntity commitment, List<RawEarning> earnings)
                    {
                        var commitment2 = commitment.Clone();
                        var commitments = new List<CommitmentEntity> {commitment, commitment2};

                        var startDate = RandomStartDateWithin1819AcademicYear(commitment.StartDate);
                        var startDate1 = startDate.FirstDayOfAcademicYear();

                        SetupEarningForCommitment(commitment, earnings.Take(3), startDate1);
                        SetupEarningForCommitment(commitment2, earnings.Skip(3), startDate1.AddMonths(3).AddDays(1));

                        commitment.EffectiveFrom = startDate1;
                        commitment.EffectiveTo = startDate1.AddMonths(3);
                        earnings[0].EndDate = null;
                        earnings[1].EndDate = null;
                        earnings[2].EndDate = null;

                        commitment2.EffectiveFrom = startDate1.AddMonths(3).AddDays(1);
                        commitment2.EffectiveTo = null;

                        return commitments;
                    }

                    [Test, AutoMoqData]
                    public void TwoPricesReturnsSixPayablePeriodMatches(
                        DatalockValidationService sut,
                        CommitmentEntity commitment
                    )
                    {
                        var fixture = new Fixture();
                        var earnings = fixture.CreateMany<RawEarning>(6).ToList();

                        var commitments = Setup(commitment, earnings);

                        commitments[1].AgreedCost = 1500;
                        earnings[3].AgreedPrice = 1500;
                        earnings[4].AgreedPrice = 1500;
                        earnings[5].AgreedPrice = 1500;
                        for (var i = 0; i < 6; i++)
                        {
                            earnings[i].Period = i + 1;
                        }
                        var providerCommitments = new ProviderCommitments(commitments);
                        var accounts = CreateNonPayableAccountsList();

                        var actual = sut.ValidateDatalockForProvider(providerCommitments, earnings, accounts);

                        actual.PriceEpisodePeriodMatches.Where(x => x.Payable).Should().HaveCount(6);
                    }
                }

                [TestFixture]
                public class WithTwoCommitmentIds : AndMultipleEarnings
                {
                    List<CommitmentEntity> Setup(CommitmentEntity commitment, List<RawEarning> earnings)
                    {
                        var commitment2 = commitment.Clone();
                        commitment2.CommitmentId += 1;

                        var commitments = new List<CommitmentEntity> { commitment, commitment2 };

                        var startDate = RandomStartDateWithin1819AcademicYear(commitment.StartDate);
                        var startDate1 = startDate.FirstDayOfAcademicYear();
                        var startDate2 = startDate1.AddMonths(3).AddDays(1);

                        SetupEarningForCommitment(commitment, earnings.Take(3), startDate1);
                        SetupEarningForCommitment(commitment2, earnings.Skip(3), startDate2);

                        commitment.EndDate = startDate1.AddMonths(3);

                        return commitments;
                    }

                    [Test, AutoMoqData]
                    public void TwoPricesReturnsSixPayablePeriodMatches(
                        DatalockValidationService sut,
                        CommitmentEntity commitment
                    )
                    {
                        var fixture = new Fixture();
                        var earnings = fixture.CreateMany<RawEarning>(6).ToList();

                        var commitments = Setup(commitment, earnings);

                        commitments[1].AgreedCost = 1500;
                        earnings[3].AgreedPrice = 1500;
                        earnings[4].AgreedPrice = 1500;
                        earnings[5].AgreedPrice = 1500;
                        for (var i = 0; i < 6; i++)
                        {
                            earnings[i].Period = i + 1;
                        }
                        var providerCommitments = new ProviderCommitments(commitments);
                        var accounts = CreateNonPayableAccountsList();

                        var actual = sut.ValidateDatalockForProvider(providerCommitments, earnings, accounts);

                        actual.PriceEpisodePeriodMatches.Where(x => x.Payable).Should().HaveCount(6);
                    }
                }

                [TestFixture]
                public class WithOneCommitmentFollowedByTwoVersions : AndMultipleEarnings
                {
                    List<CommitmentEntity> Setup(CommitmentEntity commitment, List<RawEarning> earnings)
                    {
                        var commitment2 = commitment.Clone();
                        commitment2.CommitmentId += 1;
                        var commitment3 = commitment2.Clone();
                        var commitments = new List<CommitmentEntity> { commitment, commitment2, commitment3 };

                        var startDate = RandomStartDateWithin1819AcademicYear(commitment.StartDate);
                        var startDate1 = startDate.FirstDayOfAcademicYear();
                        var startDate2 = startDate1.AddMonths(2).AddDays(1);

                        SetupEarningForCommitment(commitment, earnings.Take(2), startDate1);
                        SetupEarningForCommitment(commitment2, earnings.Skip(2), startDate2);
                        SetupEarningForCommitment(commitment3, earnings.Skip(4), startDate2.AddMonths(2).AddDays(1));

                        commitment.EndDate = startDate1.AddMonths(2);

                        commitment2.EffectiveFrom = startDate2;
                        commitment2.EffectiveTo = startDate2.AddMonths(2);
                        commitment3.EffectiveFrom = startDate2.AddMonths(2).AddDays(1);
                        commitment3.EffectiveTo = null;

                        return commitments;
                    }

                    [Test, AutoMoqData]
                    public void ThreePricesReturnsSixPayablePeriodMatches(
                        DatalockValidationService sut,
                        CommitmentEntity commitment
                    )
                    {
                        var fixture = new Fixture();
                        var earnings = fixture.CreateMany<RawEarning>(6).ToList();

                        var commitments = Setup(commitment, earnings);

                        commitments[1].AgreedCost = 1500;
                        commitments[2].AgreedCost = 2000;
                        earnings[2].AgreedPrice = 1500;
                        earnings[3].AgreedPrice = 1500;
                        earnings[4].AgreedPrice = 2000;
                        earnings[5].AgreedPrice = 2000;
                        for (var i = 0; i < 6; i++)
                        {
                            earnings[i].Period = i + 1;
                        }
                        var providerCommitments = new ProviderCommitments(commitments);
                        var accounts = CreateNonPayableAccountsList();

                        var actual = sut.ValidateDatalockForProvider(providerCommitments, earnings, accounts);

                        actual.PriceEpisodePeriodMatches.Where(x => x.Payable).Should().HaveCount(6);
                    }
                }
            }
        }

        [TestFixture]
        public class WithPriceEpisodesFromFutureAcademicYears : WhenCallingValidateDatalocksForProvider
        {
            [Test, AutoMoqData]
            public void ThenValidationErrorsOnlyHasCurrectAcademicYear(
                List<RawEarning> earnings,
                CommitmentEntity commitment
            )
            {
                earnings[0].Period = 1;
                earnings[1].Period = 2;
                earnings[2].Period = 3;
                AssociateEarningsWithCommitment(earnings, commitment);
                var accounts = CreateNonPayableAccountsList();
                var commitments = new List<CommitmentEntity> { commitment };
                var providerCommitments = new ProviderCommitments(commitments);

                var originalPriceEpisode = new PriceEpisode(earnings[2].PriceEpisodeIdentifier);

                earnings[2].PriceEpisodeIdentifier = earnings[2].EpisodeStartDate?.AddYears(1).EpisodeIdentifier();
                earnings[2].AgreedPrice += 10;
                
                var sut = new DatalockValidationService(MatcherFactory.CreateMatcher(), originalPriceEpisode.AcademicYear);
                var actual = sut.ValidateDatalockForProvider(providerCommitments, earnings, accounts);

                actual.ValidationErrors.Should().BeEmpty();
            }
        }

        [TestFixture]
        public class WithPriceEpisodesFromPastAcademicYears : WhenCallingValidateDatalocksForProvider
        {
            [Test, AutoMoqData]
            public void ThenValidationErrorsOnlyHasCurrectAcademicYear(
                List<RawEarning> earnings,
                CommitmentEntity commitment
            )
            {
                earnings[0].Period = 1;
                earnings[1].Period = 2;
                earnings[2].Period = 3;
                AssociateEarningsWithCommitment(earnings, commitment);
                var accounts = CreateNonPayableAccountsList();
                var commitments = new List<CommitmentEntity> { commitment };
                var providerCommitments = new ProviderCommitments(commitments);

                var originalPriceEpisode = new PriceEpisode(earnings[2].PriceEpisodeIdentifier);

                earnings[2].PriceEpisodeIdentifier = earnings[2].EpisodeStartDate?.AddYears(-1).EpisodeIdentifier();
                earnings[2].AgreedPrice += 10;

                var sut = new DatalockValidationService(MatcherFactory.CreateMatcher(), originalPriceEpisode.AcademicYear);
                var actual = sut.ValidateDatalockForProvider(providerCommitments, earnings, accounts);

                actual.ValidationErrors.Should().BeEmpty();
            }
        }
    }
}
