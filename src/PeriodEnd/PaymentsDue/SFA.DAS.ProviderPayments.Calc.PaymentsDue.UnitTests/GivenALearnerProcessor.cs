using System.Collections.Generic;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Domain;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Dto;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data.Entities;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Services;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.UnitTests.Utilities;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.UnitTests
{
    [TestFixture]
    public class GivenALearnerProcessor
    {
        [TestFixture]
        public class WhenCallingProcess
        {
            [Test, PaymentsDueAutoData]
            public void ThenItGetsPriceEpisodesFromDataLockValidation(
                LearnerProcessParameters parameters,
                [Frozen] Mock<IIShouldBeInTheDataLockComponent> mockDataLockComponent,
                [Frozen] Mock<IDataLockComponentFactory> mockDataLockFactory,
                LearnerProcessor sut)
            {
                mockDataLockFactory
                    .Setup(factory => factory.CreateDataLockComponent())
                    .Returns(mockDataLockComponent.Object);
                
                sut.Process(parameters);

                mockDataLockComponent
                    .Verify(dataLockComponent => dataLockComponent.ValidatePriceEpisodes(parameters.Commitments, parameters.DataLocks, It.IsAny<int>()), 
                    Times.Once);
            }

            [Test, PaymentsDueAutoData]
            public void ThenItCalculatesPaymentsFromLearner(
                LearnerProcessParameters parameters,
                List<PriceEpisode> priceEpisodes,
                [Frozen] Mock<IIShouldBeInTheDataLockComponent> mockDataLockComponent,
                [Frozen] Mock<IDataLockComponentFactory> mockDataLockFactory,
                [Frozen] Mock<ILearner> mockLearner,
                [Frozen] Mock<ILearnerFactory> mockLearnerFactory,
                LearnerProcessor sut)
            {
                mockDataLockFactory
                    .Setup(factory => factory.CreateDataLockComponent())
                    .Returns(mockDataLockComponent.Object);

                mockDataLockComponent
                    .Setup(component => component.ValidatePriceEpisodes(It.IsAny<List<Commitment>>(),
                        It.IsAny<List<DatalockOutput>>(), It.IsAny<int>()))
                    .Returns(priceEpisodes);

                mockLearnerFactory
                    .Setup(factory => factory.CreateLearner(
                        parameters.RawEarnings, 
                        parameters.RawEarningsMathsEnglish, 
                        priceEpisodes, 
                        parameters.HistoricalPayments))
                    .Returns(mockLearner.Object);

                sut.Process(parameters);

                mockLearner
                    .Verify(learner => learner.CalculatePaymentsDue(),
                    Times.Once);
            }

            [Test, PaymentsDueAutoData]
            public void ThenItReturnsTheCalculationResult(
                LearnerProcessParameters parameters,
                List<PriceEpisode> priceEpisodes,
                LearnerProcessResults processResults,
                [Frozen] Mock<IIShouldBeInTheDataLockComponent> mockDataLockComponent,
                [Frozen] Mock<IDataLockComponentFactory> mockDataLockFactory,
                [Frozen] Mock<ILearner> mockLearner,
                [Frozen] Mock<ILearnerFactory> mockLearnerFactory,
                LearnerProcessor sut)
            {
                mockDataLockFactory
                    .Setup(factory => factory.CreateDataLockComponent())
                    .Returns(mockDataLockComponent.Object);

                mockDataLockComponent
                    .Setup(component => component.ValidatePriceEpisodes(It.IsAny<List<Commitment>>(),
                        It.IsAny<List<DatalockOutput>>(), It.IsAny<int>()))
                    .Returns(priceEpisodes);

                mockLearnerFactory
                    .Setup(factory => factory.CreateLearner(
                        parameters.RawEarnings, 
                        parameters.RawEarningsMathsEnglish, 
                        priceEpisodes, 
                        parameters.HistoricalPayments))
                    .Returns(mockLearner.Object);

                mockLearner
                    .Setup(learner => learner.CalculatePaymentsDue())
                    .Returns(processResults);

                var actualResults = sut.Process(parameters);

                actualResults.PayableEarnings.ShouldAllBeEquivalentTo(processResults.PayableEarnings);
                actualResults.NonPayableEarnings.ShouldAllBeEquivalentTo(processResults.NonPayableEarnings);
            }
        }
    }
}