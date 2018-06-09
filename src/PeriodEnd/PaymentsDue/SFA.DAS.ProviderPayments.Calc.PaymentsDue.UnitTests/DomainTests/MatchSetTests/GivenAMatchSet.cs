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
            long? accountId,
            long? commitmentId)
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
                accountId,
                commitmentId);
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
                accountId,
                commitmentId);

            var actual = object1.Equals(object2);

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
