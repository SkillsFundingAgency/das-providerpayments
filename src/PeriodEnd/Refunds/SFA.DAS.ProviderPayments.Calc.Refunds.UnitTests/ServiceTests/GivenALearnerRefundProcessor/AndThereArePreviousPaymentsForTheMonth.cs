using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.ProviderPayments.Calc.Refunds.Services;
using SFA.DAS.ProviderPayments.Calc.Refunds.UnitTests.Utilities.TestHelpers;
using SFA.DAS.ProviderPayments.Calc.Shared.Infrastructure.Data.Entities;

namespace SFA.DAS.ProviderPayments.Calc.Refunds.UnitTests.ServiceTests.GivenALearnerRefundProcessor
{
    [TestFixture]
    public class AndThereArePreviousPaymentsForTheMonth
    {
        [TestFixture]
        public class AndThePaymentsAreSuffientToCoverTheRefund
        {
            [Test]
            [CreateMatchingRefundsAndPayments]
            public void ThenThereAreRefundPaymentsForAllPastPayments(
                List<RequiredPaymentEntity> refunds, 
                List<HistoricalPaymentEntity> payments,
                LearnerRefundProcessor sut)
            {
                var actual = sut.ProcessRefundsForLearner(refunds, payments);

                actual.Should().HaveCount(9);
            }
        }

        [TestFixture]
        public class AndThePaymentsAreNotSufficientToCoverTheRefund
        {
            [Test]
            [CreateMatchingRefundsAndPayments(hasMatchingPastPayments: false)]
            public void ThenTheRefundsAreForThePaymentAmount(
                List<RequiredPaymentEntity> refunds,
                List<HistoricalPaymentEntity> payments,
                LearnerRefundProcessor sut)
            {
                var actual = sut.ProcessRefundsForLearner(refunds, payments);

                var expectedAmount = payments.Sum(x => x.Amount);
                actual.Sum(x => x.Amount).Should().Be(expectedAmount);
            }
        }

        [TestFixture]
        public class AndThereAreNetNegativeFundingSources
        {
            [Test]
            [CreateMatchingRefundsAndPayments(hasNegativeFundingSources: true)]
            public void ThenThereAreNoRefundsForPaymentsThatHaveNegativeFundingSources(
                List<RequiredPaymentEntity> refunds,
                List<HistoricalPaymentEntity> payments,
                LearnerRefundProcessor sut)
            {
                var actual = sut.ProcessRefundsForLearner(refunds, payments);

                actual.Should().HaveCount(0);
            }
        }
    }
}
