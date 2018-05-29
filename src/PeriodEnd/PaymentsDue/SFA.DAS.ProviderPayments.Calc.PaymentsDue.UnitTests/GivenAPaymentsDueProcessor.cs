using System.Collections.Generic;
using System.Linq;
using AutoFixture.NUnit3;
using Moq;
using NUnit.Framework;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data.Entities;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data.Repositories;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.UnitTests.Utilities;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.UnitTests
{
    [TestFixture]
    public class GivenAPaymentsDueProcessor
    {
        [Test, PaymentsDueAutoData]
        public void ThenItGetsProvidersFromRepository(
            [Frozen] Mock<IProviderRepository> mockProviderRepository,
            PaymentsDueProcessorV2 sut)
        {
            sut.Process();

            mockProviderRepository.Verify(repository => repository.GetAllProviders(), Times.Once);
        }

        [Test, PaymentsDueAutoData]
        public void ThenItGetsPayableEarningsForEachProvider(
            List<ProviderEntity> providers,
            [Frozen] Mock<IProviderRepository> mockProviderRepository,
            [Frozen] Mock<IPayableEarningsCalculator> mockPayableEarningsCalculator,
            PaymentsDueProcessorV2 sut)
        {
            mockProviderRepository
                .Setup(repository => repository.GetAllProviders())
                .Returns(providers.ToArray());

            sut.Process();

            mockPayableEarningsCalculator
                .Verify(calculator => calculator.Calculate(It.IsIn(providers.Select(entity => entity.Ukprn))),
                Times.Exactly(providers.Count));
        }

        [Test, PaymentsDueAutoData]
        public void ThenItGetsHistoricalPaymentsForEachProvider(
            List<ProviderEntity> providers,
            [Frozen] Mock<IProviderRepository> mockProviderRepository,
            [Frozen] Mock<IRequiredPaymentsHistoryRepository> mockHistoricalPaymentsRepository,
            PaymentsDueProcessorV2 sut)
        {
            mockProviderRepository
                .Setup(repository => repository.GetAllProviders())
                .Returns(providers.ToArray());

            sut.Process();

            mockHistoricalPaymentsRepository
                .Verify(repository => repository.GetAllForProvider(It.IsIn(providers.Select(entity => entity.Ukprn))),
                    Times.Exactly(providers.Count));
        }
    }
}