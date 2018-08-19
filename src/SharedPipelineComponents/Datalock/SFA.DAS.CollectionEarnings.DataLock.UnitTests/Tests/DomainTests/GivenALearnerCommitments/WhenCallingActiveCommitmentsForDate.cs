using System;
using System.Collections.Generic;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.CollectionEarnings.DataLock.Domain;
using SFA.DAS.CollectionEarnings.DataLock.Infrastructure.Data.Entities;
using SFA.DAS.CollectionEarnings.DataLock.UnitTests.Utilities.Attributes;

namespace SFA.DAS.CollectionEarnings.DataLock.UnitTests.Tests.DomainTests.GivenALearnerCommitments
{
    [TestFixture]
    public class WhenCallingActiveCommitmentsForDate
    {
        [TestFixture]
        public class WithOneCommitment
        {
            /// <summary>
            /// Start Date 2018-09-01 --
            /// End Date 2020-01-01
            /// </summary>
            List<CommitmentEntity> SetupCommitments(CommitmentEntity commitment)
            {
                commitment.StartDate = new DateTime(2018, 09, 01);
                commitment.EndDate = new DateTime(2020, 01, 01);
                commitment.WithdrawnOnDate = null;
                commitment.PausedOnDate = null;
                commitment.PaymentStatus = 1;
                return new List<CommitmentEntity> { commitment };
            }

            [Test, AutoMoqData]
            public void AndADatePriorToTheCommitmentStartDateReturnsAnEmptyList(
                CommitmentEntity commitment,
                long uln
                )
            {
                var commitments = SetupCommitments(commitment);

                var sut = new LearnerCommitments(uln, commitments);

                var actual = sut.ActiveCommitmentsForDate(new DateTime(2018, 08, 01));
                actual.Should().BeEmpty();
            }

            [Test, AutoMoqData]
            public void AndADateAfterTheCommitmentStartDateReturnsTheCommitment(
                CommitmentEntity commitment,
                long uln
            )
            {
                var commitments = SetupCommitments(commitment);

                var sut = new LearnerCommitments(uln, commitments);

                var actual = sut.ActiveCommitmentsForDate(new DateTime(2018, 10, 01));
                actual.Should().AllBeEquivalentTo(commitment);
            }
        }

        [TestFixture]
        public class WithTwoCommitmentVersions
        {
            /// <summary>
            /// <para>
            /// Commitment1 Effective From 2018-09-01 --
            /// Effective To 2018-11-01
            /// </para>
            /// <para>
            /// Commitment2 Effective From 2018-11-02 --
            /// Effective To End --
            /// End Date 2020-01-01
            /// </para>
            /// </summary>
            List<CommitmentEntity> SetupCommitments(CommitmentEntity commitment1, CommitmentEntity commitment2)
            {
                commitment2.CommitmentId = commitment1.CommitmentId;

                commitment1.StartDate = new DateTime(2018, 09, 01);
                commitment1.EndDate = new DateTime(2020, 01, 01);
                commitment1.EffectiveFrom = new DateTime(2018, 09, 01);
                commitment1.EffectiveTo = new DateTime(2018, 11, 01);
                commitment1.WithdrawnOnDate = null;
                commitment1.PausedOnDate = null;
                commitment1.PaymentStatus = 1;

                commitment2.StartDate = commitment1.StartDate;
                commitment2.EndDate = commitment1.EndDate;
                commitment2.EffectiveFrom = new DateTime(2018, 11, 02);
                commitment2.EffectiveTo = null;
                commitment2.WithdrawnOnDate = null;
                commitment2.PausedOnDate = null;
                commitment2.PaymentStatus = 1;

                return new List<CommitmentEntity> { commitment1, commitment2 };
            }

            [Test, AutoMoqData]
            public void AndADatePriorToTheFirstCommitmentStartDateReturnsAnEmptyList(
                CommitmentEntity commitment1,
                CommitmentEntity commitment2,
                long uln
            )
            {
                var commitments = SetupCommitments(commitment1, commitment2);

                var sut = new LearnerCommitments(uln, commitments);

                var actual = sut.ActiveCommitmentsForDate(new DateTime(2018, 08, 01));
                actual.Should().BeEmpty();
            }

