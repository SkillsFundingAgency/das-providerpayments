using AutoFixture;
using NUnit.Framework;
using NUnit.Framework.Interfaces;

namespace SFA.DAS.ProviderPayments.Calc.Shared.IntegrationTests.Attributes
{
    public class SetupAccountIdAttribute : TestActionAttribute
    {
        public override ActionTargets Targets { get; } = ActionTargets.Suite;

        public override void BeforeTest(ITest test)
        {
            var fixture = new Fixture();

            SharedTestContext.AccountId = fixture.Create<long>();

            base.BeforeTest(test);
        }
    }
}