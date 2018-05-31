using System.Collections.Generic;
using AutoFixture.NUnit3;
using Moq;
using NUnit.Framework;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data.Repositories;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.UnitTests.Utilities;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.UnitTests
{
    [TestFixture]
    public class GivenAProviderLearnersBuilder
    {
        [Test, PaymentsDueAutoData]
        public void ThenItGetsRawEarningsFromRepository(
            long ukprn,
            [Frozen] Mock<IRawEarningsRepository> mockRawEarningsRepository,
            ProviderLearnersBuilder sut)
        {
            sut.Build(ukprn);

            mockRawEarningsRepository.Verify(repository => repository.GetAllForProvider(ukprn), Times.Once);
        }

        [Test, PaymentsDueAutoData]
        public void ThenItGetsRawEarningsMathsEnglishFromRepository(
            long ukprn,
            [Frozen] Mock<IRawEarningsMathsEnglishRepository> mockRawEarningsMathsEnglishRepository,
            ProviderLearnersBuilder sut)
        {
            sut.Build(ukprn);

            mockRawEarningsMathsEnglishRepository.Verify(repository => repository.GetAllForProvider(ukprn), Times.Once);
        }
    }

    public class ProviderLearnersBuilder
    {
        private readonly IRawEarningsRepository _rawEarningsRepository;
        private readonly IRawEarningsMathsEnglishRepository _rawEarningsMathsEnglishRepository;

        public ProviderLearnersBuilder(IRawEarningsRepository rawEarningsRepository, IRawEarningsMathsEnglishRepository rawEarningsMathsEnglishRepository)
        {
            _rawEarningsRepository = rawEarningsRepository;
            _rawEarningsMathsEnglishRepository = rawEarningsMathsEnglishRepository;
        }

        public Dictionary<string, Learner> Build(long ukprn)
        {
             
            _rawEarningsRepository.GetAllForProvider(ukprn);
            _rawEarningsMathsEnglishRepository.GetAllForProvider(ukprn);
            return null;
        }
    }
}