using System.Collections.Generic;
using System.Linq;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.Payments.DCFS.Domain;
using SFA.DAS.ProviderPayments.Calc.Refunds.Dto;
using SFA.DAS.ProviderPayments.Calc.Refunds.Infrastructure.Entities;
using SFA.DAS.ProviderPayments.Calc.Refunds.Infrastructure.Repositories;
using SFA.DAS.ProviderPayments.Calc.Refunds.Services;
using SFA.DAS.ProviderPayments.Calc.Refunds.UnitTests.Utilities;
using SFA.DAS.ProviderPayments.Calc.Shared.Infrastructure.Data;
using SFA.DAS.ProviderPayments.Calc.Shared.Infrastructure.Data.Entities;

namespace SFA.DAS.ProviderPayments.Calc.Refunds.UnitTests
{
    [TestFixture]
    public class GivenASortProviderDataIntoLearnerData
    {
        [Test, RefundsAutoData]
        public void ThenItCreatesASingleNewLearnerInstanceForAllRequiredPaymentsWithLearnerRefNumber(
            long ukprn,
            string learnRefNumber,
            List<RequiredPaymentEntity> requiredPayments,
            [Frozen] Mock<IRequiredPaymentRepository> requiredPaymentsRepository,
            LearnerBuilder sut)
        {
            requiredPayments.ForEach(entity => entity.LearnRefNumber = learnRefNumber);

            requiredPaymentsRepository
                .Setup(repository => repository.GetRefundsForProvider(ukprn))
                .Returns(requiredPayments);

            var learners = sut.CreateLearnersForProvider(ukprn);

            learners.Count.Should().Be(1);
            learners[0].LearnRefNumber.Should().Be(learnRefNumber);
            learners[0].RequiredRefunds.Should().BeEquivalentTo(requiredPayments);
        }

        [Test, RefundsAutoData]
        public void ThenItCreatesASingleNewLearnerInstanceForAllHistoricPaymentsWithLearnerRefNumber(
            long ukprn,
            string learnRefNumber,
            List<HistoricalPaymentEntity> historicalPayments,
            [Frozen] Mock<IHistoricalPaymentsRepository> historicalPaymentsRepository,
            LearnerBuilder sut)
        {
            historicalPayments.ForEach(entity => entity.LearnRefNumber = learnRefNumber);

            historicalPaymentsRepository
                .Setup(repository => repository.GetAllForProvider(ukprn))
                .Returns(historicalPayments);

            var learners = sut.CreateLearnersForProvider(ukprn);

            learners.Count.Should().Be(1);
            learners[0].LearnRefNumber.Should().Be(learnRefNumber);
            DomainHistoricalPaymentsShouldMatchEntityValues(learners[0].HistoricalPayments, historicalPayments);
        }

        [Test, RefundsAutoData]
        public void ThenItCreatesTwoLearnersOneWithOnlyRequiredPaymentsAndOneWithOnlyHistoricalPayments(
            long ukprn,
            List<RequiredPaymentEntity> requiredPayments,
            List<HistoricalPaymentEntity> historicalPayments,
            [Frozen] Mock<IRequiredPaymentRepository> requiredPaymentsRepository,
            [Frozen] Mock<IHistoricalPaymentsRepository> historicalPaymentsRepository,
            LearnerBuilder sut)
        {
            requiredPayments.ForEach(x => x.LearnRefNumber = "A");
            historicalPayments.ForEach(x => x.LearnRefNumber = "B");

            requiredPaymentsRepository
                .Setup(repository => repository.GetRefundsForProvider(ukprn))
                .Returns(requiredPayments);

            historicalPaymentsRepository
                .Setup(repository => repository.GetAllForProvider(ukprn))
                .Returns(historicalPayments);

            var learners = sut.CreateLearnersForProvider(ukprn);

            learners.Count.Should().Be(2);

            var learnerA = learners.First(x=>x.LearnRefNumber == "A");
            learnerA.RequiredRefunds.Should().BeEquivalentTo(requiredPayments);
            learnerA.HistoricalPayments.Should().BeEmpty();

            var learnerB = learners.First(x => x.LearnRefNumber == "B");
            learnerB.RequiredRefunds.Should().BeEmpty();
            DomainHistoricalPaymentsShouldMatchEntityValues(learnerB.HistoricalPayments, historicalPayments);
        }


