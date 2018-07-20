using AutoFixture;
using NUnit.Framework;
using NUnit.Framework.Interfaces;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.IntegrationTests.Utilities
{
    public class SetupUkprnAttribute : TestActionAttribute
    {
        public override ActionTargets Targets { get; } = ActionTargets.Suite;

        public override void BeforeTest(ITest test)
        {
            var fixture = new Fixture();

            PaymentsDueTestContext.Ukprn = fixture.Create<long>();

            base.BeforeTest(test);
        }
    }
}