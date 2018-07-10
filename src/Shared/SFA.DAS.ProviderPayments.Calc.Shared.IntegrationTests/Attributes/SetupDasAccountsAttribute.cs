using System.Linq;
using AutoFixture;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using SFA.DAS.ProviderPayments.Calc.Shared.Infrastructure.Data.Entities;
using SFA.DAS.ProviderPayments.Calc.Shared.IntegrationTests.Helpers;
using SFA.DAS.ProviderPayments.Calc.Shared.IntegrationTests.SpecimenBuilders;

namespace SFA.DAS.ProviderPayments.Calc.Shared.IntegrationTests.Attributes
{
    public class SetupDasAccountsAttribute : TestActionAttribute
    {
        public override ActionTargets Targets { get; } = ActionTargets.Suite;

        public override void BeforeTest(ITest test)
        {
            DasAccountDataHelper.Truncate();

            var fixture = new Fixture();
            fixture.Customizations.Add(new UnknownAccountIdBuilder());

            var dasAccounts = fixture.Build<DasAccountEntity>()
                .CreateMany(3)
                .ToList();

            var accountMatchingAccountId = fixture.Build<DasAccountEntity>()
                .With(account => account.AccountId, SharedTestContext.AccountId)
                .Create();

            dasAccounts.Add(accountMatchingAccountId);

            foreach (var dasAccount in dasAccounts)
            {
                DasAccountDataHelper.Create(dasAccount);
            }

            SharedTestContext.DasAccounts = dasAccounts;
            
            base.BeforeTest(test);
        }
    }
}