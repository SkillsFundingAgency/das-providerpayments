using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Domain;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data.Entities;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Services;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.UnitTests.Utilities;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.UnitTests.ServiceTests.GivenACalculator
{
    [TestFixture]
    public class WithValidPayments
    {
        [Test, PaymentsDueAutoData]
        public void ThenThePaymentCommitmentVersionIdShouldMatchTheEarning(
            FundingDue earning,
            PaymentsDueCalculationService sut
        )
        {
            var earnings = new List<FundingDue> {earning};
            var actual = sut.Calculate(earnings, new List<int>(), new List<RequiredPaymentEntity>());

            actual.First().CommitmentVersionId.Should().Be(earning.CommitmentVersionId);
        }
    }
}
