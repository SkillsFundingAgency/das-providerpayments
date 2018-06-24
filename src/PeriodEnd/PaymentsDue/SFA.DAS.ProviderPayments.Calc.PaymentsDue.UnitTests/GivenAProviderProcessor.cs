using System.Collections.Generic;
using System.Linq;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Dto;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data.Entities;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data.Repositories;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Services;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Services.Dependencies;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.UnitTests.Utilities;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.UnitTests
{
    [TestFixture]
    public class GivenAProviderProcessor
    {
        [TestFixture(Ignore = "To fix")]
        public class WhenCallingProcess
        {
            [Test, PaymentsDueAutoData]
            public void ThenItGetsLearnerParametersForTheProvider(
            ProviderEntity provider,
            [Frozen] Mock<ISortProviderDataIntoLearnerData> mockParametersBuilder,
            ProviderProcessor sut)
            {
                sut.Process(provider);

                mockParametersBuilder.Verify(builder => builder.Sort(provider.Ukprn), Times.Once);
            }

            [Test, PaymentsDueAutoData]
            public void ThenItProcessesEachLearner(
                ProviderEntity provider,
                List<LearnerData> learnerParameters,
                [Frozen] Mock<ISortProviderDataIntoLearnerData> mockParametersBuilder,
                [Frozen] Mock<ILearnerProcessor> mockLearnerProcessor,
                ProviderProcessor sut)
            {
                mockParametersBuilder
                    .Setup(builder => builder.Sort(provider.Ukprn))
                    .Returns(learnerParameters);

                sut.Process(provider);

                foreach (var parameter in learnerParameters)
                {
                    mockLearnerProcessor.Verify(builder => builder.Process(parameter, It.IsAny<long>()), Times.Once);
                }
            }

            [Test, PaymentsDueAutoData]
            public void ThenItSavesAllNonPayableEarningsForProvider(
                ProviderEntity provider,
                List<LearnerData> learnerParameters,
                PaymentsDueResult learnerResult,
                [Frozen] Mock<ISortProviderDataIntoLearnerData> mockParametersBuilder,
                [Frozen] Mock<ILearnerProcessor> mockLearnerProcessor,
                [Frozen] Mock<INonPayableEarningRepository> mockNonPayableEarningsRepository,
                ProviderProcessor sut)
            {
                var expectedNonPayableEarnings = new List<NonPayableEarningEntity>();

                mockParametersBuilder
                    .Setup(builder => builder.Sort(provider.Ukprn))
                    .Returns(learnerParameters);

                mockLearnerProcessor
                    .Setup(processor => processor.Process(It.IsAny<LearnerData>(), It.IsAny<long>()))
                    .Returns(learnerResult)
                    .Callback(() => expectedNonPayableEarnings.AddRange(learnerResult.NonPayableEarnings));

                sut.Process(provider);

                mockNonPayableEarningsRepository
                    .Verify(repository => repository.AddMany(expectedNonPayableEarnings));
            }

            [Test, PaymentsDueAutoData]
            public void ThenItSavesAllPayableEarningsForProvider(
                ProviderEntity provider,
                List<LearnerData> learnerParameters,
                PaymentsDueResult learnerResult,
                [Frozen] Mock<ISortProviderDataIntoLearnerData> mockParametersBuilder,
                [Frozen] Mock<ILearnerProcessor> mockLearnerProcessor,
                [Frozen] Mock<IRequiredPaymentRepository> mockRequiredPaymentsRepository,
                ProviderProcessor sut)
            {
                var expectedPayableEarnings = new List<RequiredPaymentEntity>();

                mockParametersBuilder
                    .Setup(builder => builder.Sort(provider.Ukprn))
                    .Returns(learnerParameters);

                mockLearnerProcessor
                    .Setup(processor => processor.Process(It.IsAny<LearnerData>(), It.IsAny<long>()))
                    .Returns(learnerResult)
                    .Callback(() => expectedPayableEarnings.AddRange(learnerResult.PayableEarnings));

                sut.Process(provider);

                var expectedEarningsArray = expectedPayableEarnings.ToArray();
                mockRequiredPaymentsRepository
                    .Verify(repository => repository.AddRequiredPayments(expectedEarningsArray));
            }

            [Test, PaymentsDueAutoData]
            public void ThenIlrSubmissionDateIsSetOnNonPayableEarnings(
                ProviderEntity provider,
                List<LearnerData> learnerParameters,
                PaymentsDueResult learnerResult,
                [Frozen] Mock<ISortProviderDataIntoLearnerData> mockParametersBuilder,
                [Frozen] Mock<ILearnerProcessor> mockLearnerProcessor,
                [Frozen] Mock<INonPayableEarningRepository> mockNonPayableEarningsRepository,
                ProviderProcessor sut)
            {
                var actualNonPayableEarnings = new List<NonPayableEarningEntity>();

                mockParametersBuilder
                    .Setup(builder => builder.Sort(provider.Ukprn))
                    .Returns(learnerParameters);

                mockLearnerProcessor
                    .Setup(processor => processor.Process(It.IsAny<LearnerData>(), It.IsAny<long>()))
                    .Returns(learnerResult);

                mockNonPayableEarningsRepository
                    .Setup(repository => repository.AddMany(It.IsAny<List<NonPayableEarningEntity>>()))
                    .Callback<List<NonPayableEarningEntity>>(nonPayableEarnings => actualNonPayableEarnings = nonPayableEarnings);

                sut.Process(provider);

                actualNonPayableEarnings.ForEach(entity => 
                    entity.IlrSubmissionDateTime.Should().Be(provider.IlrSubmissionDateTime));
            }
            
            [Test, PaymentsDueAutoData]
            public void ThenCollectionFieldsAreSetOnNonPayableEarnings(
                ProviderEntity provider,
                List<LearnerData> learnerParameters,
                CollectionPeriodEntity collectionPeriod,
                PaymentsDueResult learnerResult,
                [Frozen] Mock<ISortProviderDataIntoLearnerData> mockParametersBuilder,
                [Frozen] Mock<ICollectionPeriodRepository> mockCollectionPeriodRepository,
                [Frozen] Mock<ILearnerProcessor> mockLearnerProcessor,
                [Frozen] Mock<INonPayableEarningRepository> mockNonPayableEarningsRepository,
                ProviderProcessor sut)
            {
                var actualNonPayableEarnings = new List<NonPayableEarningEntity>();

                mockParametersBuilder
                    .Setup(builder => builder.Sort(provider.Ukprn))
                    .Returns(learnerParameters);

                mockCollectionPeriodRepository
                    .Setup(repository => repository.GetCurrentCollectionPeriod())
                    .Returns(collectionPeriod);

                mockLearnerProcessor
                    .Setup(processor => processor.Process(It.IsAny<LearnerData>(), It.IsAny<long>()))
                    .Returns(learnerResult);

                mockNonPayableEarningsRepository
                    .Setup(repository => repository.AddMany(It.IsAny<List<NonPayableEarningEntity>>()))
                    .Callback<List<NonPayableEarningEntity>>(nonPayableEarnings => actualNonPayableEarnings = nonPayableEarnings);

                sut.Process(provider);

                actualNonPayableEarnings.ForEach(entity =>
                {
                    entity.CollectionPeriodName.Should().Be(collectionPeriod.CollectionPeriodName);
                    entity.CollectionPeriodMonth.Should().Be(collectionPeriod.Month);
                    entity.CollectionPeriodYear.Should().Be(collectionPeriod.Year);
                });
            }

            [Test, PaymentsDueAutoData]
            public void ThenIlrSubmissionDateIsSetOnPayableEarnings(
                ProviderEntity provider,
                List<LearnerData> learnerParameters,
                PaymentsDueResult learnerResult,
                [Frozen] Mock<ISortProviderDataIntoLearnerData> mockParametersBuilder,
                [Frozen] Mock<ILearnerProcessor> mockLearnerProcessor,
                [Frozen] Mock<IRequiredPaymentRepository> mockRequiredPaymentsRepository,
                ProviderProcessor sut)
            {
                var actualPayableEarnings = new List<RequiredPaymentEntity>();

                mockParametersBuilder
                    .Setup(builder => builder.Sort(provider.Ukprn))
                    .Returns(learnerParameters);

                mockLearnerProcessor
                    .Setup(processor => processor.Process(It.IsAny<LearnerData>(), It.IsAny<long>()))
                    .Returns(learnerResult);

                mockRequiredPaymentsRepository
                    .Setup(repository => repository.AddRequiredPayments(It.IsAny<RequiredPaymentEntity[]>()))
                    .Callback<RequiredPaymentEntity[]>(payableEarnings => actualPayableEarnings = payableEarnings.ToList());

                sut.Process(provider);

                actualPayableEarnings.ForEach(entity =>
                    entity.IlrSubmissionDateTime.Should().Be(provider.IlrSubmissionDateTime));
            }

            [Test, PaymentsDueAutoData]
            public void ThenCollectionFieldsAreSetOnPayableEarnings(
                ProviderEntity provider,
                List<LearnerData> learnerParameters,
                CollectionPeriodEntity collectionPeriod,
                PaymentsDueResult learnerResult,
                [Frozen] Mock<ISortProviderDataIntoLearnerData> mockParametersBuilder,
                [Frozen] Mock<ICollectionPeriodRepository> mockCollectionPeriodRepository,
                [Frozen] Mock<ILearnerProcessor> mockLearnerProcessor,
                [Frozen] Mock<IRequiredPaymentRepository> mockRequiredPaymentsRepository,
                ProviderProcessor sut)
            {
                var actualPayableEarnings = new List<RequiredPaymentEntity>();

                mockParametersBuilder
                    .Setup(builder => builder.Sort(provider.Ukprn))
                    .Returns(learnerParameters);

                mockCollectionPeriodRepository
                    .Setup(repository => repository.GetCurrentCollectionPeriod())
                    .Returns(collectionPeriod);

                mockLearnerProcessor
                    .Setup(processor => processor.Process(It.IsAny<LearnerData>(), It.IsAny<long>()))
                    .Returns(learnerResult);

                mockRequiredPaymentsRepository
                    .Setup(repository => repository.AddRequiredPayments(It.IsAny<RequiredPaymentEntity[]>()))
                    .Callback<RequiredPaymentEntity[]>(payableEarnings => actualPayableEarnings = payableEarnings.ToList());

                sut.Process(provider);

                actualPayableEarnings.ForEach(entity =>
                {
                    entity.CollectionPeriodName.Should().Be(collectionPeriod.CollectionPeriodName);
                    entity.CollectionPeriodMonth.Should().Be(collectionPeriod.Month);
                    entity.CollectionPeriodYear.Should().Be(collectionPeriod.Year);
                });
            }
        } 
    }
}