        [Test, RefundsAutoData]
        public void ThenItCreatesTwoLearnersWithRequiredPaymentsAndHistoricalPayments(
            long ukprn,
            List<RequiredPaymentEntity> requiredPaymentsA,
            List<RequiredPaymentEntity> requiredPaymentsB,
            List<HistoricalPaymentEntity> historicalPaymentsA,
            List<HistoricalPaymentEntity> historicalPaymentsB,
            [Frozen] Mock<IRequiredPaymentRepository> requiredPaymentsRepository,
            [Frozen] Mock<IHistoricalPaymentsRepository> historicalPaymentsRepository,
            LearnerBuilder sut)
        {
            requiredPaymentsA.ForEach(x => x.LearnRefNumber = "A");
            historicalPaymentsA.ForEach(x => x.LearnRefNumber = "A");
            requiredPaymentsB.ForEach(x => x.LearnRefNumber = "B");
            historicalPaymentsB.ForEach(x => x.LearnRefNumber = "B");

            requiredPaymentsA.AddRange(requiredPaymentsB);
            historicalPaymentsA.AddRange(historicalPaymentsB);

            requiredPaymentsRepository
                .Setup(repository => repository.GetRefundsForProvider(ukprn))
                .Returns(requiredPaymentsA);

            historicalPaymentsRepository
                .Setup(repository => repository.GetAllForProvider(ukprn))
                .Returns(historicalPaymentsA);

            var learners = sut.CreateLearnersForProvider(ukprn);

            learners.Count.Should().Be(2);

            var learnerA = learners.First(x => x.LearnRefNumber == "A");
            learnerA.RequiredRefunds.Should().BeEquivalentTo(requiredPaymentsA.Take(3));
            DomainHistoricalPaymentsShouldMatchEntityValues(learnerA.HistoricalPayments, historicalPaymentsA.Take(3).ToList());

            var learnerB = learners.First(x => x.LearnRefNumber == "B");
            learnerB.RequiredRefunds.Should().BeEquivalentTo(requiredPaymentsB);
            DomainHistoricalPaymentsShouldMatchEntityValues(learnerB.HistoricalPayments, historicalPaymentsB);
        }

        private void DomainHistoricalPaymentsShouldMatchEntityValues(List<HistoricalPayment> domainHistoricalPayments, List<HistoricalPaymentEntity> historicalPayments)
        {
            for (var i = 0; i < domainHistoricalPayments.Count; i++)
            {
                var historicalPayment = domainHistoricalPayments[i];
                historicalPayment.DeliveryMonth.Should().Be(historicalPayments[i].DeliveryMonth);
                historicalPayment.DeliveryYear.Should().Be(historicalPayments[i].DeliveryYear);
                historicalPayment.Amount.Should().Be(historicalPayments[i].Amount);
                historicalPayment.RequiredPaymentId.Should().Be(historicalPayments[i].RequiredPaymentId);
                historicalPayment.CollectionPeriodMonth.Should().Be(historicalPayments[i].CollectionPeriodMonth);
                historicalPayment.CollectionPeriodYear.Should().Be(historicalPayments[i].CollectionPeriodYear);
                historicalPayment.CollectionPeriodName.Should().Be(historicalPayments[i].CollectionPeriodName);
                historicalPayment.FundingSource.Should().Be((FundingSource)historicalPayments[i].FundingSource);
                historicalPayment.TransactionType.Should().Be((TransactionType)historicalPayments[i].TransactionType);
                historicalPayment.AccountId.Should().Be(historicalPayments[i].AccountId);
                historicalPayment.ApprenticeshipContractType.Should().Be(historicalPayments[i].ApprenticeshipContractType);
            }

        }

    }
}