using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Domain;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.UnitTests.Utilities;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.UnitTests.DomainTests.MatchSetTests
{
    [TestFixture]
    public class GivenAMatchSet
    {
        [Test, PaymentsDueAutoData]
        public void ThenTwoIdenticalObjectsWillBeEqualUsingObject(
            PaymentGroup sut)
        {
            var actual = sut.Equals((object)sut);

            actual.Should().BeTrue();
        }

        [Test, PaymentsDueAutoData]
        public void ThenTwoIdenticalObjectsWillBeEqualUsingShortcut(
            PaymentGroup sut)
        {
            var actual = sut == sut;

            actual.Should().BeTrue();
        }

        [Test, PaymentsDueAutoData]
        public void ThenTwoDifferentObjectsWithIdenticalPropertiesWillBeEqualUsingShorthand(
            int standardCode,
            int frameworkCode,
            int programmeType,
            int pathwayCode,
            int apprenticeshipContractType,
            int transactionType,
            decimal sfaContributionPercentage,
            string learnAimRef,
            string fundingLineType,
            int deliveryYear,
            int deliveryMonth,
            long? accountId)
        {
            var object1 = new PaymentGroup
            (
                standardCode,
                frameworkCode,
                programmeType,
                pathwayCode,
                apprenticeshipContractType,
                transactionType,
                sfaContributionPercentage,
                learnAimRef,
                fundingLineType,
                deliveryYear,
                deliveryMonth,
                accountId);
            var object2 = new PaymentGroup
            (
                standardCode,
                frameworkCode,
                programmeType,
                pathwayCode,
                apprenticeshipContractType,
                transactionType,
                sfaContributionPercentage,
                learnAimRef,
                fundingLineType,
                deliveryYear,
                deliveryMonth,
                accountId);

            var actual = object1 == object2;

            actual.Should().BeTrue();
        }

        [Test, PaymentsDueAutoData]
        public void ThenTwoDifferentObjectsWithDifferentPropertiesWillNotBeEqualUsingShorthand(
            PaymentGroup object1,
            PaymentGroup object2
        )
        {
            var actual = object1 == object2;

            actual.Should().BeFalse();
        }

        [Test, PaymentsDueAutoData]
        public void ComparingToNullWillNotBeEqual(
            PaymentGroup test)
        {
            var actual = test.Equals(null);

            actual.Should().BeFalse();
        }

        [Test, PaymentsDueAutoData]
        public void ComparingWithNullWillNotBeEqual1(
            PaymentGroup test)
        {
            var actual = test == null;

            actual.Should().BeFalse();
        }

        [Test, PaymentsDueAutoData]
        public void ComparingWithNullWillNotBeEqual2(
            PaymentGroup test)
        {
            var actual = null == test;

            actual.Should().BeFalse();
        }

        [Test, PaymentsDueAutoData]
        public void ObjectComparingWithNullWillNotBeEqual2(
            PaymentGroup test)
        {
            var actual = null == (object)test;

            actual.Should().BeFalse();
        }


        [Test, PaymentsDueAutoData]
        public void ThenTwoIdenticalObjectsWillBeEqual(
            PaymentGroup sut)
        {
            var actual = sut.Equals(sut);

            actual.Should().BeTrue();
        }

        [Test, PaymentsDueAutoData]
        public void ThenTwoDifferentObjectsWithIdenticalPropertiesWillBeEqual(
            int standardCode,
            int frameworkCode,
            int programmeType,
            int pathwayCode,
            int apprenticeshipContractType,
            int transactionType,
            decimal sfaContributionPercentage,
            string learnAimRef,
            string fundingLineType,
            int deliveryYear,
            int deliveryMonth,
            long? accountId)
        {
            var object1 = new PaymentGroup
            (
                standardCode,
                frameworkCode,
                programmeType,
                pathwayCode,
                apprenticeshipContractType,
                transactionType,
                sfaContributionPercentage,
                learnAimRef,
                fundingLineType,
                deliveryYear,
                deliveryMonth,
                accountId);
            var object2 = new PaymentGroup
            (
                standardCode,
                frameworkCode,
                programmeType,
                pathwayCode,
                apprenticeshipContractType,
                transactionType,
                sfaContributionPercentage,
                learnAimRef,
                fundingLineType,
                deliveryYear,
                deliveryMonth,
                accountId);

            var actual = object1.Equals(object2);

            actual.Should().BeTrue();
        }

        [Test, PaymentsDueAutoData]
        public void ThenTwoDifferentObjectsWithIdenticalPropertiesWillBeEqualUsingShortcut(
            int standardCode,
            int frameworkCode,
            int programmeType,
            int pathwayCode,
            int apprenticeshipContractType,
            int transactionType,
            decimal sfaContributionPercentage,
            string learnAimRef,
            string fundingLineType,
            int deliveryYear,
            int deliveryMonth,
            long? accountId)
        {
            var object1 = new PaymentGroup
            (
                standardCode,
                frameworkCode,
                programmeType,
                pathwayCode,
                apprenticeshipContractType,
                transactionType,
                sfaContributionPercentage,
                learnAimRef,
                fundingLineType,
                deliveryYear,
                deliveryMonth,
                accountId);
            var object2 = new PaymentGroup
            (
                standardCode,
                frameworkCode,
                programmeType,
                pathwayCode,
                apprenticeshipContractType,
                transactionType,
                sfaContributionPercentage,
                learnAimRef,
                fundingLineType,
                deliveryYear,
                deliveryMonth,
                accountId);

            var actual = object1 == object2;

            actual.Should().BeTrue();
        }

        [Test, PaymentsDueAutoData]
        public void ThenTwoDifferentObjectsWithDifferentPropertiesWillNotBeEqual(
            PaymentGroup object1,
            PaymentGroup object2
            )
        {
            var actual = object1.Equals(object2);

            actual.Should().BeFalse();
        }
    }
}
