using System;
using Moq;
using NUnit.Framework;
using SFA.DAS.CollectionEarnings.Calculator.Application.EarningsCalculation.GetLearningDeliveriesEarningsQuery;
using SFA.DAS.CollectionEarnings.Calculator.Tools.Providers;
using SFA.DAS.CollectionEarnings.Calculator.UnitTests.Tools.Builders;
using System.Linq;

namespace SFA.DAS.CollectionEarnings.Calculator.UnitTests.Application.EarningsCalculation.GetLearningDeliveriesEarningsQuery.GetLearningDeliveriesEarningsQueryHandler
{
    public class WhenHandlingMultipleLearners
    {
        private Mock<IDateTimeProvider> _dateTimeProvider;

        private GetLearningDeliveriesEarningsQueryRequest _request;
        private Calculator.Application.EarningsCalculation.GetLearningDeliveriesEarningsQuery.GetLearningDeliveriesEarningsQueryHandler _handler;

        [SetUp]
        public void Arrange()
        {
            _dateTimeProvider = new Mock<IDateTimeProvider>();

            InitialMockSetup();

            _handler = new Calculator.Application.EarningsCalculation.GetLearningDeliveriesEarningsQuery.GetLearningDeliveriesEarningsQueryHandler(_dateTimeProvider.Object);
        }

        private void InitialMockSetup()
        {
            _dateTimeProvider
                .Setup(dtp => dtp.YearOfCollectionStart)
                .Returns(new DateTime(2017, 8, 1));

            _dateTimeProvider
                .Setup(dtp => dtp.Now)
                .Returns(new DateTime(2017, 12, 15, 12, 0, 0));
        }

        [Test]
        public void ThenTwelvePeriodEarningsAreCreatedForEachLearner()
        {
            // Arrange
            var learningDeliveries = new[]
            {
                new LearningDeliveryBuilder().WithLearnStartDate(new DateTime(2017, 11, 1)).Build(),
                new LearningDeliveryBuilder().WithLearnStartDate(new DateTime(2018, 1, 1)).WithLearnRefNumber("Lrn002").Build()
            };

            _request = new GetLearningDeliveriesEarningsQueryRequest
            {
                LearningDeliveries = learningDeliveries
            };

            // Act
            var result = _handler.Handle(_request);

            // Assert
            Assert.IsNotNull(result.IsValid);
            Assert.IsTrue(result.IsValid);

            for (var learnerIdx = 0; learnerIdx < learningDeliveries.Length; learnerIdx++)
            {
                Assert.AreEqual(12, result.PriceEpisodesPeriodsEarnings.Count(pe => pe.LearnerReferenceNumber == learningDeliveries[learnerIdx].LearnerReferenceNumber && pe.AimSequenceNumber == learningDeliveries[learnerIdx].AimSequenceNumber), $"expecting 12 period earnings for learner {learningDeliveries[learnerIdx].LearnerReferenceNumber} with aim {learningDeliveries[learnerIdx].AimSequenceNumber}");

                for (var period = 1; period <= 12; period++)
                {
                    Assert.AreEqual(1, result.PriceEpisodesPeriodsEarnings.Count(pe => pe.LearnerReferenceNumber == learningDeliveries[learnerIdx].LearnerReferenceNumber && pe.AimSequenceNumber == learningDeliveries[learnerIdx].AimSequenceNumber && pe.Period == period), $"expecting one period earning for learner {learningDeliveries[learnerIdx].LearnerReferenceNumber} with aim {learningDeliveries[learnerIdx].AimSequenceNumber} in period {period}");
                }
            }
        }
    }
}