using System.Collections.Generic;
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
            [Frozen] Mock<ICorrelateLearnerData> providerDataSorter,
            ProviderPaymentsDueProcessor sut)
            {
                sut.Process(provider);

                providerDataSorter.Verify(builder => builder.CreateLearnerDataForProvider(provider.Ukprn), Times.Once);
            }

            [Test, PaymentsDueAutoData]
            public void ThenItProcessesEachLearner(
                ProviderEntity provider,
                List<LearnerData> learnerParameters,
                [Frozen] Mock<ICorrelateLearnerData> providerDataSorter,
                [Frozen] Mock<IProcessPaymentsDue> mockLearnerProcessor,
                ProviderPaymentsDueProcessor sut,
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
                    mockLearnerProcessor.Setup(x => x.GetPayableAndNonPayableEarnings(parameter, It.IsAny<long>()))
                        .Returns(returnValue);
                }
                
                sut.Process(provider);

                foreach (var parameter in learnerParameters)
                {
                    mockLearnerProcessor.Verify(builder => builder.GetPayableAndNonPayableEarnings(parameter, It.IsAny<long>()), Times.Once);
                }
            }

            [Test, PaymentsDueAutoData]
            public void ThenItSavesAllNonPayableEarningsForProvider(
                ProviderEntity provider,
                List<LearnerData> learnerParameters,
                PaymentsDueResult learnerResult,
                [Frozen] Mock<ICorrelateLearnerData> providerDataSorter,
                [Frozen] Mock<IProcessPaymentsDue> mockLearnerProcessor,
                [Frozen] Mock<INonPayableEarningRepository> mockNonPayableEarningsRepository,
                ProviderPaymentsDueProcessor sut)
            {
                var expectedNonPayableEarnings = new List<NonPayableEarning>();

                providerDataSorter
                    .Setup(builder => builder.CreateLearnerDataForProvider(provider.Ukprn))
                    .Returns(learnerParameters);

                mockLearnerProcessor
                    .Setup(processor => processor.GetPayableAndNonPayableEarnings(It.IsAny<LearnerData>(), It.IsAny<long>()))
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
                [Frozen] Mock<ICorrelateLearnerData> providerDataSorter,
                [Frozen] Mock<IProcessPaymentsDue> mockLearnerProcessor,
                [Frozen] Mock<IRequiredPaymentRepository> mockRequiredPaymentsRepository,
                ProviderPaymentsDueProcessor sut)
            {
                var expectedPayableEarnings = new List<RequiredPayment>();

                providerDataSorter
                    .Setup(builder => builder.CreateLearnerDataForProvider(provider.Ukprn))
                    .Returns(learnerParameters);

                mockLearnerProcessor
                    .Setup(processor => processor.GetPayableAndNonPayableEarnings(It.IsAny<LearnerData>(), It.IsAny<long>()))
                    .Returns(learnerResult)
                    .Callback(() => expectedPayableEarnings.AddRange(learnerResult.PayableEarnings));

                sut.Process(provider);

                mockRequiredPaymentsRepository
                    .Verify(repository => repository.AddMany(expectedPayableEarnings));
            }

            [Test, PaymentsDueAutoData]
            public void ThenIlrSubmissionDateIsSetOnNonPayableEarnings(
                ProviderEntity provider,
                List<LearnerData> learnerParameters,
                PaymentsDueResult learnerResult,
                [Frozen] Mock<ICorrelateLearnerData> providerDataSorter,
                [Frozen] Mock<IProcessPaymentsDue> mockLearnerProcessor,
                [Frozen] Mock<INonPayableEarningRepository> mockNonPayableEarningsRepository,
                ProviderPaymentsDueProcessor sut)
            {
                var actualNonPayableEarnings = new List<NonPayableEarning>();

                providerDataSorter
                    .Setup(builder => builder.CreateLearnerDataForProvider(provider.Ukprn))
                    .Returns(learnerParameters);

                mockLearnerProcessor
                    .Setup(processor => processor.GetPayableAndNonPayableEarnings(It.IsAny<LearnerData>(), It.IsAny<long>()))
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
                [Frozen] Mock<ICorrelateLearnerData> providerDataSorter,
                [Frozen] Mock<ICollectionPeriodRepository> mockCollectionPeriodRepository,
                [Frozen] Mock<IProcessPaymentsDue> mockLearnerProcessor,
                [Frozen] Mock<INonPayableEarningRepository> mockNonPayableEarningsRepository,
                ProviderPaymentsDueProcessor sut)
            {
                var actualNonPayableEarnings = new List<NonPayableEarning>();

                providerDataSorter
                    .Setup(builder => builder.CreateLearnerDataForProvider(provider.Ukprn))
                    .Returns(learnerParameters);

                mockCollectionPeriodRepository
                    .Setup(repository => repository.GetCurrentCollectionPeriod())
                    .Returns(collectionPeriod);

                mockLearnerProcessor
                    .Setup(processor => processor.GetPayableAndNonPayableEarnings(It.IsAny<LearnerData>(), It.IsAny<long>()))
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
                [Frozen] Mock<ICorrelateLearnerData> providerDataSorter,
                [Frozen] Mock<IProcessPaymentsDue> mockLearnerProcessor,
                [Frozen] Mock<IRequiredPaymentRepository> mockRequiredPaymentsRepository,
                ProviderPaymentsDueProcessor sut)
            {
                var actualPayableEarnings = new List<RequiredPayment>();

                providerDataSorter
                    .Setup(builder => builder.CreateLearnerDataForProvider(provider.Ukprn))
                    .Returns(learnerParameters);

                mockLearnerProcessor
                    .Setup(processor => processor.GetPayableAndNonPayableEarnings(It.IsAny<LearnerData>(), It.IsAny<long>()))
                    .Returns(learnerResult);

                mockRequiredPaymentsRepository
                    .Setup(repository => repository.AddMany(It.IsAny<List<RequiredPayment>>()))
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
                [Frozen] Mock<ICorrelateLearnerData> providerDataSorter,
                [Frozen] Mock<ICollectionPeriodRepository> mockCollectionPeriodRepository,
                [Frozen] Mock<IProcessPaymentsDue> mockLearnerProcessor,
                [Frozen] Mock<IRequiredPaymentRepository> mockRequiredPaymentsRepository,
                ProviderPaymentsDueProcessor sut)
            {
                var actualPayableEarnings = new List<RequiredPayment>();

                providerDataSorter
                    .Setup(builder => builder.CreateLearnerDataForProvider(provider.Ukprn))
                    .Returns(learnerParameters);

                mockCollectionPeriodRepository
                    .Setup(repository => repository.GetCurrentCollectionPeriod())
                    .Returns(collectionPeriod);

                mockLearnerProcessor
                    .Setup(processor => processor.GetPayableAndNonPayableEarnings(It.IsAny<LearnerData>(), It.IsAny<long>()))
                    .Returns(learnerResult);

                mockRequiredPaymentsRepository
                    .Setup(repository => repository.AddMany(It.IsAny<List<RequiredPayment>>()))
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