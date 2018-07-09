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
        public void ThenItProcessesEachCredit(
            List<AccountLevyCredit> credits,
            [Frozen]Mock<IDasAccountRepository> dasAccountRepository,
            DasAccountService sut
            )
        {
            sut.UpdateAccountLevyBalances(credits);
            dasAccountRepository.Verify(x=>x.UpdateBalance(It.IsAny<long>(), It.IsAny<decimal>()), Times.Exactly(3));
        }

        [Test, RefundsAutoData]
        public void ThenItProcessesEachCredit(
            AccountLevyCredit credit,
            [Frozen]Mock<IDasAccountRepository> dasAccountRepository,
            DasAccountService sut
        )
        {
            sut.UpdateAccountLevyBalances(new List<AccountLevyCredit>{credit});
            dasAccountRepository.Verify(x => x.UpdateBalance(credit.AccountId, credit.LevyCredit), Times.Once);
        }
    }
}
