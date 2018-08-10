using System.Collections.Generic;
using System.Linq;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Domain;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data.Entities;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data.Repositories;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Services;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.UnitTests.Utilities;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.UnitTests
{
    [TestFixture]
    public class GivenASortProviderDataIntoLearnerData
    {
        private static readonly List<CollectionPeriodEntity> CollectionPeriods = new List<CollectionPeriodEntity>
        {
            new CollectionPeriodEntity {AcademicYear = "1718", Id = 1, Month = 8, Year = 2017, },
            new CollectionPeriodEntity {AcademicYear = "1718", Id = 2, Month = 9, Year = 2017, },
            new CollectionPeriodEntity {AcademicYear = "1718", Id = 3, Month = 10, Year = 2017, },
            new CollectionPeriodEntity {AcademicYear = "1718", Id = 4, Month = 11, Year = 2017, },
            new CollectionPeriodEntity {AcademicYear = "1718", Id = 5, Month = 12, Year = 2017, },
            new CollectionPeriodEntity {AcademicYear = "1718", Id = 6, Month = 1, Year = 2018, },
            new CollectionPeriodEntity {AcademicYear = "1718", Id = 7, Month = 2, Year = 2018, },
            new CollectionPeriodEntity {AcademicYear = "1718", Id = 8, Month = 3, Year = 2018, },
            new CollectionPeriodEntity {AcademicYear = "1718", Id = 9, Month = 4, Year = 2018, },
            new CollectionPeriodEntity {AcademicYear = "1718", Id = 10, Month = 5, Year = 2018, },
            new CollectionPeriodEntity {AcademicYear = "1718", Id = 11, Month = 6, Year = 2018, },
            new CollectionPeriodEntity {AcademicYear = "1718", Id = 12, Month = 7, Year = 2018, },
            new CollectionPeriodEntity {AcademicYear = "1718", Id = 13, Month = 9, Year = 2018, },
            new CollectionPeriodEntity {AcademicYear = "1718", Id = 14, Month = 10, Year = 2018, },
        };

        [Test, PaymentsDueAutoData]
        public void ThenItCreatesASingleNewLearnerProcessParametersInstanceForAllRawEarningsWithLearnerRefNumberAndUln(
            long ukprn,
            string learnRefNumber,
            long uln,
            List<RawEarning> rawEarnings,
            [Frozen] Mock<ICollectionPeriodRepository> collectionPeriodRepository,
            [Frozen] Mock<IRawEarningsRepository> mockRawEarningsRepository,
            SortProviderDataIntoLearnerDataService sut)
        {
            rawEarnings.ForEach(entity =>
            {
                entity.LearnRefNumber = learnRefNumber;
                entity.Uln = uln;
            });
 
            mockRawEarningsRepository
                .Setup(repository => repository.GetAllForProvider(ukprn))
                .Returns(rawEarnings);

            collectionPeriodRepository
                .Setup(x => x.GetAllCollectionPeriods())
                .Returns(CollectionPeriods);

            var learners = sut.Sort(ukprn);

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
            [Frozen] Mock<ICollectionPeriodRepository> collectionPeriodRepository,
            List<RawEarningForMathsOrEnglish> rawEarningsMathsEnglish,
            [Frozen] Mock<IRawEarningsMathsEnglishRepository> mockRawEarningsMathsEnglishRepository,
            SortProviderDataIntoLearnerDataService sut)
        {
            rawEarningsMathsEnglish.ForEach(entity =>
            {
                entity.LearnRefNumber = learnRefNumber;
                entity.Uln = uln;
            });

            mockRawEarningsMathsEnglishRepository
                .Setup(repository => repository.GetAllForProvider(ukprn))
                .Returns(rawEarningsMathsEnglish);

            collectionPeriodRepository
                .Setup(x => x.GetAllCollectionPeriods())
                .Returns(CollectionPeriods);

            var learners = sut.Sort(ukprn);

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
            List<RequiredPayment> historicalPayments,
            [Frozen] Mock<IRequiredPaymentsHistoryRepository> mockHistoricalPaymentsRepository,
            SortProviderDataIntoLearnerDataService sut)
        {
            historicalPayments.ForEach(entity =>
            {
                entity.LearnRefNumber = learnRefNumber;
                entity.Uln = uln;
            });

            mockHistoricalPaymentsRepository
                .Setup(repository => repository.GetAllForProvider(ukprn))
                .Returns(historicalPayments);

            var learners = sut.Sort(ukprn);

            learners.Count.Should().Be(1);
            learners[0].LearnRefNumber.Should().Be(learnRefNumber);
            learners[0].Uln.Should().Be(uln);
            learners[0].HistoricalRequiredPayments.ShouldAllBeEquivalentTo(historicalPayments);
        }

        [Test, PaymentsDueAutoData]
        public void ThenItCreatesASingleNewLearnerForAllDataLocksWithLearnerRefNumberButNoUln(
            long ukprn,
            string learnRefNumber,
            List<DatalockOutputEntity> dataLocks,
            [Frozen] Mock<IDatalockRepository> mockDataLockRepository,
            SortProviderDataIntoLearnerDataService sut)
        {
            dataLocks.ForEach(entity => entity.LearnRefNumber = learnRefNumber);

            mockDataLockRepository
                .Setup(repository => repository.GetDatalockOutputForProvider(ukprn))
                .Returns(dataLocks);

            var learners = sut.Sort(ukprn);

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
            [Frozen] Mock<ICollectionPeriodRepository> collectionPeriodRepository,
            List<RawEarning> rawEarnings,
            [Frozen] Mock<IRawEarningsRepository> mockRawEarningsRepository,
            List<RawEarningForMathsOrEnglish> rawEarningsMathsEnglish,
            [Frozen] Mock<IRawEarningsMathsEnglishRepository> mockRawEarningsMathsEnglishRepository,
            List<RequiredPayment> historicalPayments,
            [Frozen] Mock<IRequiredPaymentsHistoryRepository> mockHistoricalPaymentsRepository,
            List<DatalockOutputEntity> dataLocks,
            [Frozen] Mock<IDatalockRepository> mockDataLockRepository,
            List<Commitment> commitments,
            [Frozen] Mock<ICommitmentRepository> mockCommitmentsRepository,
            SortProviderDataIntoLearnerDataService sut)
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
            mockDataLockRepository.Setup(repository => repository.GetDatalockOutputForProvider(ukprn)).Returns(dataLocks);
            mockCommitmentsRepository.Setup(repository => repository.GetProviderCommitments(ukprn)).Returns(commitments);

            collectionPeriodRepository
                .Setup(x => x.GetAllCollectionPeriods())
                .Returns(CollectionPeriods);

            var learners = sut.Sort(ukprn);
            
            learners.Count.Should().Be(3);

            for (var i = 0; i <= 2; i++)
            {
                learners[i].LearnRefNumber.Should().Be(learnRefNumbers[i]);
                learners[i].Uln.Should().Be(ulns[i]);
                learners[i].RawEarnings.Count.Should().Be(1);
                learners[i].RawEarnings[0].Should().Be(rawEarnings[i]);
                learners[i].RawEarningsMathsEnglish.Count.Should().Be(1);
                learners[i].RawEarningsMathsEnglish[0].Should().Be(rawEarningsMathsEnglish[i]);
                learners[i].HistoricalRequiredPayments.Count.Should().Be(1);
                learners[i].HistoricalRequiredPayments[0].Should().Be(historicalPayments[i]);
                learners[i].DataLocks.Count.Should().Be(1);
                learners[i].DataLocks.Any(x=> x.Equals(dataLocks[i])).Should().BeTrue();

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
            [Frozen] Mock<ICollectionPeriodRepository> collectionPeriodRepository,
            List<RawEarning> rawEarnings,
            List<Commitment> commitments,
            [Frozen] Mock<IRawEarningsRepository> mockRawEarningsRepository,
            [Frozen] Mock<ICommitmentRepository> mockCommitmentsRepository,
            SortProviderDataIntoLearnerDataService sut)
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

            collectionPeriodRepository
                .Setup(x => x.GetAllCollectionPeriods())
                .Returns(CollectionPeriods);

            var learners = sut.Sort(ukprn);

            learners.Count.Should().Be(1);
            learners[0].LearnRefNumber.Should().Be(learnRefNumber);
            learners[0].Uln.Should().Be(uln);
            learners[0].RawEarnings.ShouldAllBeEquivalentTo(rawEarnings);
            learners[0].Commitments.ShouldAllBeEquivalentTo(commitments);
        }

        [Test, PaymentsDueAutoData]
        public void ThenDeliveryPeriodIsSetOnEarnings(
            long ukprn,
            string learnRefNumber,
            long uln,
            List<RawEarning> rawEarnings,
            List<Commitment> commitments,
            [Frozen] Mock<IRawEarningsRepository> mockRawEarningsRepository,
            [Frozen] Mock<ICommitmentRepository> mockCommitmentsRepository,
            [Frozen] Mock<ICollectionPeriodRepository> collectionPeriodRepository,
            SortProviderDataIntoLearnerDataService sut)
        {
            var actualPayableEarnings = new List<RequiredPayment>();
            

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

            collectionPeriodRepository
                .Setup(x => x.GetAllCollectionPeriods())
                .Returns(CollectionPeriods);

            var learners = sut.Sort(ukprn);

            learners.Count.Should().Be(1);
            learners[0].RawEarnings.ForEach(x => x.DeliveryMonth.Should().Be(CollectionPeriods.First(y => y.Id == x.Period).Month));
            learners[0].RawEarnings.ForEach(x => x.DeliveryYear.Should().Be(CollectionPeriods.First(y => y.Id == x.Period).Year));
            learners[0].RawEarningsMathsEnglish.ForEach(x => x.DeliveryMonth.Should().Be(CollectionPeriods.First(y => y.Id == x.Period).Month));
            learners[0].RawEarningsMathsEnglish.ForEach(x => x.DeliveryYear.Should().Be(CollectionPeriods.First(y => y.Id == x.Period).Year));
        }
    }
}