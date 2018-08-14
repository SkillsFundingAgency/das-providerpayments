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
    public class WhenCallingAllUlns
    {
        public static void AssociateLearnerWithEarnings(List<RawEarning> earnings, long uln)
        {
            foreach (var earning in earnings)
            {
                earning.Uln = uln;
            }
        }


        [TestFixture]
        public class WithNoLearners
        {
            [Test]
            public void ThenAnEmptyListIsReturned()
            {
                var sut = new ProviderEarnings(new List<RawEarning>());

                var actual = sut.AllUlns();

                actual.Should().BeEmpty();
            }
        }

        [TestFixture]
        public class WithOneLearner
        {
            [Test, AutoMoqData]
            public void ThenAListWithThatLearnersUlnIsReturned(
                    List<RawEarning> earnings,
                    long uln
                )
            {
                AssociateLearnerWithEarnings(earnings, uln);
                var sut = new ProviderEarnings(earnings);

                var actual = sut.AllUlns().ToList();

                actual.Should().Contain(uln);
                actual.Should().HaveCount(1);
            }
        }

        [TestFixture]
        public class WithMultipleLearners
        {
            [Test, AutoMoqData]
            public void ThenAListWithThatLearnersUlnIsReturned(
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

                var actual = sut.AllUlns().ToList();

                actual.Should().Contain(ulnForLearner1);
                actual.Should().Contain(ulnForLearner2);
                actual.Should().Contain(ulnForLearner3);
                actual.Should().HaveCount(3);
            }
        }
    }
}
