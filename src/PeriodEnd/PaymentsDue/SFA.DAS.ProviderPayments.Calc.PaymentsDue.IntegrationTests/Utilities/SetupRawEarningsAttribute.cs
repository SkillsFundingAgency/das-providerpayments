using System;
using System.Linq;
using AutoFixture;
using NUnit.Framework;
using NUnit.Framework.Interfaces;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.IntegrationTests.Utilities
{
    public class SetupRawEarningsAttribute : TestActionAttribute
    {
        public override ActionTargets Targets { get; } = ActionTargets.Suite;

        public override void BeforeTest(ITest test)
        {
            var fixture = new Fixture();
            var earnings = fixture.Build<RawEarning>()
                .CreateMany(3)
                .ToList();

            foreach (var rawEarning in earnings)
            {
                RawEarningsDataHelper.CreateRawEarning(rawEarning);
            }

            var expectedRawEarnings = test.Fixture as IExpectedRawEarnings;
            if (expectedRawEarnings != null)
            {
                expectedRawEarnings.RawEarnings = earnings;
            }
            else
            {
                throw new InvalidOperationException($"{nameof(SetupRawEarningsAttribute)} can only decorate types of {nameof(IExpectedRawEarnings)}.");
            }

            base.BeforeTest(test);
        }

        public override void AfterTest(ITest test)
        {
            RawEarningsDataHelper.Truncate();
            base.AfterTest(test);
        }
    }
}