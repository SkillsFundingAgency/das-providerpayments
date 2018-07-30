using System.Linq;
using AutoFixture;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using SFA.DAS.ProviderPayments.Calc.Shared.Infrastructure.Data.Entities;
using SFA.DAS.ProviderPayments.Calc.Shared.IntegrationTests.Helpers;

namespace SFA.DAS.ProviderPayments.Calc.Shared.IntegrationTests.Attributes
{
    public class SetupCommitmentsAttribute : TestActionAttribute
    {
        public override ActionTargets Targets { get; } = ActionTargets.Suite;

        public override void BeforeTest(ITest test)
        {
            CommitmentsDataHelper.Truncate();

            var fixture = new Fixture();
            
            var commitments = fixture.Build<CommitmentEntity>()
                .CreateMany(3)
                .ToList();

            var accountMatchingAccountId = fixture.Build<CommitmentEntity>()
                .With(account => account.AccountId, SharedTestContext.AccountId)
                .With(x => x.Ukprn, SharedTestContext.Ukprn)
                .Create();

            commitments.Add(accountMatchingAccountId);

            foreach (var commitment in commitments)
            {
                CommitmentsDataHelper.Create(commitment);
            }

            SharedTestContext.Commitments = commitments;
            
            base.BeforeTest(test);
        }
    }
}