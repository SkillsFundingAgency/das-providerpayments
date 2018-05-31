using System.Collections.Generic;
using System.Linq;
using AutoFixture.NUnit3;
using FluentAssertions;
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

        [Test, PaymentsDueAutoData, Ignore("for now")]
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

        [Test, PaymentsDueAutoData, Ignore("for now")]
        public void ThenItSavesRefunds(
            ProviderEntity provider,
            List<PayableEarning> payableEarnings,
            List<RequiredPaymentsHistoryEntity> historicalPayments,
            [Frozen] Mock<IProviderRepository> mockProviderRepository,
            [Frozen] Mock<IPayableEarningsCalculator> mockPayableEarningsCalculator,
            [Frozen] Mock<IRequiredPaymentsHistoryRepository> mockHistoricalPaymentsRepository,
            [Frozen] Mock<IRequiredPaymentRepository> mockRequiredPaymentsRepository,
            PaymentsDueProcessorV2 sut)
        {
            RequiredPaymentEntity[] actualRequiredPayments = {};
            RequiredPaymentEntity[] expectedRequiredPayments = {};

            payableEarnings.ForEach(earning =>
            {
                earning.Ukprn = provider.Ukprn;
                earning.Uln = 4;//todo get from comparer poco
            });

            mockProviderRepository
                .Setup(repository => repository.GetAllProviders())
                .Returns(new[] {provider});

            mockPayableEarningsCalculator
                .Setup(calculator => calculator.Calculate(It.IsAny<long>()))
                .Returns(payableEarnings);

            mockHistoricalPaymentsRepository
                .Setup(repository => repository.GetAllForProvider(It.IsAny<long>()))
                .Returns(historicalPayments);

            mockRequiredPaymentsRepository
                .Setup(repository => repository.AddRequiredPayments(It.IsAny<RequiredPaymentEntity[]>()))
                .Callback((RequiredPaymentEntity[] actualPaymentEntities) =>
                {
                    actualRequiredPayments = actualPaymentEntities;
                });

            sut.Process();

            actualRequiredPayments.ShouldAllBeEquivalentTo(expectedRequiredPayments);
        }
    }
}