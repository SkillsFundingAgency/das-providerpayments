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
    public class CallingCommitmentsForDate
    {
        [TestFixture]
        public class WithOneCommitment
        {
            List<CommitmentEntity> SetupCommitments(CommitmentEntity commitment)
            {
                commitment.StartDate = new DateTime(2018, 09, 01);
                commitment.EndDate = new DateTime(2020, 01, 01);
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

                var actual = sut.CommitmentsForDate(new DateTime(2018, 08, 01));
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

                var actual = sut.CommitmentsForDate(new DateTime(2018, 10, 01));
                actual.Should().AllBeEquivalentTo(commitment);
            }
        }

        [TestFixture]
        public class WithTwoCommitmentVersions
        {
            List<CommitmentEntity> SetupCommitments(CommitmentEntity commitment1, CommitmentEntity commitment2)
            {
                commitment2.CommitmentId = commitment1.CommitmentId;

                commitment1.StartDate = new DateTime(2018, 09, 01);
                commitment1.EndDate = new DateTime(2020, 01, 01);
                commitment1.EffectiveFrom = new DateTime(2018, 09, 01);
                commitment1.EffectiveTo = new DateTime(2018, 11, 01);

                commitment2.StartDate = commitment1.StartDate;
                commitment2.EndDate = commitment1.EndDate;
                commitment2.EffectiveFrom = new DateTime(2018, 11, 02);
                commitment2.EffectiveTo = null;

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

                var actual = sut.CommitmentsForDate(new DateTime(2018, 08, 01));
                actual.Should().BeEmpty();
            }

            [Test, AutoMoqData]
            public void AndADateAfterTheFirstVersionAndBeforeTheSecondVersionReturnsTheFirstVersion(
                CommitmentEntity commitment1,
                CommitmentEntity commitment2,
                long uln
            )
            {
                var commitments = SetupCommitments(commitment1, commitment2);

                var sut = new LearnerCommitments(uln, commitments);

                var actual = sut.CommitmentsForDate(new DateTime(2018, 10, 01));
                actual.Should().AllBeEquivalentTo(commitment1);
                actual.Should().HaveCount(1);
            }

            [Test, AutoMoqData]
            public void AndADateAfterTheSecondVersionReturnsTheSecondVersion(
                CommitmentEntity commitment1,
                CommitmentEntity commitment2,
                long uln
            )
            {
                var commitments = SetupCommitments(commitment1, commitment2);

                var sut = new LearnerCommitments(uln, commitments);

                var actual = sut.CommitmentsForDate(new DateTime(2018, 12, 01));
                actual.Should().AllBeEquivalentTo(commitment2);
                actual.Should().HaveCount(1);
            }
        }

        [TestFixture]
        public class WithTwoCommitmentWithDifferentIds
        {
            List<CommitmentEntity> SetupCommitments(CommitmentEntity commitment1, CommitmentEntity commitment2)
            {
                commitment1.StartDate = new DateTime(2018, 09, 01);
                commitment1.WithdrawnOnDate = new DateTime(2018, 11, 01);
                commitment1.EndDate = commitment1.WithdrawnOnDate.Value;

                commitment2.StartDate = new DateTime(2018, 11, 02);
                commitment2.EndDate = new DateTime(2020, 01, 01);

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

                var actual = sut.CommitmentsForDate(new DateTime(2018, 08, 01));
                actual.Should().BeEmpty();
            }

            [Test, AutoMoqData]
            public void AndADateAfterTheFirstCommitmentAndBeforeTheSecondCommitmentReturnsTheFirstCommitment(
                CommitmentEntity commitment1,
                CommitmentEntity commitment2,
                long uln
            )
            {
                var commitments = SetupCommitments(commitment1, commitment2);

                var sut = new LearnerCommitments(uln, commitments);

                var actual = sut.CommitmentsForDate(new DateTime(2018, 10, 01));
                actual.Should().AllBeEquivalentTo(commitment1);
                actual.Should().HaveCount(1);
            }

            [Test, AutoMoqData]
            public void AndADateAfterTheSecondCommitmentReturnsTheSecondCommitment(
                CommitmentEntity commitment1,
                CommitmentEntity commitment2,
                long uln
            )
            {
                var commitments = SetupCommitments(commitment1, commitment2);

                var sut = new LearnerCommitments(uln, commitments);

                var actual = sut.CommitmentsForDate(new DateTime(2018, 12, 01));
                actual.Should().AllBeEquivalentTo(commitment2);
                actual.Should().HaveCount(1);
            }

            [TestFixture]
            public class AndTheCommitmentsHaveOverlappingDates
            {
                /// <summary>
                /// Commitment1: 2018-09-01 -> 2018-11-20
                /// Commitment2: 2018-11-10 -> 2020-01-01
                /// </summary>
                List<CommitmentEntity> SetupCommitments(CommitmentEntity commitment1, CommitmentEntity commitment2)
                {
                    commitment1.StartDate = new DateTime(2018, 09, 01);
                    commitment1.WithdrawnOnDate = new DateTime(2018, 11, 20);
                    commitment1.EndDate = commitment1.WithdrawnOnDate.Value;

                    commitment2.StartDate = new DateTime(2018, 11, 10);
                    commitment2.EndDate = new DateTime(2020, 01, 01);

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

                    var actual = sut.CommitmentsForDate(new DateTime(2018, 11, 15));
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

                    var actual = sut.CommitmentsForDate(new DateTime(2018, 09, 15));
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

                    var actual = sut.CommitmentsForDate(new DateTime(2018, 07, 15));
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

                    var actual = sut.CommitmentsForDate(new DateTime(2018, 12, 15));
                    actual.Should().AllBeEquivalentTo(commitment2);
                    actual.Should().HaveCount(1);
                }

                [Test, AutoMoqData]
                public void AndADateAfterTheTheEndDate(
                    CommitmentEntity commitment1,
                    CommitmentEntity commitment2,
                    long uln
                )
                {
                    var commitments = SetupCommitments(commitment1, commitment2);

                    var sut = new LearnerCommitments(uln, commitments);

                    var actual = sut.CommitmentsForDate(new DateTime(2020, 12, 15));
                    actual.Should().BeEmpty();
                }
            }

            [TestFixture]
            public class AndTheSecondCommitmentHavingTwoVersions
            {
                List<CommitmentEntity> SetupCommitments(
                    CommitmentEntity commitment1,
                    CommitmentEntity commitment2,
                    CommitmentEntity commitment3)
                {
                    commitment1.StartDate = new DateTime(2018, 09, 01);
                    commitment1.WithdrawnOnDate = new DateTime(2018, 11, 01);
                    commitment1.EndDate = commitment1.WithdrawnOnDate.Value;

                    commitment2.StartDate = new DateTime(2018, 11, 02);
                    commitment2.EndDate = new DateTime(2020, 01, 01);
                    commitment2.EffectiveFrom = new DateTime(2018, 11, 02);
                    commitment2.EffectiveTo = new DateTime(2019, 01, 01);

                    commitment3.CommitmentId = commitment2.CommitmentId;
                    commitment3.StartDate = commitment2.StartDate;
                    commitment3.EndDate = commitment2.EndDate;
                    commitment3.EffectiveFrom = new DateTime(2019, 01, 01);
                    commitment3.EffectiveTo = null;

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

                    var actual = sut.CommitmentsForDate(new DateTime(2018, 08, 01));
                    actual.Should().BeEmpty();
                }

                [Test, AutoMoqData]
                public void AndADateAfterTheFirstCommitmentAndBeforeTheSecondCommitmentReturnsTheFirstCommitment(
                    CommitmentEntity commitment1,
                    CommitmentEntity commitment2,
                    CommitmentEntity commitment3,
                    long uln
                )
                {
                    var commitments = SetupCommitments(commitment1, commitment2, commitment3);

                    var sut = new LearnerCommitments(uln, commitments);

                    var actual = sut.CommitmentsForDate(new DateTime(2018, 10, 01));
                    actual.Should().AllBeEquivalentTo(commitment1);
                    actual.Should().HaveCount(1);
                }

                [Test, AutoMoqData]
                public void AndADateAfterTheSecondCommitmentReturnsTheSecondCommitment(
                    CommitmentEntity commitment1,
                    CommitmentEntity commitment2,
                    CommitmentEntity commitment3,
                    long uln
                )
                {
                    var commitments = SetupCommitments(commitment1, commitment2, commitment3);

                    var sut = new LearnerCommitments(uln, commitments);

                    var actual = sut.CommitmentsForDate(new DateTime(2019, 03, 01));
                    actual.Should().AllBeEquivalentTo(commitment3);
                    actual.Should().HaveCount(1);
                }

                [TestFixture]
                public class AndTheDateIsForTheCommitmentWithTwoVersions : AndTheSecondCommitmentHavingTwoVersions
                {
                    [Test, AutoMoqData]
                    public void AndADateAfterTheFirstVersionAndBeforeTheSecondVersionReturnsTheFirstVersion(
                        CommitmentEntity commitment1,
                        CommitmentEntity commitment2,
                        CommitmentEntity commitment3,
                        long uln
                    )
                    {
                        var commitments = SetupCommitments(commitment1, commitment2, commitment3);

                        var sut = new LearnerCommitments(uln, commitments);

                        var actual = sut.CommitmentsForDate(new DateTime(2018, 12, 01));
                        actual.Should().AllBeEquivalentTo(commitment2);
                        actual.Should().HaveCount(1);
                    }

                    [Test, AutoMoqData]
                    public void AndADateAfterTheSecondVersionReturnsTheSecondVersion(
                        CommitmentEntity commitment1,
                        CommitmentEntity commitment2,
                        CommitmentEntity commitment3,
                        long uln
                    )
                    {
                        var commitments = SetupCommitments(commitment1, commitment2, commitment3);

                        var sut = new LearnerCommitments(uln, commitments);

                        var actual = sut.CommitmentsForDate(new DateTime(2019, 02, 01));
                        actual.Should().AllBeEquivalentTo(commitment3);
                        actual.Should().HaveCount(1);
                    }
                }
            }
        }
    }
}
