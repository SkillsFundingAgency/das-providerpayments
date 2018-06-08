using System.Collections.Generic;
using System.Linq;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Application;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Dto;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data.Entities;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data.Repositories;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Services;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.UnitTests.Utilities;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.UnitTests
{
    [TestFixture]
    public class GivenAProviderProcessor
    {
        [TestFixture]
        public class WhenCallingProcess
        {
            [Test, PaymentsDueAutoData]
            public void ThenItGetsLearnerParametersForTheProvider(
            ProviderEntity provider,
            [Frozen] Mock<ILearnerProcessParametersBuilder> mockParametersBuilder,
            ProviderProcessor sut)
            {
                sut.Process(provider);

                mockParametersBuilder.Verify(builder => builder.Build(provider.Ukprn), Times.Once);
            }

            [Test, PaymentsDueAutoData]
            public void ThenItProcessesEachLearner(
                ProviderEntity provider,
                List<LearnerProcessParameters> learnerParameters,
                [Frozen] Mock<ILearnerProcessParametersBuilder> mockParametersBuilder,
                [Frozen] Mock<ILearnerProcessor> mockLearnerProcessor,
                ProviderProcessor sut)
            {
                mockParametersBuilder
                    .Setup(builder => builder.Build(provider.Ukprn))
                    .Returns(learnerParameters);

                sut.Process(provider);

                foreach (var parameter in learnerParameters)
                {
                    mockLearnerProcessor.Verify(builder => builder.Process(parameter), Times.Once);
                }
            }

            [Test, PaymentsDueAutoData]
            public void ThenItSavesAllNonPayableEarningsForProvider(
                ProviderEntity provider,
                List<LearnerProcessParameters> learnerParameters,
                LearnerProcessResults learnerResult,
                [Frozen] Mock<ILearnerProcessParametersBuilder> mockParametersBuilder,
                [Frozen] Mock<ILearnerProcessor> mockLearnerProcessor,
                [Frozen] Mock<INonPayableEarningRepository> mockNonPayableEarningsRepository,
                ProviderProcessor sut)
            {
                var expectedNonPayableEarnings = new List<NonPayableEarningEntity>();

                mockParametersBuilder
                    .Setup(builder => builder.Build(provider.Ukprn))
                    .Returns(learnerParameters);

                mockLearnerProcessor
                    .Setup(processor => processor.Process(It.IsAny<LearnerProcessParameters>()))
                    .Returns(learnerResult)
                    .Callback(() => expectedNonPayableEarnings.AddRange(learnerResult.NonPayableEarnings));

                sut.Process(provider);

                mockNonPayableEarningsRepository
                    .Verify(repository => repository.AddMany(expectedNonPayableEarnings));
            }

            [Test, PaymentsDueAutoData]
            public void ThenItSavesAllPayableEarningsForProvider(
                ProviderEntity provider,
                List<LearnerProcessParameters> learnerParameters,
                LearnerProcessResults learnerResult,
                [Frozen] Mock<ILearnerProcessParametersBuilder> mockParametersBuilder,
                [Frozen] Mock<ILearnerProcessor> mockLearnerProcessor,
                [Frozen] Mock<IRequiredPaymentRepository> mockRequiredPaymentsRepository,
                ProviderProcessor sut)
            {
                var expectedPayableEarnings = new List<RequiredPaymentEntity>();

                mockParametersBuilder
                    .Setup(builder => builder.Build(provider.Ukprn))
                    .Returns(learnerParameters);

                mockLearnerProcessor
                    .Setup(processor => processor.Process(It.IsAny<LearnerProcessParameters>()))
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
                List<LearnerProcessParameters> learnerParameters,
                LearnerProcessResults learnerResult,
                [Frozen] Mock<ILearnerProcessParametersBuilder> mockParametersBuilder,
                [Frozen] Mock<ILearnerProcessor> mockLearnerProcessor,
                [Frozen] Mock<INonPayableEarningRepository> mockNonPayableEarningsRepository,
                ProviderProcessor sut)
            {
                var actualNonPayableEarnings = new List<NonPayableEarningEntity>();

                mockParametersBuilder
                    .Setup(builder => builder.Build(provider.Ukprn))
                    .Returns(learnerParameters);

                mockLearnerProcessor
                    .Setup(processor => processor.Process(It.IsAny<LearnerProcessParameters>()))
                    .Returns(learnerResult);

                mockNonPayableEarningsRepository
                    .Setup(repository => repository.AddMany(It.IsAny<List<NonPayableEarningEntity>>()))
                    .Callback<List<NonPayableEarningEntity>>(nonPayableEarnings => actualNonPayableEarnings = nonPayableEarnings);

                sut.Process(provider);

                actualNonPayableEarnings.ForEach(entity => 
                    entity.IlrSubmissionDateTime.Should().Be(provider.IlrSubmissionDateTime));
            }

            [Test, PaymentsDueAutoData]
            public void ThenCollectionNameIsSetOnNonPayableEarnings(
                ProviderEntity provider,
                List<LearnerProcessParameters> learnerParameters,
                CollectionPeriodEntity collectionPeriod,
                LearnerProcessResults learnerResult,
                [Frozen] Mock<ILearnerProcessParametersBuilder> mockParametersBuilder,
                [Frozen] Mock<ICollectionPeriodRepository> mockCollectionPeriodRepository,
                [Frozen] Mock<ILearnerProcessor> mockLearnerProcessor,
                [Frozen] Mock<INonPayableEarningRepository> mockNonPayableEarningsRepository,
                ProviderProcessor sut)
            {
                var actualNonPayableEarnings = new List<NonPayableEarningEntity>();

                mockParametersBuilder
                    .Setup(builder => builder.Build(provider.Ukprn))
                    .Returns(learnerParameters);

                mockCollectionPeriodRepository
                    .Setup(repository => repository.GetCurrentCollectionPeriod())
                    .Returns(collectionPeriod);

                mockLearnerProcessor
                    .Setup(processor => processor.Process(It.IsAny<LearnerProcessParameters>()))
                    .Returns(learnerResult);

                mockNonPayableEarningsRepository
                    .Setup(repository => repository.AddMany(It.IsAny<List<NonPayableEarningEntity>>()))
                    .Callback<List<NonPayableEarningEntity>>(nonPayableEarnings => actualNonPayableEarnings = nonPayableEarnings);

                sut.Process(provider);

                actualNonPayableEarnings.ForEach(entity =>
                    entity.CollectionPeriodName.Should().Be(collectionPeriod.Name));
            }

            [Test, PaymentsDueAutoData]
            public void ThenCollectionMonthIsSetOnNonPayableEarnings(
                ProviderEntity provider,
                List<LearnerProcessParameters> learnerParameters,
                CollectionPeriodEntity collectionPeriod,
                LearnerProcessResults learnerResult,
                [Frozen] Mock<ILearnerProcessParametersBuilder> mockParametersBuilder,
                [Frozen] Mock<ICollectionPeriodRepository> mockCollectionPeriodRepository,
                [Frozen] Mock<ILearnerProcessor> mockLearnerProcessor,
                [Frozen] Mock<INonPayableEarningRepository> mockNonPayableEarningsRepository,
                ProviderProcessor sut)
            {
                var actualNonPayableEarnings = new List<NonPayableEarningEntity>();

                mockParametersBuilder
                    .Setup(builder => builder.Build(provider.Ukprn))
                    .Returns(learnerParameters);

                mockCollectionPeriodRepository
                    .Setup(repository => repository.GetCurrentCollectionPeriod())
                    .Returns(collectionPeriod);

                mockLearnerProcessor
                    .Setup(processor => processor.Process(It.IsAny<LearnerProcessParameters>()))
                    .Returns(learnerResult);

                mockNonPayableEarningsRepository
                    .Setup(repository => repository.AddMany(It.IsAny<List<NonPayableEarningEntity>>()))
                    .Callback<List<NonPayableEarningEntity>>(nonPayableEarnings => actualNonPayableEarnings = nonPayableEarnings);

                sut.Process(provider);

                actualNonPayableEarnings.ForEach(entity =>
                    entity.CollectionPeriodMonth.Should().Be(collectionPeriod.Month));
            }

            [Test, PaymentsDueAutoData]
            public void ThenCollectionYearIsSetOnNonPayableEarnings(
                ProviderEntity provider,
                List<LearnerProcessParameters> learnerParameters,
                CollectionPeriodEntity collectionPeriod,
                LearnerProcessResults learnerResult,
                [Frozen] Mock<ILearnerProcessParametersBuilder> mockParametersBuilder,
                [Frozen] Mock<ICollectionPeriodRepository> mockCollectionPeriodRepository,
                [Frozen] Mock<ILearnerProcessor> mockLearnerProcessor,
                [Frozen] Mock<INonPayableEarningRepository> mockNonPayableEarningsRepository,
                ProviderProcessor sut)
            {
                var actualNonPayableEarnings = new List<NonPayableEarningEntity>();

                mockParametersBuilder
                    .Setup(builder => builder.Build(provider.Ukprn))
                    .Returns(learnerParameters);

                mockCollectionPeriodRepository
                    .Setup(repository => repository.GetCurrentCollectionPeriod())
                    .Returns(collectionPeriod);

                mockLearnerProcessor
                    .Setup(processor => processor.Process(It.IsAny<LearnerProcessParameters>()))
                    .Returns(learnerResult);

                mockNonPayableEarningsRepository
                    .Setup(repository => repository.AddMany(It.IsAny<List<NonPayableEarningEntity>>()))
                    .Callback<List<NonPayableEarningEntity>>(nonPayableEarnings => actualNonPayableEarnings = nonPayableEarnings);

                sut.Process(provider);

                actualNonPayableEarnings.ForEach(entity =>
                    entity.CollectionPeriodYear.Should().Be(collectionPeriod.Year));
            }

            [Test, PaymentsDueAutoData]
            public void ThenIlrSubmissionDateIsSetOnPayableEarnings(
                ProviderEntity provider,
                List<LearnerProcessParameters> learnerParameters,
                LearnerProcessResults learnerResult,
                [Frozen] Mock<ILearnerProcessParametersBuilder> mockParametersBuilder,
                [Frozen] Mock<ILearnerProcessor> mockLearnerProcessor,
                [Frozen] Mock<IRequiredPaymentRepository> mockRequiredPaymentsRepository,
                ProviderProcessor sut)
            {
                var actualPayableEarnings = new List<RequiredPaymentEntity>();

                mockParametersBuilder
                    .Setup(builder => builder.Build(provider.Ukprn))
                    .Returns(learnerParameters);

                mockLearnerProcessor
                    .Setup(processor => processor.Process(It.IsAny<LearnerProcessParameters>()))
                    .Returns(learnerResult);

                mockRequiredPaymentsRepository
                    .Setup(repository => repository.AddRequiredPayments(It.IsAny<RequiredPaymentEntity[]>()))
                    .Callback<RequiredPaymentEntity[]>(payableEarnings => actualPayableEarnings = payableEarnings.ToList());

                sut.Process(provider);

                actualPayableEarnings.ForEach(entity =>
                    entity.IlrSubmissionDateTime.Should().Be(provider.IlrSubmissionDateTime));
            }

            [Test, PaymentsDueAutoData]
            public void ThenCollectionNameIsSetOnPayableEarnings(
                ProviderEntity provider,
                List<LearnerProcessParameters> learnerParameters,
                CollectionPeriodEntity collectionPeriod,
                LearnerProcessResults learnerResult,
                [Frozen] Mock<ILearnerProcessParametersBuilder> mockParametersBuilder,
                [Frozen] Mock<ICollectionPeriodRepository> mockCollectionPeriodRepository,
                [Frozen] Mock<ILearnerProcessor> mockLearnerProcessor,
                [Frozen] Mock<IRequiredPaymentRepository> mockRequiredPaymentsRepository,
                ProviderProcessor sut)
            {
                var actualPayableEarnings = new List<RequiredPaymentEntity>();

                mockParametersBuilder
                    .Setup(builder => builder.Build(provider.Ukprn))
                    .Returns(learnerParameters);

                mockCollectionPeriodRepository
                    .Setup(repository => repository.GetCurrentCollectionPeriod())
                    .Returns(collectionPeriod);

                mockLearnerProcessor
                    .Setup(processor => processor.Process(It.IsAny<LearnerProcessParameters>()))
                    .Returns(learnerResult);

                mockRequiredPaymentsRepository
                    .Setup(repository => repository.AddRequiredPayments(It.IsAny<RequiredPaymentEntity[]>()))
                    .Callback<RequiredPaymentEntity[]>(payableEarnings => actualPayableEarnings = payableEarnings.ToList());

                sut.Process(provider);

                actualPayableEarnings.ForEach(entity =>
                    entity.CollectionPeriodName.Should().Be(collectionPeriod.Name));
            }

            [Test, PaymentsDueAutoData]
            public void ThenCollectionMonthIsSetOnPayableEarnings(
                ProviderEntity provider,
                List<LearnerProcessParameters> learnerParameters,
                CollectionPeriodEntity collectionPeriod,
                LearnerProcessResults learnerResult,
                [Frozen] Mock<ILearnerProcessParametersBuilder> mockParametersBuilder,
                [Frozen] Mock<ICollectionPeriodRepository> mockCollectionPeriodRepository,
                [Frozen] Mock<ILearnerProcessor> mockLearnerProcessor,
                [Frozen] Mock<IRequiredPaymentRepository> mockRequiredPaymentsRepository,
                ProviderProcessor sut)
            {
                var actualPayableEarnings = new List<RequiredPaymentEntity>();

                mockParametersBuilder
                    .Setup(builder => builder.Build(provider.Ukprn))
                    .Returns(learnerParameters);

                mockCollectionPeriodRepository
                    .Setup(repository => repository.GetCurrentCollectionPeriod())
                    .Returns(collectionPeriod);

                mockLearnerProcessor
                    .Setup(processor => processor.Process(It.IsAny<LearnerProcessParameters>()))
                    .Returns(learnerResult);

                mockRequiredPaymentsRepository
                    .Setup(repository => repository.AddRequiredPayments(It.IsAny<RequiredPaymentEntity[]>()))
                    .Callback<RequiredPaymentEntity[]>(payableEarnings => actualPayableEarnings = payableEarnings.ToList());

                sut.Process(provider);

                actualPayableEarnings.ForEach(entity =>
                    entity.CollectionPeriodMonth.Should().Be(collectionPeriod.Month));
            }

            [Test, PaymentsDueAutoData]
            public void ThenCollectionYearIsSetOnPayableEarnings(
                ProviderEntity provider,
                List<LearnerProcessParameters> learnerParameters,
                CollectionPeriodEntity collectionPeriod,
                LearnerProcessResults learnerResult,
                [Frozen] Mock<ILearnerProcessParametersBuilder> mockParametersBuilder,
                [Frozen] Mock<ICollectionPeriodRepository> mockCollectionPeriodRepository,
                [Frozen] Mock<ILearnerProcessor> mockLearnerProcessor,
                [Frozen] Mock<IRequiredPaymentRepository> mockRequiredPaymentsRepository,
                ProviderProcessor sut)
            {
                var actualPayableEarnings = new List<RequiredPaymentEntity>();

                mockParametersBuilder
                    .Setup(builder => builder.Build(provider.Ukprn))
                    .Returns(learnerParameters);

                mockCollectionPeriodRepository
                    .Setup(repository => repository.GetCurrentCollectionPeriod())
                    .Returns(collectionPeriod);

                mockLearnerProcessor
                    .Setup(processor => processor.Process(It.IsAny<LearnerProcessParameters>()))
                    .Returns(learnerResult);

                mockRequiredPaymentsRepository
                    .Setup(repository => repository.AddRequiredPayments(It.IsAny<RequiredPaymentEntity[]>()))
                    .Callback<RequiredPaymentEntity[]>(payableEarnings => actualPayableEarnings = payableEarnings.ToList());

                sut.Process(provider);

                actualPayableEarnings.ForEach(entity =>
                    entity.CollectionPeriodYear.Should().Be(collectionPeriod.Year));
            }
        } 
    }
}