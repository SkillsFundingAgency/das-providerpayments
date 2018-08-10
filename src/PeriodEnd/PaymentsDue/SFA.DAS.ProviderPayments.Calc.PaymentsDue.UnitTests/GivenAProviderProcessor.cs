using System.Collections.Generic;
using System.Linq;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Domain;
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
        [TestFixture]
        public class WhenCallingProcess
        {
            [Test, PaymentsDueAutoData]
            public void ThenItGetsLearnerParametersForTheProvider(
            ProviderEntity provider,
            [Frozen] Mock<ISortProviderDataIntoLearnerData> providerDataSorter,
            ProviderProcessor sut)
            {
                sut.Process(provider);

                providerDataSorter.Verify(builder => builder.CreateLearnerDataForProvider(provider.Ukprn), Times.Once);
            }

            [Test, PaymentsDueAutoData]
            public void ThenItProcessesEachLearner(
                ProviderEntity provider,
                List<LearnerData> learnerParameters,
                [Frozen] Mock<ISortProviderDataIntoLearnerData> providerDataSorter,
                [Frozen] Mock<ILearnerProcessor> mockLearnerProcessor,
                ProviderProcessor sut,
                List<PaymentsDueResult> testResults
                )
            {
                providerDataSorter
                    .Setup(builder => builder.CreateLearnerDataForProvider(provider.Ukprn))
                    .Returns(learnerParameters);

                for (var i = 0; i < learnerParameters.Count; i++)
                {
                    var returnValue = testResults[i];
                    var parameter = learnerParameters[i];
                    mockLearnerProcessor.Setup(x => x.Process(parameter, It.IsAny<long>()))
                        .Returns(returnValue);
                }
                
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
                [Frozen] Mock<ISortProviderDataIntoLearnerData> providerDataSorter,
                [Frozen] Mock<ILearnerProcessor> mockLearnerProcessor,
                [Frozen] Mock<INonPayableEarningRepository> mockNonPayableEarningsRepository,
                ProviderProcessor sut)
            {
                var expectedNonPayableEarnings = new List<NonPayableEarning>();

                providerDataSorter
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
                [Frozen] Mock<ISortProviderDataIntoLearnerData> providerDataSorter,
                [Frozen] Mock<ILearnerProcessor> mockLearnerProcessor,
                [Frozen] Mock<IRequiredPaymentRepository> mockRequiredPaymentsRepository,
                ProviderProcessor sut)
            {
                var expectedPayableEarnings = new List<RequiredPayment>();

                providerDataSorter
                    .Setup(builder => builder.Sort(provider.Ukprn))
                    .Returns(learnerParameters);

                mockLearnerProcessor
                    .Setup(processor => processor.Process(It.IsAny<LearnerData>(), It.IsAny<long>()))
                    .Returns(learnerResult)
                    .Callback(() => expectedPayableEarnings.AddRange(learnerResult.PayableEarnings));

                sut.Process(provider);

                mockRequiredPaymentsRepository
                    .Verify(repository => repository.AddRequiredPayments(expectedPayableEarnings));
            }

            [Test, PaymentsDueAutoData]
            public void ThenIlrSubmissionDateIsSetOnNonPayableEarnings(
                ProviderEntity provider,
                List<LearnerData> learnerParameters,
                PaymentsDueResult learnerResult,
                [Frozen] Mock<ISortProviderDataIntoLearnerData> providerDataSorter,
                [Frozen] Mock<ILearnerProcessor> mockLearnerProcessor,
                [Frozen] Mock<INonPayableEarningRepository> mockNonPayableEarningsRepository,
                ProviderProcessor sut)
            {
                var actualNonPayableEarnings = new List<NonPayableEarning>();

                providerDataSorter
                    .Setup(builder => builder.Sort(provider.Ukprn))
                    .Returns(learnerParameters);

                mockLearnerProcessor
                    .Setup(processor => processor.Process(It.IsAny<LearnerData>(), It.IsAny<long>()))
                    .Returns(learnerResult);

                mockNonPayableEarningsRepository
                    .Setup(repository => repository.AddMany(It.IsAny<List<NonPayableEarning>>()))
                    .Callback<List<NonPayableEarning>>(nonPayableEarnings => actualNonPayableEarnings = nonPayableEarnings);

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
                [Frozen] Mock<ISortProviderDataIntoLearnerData> providerDataSorter,
                [Frozen] Mock<ICollectionPeriodRepository> mockCollectionPeriodRepository,
                [Frozen] Mock<ILearnerProcessor> mockLearnerProcessor,
                [Frozen] Mock<INonPayableEarningRepository> mockNonPayableEarningsRepository,
                ProviderProcessor sut)
            {
                var actualNonPayableEarnings = new List<NonPayableEarning>();

                providerDataSorter
                    .Setup(builder => builder.Sort(provider.Ukprn))
                    .Returns(learnerParameters);

                mockCollectionPeriodRepository
                    .Setup(repository => repository.GetCurrentCollectionPeriod())
                    .Returns(collectionPeriod);

                mockLearnerProcessor
                    .Setup(processor => processor.Process(It.IsAny<LearnerData>(), It.IsAny<long>()))
                    .Returns(learnerResult);

                mockNonPayableEarningsRepository
                    .Setup(repository => repository.AddMany(It.IsAny<List<NonPayableEarning>>()))
                    .Callback<List<NonPayableEarning>>(nonPayableEarnings => actualNonPayableEarnings = nonPayableEarnings);

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
                [Frozen] Mock<ISortProviderDataIntoLearnerData> providerDataSorter,
                [Frozen] Mock<ILearnerProcessor> mockLearnerProcessor,
                [Frozen] Mock<IRequiredPaymentRepository> mockRequiredPaymentsRepository,
                ProviderProcessor sut)
            {
                var actualPayableEarnings = new List<RequiredPayment>();

                providerDataSorter
                    .Setup(builder => builder.Sort(provider.Ukprn))
                    .Returns(learnerParameters);

                mockLearnerProcessor
                    .Setup(processor => processor.Process(It.IsAny<LearnerData>(), It.IsAny<long>()))
                    .Returns(learnerResult);

                mockRequiredPaymentsRepository
                    .Setup(repository => repository.AddRequiredPayments(It.IsAny<List<RequiredPayment>>()))
                    .Callback<List<RequiredPayment>>(payableEarnings => actualPayableEarnings = payableEarnings);

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
                [Frozen] Mock<ISortProviderDataIntoLearnerData> providerDataSorter,
                [Frozen] Mock<ICollectionPeriodRepository> mockCollectionPeriodRepository,
                [Frozen] Mock<ILearnerProcessor> mockLearnerProcessor,
                [Frozen] Mock<IRequiredPaymentRepository> mockRequiredPaymentsRepository,
                ProviderProcessor sut)
            {
                var actualPayableEarnings = new List<RequiredPayment>();

                providerDataSorter
                    .Setup(builder => builder.Sort(provider.Ukprn))
                    .Returns(learnerParameters);

                mockCollectionPeriodRepository
                    .Setup(repository => repository.GetCurrentCollectionPeriod())
                    .Returns(collectionPeriod);

                mockLearnerProcessor
                    .Setup(processor => processor.Process(It.IsAny<LearnerData>(), It.IsAny<long>()))
                    .Returns(learnerResult);

                mockRequiredPaymentsRepository
                    .Setup(repository => repository.AddRequiredPayments(It.IsAny<List<RequiredPayment>>()))
                    .Callback<List<RequiredPayment>>(payableEarnings => actualPayableEarnings = payableEarnings);

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