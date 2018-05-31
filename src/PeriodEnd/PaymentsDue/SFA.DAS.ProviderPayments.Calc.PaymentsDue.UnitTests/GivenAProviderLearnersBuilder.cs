using System.Collections.Generic;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data.Entities;
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

        [Test, PaymentsDueAutoData]
        public void ThenItGetsHistoricalPaymentsFromRepository(
            long ukprn,
            [Frozen] Mock<IRequiredPaymentsHistoryRepository> mockHistoricalPaymentsRepository,
            ProviderLearnersBuilder sut)
        {
            sut.Build(ukprn);

            mockHistoricalPaymentsRepository.Verify(repository => repository.GetAllForProvider(ukprn), Times.Once);
        }

        [Test, PaymentsDueAutoData]
        public void ThenItGetsDataLocksFromRepository(
            long ukprn,
            [Frozen] Mock<IDataLockPriceEpisodePeriodMatchesRepository> mockDataLockRepository,
            ProviderLearnersBuilder sut)
        {
            sut.Build(ukprn);

            mockDataLockRepository.Verify(repository => repository.GetAllForProvider(ukprn), Times.Once);
        }

        [Test, PaymentsDueAutoData]
        public void ThenItCreatesASingleNewLearnerForAllRawEarningsIfLearnerNotAlreadyExists(
            long ukprn,
            string learnRefNumber,
            List<RawEarningEntity> rawEarnings,
            [Frozen] Mock<IRawEarningsRepository> mockRawEarningsRepository,
            ProviderLearnersBuilder sut)
        {
            rawEarnings.ForEach(entity => entity.LearnRefNumber = learnRefNumber);

            mockRawEarningsRepository
                .Setup(repository => repository.GetAllForProvider(ukprn))
                .Returns(rawEarnings);

            var learners = sut.Build(ukprn);

            learners[learnRefNumber].RawEarningEntities.Count
                .Should().Be(rawEarnings.Count);
        }
    }

    public class ProviderLearnersBuilder
    {
        private readonly IRawEarningsRepository _rawEarningsRepository;
        private readonly IRawEarningsMathsEnglishRepository _rawEarningsMathsEnglishRepository;
        private readonly IRequiredPaymentsHistoryRepository _requiredPaymentsHistoryRepository;
        private readonly IDataLockPriceEpisodePeriodMatchesRepository _dataLockPriceEpisodePeriodMatchesRepository;

        public ProviderLearnersBuilder(
            IRawEarningsRepository rawEarningsRepository, 
            IRawEarningsMathsEnglishRepository rawEarningsMathsEnglishRepository, 
            IRequiredPaymentsHistoryRepository requiredPaymentsHistoryRepository, 
            IDataLockPriceEpisodePeriodMatchesRepository dataLockPriceEpisodePeriodMatchesRepository)
        {
            _rawEarningsRepository = rawEarningsRepository;
            _rawEarningsMathsEnglishRepository = rawEarningsMathsEnglishRepository;
            _requiredPaymentsHistoryRepository = requiredPaymentsHistoryRepository;
            _dataLockPriceEpisodePeriodMatchesRepository = dataLockPriceEpisodePeriodMatchesRepository;
        }

        public Dictionary<string, Learner> Build(long ukprn)
        {
            var learners = new Dictionary<string, Learner>();

            _rawEarningsMathsEnglishRepository.GetAllForProvider(ukprn);
            _requiredPaymentsHistoryRepository.GetAllForProvider(ukprn);
            _dataLockPriceEpisodePeriodMatchesRepository.GetAllForProvider(ukprn);

            foreach (var rawEarning in _rawEarningsRepository.GetAllForProvider(ukprn))
            {
                if (!learners.ContainsKey(rawEarning.LearnRefNumber))
                {
                    var learner = new Learner();
                    learners.Add(rawEarning.LearnRefNumber, learner);
                }

                learners[rawEarning.LearnRefNumber].RawEarningEntities.Add(rawEarning);
            }

            return learners;
        }
    }
}