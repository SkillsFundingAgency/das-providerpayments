using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.CollectionEarnings.DataLock.Domain;
using SFA.DAS.CollectionEarnings.DataLock.Exceptions;
using SFA.DAS.CollectionEarnings.DataLock.Infrastructure.Data.Entities;
using SFA.DAS.CollectionEarnings.DataLock.UnitTests.Utilities.Attributes;

namespace SFA.DAS.CollectionEarnings.DataLock.UnitTests.Tests.DomainTests.GivenAProviderCommitments
{
    [TestFixture]
    public class WhenCallingCommitmentsForLearner
    {
        [TestFixture]
        public class WithNoLearners
        {
            [Test]
            public void ThrowsLearnerNotFoundException()
            {
                var sut = new ProviderCommitments(new List<CommitmentEntity>());

                var actual =  sut.Invoking(_ => _.CommitmentsForLearner(0));

                actual.Should().Throw<LearnerNotFoundException>();
            }
        }

        static void SetCommtmentDates(List<CommitmentEntity> commitments)
        {
            foreach (var commitment in commitments)
            {
                commitment.StartDate = new DateTime(2017, 08, 01);
                commitment.EndDate = new DateTime(2020, 01, 01);
                commitment.WithdrawnOnDate = null;
                commitment.PaymentStatus = 1;
            }
        }

        static void SetCommtmentDates(CommitmentEntity commitment)
        {
            SetCommtmentDates(new List<CommitmentEntity>{commitment});
        }

        static void AssignCommitmentToLearner(List<CommitmentEntity> commitments, long uln)
        {
            foreach (var commitment in commitments)
            {
                commitment.Uln = uln;
            }
        }

        [TestFixture]
        public class WithOneLearner
        {
            [TestFixture]
            public class AndOneCommitment
            {
                [TestFixture]
                public class CalledWithTheLearnersUln
                {
                    [Test, AutoMoqData]
                    public void ReturnsTheCommitment(
                        CommitmentEntity commitment,
                        long uln)
                    {
                        commitment.Uln = uln;
                        SetCommtmentDates(commitment);

                        var sut = new ProviderCommitments(new List<CommitmentEntity>{commitment});

                        var learner = sut.CommitmentsForLearner(uln);

                        var actual = learner.ActiveCommitmentsForDate(new DateTime(2018, 01, 01));
                        actual.Should().AllBeEquivalentTo(commitment);
                    }
                }

                [TestFixture]
                public class CalledWithADifferentUln
                {
                    [Test, AutoMoqData]
                    public void DoesNotReturnTheCommitment(
                        CommitmentEntity commitment,
                        long testUln,
                        long otherUln)
                    {
                        commitment.Uln = testUln;
                        
                        var sut = new ProviderCommitments(new List<CommitmentEntity>());

                        var actual = sut.Invoking(_ => _.CommitmentsForLearner(otherUln));

                        actual.Should().Throw<LearnerNotFoundException>();
                    }
                }
            }

            public class AndThreeCommitments
            {
                [TestFixture]
                public class CalledWithTheLearnersUln
                {
                    [Test, AutoMoqData]
                    public void ReturnsAllCommitments(
                        List<CommitmentEntity> commitments,
                        long uln)
                    {
                        AssignCommitmentToLearner(commitments, uln);
                        SetCommtmentDates(commitments);

                        var sut = new ProviderCommitments(commitments);

                        var learner = sut.CommitmentsForLearner(uln);

                        var actual = learner.ActiveCommitmentsForDate(new DateTime(2018, 01, 01));
                        actual.Should().BeEquivalentTo(commitments);
                    }
                }
            }
        }

        [TestFixture]
        public class WithThreeLearners
        {
            public class AndThreeCommitmentsPerLearner
            {
                [TestFixture]
                public class CalledWithTheLearnersUln
                {
                    [Test, AutoMoqData]
                    public void ReturnsAllCommitments(
                        List<CommitmentEntity> commitmentsForLearner1,
                        List<CommitmentEntity> commitmentsForLearner2,
                        List<CommitmentEntity> commitmentsForLearner3,
                        long learner1Uln,
                        long learner2Uln,
                        long learner3Uln
                    )
                    {
                        AssignCommitmentToLearner(commitmentsForLearner1, learner1Uln);
                        SetCommtmentDates(commitmentsForLearner1);
                        AssignCommitmentToLearner(commitmentsForLearner2, learner2Uln);
                        SetCommtmentDates(commitmentsForLearner2);
                        AssignCommitmentToLearner(commitmentsForLearner3, learner3Uln);
                        SetCommtmentDates(commitmentsForLearner3);

                        var sut = new ProviderCommitments(commitmentsForLearner1.Union(commitmentsForLearner2).Union(commitmentsForLearner3));

                        var learner = sut.CommitmentsForLearner(learner1Uln);

                        var actual = learner.ActiveCommitmentsForDate(new DateTime(2018, 01, 01));
                        actual.Should().BeEquivalentTo(commitmentsForLearner1);
                        actual.Should().NotBeEquivalentTo(commitmentsForLearner2);
                        actual.Should().NotBeEquivalentTo(commitmentsForLearner3);
                    }
                }
            }
        }
    }
}
