﻿using System.Collections.Generic;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Domain;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Services;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.UnitTests.Utilities;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.UnitTests.ServiceTests.GivenACalculator
{
    [TestFixture]
    public class ThatIsUsedForMultipleCalculations
    {
        [Test, PaymentsDueAutoData]
        public void CallingTwiceWithTheSameDataReturnsTheSameResults(
            PaymentsDueCalculationService sut,
            List<FundingDue> earnings)
        {
            var runOne = sut.Calculate(
                earnings,
                new HashSet<int>(), 
                new List<RequiredPayment>());

            var runTwo = sut.Calculate(
                earnings,
                new HashSet<int>(),
                new List<RequiredPayment>());

            runOne.Should().BeEquivalentTo(runTwo, config => config.Excluding(x => x.SelectedMemberInfo.Name == nameof(RequiredPayment.Id)));
        }
    }
}
