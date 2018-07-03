using System;
using System.Collections.Generic;
using AutoFixture.NUnit3;
using Moq;
using NLog;
using NUnit.Framework;
using SFA.DAS.ProviderPayments.Calc.Refunds.Dto;
using SFA.DAS.ProviderPayments.Calc.Refunds.Services.Dependencies;
using SFA.DAS.ProviderPayments.Calc.Refunds.UnitTests.Utilities;
using SFA.DAS.ProviderPayments.Calc.Shared.Infrastructure.Data;
using SFA.DAS.ProviderPayments.Calc.Shared.Infrastructure.Data.Entities;

namespace SFA.DAS.ProviderPayments.Calc.Refunds.UnitTests
{
    [TestFixture]
    public class GivenARefundProcessor
    {
        [TestFixture]
        public class WhenCallingProcess
        {
            [Test, RefundsAutoData]
            public void ThenItGetsAllTheProviders(
                [Frozen] Mock<IProviderRepository> providerRepository,
                RefundsProcessor sut)
            {
                sut.Process();
                providerRepository.Verify(x => x.GetAllProviders(), Times.Once);
            }


            [Test, RefundsAutoData]
            public void ThenItProcessesEachProviderAddingLevyBalanceUpdatesToTheAccountBalance(
                List<ProviderEntity> providers,
                List<AccountLevyCredit>[] credits,
                [Frozen] Mock<IProviderRepository> providerRepository,
                [Frozen] Mock<IProviderProcessor> providerProcessor,
                [Frozen] Mock<ISummariseAccountBalances> summariseAccountBalances,
                RefundsProcessor sut
            )
            {
                providerRepository.Setup(x => x.GetAllProviders()).Returns(providers);

                for (var i = 0; i < providers.Count; i++)
                {
                    var providerCredits = credits[i];
                    var provider = providers[i];
                    providerProcessor.Setup(x => x.Process(provider)).Returns(providerCredits);
                }

                sut.Process();

                for (var i = 0; i < providers.Count; i++)
                {
                    var providerCredits = credits[i];
                    var provider = providers[i];
                    providerProcessor.Verify(x => x.Process(provider), Times.Once);
                    summariseAccountBalances.Verify(x => x.IncrementAccountLevyBalance(providerCredits), Times.Once);
                }
            }

            [Test, RefundsAutoData]
            public void ThenItSendsAllCreditsToTheDasAccountService(
                List<ProviderEntity> providers,
                List<AccountLevyCredit> credits,
                [Frozen] Mock<ISummariseAccountBalances> summariseAccountBalances,
                [Frozen] Mock<IDasAccountService> dasAccountService,
                RefundsProcessor sut
            )
            {
                summariseAccountBalances.Setup(x => x.AsList()).Returns(credits);

                sut.Process();

                dasAccountService.Verify(x => x.UpdateAccountLevyBalances(It.Is<List<AccountLevyCredit>>(p => p == credits)), Times.Once);
            }

            [Test, RefundsAutoData]
            public void ThenItCatchesAnUnhandledExceptionAndLogsIt(
                [Frozen] Mock<IProviderRepository> providerRepository,
                [Frozen] Mock<ILogger> logger,
                RefundsProcessor sut
            )
            {
                providerRepository.Setup(x => x.GetAllProviders()).Throws<KeyNotFoundException>();

                try
                {
                    sut.Process();
                    Assert.Fail();
                }
                catch (KeyNotFoundException)
                {
                    logger.Verify(x=>x.Error(It.IsAny<Exception>()));
                }
                catch(Exception)
                {
                    Assert.Fail();
                }

            }
        }


    }
}
