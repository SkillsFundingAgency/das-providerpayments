using NUnit.Framework;
using NUnit.Framework.Interfaces;
using SFA.DAS.ProviderPayments.Calc.Shared.IntegrationTests.Helpers;

namespace SFA.DAS.ProviderPayments.Calc.Shared.IntegrationTests.Attributes
{
    public class SetupProviderRepositoryAttribute : TestActionAttribute
    {
        private readonly int _numberToGenerate;

        public SetupProviderRepositoryAttribute(int numberToGenerate)
        {
            _numberToGenerate = numberToGenerate;
        }
        public override ActionTargets Targets { get; } = ActionTargets.Default;

        public override void BeforeTest(ITest test)
        {
            TestDataHelper.Execute("TRUNCATE TABLE Reference.Providers");
            for (int i = 1; i <= _numberToGenerate; i++)
            {
                TestDataHelper.Execute($"INSERT INTO Reference.Providers (UKPRN, IlrSubmissionDateTime) VALUES ({i}, '2018-01-01')");
            }
            base.BeforeTest(test);
        }
    }
}