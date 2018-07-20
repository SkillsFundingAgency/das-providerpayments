using AutoFixture;
using NUnit.Framework;
using NUnit.Framework.Interfaces;

namespace SFA.DAS.ProviderPayments.Calc.Refunds.IntegrationTests.Attributes
{
    public class SetupUkprnAttribute : TestActionAttribute
    {
        public override ActionTargets Targets { get; } = ActionTargets.Suite;

        public override void BeforeTest(ITest test)
        {
            var fixture = new Fixture();

            RefundsTestContext.Ukprn = fixture.Create<long>();

            base.BeforeTest(test);
        }
    }
}