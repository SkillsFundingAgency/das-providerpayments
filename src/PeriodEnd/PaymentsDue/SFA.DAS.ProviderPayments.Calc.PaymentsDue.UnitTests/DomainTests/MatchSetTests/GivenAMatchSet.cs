using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Domain;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.UnitTests.Utilities;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.UnitTests.DomainTests.MatchSetTests
{
    [TestFixture]
    public class GivenAMatchSet
    {
        [Theory, PaymentsDueAutoData]
        public void ThenTwoIdenticalObjectsWillBeEqualUsingObject(
            MatchSetForPayments sut)
        {
            var actual = sut.Equals((object)sut);

            actual.Should().BeTrue();
        }

        [Theory, PaymentsDueAutoData]
        public void ThenTwoIdenticalObjectsWillBeEqualUsingShortcut(
            MatchSetForPayments sut)
        {
            var actual = sut == sut;

            actual.Should().BeTrue();
        }

        [Theory, PaymentsDueAutoData]
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
            var object1 = new MatchSetForPayments
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
            var object2 = new MatchSetForPayments
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

        [Theory, PaymentsDueAutoData]
        public void ThenTwoDifferentObjectsWithDifferentPropertiesWillNotBeEqualUsingShorthand(
            MatchSetForPayments object1,
            MatchSetForPayments object2
        )
        {
            var actual = object1 == object2;

            actual.Should().BeFalse();
        }

        [Theory, PaymentsDueAutoData]
        public void ComparingToNullWillNotBeEqual(
            MatchSetForPayments test)
        {
            var actual = test.Equals(null);

            actual.Should().BeFalse();
        }

        [Theory, PaymentsDueAutoData]
        public void ComparingWithNullWillNotBeEqual1(
            MatchSetForPayments test)
        {
            var actual = test == null;

            actual.Should().BeFalse();
        }

        [Theory, PaymentsDueAutoData]
        public void ComparingWithNullWillNotBeEqual2(
            MatchSetForPayments test)
        {
            var actual = null == test;

            actual.Should().BeFalse();
        }

        [Theory, PaymentsDueAutoData]
        public void ObjectComparingWithNullWillNotBeEqual2(
            MatchSetForPayments test)
        {
            var actual = null == (object)test;

            actual.Should().BeFalse();
        }


        [Theory, PaymentsDueAutoData]
        public void ThenTwoIdenticalObjectsWillBeEqual(
            MatchSetForPayments sut)
        {
            var actual = sut.Equals(sut);

            actual.Should().BeTrue();
        }

        [Theory, PaymentsDueAutoData]
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
            var object1 = new MatchSetForPayments
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
            var object2 = new MatchSetForPayments
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

        [Theory, PaymentsDueAutoData]
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
            var object1 = new MatchSetForPayments
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
            var object2 = new MatchSetForPayments
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

        [Theory, PaymentsDueAutoData]
        public void ThenTwoDifferentObjectsWithDifferentPropertiesWillNotBeEqual(
            MatchSetForPayments object1,
            MatchSetForPayments object2
            )
        {
            var actual = object1.Equals(object2);

            actual.Should().BeFalse();
        }
    }
}
