using System.Collections.Generic;
using AutoFixture.NUnit3;
using Moq;
using NUnit.Framework;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data.Entities;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Services;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Services.Dependencies;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.UnitTests.Utilities;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.UnitTests
{
    [TestFixture]
    public class GivenAPaymentsDueProcessor
    {
        [Test, PaymentsDueAutoData]
        public void ThenItGetsProvidersFromRepository(
            [Frozen] Mock<IProviderRepository> mockProviderRepository,
            PaymentsDueProcessor sut)
        {
            sut.Process();

            mockProviderRepository.Verify(repository => repository.GetAllProviders(), Times.Once);
        }

        [Test, PaymentsDueAutoData]
        public void ThenItCallsProviderProcessorForEachProvider(
            List<ProviderEntity> providers,
            [Frozen] Mock<IProviderRepository> mockProviderRepository,
            [Frozen] Mock<IProviderProcessor> mockProviderProcessor,
            PaymentsDueProcessor sut)
        {
            mockProviderRepository
                .Setup(repository => repository.GetAllProviders())
                .Returns(providers.ToArray());

            sut.Process();

            providers.ForEach(provider =>
                mockProviderProcessor.Verify(processor =>
                    processor.Process(provider), Times.Once));
        }
    }
}