            [Test, AutoMoqData]
            public void AndADateDuringTheFirstVersionReturnsTheFirstVersion(
                CommitmentEntity commitment1,
                CommitmentEntity commitment2,
                long uln
            )
            {
                var commitments = SetupCommitments(commitment1, commitment2);

                var sut = new LearnerCommitments(uln, commitments);

                var actual = sut.ActiveCommitmentsForDate(new DateTime(2018, 10, 01));
                actual.Should().AllBeEquivalentTo(commitment1);
                actual.Should().HaveCount(1);
            }

            [Test, AutoMoqData]
            public void AndADateDuringTheSecondVersionReturnsTheSecondVersion(
                CommitmentEntity commitment1,
                CommitmentEntity commitment2,
                long uln
            )
            {
                var commitments = SetupCommitments(commitment1, commitment2);

                var sut = new LearnerCommitments(uln, commitments);

                var actual = sut.ActiveCommitmentsForDate(new DateTime(2018, 12, 01));
                actual.Should().AllBeEquivalentTo(commitment2);
                actual.Should().HaveCount(1);
            }

            [Test, AutoMoqData]
            public void AndADateAfterTheSecondVersionEndDateReturnsTheSecondVersion(
                CommitmentEntity commitment1,
                CommitmentEntity commitment2,
                long uln
            )
            {
                var commitments = SetupCommitments(commitment1, commitment2);

                var sut = new LearnerCommitments(uln, commitments);

                var actual = sut.ActiveCommitmentsForDate(new DateTime(2020, 02, 01));
                actual.Should().AllBeEquivalentTo(commitment2);
                actual.Should().HaveCount(1);
            }
        }

        [TestFixture]
        public class WithTwoCommitmentWithDifferentIds
        {
            /// <summary>
            /// <para>
            /// Commitment1 Start Date 2018-09-01 --
            /// Withdrawn Date 2018-11-01
            /// End Date 2020-01-01
            /// </para>
            /// <para>
            /// Commitment2 Start Date 2018-11-02 --
            /// End Date 2020-01-01
            /// </para>
            /// </summary>
            List<CommitmentEntity> SetupCommitments(CommitmentEntity commitment1, CommitmentEntity commitment2)
            {
                commitment1.StartDate = new DateTime(2018, 09, 01);
                commitment1.EndDate = new DateTime(2020, 01, 01);
                commitment1.WithdrawnOnDate = new DateTime(2018, 11, 01);
                commitment1.PausedOnDate = null;
                commitment1.PaymentStatus = 3;

                commitment2.StartDate = new DateTime(2018, 11, 02);
                commitment2.EndDate = new DateTime(2020, 01, 01);
                commitment2.WithdrawnOnDate = null;
                commitment2.PausedOnDate = null;
                commitment2.PaymentStatus = 1;

                return new List<CommitmentEntity> { commitment1, commitment2 };
            }

            [Test, AutoMoqData]
            public void AndADatePriorToTheFirstCommitmentStartDateReturnsAnEmptyList(
                CommitmentEntity commitment1,
                CommitmentEntity commitment2,
                long uln
            )
            {
                var commitments = SetupCommitments(commitment1, commitment2);

                var sut = new LearnerCommitments(uln, commitments);

                var actual = sut.ActiveCommitmentsForDate(new DateTime(2018, 08, 01));
                actual.Should().BeEmpty();
            }

            [Test, AutoMoqData]
            public void AndADateDuringTheFirstCommitmentReturnsTheFirstCommitment(
                CommitmentEntity commitment1,
                CommitmentEntity commitment2,
                long uln
            )
            {
                var commitments = SetupCommitments(commitment1, commitment2);

                var sut = new LearnerCommitments(uln, commitments);

                var actual = sut.ActiveCommitmentsForDate(new DateTime(2018, 10, 01));
                actual.Should().AllBeEquivalentTo(commitment1);
                actual.Should().HaveCount(1);
            }

            [Test, AutoMoqData]
            public void AndADateDuringTheSecondCommitmentReturnsTheSecondCommitment(
                CommitmentEntity commitment1,
                CommitmentEntity commitment2,
                long uln
            )
            {
                var commitments = SetupCommitments(commitment1, commitment2);

                var sut = new LearnerCommitments(uln, commitments);

                var actual = sut.ActiveCommitmentsForDate(new DateTime(2018, 12, 01));
                actual.Should().AllBeEquivalentTo(commitment2);
                actual.Should().HaveCount(1);
            }

