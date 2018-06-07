using System.Collections.Generic;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Application;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data.Entities;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data.Repositories;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.UnitTests.Utilities;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.UnitTests
{
    [TestFixture]
    public class GivenAProviderLearnersBuilder
    {
        [Test, PaymentsDueAutoData]
        public void ThenItCreatesASingleNewLearnerForAllRawEarningsIfLearnerNotAlreadyExists(
            long ukprn,
            string learnRefNumber,
            List<RawEarning> rawEarnings,
            [Frozen] Mock<IRawEarningsRepository> mockRawEarningsRepository,
            ProviderLearnersBuilder sut)
        {
            rawEarnings.ForEach(entity => entity.LearnRefNumber = learnRefNumber);

            mockRawEarningsRepository
                .Setup(repository => repository.GetAllForProvider(ukprn))
                .Returns(rawEarnings);

            var learners = sut.Build(ukprn);

            learners[learnRefNumber].RawEarnings
                .ShouldAllBeEquivalentTo(rawEarnings);
        }

        [Test, PaymentsDueAutoData]
        public void ThenItCreatesASingleNewLearnerForAllRawEarningsMathsEnglishIfLearnerNotAlreadyExists(
            long ukprn,
            string learnRefNumber,
            List<RawEarningForMathsOrEnglish> rawEarningsMathsEnglish,
            [Frozen] Mock<IRawEarningsMathsEnglishRepository> mockRawEarningsMathsEnglishRepository,
            ProviderLearnersBuilder sut)
        {
            rawEarningsMathsEnglish.ForEach(entity => entity.LearnRefNumber = learnRefNumber);

            mockRawEarningsMathsEnglishRepository
                .Setup(repository => repository.GetAllForProvider(ukprn))
                .Returns(rawEarningsMathsEnglish);

            var learners = sut.Build(ukprn);

            learners[learnRefNumber].RawEarningsMathsEnglish
                .ShouldAllBeEquivalentTo(rawEarningsMathsEnglish);
        }

        [Test, PaymentsDueAutoData]
        public void ThenItCreatesASingleNewLearnerForAllHistoricalPaymentsIfLearnerNotAlreadyExists(
            long ukprn,
            string learnRefNumber,
            List<RequiredPaymentsHistoryEntity> historicalPayments,
            [Frozen] Mock<IRequiredPaymentsHistoryRepository> mockHistoricalPaymentsRepository,
            ProviderLearnersBuilder sut)
        {
            historicalPayments.ForEach(entity => entity.LearnRefNumber = learnRefNumber);

            mockHistoricalPaymentsRepository
                .Setup(repository => repository.GetAllForProvider(ukprn))
                .Returns(historicalPayments);

            var learners = sut.Build(ukprn);

            learners[learnRefNumber].PastPayments
                .ShouldAllBeEquivalentTo(historicalPayments);
        }

        [Test, PaymentsDueAutoData]
        public void ThenItCreatesASingleNewLearnerForAllDataLocksIfLearnerNotAlreadyExists(
            long ukprn,
            string learnRefNumber,
            List<DataLockPriceEpisodePeriodMatchEntity> dataLocks,
            [Frozen] Mock<IDataLockPriceEpisodePeriodMatchesRepository> mockDataLockRepository,
            ProviderLearnersBuilder sut)
        {
            dataLocks.ForEach(entity => entity.LearnRefNumber = learnRefNumber);

            mockDataLockRepository
                .Setup(repository => repository.GetAllForProvider(ukprn))
                .Returns(dataLocks);

            var learners = sut.Build(ukprn);

            learners[learnRefNumber].DataLocks
                .ShouldAllBeEquivalentTo(dataLocks);
        }
    }
}