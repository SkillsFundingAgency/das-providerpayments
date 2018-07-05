using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.ProviderPayments.Calc.Refunds.Services;
using SFA.DAS.ProviderPayments.Calc.Refunds.UnitTests.Utilities.Extensions;
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

        [TestFixture]
        public class AndThereIsOneRefund
        {
            [TestFixture]
            public class AndThePastPaymentsForRefundMonthDoNotCoverRefund
            {
                [TestFixture]
                public class AndThePaymentsForThePreviousMonthCoverTheRefund
                {
                    [Test]
                    [CreateMatchingRefundsAndPayments(paymentAmount:200)]
                    public void ThenTheRefundPaymentAmountMatchesTheRefundAmount(
                        List<RequiredPaymentEntity> refunds,
                        List<HistoricalPaymentEntity> payments,
                        LearnerRefundProcessor sut)
                    {
                        var refund = refunds.Latest();
                        var actual = sut.ProcessRefundsForLearner(new List<RequiredPaymentEntity>{refund}, payments);

                        var expectedAmount = refund.AmountDue;
                        actual.Sum(x => x.Amount).Should().Be(expectedAmount);
                    }
                }
            }
        }
    }

    [TestFixture]
    public class AndThereAreNoPreviousPayments
    {
        [Test]
        [CreateMatchingRefundsAndPayments(hasMatchingPastPayments: false)]
        public void ThenThereAreNoRefunds(
            List<RequiredPaymentEntity> refunds,
            List<HistoricalPaymentEntity> payments,
            LearnerRefundProcessor sut)
        {
            var actual = sut.ProcessRefundsForLearner(refunds, payments);

            actual.Should().HaveCount(0);
        }
    }
}
