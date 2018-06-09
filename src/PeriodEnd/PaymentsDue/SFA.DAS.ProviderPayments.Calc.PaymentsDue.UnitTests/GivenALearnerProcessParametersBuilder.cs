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
    public class GivenALearnerProcessParametersBuilder
    {
        [Test, PaymentsDueAutoData]
        public void ThenItCreatesASingleNewLearnerProcessParametersInstanceForAllRawEarningsWithLearnerRefNumberAndUln(
            long ukprn,
            string learnRefNumber,
            long uln,
            List<RawEarning> rawEarnings,
            [Frozen] Mock<IRawEarningsRepository> mockRawEarningsRepository,
            LearnerProcessParametersBuilder sut)
        {
            rawEarnings.ForEach(entity =>
            {
                entity.LearnRefNumber = learnRefNumber;
                entity.Uln = uln;
            });
 
            mockRawEarningsRepository
                .Setup(repository => repository.GetAllForProvider(ukprn))
                .Returns(rawEarnings);

            var learners = sut.Build(ukprn);

            learners.Count.Should().Be(1);
            learners[0].LearnRefNumber.Should().Be(learnRefNumber);
            learners[0].Uln.Should().Be(uln);
            learners[0].RawEarnings.ShouldAllBeEquivalentTo(rawEarnings);
        }


        [Test, PaymentsDueAutoData]
        public void ThenItCreatesASingleNewLearnerForAllRawEarningsMathsEnglishWithLearnerRefNumberAndUln(
            long ukprn,
            string learnRefNumber,
            long uln,
            List<RawEarningForMathsOrEnglish> rawEarningsMathsEnglish,
            [Frozen] Mock<IRawEarningsMathsEnglishRepository> mockRawEarningsMathsEnglishRepository,
            LearnerProcessParametersBuilder sut)
        {
            rawEarningsMathsEnglish.ForEach(entity =>
            {
                entity.LearnRefNumber = learnRefNumber;
                entity.Uln = uln;
            });

            mockRawEarningsMathsEnglishRepository
                .Setup(repository => repository.GetAllForProvider(ukprn))
                .Returns(rawEarningsMathsEnglish);

            var learners = sut.Build(ukprn);

            learners.Count.Should().Be(1);
            learners[0].LearnRefNumber.Should().Be(learnRefNumber);
            learners[0].Uln.Should().Be(uln);
            learners[0].RawEarningsMathsEnglish.ShouldAllBeEquivalentTo(rawEarningsMathsEnglish);
        }

        [Test, PaymentsDueAutoData]
        public void ThenItCreatesASingleNewLearnerForAllHistoricalPaymentsWithLearnerRefNumberButNoUln(
            long ukprn,
            string learnRefNumber,
            long uln,
            List<RequiredPaymentEntity> historicalPayments,
            [Frozen] Mock<IRequiredPaymentsHistoryRepository> mockHistoricalPaymentsRepository,
            LearnerProcessParametersBuilder sut)
        {
            historicalPayments.ForEach(entity =>
            {
                entity.LearnRefNumber = learnRefNumber;
                entity.Uln = uln;
            });

            mockHistoricalPaymentsRepository
                .Setup(repository => repository.GetAllForProvider(ukprn))
                .Returns(historicalPayments);

            var learners = sut.Build(ukprn);

            learners.Count.Should().Be(1);
            learners[0].LearnRefNumber.Should().Be(learnRefNumber);
            learners[0].Uln.Should().Be(uln);
            learners[0].HistoricalPayments.ShouldAllBeEquivalentTo(historicalPayments);
        }

        [Test, PaymentsDueAutoData]
        public void ThenItCreatesASingleNewLearnerForAllDataLocksWithLearnerRefNumberButNoUln(
            long ukprn,
            string learnRefNumber,
            List<DataLockPriceEpisodePeriodMatchEntity> dataLocks,
            [Frozen] Mock<IDatalockOutputRepository> mockDataLockRepository,
            LearnerProcessParametersBuilder sut)
        {
            dataLocks.ForEach(entity => entity.LearnRefNumber = learnRefNumber);

            mockDataLockRepository
                .Setup(repository => repository.GetAllForProvider(ukprn))
                .Returns(dataLocks);

            var learners = sut.Build(ukprn);

            learners.Count.Should().Be(1);
            learners[0].LearnRefNumber.Should().Be(learnRefNumber);
            learners[0].Uln.Should().BeNull();
            learners[0].DataLocks.ShouldAllBeEquivalentTo(dataLocks);
        }

        [Test, PaymentsDueAutoData]
        public void ThenItCreatesThreeNewLearnerProcessParametersInstancesWithAllDataMappingButOnlyTwoCommitmentRecords(
            long ukprn,
            string[] learnRefNumbers,
            long[] ulns,
            List<RawEarning> rawEarnings,
            [Frozen] Mock<IRawEarningsRepository> mockRawEarningsRepository,
            List<RawEarningForMathsOrEnglish> rawEarningsMathsEnglish,
            [Frozen] Mock<IRawEarningsMathsEnglishRepository> mockRawEarningsMathsEnglishRepository,
            List<RequiredPaymentEntity> historicalPayments,
            [Frozen] Mock<IRequiredPaymentsHistoryRepository> mockHistoricalPaymentsRepository,
            List<DataLockPriceEpisodePeriodMatchEntity> dataLocks,
            [Frozen] Mock<IDatalockOutputRepository> mockDataLockRepository,
            List<Commitment> commitments,
            [Frozen] Mock<ICommitmentRepository> mockCommitmentsRepository,
            LearnerProcessParametersBuilder sut)
        {

            // Arrange data so each maps to the Uln or LearnerRefNumber
            for (var i = 0; i <= 2; i++)
            {
                rawEarnings[i].LearnRefNumber = learnRefNumbers[i];
                rawEarnings[i].Uln = ulns[i];
                rawEarningsMathsEnglish[i].LearnRefNumber = learnRefNumbers[i];
                rawEarningsMathsEnglish[i].Uln = ulns[i];
                historicalPayments[i].LearnRefNumber = learnRefNumbers[i];
                historicalPayments[i].Uln = ulns[i];
                dataLocks[i].LearnRefNumber = learnRefNumbers[i];
                commitments[i].Uln = ulns[i];
            }
            // Force this commitment to not match
            commitments[0].Uln = -99897689;

            mockRawEarningsRepository.Setup(repository => repository.GetAllForProvider(ukprn)).Returns(rawEarnings);
            mockRawEarningsMathsEnglishRepository.Setup(repository => repository.GetAllForProvider(ukprn)).Returns(rawEarningsMathsEnglish);
            mockHistoricalPaymentsRepository.Setup(repository => repository.GetAllForProvider(ukprn)).Returns(historicalPayments);
            mockDataLockRepository.Setup(repository => repository.GetAllForProvider(ukprn)).Returns(dataLocks);
            mockCommitmentsRepository.Setup(repository => repository.GetProviderCommitments(ukprn)).Returns(commitments);

            var learners = sut.Build(ukprn);
            
            learners.Count.Should().Be(3);

            for (var i = 0; i <= 2; i++)
            {
                learners[i].LearnRefNumber.Should().Be(learnRefNumbers[i]);
                learners[i].Uln.Should().Be(ulns[i]);
                learners[i].RawEarnings.Count.Should().Be(1);
                learners[i].RawEarnings[0].Should().Be(rawEarnings[i]);
                learners[i].RawEarningsMathsEnglish.Count.Should().Be(1);
                learners[i].RawEarningsMathsEnglish[0].Should().Be(rawEarningsMathsEnglish[i]);
                learners[i].HistoricalPayments.Count.Should().Be(1);
                learners[i].HistoricalPayments[0].Should().Be(historicalPayments[i]);
                learners[i].DataLocks.Count.Should().Be(1);
                learners[i].DataLocks[0].Should().Be(dataLocks[i]);

                if (i == 0)
                {
                    learners[i].Commitments.Count.Should().Be(0);
                }
                else
                {
                    learners[i].Commitments.Count.Should().Be(1);
                    learners[i].Commitments[0].Should().Be(commitments[i]);

                }
            }
        }

        [Test, PaymentsDueAutoData]
        public void ThenItCreatesASingleNewLearnerProcessParametersInstanceFromRawEarningsWithLearnerRefNumberAndUlnAndUsesThisUlnToMapTheCommitmentRecords(
            long ukprn,
            string learnRefNumber,
            long uln,
            List<RawEarning> rawEarnings,
            List<Commitment> commitments,
            [Frozen] Mock<IRawEarningsRepository> mockRawEarningsRepository,
            [Frozen] Mock<ICommitmentRepository> mockCommitmentsRepository,
            LearnerProcessParametersBuilder sut)
        {
            rawEarnings.ForEach(entity =>
            {
                entity.LearnRefNumber = learnRefNumber;
                entity.Uln = uln;
            });
            commitments.ForEach(entity => entity.Uln = uln);

            mockRawEarningsRepository
                .Setup(repository => repository.GetAllForProvider(ukprn))
                .Returns(rawEarnings);

            mockCommitmentsRepository
                .Setup(repository => repository.GetProviderCommitments(ukprn))
                .Returns(commitments);

            var learners = sut.Build(ukprn);

            learners.Count.Should().Be(1);
            learners[0].LearnRefNumber.Should().Be(learnRefNumber);
            learners[0].Uln.Should().Be(uln);
            learners[0].RawEarnings.ShouldAllBeEquivalentTo(rawEarnings);
            learners[0].Commitments.ShouldAllBeEquivalentTo(commitments);
        }
    }
}