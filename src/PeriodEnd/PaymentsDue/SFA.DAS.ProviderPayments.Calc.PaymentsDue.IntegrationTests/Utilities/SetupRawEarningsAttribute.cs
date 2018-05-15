using System;
using System.Linq;
using System.Runtime.Remoting.Messaging;
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
            RawEarningsDataHelper.Truncate();

            var fixture = new Fixture();

            fixture.Build<decimal>();
            var earnings = fixture.Build<RawEarning>()
                .CreateMany(3)
                .ToList();

            foreach (var rawEarning in earnings)
            {
                RawEarningsDataHelper.CreateRawEarning(rawEarning);
            }

            PaymentsDueTestContext.RawEarnings = earnings;
            
            base.BeforeTest(test);
        }
    }
}