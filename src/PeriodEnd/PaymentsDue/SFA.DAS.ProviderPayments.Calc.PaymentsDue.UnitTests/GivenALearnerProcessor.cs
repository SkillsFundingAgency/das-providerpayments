﻿using System.Collections.Generic;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Domain;
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
                    .Verify(dataLockComponent => dataLockComponent.ValidatePriceEpisodes(parameters.Commitments, parameters.DataLocks), 
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

                mockLearnerFactory
                    .Setup(factory => factory.CreateLearner())
                    .Returns(mockLearner.Object);

                mockDataLockComponent
                    .Setup(component => component.ValidatePriceEpisodes(It.IsAny<List<Commitment>>(),
                        It.IsAny<List<DataLockPriceEpisodePeriodMatchEntity>>()))
                    .Returns(priceEpisodes);

                sut.Process(parameters);

                mockLearner
                    .Verify(learner => learner.CalculatePaymentsDue(), //todo: refactor to take params, inc priceEpisode from datalock components
                    Times.Once);
            }

            [Test, PaymentsDueAutoData, Ignore("till implemented todos")]
            public void ThenItReturnsTheCalculationResult(
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

                mockLearnerFactory
                    .Setup(factory => factory.CreateLearner())
                    .Returns(mockLearner.Object);

                // todo: mockLearner.Result 

                var result = sut.Process(parameters);

                // todo: result.PayableEarnings.ShouldAllBeEquivalentTo(mockLearner.PayableEarnings);
                // todo: result.NonPayableEarnings.ShouldAllBeEquivalentTo(mockLearner.NonPayableEarnings);
            }
        }
    }
}