            [TestFixture]
            public class AndTheCommitmentsHaveOverlappingDates
            {
                /// <summary>
                /// <para>Commitment1: 2018-09-01 -> 2018-11-20</para>
                /// <para>Commitment2: 2018-11-10 -> 2020-01-01</para>
                /// </summary>
                List<CommitmentEntity> SetupCommitments(CommitmentEntity commitment1, CommitmentEntity commitment2)
                {
                    commitment1.StartDate = new DateTime(2018, 09, 01);
                    commitment1.EndDate = new DateTime(2020, 01, 01);
                    commitment1.WithdrawnOnDate = new DateTime(2018, 11, 20);
                    commitment1.PausedOnDate = null;
                    commitment1.PaymentStatus = 3;
                    
                    commitment2.StartDate = new DateTime(2018, 11, 10);
                    commitment2.EndDate = new DateTime(2020, 01, 01);
                    commitment2.WithdrawnOnDate = null;
                    commitment2.PausedOnDate = null;
                    commitment2.PaymentStatus = 1;
                    
                    return new List<CommitmentEntity> { commitment1, commitment2 };
                }

                [Test, AutoMoqData]
                public void AndADateInTheOverlapPeriod(
                    CommitmentEntity commitment1,
                    CommitmentEntity commitment2,
                    long uln
                    )
                {
                    var commitments = SetupCommitments(commitment1, commitment2);

                    var sut = new LearnerCommitments(uln, commitments);

                    var actual = sut.ActiveCommitmentsForDate(new DateTime(2018, 11, 15));
                    actual.Should().HaveCount(2);
                }

                [Test, AutoMoqData]
                public void AndADateBeforeTheOverlapPeriodButAfterTheStartDate(
                    CommitmentEntity commitment1,
                    CommitmentEntity commitment2,
                    long uln
                )
                {
                    var commitments = SetupCommitments(commitment1, commitment2);

                    var sut = new LearnerCommitments(uln, commitments);

                    var actual = sut.ActiveCommitmentsForDate(new DateTime(2018, 09, 15));
                    actual.Should().AllBeEquivalentTo(commitment1);
                    actual.Should().HaveCount(1);
                }

                [Test, AutoMoqData]
                public void AndADateBeforeTheStartDate(
                    CommitmentEntity commitment1,
                    CommitmentEntity commitment2,
                    long uln
                )
                {
                    var commitments = SetupCommitments(commitment1, commitment2);

                    var sut = new LearnerCommitments(uln, commitments);

                    var actual = sut.ActiveCommitmentsForDate(new DateTime(2018, 07, 15));
                    actual.Should().BeEmpty();
                }

                [Test, AutoMoqData]
                public void AndADateAfterTheOverlapPeriodButBeforeTheEndDate(
                    CommitmentEntity commitment1,
                    CommitmentEntity commitment2,
                    long uln
                )
                {
                    var commitments = SetupCommitments(commitment1, commitment2);

                    var sut = new LearnerCommitments(uln, commitments);

                    var actual = sut.ActiveCommitmentsForDate(new DateTime(2018, 12, 15));
                    actual.Should().AllBeEquivalentTo(commitment2);
                    actual.Should().HaveCount(1);
                }

                [Test, AutoMoqData]
                public void AndADateAfterTheTheEndDateReturnsTheLatestCommitment(
                    CommitmentEntity commitment1,
                    CommitmentEntity commitment2,
                    long uln
                )
                {
                    var commitments = SetupCommitments(commitment1, commitment2);

                    var sut = new LearnerCommitments(uln, commitments);

                    var actual = sut.ActiveCommitmentsForDate(new DateTime(2020, 12, 15));
                    actual.Should().AllBeEquivalentTo(commitment2);
                    actual.Should().HaveCount(1);
                }
            }

            [TestFixture]
            public class AndTheSecondCommitmentHavingTwoVersions
            {

