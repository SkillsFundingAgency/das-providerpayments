using System.Collections.Generic;
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

        }
    }
}
