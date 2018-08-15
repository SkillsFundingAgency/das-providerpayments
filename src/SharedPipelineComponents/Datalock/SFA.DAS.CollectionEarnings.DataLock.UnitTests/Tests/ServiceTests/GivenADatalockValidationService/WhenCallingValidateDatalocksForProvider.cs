using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.CollectionEarnings.DataLock.Application.DataLock.Matcher;
using SFA.DAS.CollectionEarnings.DataLock.Domain;
using SFA.DAS.CollectionEarnings.DataLock.Infrastructure.Data.Entities;
using SFA.DAS.CollectionEarnings.DataLock.Services;
using SFA.DAS.CollectionEarnings.DataLock.UnitTests.Utilities.Attributes;
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
        static void AssociateEarningsWithCommitment(List<RawEarning> earnings, CommitmentEntity commitment)
        {
            commitment.StartDate = commitment.StartDate.FirstDayOfAcademicYear();
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
                
                earning.EpisodeStartDate = commitment.StartDate.AddDays(5);
                earning.PriceEpisodeIdentifier = earning.EpisodeStartDate.Value.EpisodeIdentifier();
                commitment.EndDate = commitment.StartDate.AddYears(2);
                commitment.WithdrawnOnDate = null;
                commitment.PausedOnDate = null;
                commitment.PaymentStatus = 1;
            }
        }

        static ImmutableHashSet<long> CreateNonPayableAccountsList(params long[] accountsToInclude)
        {
            return ImmutableHashSet.Create<long>(accountsToInclude);
        }

        [TestFixture]
        public class WithOneRawEarning
        {
            [TestFixture]
            public class WithAValidCommitment
            {
                [Test, AutoMoqData]
                public void ThenThereIsAPayablePeriodMatch(
                    RawEarning earning,
                    CommitmentEntity commitment
                    )
                {
                    var earnings = new List<RawEarning>{earning};
                    AssociateEarningsWithCommitment(earnings, commitment);
                    var accounts = CreateNonPayableAccountsList();
                    var commitments = new List<CommitmentEntity> {commitment};
                    var providerCommitments = new ProviderCommitments(commitments);

                    var sut = new DatalockValidationService(MatcherFactory.CreateMatcher());

                    var actual = sut.ValidateDatalockForProvider(providerCommitments, earnings, accounts);

                    actual.PriceEpisodePeriodMatches.Should().Contain(x => x.Payable);
                }

                [Test, AutoMoqData]
                public void ThenThereAreNoValidationErrors(
                    RawEarning earning,
                    CommitmentEntity commitment
                )
                {
                    var earnings = new List<RawEarning> { earning };
                    AssociateEarningsWithCommitment(earnings, commitment);
                    var accounts = CreateNonPayableAccountsList();
                    var commitments = new List<CommitmentEntity> { commitment };
                    var providerCommitments = new ProviderCommitments(commitments);

                    var sut = new DatalockValidationService(MatcherFactory.CreateMatcher());

                    var actual = sut.ValidateDatalockForProvider(providerCommitments, earnings, accounts);

                    actual.ValidationErrors.Should().BeEmpty();
                }

                [Test, AutoMoqData]
                public void ThenThereIsAMatch(
                    RawEarning earning,
                    CommitmentEntity commitment
                )
                {
                    var earnings = new List<RawEarning> { earning };
                    AssociateEarningsWithCommitment(earnings, commitment);
                    var accounts = CreateNonPayableAccountsList();
                    var commitments = new List<CommitmentEntity> { commitment };
                    var providerCommitments = new ProviderCommitments(commitments);

                    var sut = new DatalockValidationService(MatcherFactory.CreateMatcher());

                    var actual = sut.ValidateDatalockForProvider(providerCommitments, earnings, accounts);

                    actual.PriceEpisodeMatches.Should().Contain(x => x.IsSuccess);
                }
            }

            [TestFixture]
            public class WithNoCommitments
            {
                [Test, AutoMoqData]
                public void ThenThereAreNoPeriodMatches(
                    RawEarning earning)
                {
                    var earnings = new List<RawEarning> { earning };
                    var accounts = CreateNonPayableAccountsList();
                    var providerCommitments = new ProviderCommitments(new List<CommitmentEntity>());

                    var sut = new DatalockValidationService(MatcherFactory.CreateMatcher());

                    var actual = sut.ValidateDatalockForProvider(providerCommitments, earnings, accounts);

                    actual.PriceEpisodePeriodMatches.Should().BeEmpty();
                }

                [Test, AutoMoqData]
                public void ThenThereAreNoValidationErrors(
                    RawEarning earning)
                {
                    var earnings = new List<RawEarning> { earning };
                    var accounts = CreateNonPayableAccountsList();
                    var providerCommitments = new ProviderCommitments(new List<CommitmentEntity>());

                    var sut = new DatalockValidationService(MatcherFactory.CreateMatcher());

                    var actual = sut.ValidateDatalockForProvider(providerCommitments, earnings, accounts);

                    actual.ValidationErrors.Should().BeEmpty();
                }

                [Test, AutoMoqData]
                public void ThenThereAreNoMatches(
                    RawEarning earning)
                {
                    var earnings = new List<RawEarning> { earning };
                    var accounts = CreateNonPayableAccountsList();
                    var providerCommitments = new ProviderCommitments(new List<CommitmentEntity>());

                    var sut = new DatalockValidationService(MatcherFactory.CreateMatcher());

                    var actual = sut.ValidateDatalockForProvider(providerCommitments, earnings, accounts);

                    actual.PriceEpisodeMatches.Should().BeEmpty();
                }
            }

            [TestFixture]
            public class WithAWithdrawnCommitment
            {
                [TestFixture]
                public class ThatWasWithdrawnBeforeTheEarning
                {
                    [Test, AutoMoqData]
                    public void ThenThereAreNoPeriodMatches(
                        RawEarning earning,
                        CommitmentEntity commitment
                    )
                    {
                        var earnings = new List<RawEarning> { earning };
                        AssociateEarningsWithCommitment(earnings, commitment);
                        commitment.WithdrawnOnDate = commitment.StartDate.AddDays(2);
                        var accounts = CreateNonPayableAccountsList();
                        var commitments = new List<CommitmentEntity> { commitment };
                        var providerCommitments = new ProviderCommitments(commitments);

                        var sut = new DatalockValidationService(MatcherFactory.CreateMatcher());

                        var actual = sut.ValidateDatalockForProvider(providerCommitments, earnings, accounts);

                        actual.PriceEpisodePeriodMatches.Should().BeEmpty();
                    }

                    [Test, AutoMoqData]
                    public void ThenThereAreNoValidationErrors(
                        RawEarning earning,
                        CommitmentEntity commitment
                    )
                    {
                        var earnings = new List<RawEarning> { earning };
                        AssociateEarningsWithCommitment(earnings, commitment);
                        commitment.WithdrawnOnDate = commitment.StartDate.AddDays(2);
                        var accounts = CreateNonPayableAccountsList();
                        var commitments = new List<CommitmentEntity> { commitment };
                        var providerCommitments = new ProviderCommitments(commitments);

                        var sut = new DatalockValidationService(MatcherFactory.CreateMatcher());

                        var actual = sut.ValidateDatalockForProvider(providerCommitments, earnings, accounts);

                        actual.ValidationErrors.Should().BeEmpty();
                    }

                    [Test, AutoMoqData]
                    public void ThenThereAreNoMatches(
                        RawEarning earning,
                        CommitmentEntity commitment
                    )
                    {
                        var earnings = new List<RawEarning> { earning };
                        AssociateEarningsWithCommitment(earnings, commitment);
                        commitment.WithdrawnOnDate = commitment.StartDate.AddDays(2);
                        var accounts = CreateNonPayableAccountsList();
                        var commitments = new List<CommitmentEntity> { commitment };
                        var providerCommitments = new ProviderCommitments(commitments);

                        var sut = new DatalockValidationService(MatcherFactory.CreateMatcher());

                        var actual = sut.ValidateDatalockForProvider(providerCommitments, earnings, accounts);

                        actual.PriceEpisodeMatches.Should().BeEmpty();
                    }
                }

                [TestFixture]
                public class ThatWasWithdrawnAfterTheEarning
                {
                    [Test, AutoMoqData]
                    public void ThenThereIsAPeriodMatches(
                        RawEarning earning,
                        CommitmentEntity commitment
                    )
                    {
                        var earnings = new List<RawEarning> { earning };
                        AssociateEarningsWithCommitment(earnings, commitment);
                        commitment.WithdrawnOnDate = commitment.StartDate.AddMonths(1);
                        earning.Period = 1;

                        var accounts = CreateNonPayableAccountsList();
                        var commitments = new List<CommitmentEntity> { commitment };
                        var providerCommitments = new ProviderCommitments(commitments);

                        var sut = new DatalockValidationService(MatcherFactory.CreateMatcher());

                        var actual = sut.ValidateDatalockForProvider(providerCommitments, earnings, accounts);

                        actual.PriceEpisodePeriodMatches.Should().Contain(x => x.Payable);
                    }

                    [Test, AutoMoqData]
                    public void ThenThereAreNoValidationErrors(
                        RawEarning earning,
                        CommitmentEntity commitment
                    )
                    {
                        var earnings = new List<RawEarning> { earning };
                        AssociateEarningsWithCommitment(earnings, commitment);
                        commitment.WithdrawnOnDate = commitment.StartDate.AddMonths(1);
                        earning.Period = 1;

                        var accounts = CreateNonPayableAccountsList();
                        var commitments = new List<CommitmentEntity> { commitment };
                        var providerCommitments = new ProviderCommitments(commitments);

                        var sut = new DatalockValidationService(MatcherFactory.CreateMatcher());

                        var actual = sut.ValidateDatalockForProvider(providerCommitments, earnings, accounts);

                        actual.ValidationErrors.Should().BeEmpty();
                    }

                    [Test, AutoMoqData]
                    public void ThenThereIsAMatch(
                        RawEarning earning,
                        CommitmentEntity commitment
                    )
                    {
                        var earnings = new List<RawEarning> { earning };
                        AssociateEarningsWithCommitment(earnings, commitment);
                        commitment.WithdrawnOnDate = commitment.StartDate.AddMonths(1);
                        earning.Period = 1;

                        var accounts = CreateNonPayableAccountsList();
                        var commitments = new List<CommitmentEntity> { commitment };
                        var providerCommitments = new ProviderCommitments(commitments);

                        var sut = new DatalockValidationService(MatcherFactory.CreateMatcher());

                        var actual = sut.ValidateDatalockForProvider(providerCommitments, earnings, accounts);

                        actual.PriceEpisodeMatches.Should().Contain(x => x.IsSuccess);
                    }
                }
            }

            [TestFixture]
            public class WithAPausedCommitment
            {

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

                    var sut = new DatalockValidationService(MatcherFactory.CreateMatcher());

                    var actual = sut.ValidateDatalockForProvider(providerCommitments, earnings, accounts);

                    actual.PriceEpisodePeriodMatches.Should().OnlyContain(x => x.Payable);
                    actual.PriceEpisodePeriodMatches.Should().HaveCount(3);
                }

                [Test, AutoMoqData]
                public void ThenThereAreNoValidationErrors(
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

                    var sut = new DatalockValidationService(MatcherFactory.CreateMatcher());

                    var actual = sut.ValidateDatalockForProvider(providerCommitments, earnings, accounts);

                    actual.ValidationErrors.Should().BeEmpty();
                }

                [Test, AutoMoqData]
                public void ThenThereAreThreeMatches(
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

                    var sut = new DatalockValidationService(MatcherFactory.CreateMatcher());

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
                    List<RawEarning> earnings
                )
                {
                    earnings[0].Period = 1;
                    earnings[1].Period = 2;
                    earnings[2].Period = 3;
                    var accounts = CreateNonPayableAccountsList();
                    var providerCommitments = new ProviderCommitments(new List<CommitmentEntity>());

                    var sut = new DatalockValidationService(MatcherFactory.CreateMatcher());

                    var actual = sut.ValidateDatalockForProvider(providerCommitments, earnings, accounts);

                    actual.PriceEpisodePeriodMatches.Should().BeEmpty();
                }

                [Test, AutoMoqData]
                public void ThenThereAreNoValidationErrors(
                    List<RawEarning> earnings
                )
                {
                    earnings[0].Period = 1;
                    earnings[1].Period = 2;
                    earnings[2].Period = 3;
                    var accounts = CreateNonPayableAccountsList();
                    var providerCommitments = new ProviderCommitments(new List<CommitmentEntity>());

                    var sut = new DatalockValidationService(MatcherFactory.CreateMatcher());

                    var actual = sut.ValidateDatalockForProvider(providerCommitments, earnings, accounts);

                    actual.ValidationErrors.Should().BeEmpty();
                }

                [Test, AutoMoqData]
                public void ThenThereAreNoMatches(
                    List<RawEarning> earnings
                )
                {
                    earnings[0].Period = 1;
                    earnings[1].Period = 2;
                    earnings[2].Period = 3;
                    var accounts = CreateNonPayableAccountsList();
                    var providerCommitments = new ProviderCommitments(new List<CommitmentEntity>());

                    var sut = new DatalockValidationService(MatcherFactory.CreateMatcher());

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
                        List<RawEarning> earnings,
                        CommitmentEntity commitment
                    )
                    {
                        earnings[0].Period = 1;
                        earnings[1].Period = 2;
                        earnings[2].Period = 3;
                        AssociateEarningsWithCommitment(earnings, commitment);
                        commitment.WithdrawnOnDate = commitment.StartDate.AddMonths(2);
                        var accounts = CreateNonPayableAccountsList();
                        var commitments = new List<CommitmentEntity> { commitment };
                        var providerCommitments = new ProviderCommitments(commitments);

                        var sut = new DatalockValidationService(MatcherFactory.CreateMatcher());

                        var actual = sut.ValidateDatalockForProvider(providerCommitments, earnings, accounts);

                        actual.PriceEpisodePeriodMatches.Should().OnlyContain(x => x.Payable);
                        actual.PriceEpisodePeriodMatches.Should().HaveCount(2);
                    }

                    [Test, AutoMoqData]
                    public void ThenThereAreNoValidationErrors(
                        List<RawEarning> earnings,
                        CommitmentEntity commitment
                    )
                    {
                        earnings[0].Period = 1;
                        earnings[1].Period = 2;
                        earnings[2].Period = 3;
                        AssociateEarningsWithCommitment(earnings, commitment);
                        commitment.WithdrawnOnDate = commitment.StartDate.AddMonths(2);
                        var accounts = CreateNonPayableAccountsList();
                        var commitments = new List<CommitmentEntity> { commitment };
                        var providerCommitments = new ProviderCommitments(commitments);

                        var sut = new DatalockValidationService(MatcherFactory.CreateMatcher());

                        var actual = sut.ValidateDatalockForProvider(providerCommitments, earnings, accounts);

                        actual.ValidationErrors.Should().BeEmpty();
                    }

                    [Test, AutoMoqData]
                    public void ThenThereAreTwoMatches(
                        List<RawEarning> earnings,
                        CommitmentEntity commitment
                    )
                    {
                        earnings[0].Period = 1;
                        earnings[1].Period = 2;
                        earnings[2].Period = 3;
                        AssociateEarningsWithCommitment(earnings, commitment);
                        commitment.WithdrawnOnDate = commitment.StartDate.AddMonths(2);
                        var accounts = CreateNonPayableAccountsList();
                        var commitments = new List<CommitmentEntity> { commitment };
                        var providerCommitments = new ProviderCommitments(commitments);

                        var sut = new DatalockValidationService(MatcherFactory.CreateMatcher());

                        var actual = sut.ValidateDatalockForProvider(providerCommitments, earnings, accounts);

                        actual.PriceEpisodeMatches.Should().HaveCount(2);
                        actual.PriceEpisodeMatches.Should().OnlyContain(x => x.IsSuccess);
                    }
                }

                [TestFixture]
                public class ThatWasWithdrawnAfterTheEarnings
                {
                    [Test, AutoMoqData]
                    public void ThenThereIsAPeriodMatches(
                        List<RawEarning> earnings,
                        CommitmentEntity commitment
                    )
                    {
                        earnings[0].Period = 1;
                        earnings[1].Period = 2;
                        earnings[2].Period = 3;
                        AssociateEarningsWithCommitment(earnings, commitment);
                        commitment.WithdrawnOnDate = commitment.StartDate.AddMonths(3);
                        var accounts = CreateNonPayableAccountsList();
                        var commitments = new List<CommitmentEntity> { commitment };
                        var providerCommitments = new ProviderCommitments(commitments);

                        var sut = new DatalockValidationService(MatcherFactory.CreateMatcher());

                        var actual = sut.ValidateDatalockForProvider(providerCommitments, earnings, accounts);

                        actual.PriceEpisodePeriodMatches.Should().OnlyContain(x => x.Payable);
                        actual.PriceEpisodePeriodMatches.Should().HaveCount(3);
                    }

                    [Test, AutoMoqData]
                    public void ThenThereAreNoValidationErrors(
                        List<RawEarning> earnings,
                        CommitmentEntity commitment
                    )
                    {
                        earnings[0].Period = 1;
                        earnings[1].Period = 2;
                        earnings[2].Period = 3;
                        AssociateEarningsWithCommitment(earnings, commitment);
                        commitment.WithdrawnOnDate = commitment.StartDate.AddMonths(3);
                        var accounts = CreateNonPayableAccountsList();
                        var commitments = new List<CommitmentEntity> { commitment };
                        var providerCommitments = new ProviderCommitments(commitments);

                        var sut = new DatalockValidationService(MatcherFactory.CreateMatcher());

                        var actual = sut.ValidateDatalockForProvider(providerCommitments, earnings, accounts);

                        actual.ValidationErrors.Should().BeEmpty();
                    }

                    [Test, AutoMoqData]
                    public void ThenThereIsAMatch(
                        List<RawEarning> earnings,
                        CommitmentEntity commitment
                    )
                    {
                        earnings[0].Period = 1;
                        earnings[1].Period = 2;
                        earnings[2].Period = 3;
                        AssociateEarningsWithCommitment(earnings, commitment);
                        commitment.WithdrawnOnDate = commitment.StartDate.AddMonths(3);
                        var accounts = CreateNonPayableAccountsList();
                        var commitments = new List<CommitmentEntity> { commitment };
                        var providerCommitments = new ProviderCommitments(commitments);

                        var sut = new DatalockValidationService(MatcherFactory.CreateMatcher());

                        var actual = sut.ValidateDatalockForProvider(providerCommitments, earnings, accounts);

                        actual.PriceEpisodeMatches.Should().OnlyContain(x => x.IsSuccess);
                        actual.PriceEpisodeMatches.Should().HaveCount(3);
                    }
                }
            }

            [TestFixture]
            public class WithAPausedCommitment
            {

            }
        }
    }
}
