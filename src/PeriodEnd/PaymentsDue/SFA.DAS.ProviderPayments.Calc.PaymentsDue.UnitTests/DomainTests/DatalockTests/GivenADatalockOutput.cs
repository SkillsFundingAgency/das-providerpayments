using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Domain;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data.Entities;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.UnitTests.Utilities;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.UnitTests.DomainTests.DatalockTests
{
    [TestFixture]
    public class GivenADatalockOutput
    {
        [Theory, PaymentsDueAutoData]
        public void ThenTwoIdenticalObjectsWillBeEqual(
            DatalockOutput sut)
        {
            var actual = sut.Equals(sut);

            actual.Should().BeTrue();
        }

        [Theory, PaymentsDueAutoData]
        public void ThenTwoIdenticalObjectsWillBeEqualUsingObject(
            DatalockOutput sut)
        {
            var actual = sut.Equals((object)sut);

            actual.Should().BeTrue();
        }

        [Theory, PaymentsDueAutoData]
        public void ThenTwoDifferentObjectsWithIdenticalPropertiesWillBeEqual(
            DatalockOutputEntity seed)
        {
            var object1 = new DatalockOutput(seed, new Commitment());
            var object2 = new DatalockOutput(seed, new Commitment());
            
            var actual = object1.Equals(object2);

            actual.Should().BeTrue();
        }

        [Theory, PaymentsDueAutoData]
        public void ThenTwoDifferentObjectsWithIdenticalPropertiesWillBeEqualUsingShorthand(
            DatalockOutputEntity seed)
        {
            var object1 = new DatalockOutput(seed, new Commitment());
            var object2 = new DatalockOutput(seed, new Commitment());

            var actual = object1 == object2;

            actual.Should().BeTrue();
        }

        [Theory, PaymentsDueAutoData]
        public void ThenTwoDifferentObjectsWithDifferentPropertiesWillNotBeEqual(
            DatalockOutput object1,
            DatalockOutput object2
            )
        {
            var actual = object1.Equals(object2);

            actual.Should().BeFalse();
        }

        [Theory, PaymentsDueAutoData]
        public void ThenTwoDifferentObjectsWithDifferentPropertiesWillNotBeEqualUsingShorthand(
            DatalockOutput object1,
            DatalockOutput object2
        )
        {
            var actual = object1 == object2;

            actual.Should().BeFalse();
        }

        [Theory, PaymentsDueAutoData]
        public void ComparingToNullWillNotBeEqual(
            DatalockOutput test)
        {
            var actual = test.Equals(null);

            actual.Should().BeFalse();
        }

        [Theory, PaymentsDueAutoData]
        public void ComparingWithNullWillNotBeEqual1(
            DatalockOutput test)
        {
            var actual = test == null;

            actual.Should().BeFalse();
        }

        [Theory, PaymentsDueAutoData]
        public void ComparingWithNullWillNotBeEqual2(
            DatalockOutput test)
        {
            var actual = null == test;

            actual.Should().BeFalse();
        }

        [Theory, PaymentsDueAutoData]
        public void ObjectComparingWithNullWillNotBeEqual2(
            DatalockOutput test)
        {
            var actual = null == (object)test;

            actual.Should().BeFalse();
        }
    }
}
