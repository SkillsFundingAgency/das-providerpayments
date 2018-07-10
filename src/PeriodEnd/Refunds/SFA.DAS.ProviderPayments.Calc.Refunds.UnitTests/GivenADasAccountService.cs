using System.Collections.Generic;
using AutoFixture.NUnit3;
using Moq;
using NUnit.Framework;
using SFA.DAS.ProviderPayments.Calc.Refunds.Dto;
using SFA.DAS.ProviderPayments.Calc.Refunds.Services;
using SFA.DAS.ProviderPayments.Calc.Refunds.UnitTests.Utilities;
using SFA.DAS.ProviderPayments.Calc.Shared.Infrastructure.Data;

namespace SFA.DAS.ProviderPayments.Calc.Refunds.UnitTests
{
    [TestFixture]
    public class GivenADasAccountService
    {
        [Test, RefundsAutoData]
        public void ThenItUpdatesEachDasAccount(
            List<AccountLevyCredit> credits,
            [Frozen]Mock<IDasAccountRepository> dasAccountRepository,
            DasAccountService sut
            )
        {
            sut.UpdateAccountLevyBalances(credits);

            credits.ForEach(credit =>
            {
                dasAccountRepository.Verify(repository => 
                    repository.AdjustBalance(credit.AccountId, credit.LevyCredit), Times.Once);
            });
        }
    }
}
