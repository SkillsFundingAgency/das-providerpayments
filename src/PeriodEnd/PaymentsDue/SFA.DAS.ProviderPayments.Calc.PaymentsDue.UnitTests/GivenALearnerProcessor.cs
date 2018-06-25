using AutoFixture.NUnit3;
using Moq;
using NUnit.Framework;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Dto;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Services;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Services.Dependencies;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.UnitTests.Utilities;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.UnitTests
{
    [TestFixture]
    public class GivenALearnerProcessor
    {
        [TestFixture(Ignore = "To Fix")]
        public class WhenCallingProcess
        {
            [Test, PaymentsDueAutoData]
            public void ThenItGetsPriceEpisodesFromDataLockValidation(
                LearnerData parameters,
                [Frozen] Mock<IDetermineWhichEarningsShouldBePaid> mockDataLockComponent,
                LearnerProcessor sut)
            {
                sut.Process(parameters, 0);

                //mockDataLockComponent
                //    .Verify(dataLockComponent => dataLockComponent.ValidatePriceEpisodes(parameters.Commitments, parameters.DataLocks, It.IsAny<DateTime>()), 
                //    Times.Once);
            }

            [Test, PaymentsDueAutoData]
            public void ThenItCalculatesPaymentsFromLearner(
                LearnerData parameters,
                [Frozen] Mock<IDetermineWhichEarningsShouldBePaid> mockDataLockComponent,
                [Frozen] Mock<ICalculatePaymentsDue> mockLearner,
                LearnerProcessor sut)
            {
                //mockDataLockComponent
                //    .Setup(component => component.ValidatePriceEpisodes(It.IsAny<List<Commitment>>(),
                //        It.IsAny<List<DatalockOutputEntity>>(), It.IsAny<DateTime>()))
                //    .Returns(priceEpisodes);

                //mockLearnerFactory
                //    .Setup(factory => factory.CreateLearner(
                //        parameters.RawEarnings, 
                //        parameters.RawEarningsMathsEnglish, 
                //        priceEpisodes, 
                //        parameters.HistoricalPayments))
                //    .Returns(mockLearner.Object);

                //sut.Process(parameters);

                //mockLearner
                //    .Verify(learner => learner.CalculatePaymentsDue(),
                //    Times.Once);
            }

            [Test, PaymentsDueAutoData]
            public void ThenItReturnsTheCalculationResult(
                LearnerData parameters,
                PaymentsDueResult processResults,
                [Frozen] Mock<IDetermineWhichEarningsShouldBePaid> mockDataLockComponent,
                [Frozen] Mock<ICalculatePaymentsDue> mockLearner,
                LearnerProcessor sut)
            {
                //mockDataLockComponent
                //    .Setup(component => component.ValidatePriceEpisodes(It.IsAny<List<Commitment>>(),
                //        It.IsAny<List<DatalockOutputEntity>>(), It.IsAny<DateTime>()))
                //    .Returns(priceEpisodes);

                //mockLearnerFactory
                //    .Setup(factory => factory.CreateLearner(
                //        parameters.RawEarnings, 
                //        parameters.RawEarningsMathsEnglish, 
                //        priceEpisodes, 
                //        parameters.HistoricalPayments))
                //    .Returns(mockLearner.Object);

                //mockLearner
                //    .Setup(learner => learner.CalculatePaymentsDue())
                //    .Returns(processResults);

                //var actualResults = sut.Process(parameters);

                //actualResults.PayableEarnings.ShouldAllBeEquivalentTo(processResults.PayableEarnings);
                //actualResults.NonPayableEarnings.ShouldAllBeEquivalentTo(processResults.NonPayableEarnings);
            }
        }
    }
}