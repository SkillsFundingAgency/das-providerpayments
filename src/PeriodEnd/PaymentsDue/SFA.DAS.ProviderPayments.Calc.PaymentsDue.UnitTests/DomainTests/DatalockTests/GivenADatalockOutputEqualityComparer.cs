using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Domain;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data.Entities;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.UnitTests.Utilities;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.UnitTests.DomainTests.DatalockTests
{
    [TestFixture]
    public class GivenADatalockOutputEqualityComparer
    {
        [TestFixture]
        public class AndTwoObjectsAreIdentical
        {
            [Test, PaymentsDueAutoData]
            public void ThenEqualWillBeTrue(
                DatalockOutput sut)
            {
                var actual = sut.Equals(sut);

                actual.Should().BeTrue();
            }

            [Test, PaymentsDueAutoData]
            public void ThenEqualUsingObjectEqualsWillBeTrue(
                DatalockOutput sut)
            {
                var actual = sut.Equals((object)sut);

                actual.Should().BeTrue();
            }
        }

        [TestFixture]
        public class AndTwoObjectsHaveIdenticalComparableProperties
        {
            [Test, PaymentsDueAutoData]
            public void ThenEqualsWillBeTrue(
                DatalockOutputEntity seed)
            {
                var object1 = new DatalockOutput(seed, new Commitment());
                var object2 = new DatalockOutput(seed, new Commitment());

                var actual = object1.Equals(object2);

                actual.Should().BeTrue();
            }

            [Test, PaymentsDueAutoData]
            public void AndDifferentCommitmentVersionWillBeEqual(
                DatalockOutputEntity seed)
            {
                var object1 = new DatalockOutput(seed, new Commitment { CommitmentVersionId = "a" });
                var object2 = new DatalockOutput(seed, new Commitment { CommitmentVersionId = "b" });

                var actual = object1.Equals(object2);

                actual.Should().BeTrue();
            }

            [Test, PaymentsDueAutoData]
            public void AndDifferentAccountIdWillBeEqual(
                DatalockOutputEntity seed)
            {
                var object1 = new DatalockOutput(seed, new Commitment { AccountId = 100 });
                var object2 = new DatalockOutput(seed, new Commitment { AccountId = 200 });

                var actual = object1.Equals(object2);

                actual.Should().BeTrue();
            }

            [Test, PaymentsDueAutoData]
            public void AndDifferentAccountVersionWillBeEqual(
                DatalockOutputEntity seed)
            {
                var object1 = new DatalockOutput(seed, new Commitment { AccountVersionId = "a" });
                var object2 = new DatalockOutput(seed, new Commitment { AccountVersionId = "b" });

                var actual = object1.Equals(object2);

                actual.Should().BeTrue();
            }

            [Test, PaymentsDueAutoData]
            public void ThenEqualUsingShorthandWillBeTrue(
                DatalockOutputEntity seed)
            {
                var object1 = new DatalockOutput(seed, new Commitment());
                var object2 = new DatalockOutput(seed, new Commitment());

                var actual = object1 == object2;

                actual.Should().BeTrue();
            }
        }

        [TestFixture]
        public class AndTwoObjectsHaveDifferentProperties
        {
            [Test, PaymentsDueAutoData]
            public void ThenEqualsWillBeFalse(
                DatalockOutput object1,
                DatalockOutput object2
            )
            {
                var actual = object1.Equals(object2);

                actual.Should().BeFalse();
            }

            [Test, PaymentsDueAutoData]
            public void ThenEqualUsingShorthandWillBeFalse(
                DatalockOutput object1,
                DatalockOutput object2
            )
            {
                var actual = object1 == object2;

                actual.Should().BeFalse();
            }
        }

        [Test, PaymentsDueAutoData]
        public void ThenComparingADatalockOutputToNullWillNotBeEqual(
            DatalockOutput test)
        {
            var actual = test.Equals(null);

            actual.Should().BeFalse();
        }

        [Test, PaymentsDueAutoData]
        public void ThenComparingADatalockOutputToNullUsingOperatorOnTheLeftHandSideWillNotBeEqual(
            DatalockOutput test)
        {
            var actual = test == null;

            actual.Should().BeFalse();
        }

        [Test, PaymentsDueAutoData]
        public void ThenComparingADatalockOutputToNullUsingOperatorOnTheRightHandSideWillNotBeEqual(
            DatalockOutput test)
        {
            var actual = null == test;

            actual.Should().BeFalse();
        }

        [Test, PaymentsDueAutoData]
        public void ThenComparingADatalockOutputToNullUsingObjectCompareWillNotBeEqual(
            DatalockOutput test)
        {
            var actual = null == (object)test;

            actual.Should().BeFalse();
        }
    }
}
