using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.CollectionEarnings.DataLock.Domain;
using SFA.DAS.CollectionEarnings.DataLock.UnitTests.Utilities.Attributes;
using SFA.DAS.ProviderPayments.Calc.Shared.Infrastructure.Data.Entities;

namespace SFA.DAS.CollectionEarnings.DataLock.UnitTests.Tests.DomainTests.GivenAProviderEarnings
{
    [TestFixture]
    public class WhenCallingEarningsForLearner
    {
        public static void AssociateLearnerWithEarnings(List<RawEarning> earnings, long uln)
        {
            foreach (var earning in earnings)
            {
                earning.Uln = uln;
            }
        }

        [TestFixture]
        public class WithOneLearner
        {
            [TestFixture]
            public class WhenCallingWithLearnerUln
            {
                [Test, AutoMoqData]
                public void OnlyTheSpecifiedLearnersEarningsAreReturned(
                    List<RawEarning> earnings,
                    long uln
                    )
                {
                    AssociateLearnerWithEarnings(earnings, uln);
                    var sut = new ProviderEarnings(earnings);

                    var actual = sut.EarningsForLearner(uln);

                    actual.Should().BeEquivalentTo(earnings);
                }
            }

            [TestFixture]
            public class WhenCallingWithUnknownUln
            {
                [Test, AutoMoqData]
                public void AnEmptyListIsReturned(
                    List<RawEarning> earnings,
                    long uln1,
                    long uln2
                )
                {
                    AssociateLearnerWithEarnings(earnings, uln1);
                    var sut = new ProviderEarnings(earnings);

                    var actual = sut.EarningsForLearner(uln2);

                    actual.Should().BeEmpty();
                }
            }
        }

        [TestFixture]
        public class WithThreeLearners
        {
            [Test, AutoMoqData]
            public void OnlyTheSpecifiedLearnersEarningsAreReturned(
                List<RawEarning> earningsForLearner1,
                List<RawEarning> earningsForLearner2,
                List<RawEarning> earningsForLearner3,
                long ulnForLearner1,
                long ulnForLearner2,
                long ulnForLearner3
            )
            {
                AssociateLearnerWithEarnings(earningsForLearner1, ulnForLearner1);
                AssociateLearnerWithEarnings(earningsForLearner2, ulnForLearner2);
                AssociateLearnerWithEarnings(earningsForLearner3, ulnForLearner3);
                var sut = new ProviderEarnings(earningsForLearner1.Union(earningsForLearner2).Union(earningsForLearner3));

                var actual = sut.EarningsForLearner(ulnForLearner1);

                actual.Should().BeEquivalentTo(earningsForLearner1);
                actual.Should().NotBeEquivalentTo(earningsForLearner2);
                actual.Should().NotBeEquivalentTo(earningsForLearner3);
            }
        }
    }
}