                /// <summary>
                /// <para>Commitment1: 2018-09-01 -> 2018-11-20</para>
                /// <para>Commitment2: 2018-11-10 -> 2020-01-01 --
                /// Effective From 2018-11-02 --
                /// Effective To 2019-01-01
                /// </para>
                /// <para>Commitment3: 2018-11-10 -> 2020-01-01 --
                /// Effective From 2019-01-00 --
                /// Effective To End
                /// </para>
                /// </summary>
                List<CommitmentEntity> SetupCommitments(
                    CommitmentEntity commitment1,
                    CommitmentEntity commitment2,
                    CommitmentEntity commitment3)
                {
                    commitment1.StartDate = new DateTime(2018, 09, 01);
                    commitment1.EndDate = new DateTime(2020, 01, 01);
                    commitment1.WithdrawnOnDate = new DateTime(2018, 11, 01);
                    commitment1.PausedOnDate = null;
                    commitment1.PaymentStatus = 3;

                    commitment2.StartDate = new DateTime(2018, 11, 02);
                    commitment2.EndDate = new DateTime(2020, 01, 01);
                    commitment2.EffectiveFrom = new DateTime(2018, 11, 02);
                    commitment2.EffectiveTo = new DateTime(2019, 01, 01);
                    commitment2.WithdrawnOnDate = null;
                    commitment2.PausedOnDate = null;
                    commitment2.PaymentStatus = 1;

                    commitment3.CommitmentId = commitment2.CommitmentId;
                    commitment3.StartDate = commitment2.StartDate;
                    commitment3.EndDate = commitment2.EndDate;
                    commitment3.EffectiveFrom = new DateTime(2019, 01, 01);
                    commitment3.EffectiveTo = null;
                    commitment3.WithdrawnOnDate = null;
                    commitment3.PausedOnDate = null;
                    commitment3.PaymentStatus = 1;

                    return new List<CommitmentEntity> { commitment1, commitment2, commitment3 };
                }

                [Test, AutoMoqData]
                public void AndADatePriorToTheFirstCommitmentStartDateReturnsAnEmptyList(
                    CommitmentEntity commitment1,
                    CommitmentEntity commitment2,
                    CommitmentEntity commitment3,
                    long uln
                )
                {
                    var commitments = SetupCommitments(commitment1, commitment2, commitment3);

                    var sut = new LearnerCommitments(uln, commitments);

                    var actual = sut.ActiveCommitmentsForDate(new DateTime(2018, 08, 01));
                    actual.Should().BeEmpty();
                }

                [Test, AutoMoqData]
                public void AndADateDuringTheFirstCommitmentReturnsTheFirstCommitment(
                    CommitmentEntity commitment1,
                    CommitmentEntity commitment2,
                    CommitmentEntity commitment3,
                    long uln
                )
                {
                    var commitments = SetupCommitments(commitment1, commitment2, commitment3);

                    var sut = new LearnerCommitments(uln, commitments);

                    var actual = sut.ActiveCommitmentsForDate(new DateTime(2018, 10, 01));
                    actual.Should().AllBeEquivalentTo(commitment1);
                    actual.Should().HaveCount(1);
                }

                [Test, AutoMoqData]
                public void AndADateAfterTheSecondCommitmentStartDateReturnsTheSecondCommitment(
                    CommitmentEntity commitment1,
                    CommitmentEntity commitment2,
                    CommitmentEntity commitment3,
                    long uln
                )
                {
                    var commitments = SetupCommitments(commitment1, commitment2, commitment3);

                    var sut = new LearnerCommitments(uln, commitments);

                    var actual = sut.ActiveCommitmentsForDate(new DateTime(2019, 03, 01));
                    actual.Should().AllBeEquivalentTo(commitment3);
                    actual.Should().HaveCount(1);
                }

                [TestFixture]
                public class AndTheDateIsForTheCommitmentWithTwoVersions : AndTheSecondCommitmentHavingTwoVersions
                {
                    [Test, AutoMoqData]
                    public void AndADateDuringTheFirstVersionReturnsTheFirstVersion(
                        CommitmentEntity commitment1,
                        CommitmentEntity commitment2,
                        CommitmentEntity commitment3,
                        long uln
                    )
                    {
                        var commitments = SetupCommitments(commitment1, commitment2, commitment3);

                        var sut = new LearnerCommitments(uln, commitments);

                        var actual = sut.ActiveCommitmentsForDate(new DateTime(2018, 12, 01));
                        actual.Should().AllBeEquivalentTo(commitment2);
                        actual.Should().HaveCount(1);
                    }

                    [Test, AutoMoqData]
                    public void AndADateAfterTheSecondVersionStartDateReturnsTheSecondVersion(
                        CommitmentEntity commitment1,
                        CommitmentEntity commitment2,
                        CommitmentEntity commitment3,
                        long uln
                    )
                    {
                        var commitments = SetupCommitments(commitment1, commitment2, commitment3);

                        var sut = new LearnerCommitments(uln, commitments);

                        var actual = sut.ActiveCommitmentsForDate(new DateTime(2019, 02, 01));
                        actual.Should().AllBeEquivalentTo(commitment3);
                        actual.Should().HaveCount(1);
                    }
                }
            }
        }
    }
}
