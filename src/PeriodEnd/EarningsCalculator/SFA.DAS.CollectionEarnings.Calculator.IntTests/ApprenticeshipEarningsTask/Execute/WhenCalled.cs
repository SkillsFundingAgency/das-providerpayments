using System.Collections.Generic;
using System.Linq;
using CS.Common.External.Interfaces;
using NUnit.Framework;
using SFA.DAS.CollectionEarnings.Calculator.Application.ApprenticeshipPriceEpisodePeriodisedValues;
using SFA.DAS.CollectionEarnings.Calculator.Context;
using SFA.DAS.CollectionEarnings.Calculator.IntegrationTests.Tools;
using SFA.DAS.CollectionEarnings.Calculator.UnitTests.Tools;
using SFA.DAS.Payments.DCFS.Context;
using System;

namespace SFA.DAS.CollectionEarnings.Calculator.IntegrationTests.ApprenticeshipEarningsTask.Execute
{
    public class WhenCalled
    {
        private readonly string _transientConnectionString = GlobalTestContext.Instance.ConnectionString;

        private IExternalTask _task;
        private IExternalContext _context;

        [SetUp]
        public void Arrange()
        {
            TestDataHelper.Clean();

            _task = new Calculator.ApprenticeshipEarningsTask();

            _context = new ExternalContextStub
            {
                Properties = new Dictionary<string, string>
                {
                    {ContextPropertyKeys.TransientDatabaseConnectionString, _transientConnectionString},
                    {ContextPropertyKeys.LogLevel, "Trace"},
                    {EarningsContextPropertyKeys.YearOfCollection, "1718"}
                }
            };
        }

        [Test]
        public void ThenProcessedLearningDeliveryWithAssociatedPeriodisedValuesIsAdded()
        {
            // Arrange
            TestDataHelper.ExecuteScript("IlrDataOneLearningDeliveryToProcess.sql");

            // Act
            _task.Execute(_context);

            // Assert
            var learningDeliveries = TestDataHelper.GetApprenticeshipPriceEpisodes();
            var periodisedValues = TestDataHelper.GetApprenticeshipPriceEpisodePeriodisedValues();
            var periodValues = TestDataHelper.GetApprenticeshipPriceEpisodePeriods();

            Assert.IsNotNull(learningDeliveries);
            Assert.IsNotNull(periodisedValues);
            Assert.IsNotNull(periodValues);

            Assert.AreEqual(1, learningDeliveries.Length);
            Assert.AreEqual(1, periodisedValues.Count(pv => pv.AttributeName == AttributeNames.PriceEpisodeOnProgPayment.ToString()));
            Assert.AreEqual(1, periodisedValues.Count(pv => pv.AttributeName == AttributeNames.PriceEpisodeCompletionPayment.ToString()));
            Assert.AreEqual(1, periodisedValues.Count(pv => pv.AttributeName == AttributeNames.PriceEpisodeBalancePayment.ToString()));
            Assert.AreEqual(12, periodValues.Length);
        }

        [Test]
        public void ThenAProcessedLearningDeliveryIsAddedForALearnerThatFinishedLate()
        {
            // Arrange
            TestDataHelper.ExecuteScript("IlrDataOneLearningDeliveryToProcessLateFinisher.sql");

            // Act
            _task.Execute(_context);

            // Assert
            var learningDeliveries = TestDataHelper.GetApprenticeshipPriceEpisodes();

            Assert.IsNotNull(learningDeliveries);
        }

        [Test]
        public void ThenPeriodisedOnProgrammeValuesAreAddedForALearnerThatFinishedLate()
        {
            // Arrange
            TestDataHelper.ExecuteScript("IlrDataOneLearningDeliveryToProcessLateFinisher.sql");

            // Act
            _task.Execute(_context);

            // Assert
            var periodisedValues = TestDataHelper.GetApprenticeshipPriceEpisodePeriodisedValues();

            Assert.IsNotNull(periodisedValues);

            var onProgrammeEarning = periodisedValues.SingleOrDefault(pv => pv.AttributeName == AttributeNames.PriceEpisodeOnProgPayment.ToString());

            Assert.IsNotNull(onProgrammeEarning);

            Assert.AreEqual(160.00m, onProgrammeEarning.Period_1);
            Assert.AreEqual(160.00m, onProgrammeEarning.Period_2);
            Assert.AreEqual(0.00m, onProgrammeEarning.Period_3);
            Assert.AreEqual(0.00m, onProgrammeEarning.Period_4);
            Assert.AreEqual(0.00m, onProgrammeEarning.Period_5);
            Assert.AreEqual(0.00m, onProgrammeEarning.Period_6);
            Assert.AreEqual(0.00m, onProgrammeEarning.Period_7);
            Assert.AreEqual(0.00m, onProgrammeEarning.Period_8);
            Assert.AreEqual(0.00m, onProgrammeEarning.Period_9);
            Assert.AreEqual(0.00m, onProgrammeEarning.Period_10);
            Assert.AreEqual(0.00m, onProgrammeEarning.Period_11);
            Assert.AreEqual(0.00m, onProgrammeEarning.Period_12);
        }

        [Test]
        public void ThenPeriodisedCompletionValuesAreAddedForALearnerThatFinishedLate()
        {
            // Arrange
            TestDataHelper.ExecuteScript("IlrDataOneLearningDeliveryToProcessLateFinisher.sql");

            // Act
            _task.Execute(_context);

            // Assert
            var periodisedValues = TestDataHelper.GetApprenticeshipPriceEpisodePeriodisedValues();

            Assert.IsNotNull(periodisedValues);

            var completionEarning = periodisedValues.SingleOrDefault(pv => pv.AttributeName == AttributeNames.PriceEpisodeCompletionPayment.ToString());

            Assert.IsNotNull(completionEarning);

            Assert.AreEqual(0.00m, completionEarning.Period_1);
            Assert.AreEqual(0.00m, completionEarning.Period_2);
            Assert.AreEqual(0.00m, completionEarning.Period_3);
            Assert.AreEqual(0.00m, completionEarning.Period_4);
            Assert.AreEqual(600.00m, completionEarning.Period_5);
            Assert.AreEqual(0.00m, completionEarning.Period_6);
            Assert.AreEqual(0.00m, completionEarning.Period_7);
            Assert.AreEqual(0.00m, completionEarning.Period_8);
            Assert.AreEqual(0.00m, completionEarning.Period_9);
            Assert.AreEqual(0.00m, completionEarning.Period_10);
            Assert.AreEqual(0.00m, completionEarning.Period_11);
            Assert.AreEqual(0.00m, completionEarning.Period_12);
        }

        [Test]
        public void ThenPeriodisedBalancingWithZeroValuesAreAddedForALearnerThatFinishedLate()
        {
            // Arrange
            TestDataHelper.ExecuteScript("IlrDataOneLearningDeliveryToProcessLateFinisher.sql");

            // Act
            _task.Execute(_context);

            // Assert
            var periodisedValues = TestDataHelper.GetApprenticeshipPriceEpisodePeriodisedValues();

            Assert.IsNotNull(periodisedValues);

            var balancingEarning = periodisedValues.SingleOrDefault(pv => pv.AttributeName == AttributeNames.PriceEpisodeBalancePayment.ToString());

            Assert.IsNotNull(balancingEarning);

            Assert.AreEqual(0.00m, balancingEarning.Period_1);
            Assert.AreEqual(0.00m, balancingEarning.Period_2);
            Assert.AreEqual(0.00m, balancingEarning.Period_3);
            Assert.AreEqual(0.00m, balancingEarning.Period_4);
            Assert.AreEqual(0.00m, balancingEarning.Period_5);
            Assert.AreEqual(0.00m, balancingEarning.Period_6);
            Assert.AreEqual(0.00m, balancingEarning.Period_7);
            Assert.AreEqual(0.00m, balancingEarning.Period_8);
            Assert.AreEqual(0.00m, balancingEarning.Period_9);
            Assert.AreEqual(0.00m, balancingEarning.Period_10);
            Assert.AreEqual(0.00m, balancingEarning.Period_11);
            Assert.AreEqual(0.00m, balancingEarning.Period_12);
        }

        [Test]
        public void ThenTwelveLeaarningDeliveryPeriodsAreAddedForALearnerThatFinishedLate()
        {
            // Arrange
            TestDataHelper.ExecuteScript("IlrDataOneLearningDeliveryToProcessLateFinisher.sql");

            // Act
            _task.Execute(_context);

            // Assert
            var periods = TestDataHelper.GetApprenticeshipPriceEpisodePeriods();

            Assert.IsNotNull(periods);
            Assert.AreEqual(12, periods.Length);

            Assert.AreEqual(1, periods[0].Period);
            Assert.AreEqual(160.00m, periods[0].PriceEpisodeOnProgPayment);
            Assert.AreEqual(0.00m, periods[0].PriceEpisodeCompletionPayment);
            Assert.AreEqual(0.00m, periods[0].PriceEpisodeBalancePayment);

            Assert.AreEqual(2, periods[1].Period);
            Assert.AreEqual(160.00m, periods[1].PriceEpisodeOnProgPayment);
            Assert.AreEqual(0.00m, periods[1].PriceEpisodeCompletionPayment);
            Assert.AreEqual(0.00m, periods[1].PriceEpisodeBalancePayment);

            Assert.AreEqual(5, periods[4].Period);
            Assert.AreEqual(0.00m, periods[4].PriceEpisodeOnProgPayment);
            Assert.AreEqual(600.00m, periods[4].PriceEpisodeCompletionPayment);
            Assert.AreEqual(0.00m, periods[4].PriceEpisodeBalancePayment);

            var zeroValuesPeriods = new[] {3, 4, 6, 7, 8, 9, 10, 11, 12};

            foreach (var period in zeroValuesPeriods)
            {
                var periodEntity = periods.SingleOrDefault(p => p.Period == period);

                Assert.IsNotNull(periodEntity);
                Assert.AreEqual(0.00m, periodEntity.PriceEpisodeOnProgPayment);
                Assert.AreEqual(0.00m, periodEntity.PriceEpisodeCompletionPayment);
                Assert.AreEqual(0.00m, periodEntity.PriceEpisodeBalancePayment);
            }
        }

        [Test]
        public void ThenPeriodisedOnProgrammeValuesAreAddedForALearnerThatFinishedEarly()
        {
            // Arrange
            TestDataHelper.ExecuteScript("IlrDataOneLearningDeliveryToProcessEarlyFinisher.sql");

            // Act
            _task.Execute(_context);

            // Assert
            var periodisedValues = TestDataHelper.GetApprenticeshipPriceEpisodePeriodisedValues();

            Assert.IsNotNull(periodisedValues);

            var onProgrammeEarning = periodisedValues.SingleOrDefault(pv => pv.AttributeName == AttributeNames.PriceEpisodeOnProgPayment.ToString());

            Assert.IsNotNull(onProgrammeEarning);

            Assert.AreEqual(160.00m, onProgrammeEarning.Period_1);
            Assert.AreEqual(160.00m, onProgrammeEarning.Period_2);
            Assert.AreEqual(0.00m, onProgrammeEarning.Period_3);
            Assert.AreEqual(0.00m, onProgrammeEarning.Period_4);
            Assert.AreEqual(0.00m, onProgrammeEarning.Period_5);
            Assert.AreEqual(0.00m, onProgrammeEarning.Period_6);
            Assert.AreEqual(0.00m, onProgrammeEarning.Period_7);
            Assert.AreEqual(0.00m, onProgrammeEarning.Period_8);
            Assert.AreEqual(0.00m, onProgrammeEarning.Period_9);
            Assert.AreEqual(0.00m, onProgrammeEarning.Period_10);
            Assert.AreEqual(0.00m, onProgrammeEarning.Period_11);
            Assert.AreEqual(0.00m, onProgrammeEarning.Period_12);
        }

        [Test]
        public void ThenPeriodisedCompletionValuesAreAddedForALearnerThatFinishedEarly()
        {
            // Arrange
            TestDataHelper.ExecuteScript("IlrDataOneLearningDeliveryToProcessEarlyFinisher.sql");

            // Act
            _task.Execute(_context);

            // Assert
            var periodisedValues = TestDataHelper.GetApprenticeshipPriceEpisodePeriodisedValues();

            Assert.IsNotNull(periodisedValues);

            var completionEarning = periodisedValues.SingleOrDefault(pv => pv.AttributeName == AttributeNames.PriceEpisodeCompletionPayment.ToString());

            Assert.IsNotNull(completionEarning);

            Assert.AreEqual(0.00m, completionEarning.Period_1);
            Assert.AreEqual(0.00m, completionEarning.Period_2);
            Assert.AreEqual(600.00m, completionEarning.Period_3);
            Assert.AreEqual(0.00m, completionEarning.Period_4);
            Assert.AreEqual(0.00m, completionEarning.Period_5);
            Assert.AreEqual(0.00m, completionEarning.Period_6);
            Assert.AreEqual(0.00m, completionEarning.Period_7);
            Assert.AreEqual(0.00m, completionEarning.Period_8);
            Assert.AreEqual(0.00m, completionEarning.Period_9);
            Assert.AreEqual(0.00m, completionEarning.Period_10);
            Assert.AreEqual(0.00m, completionEarning.Period_11);
            Assert.AreEqual(0.00m, completionEarning.Period_12);
        }

        [Test]
        public void ThenPeriodisedBalancingValuesAreAddedForALearnerThatFinishedEarly()
        {
            // Arrange
            TestDataHelper.ExecuteScript("IlrDataOneLearningDeliveryToProcessEarlyFinisher.sql");

            // Act
            _task.Execute(_context);

            // Assert
            var periodisedValues = TestDataHelper.GetApprenticeshipPriceEpisodePeriodisedValues();

            Assert.IsNotNull(periodisedValues);

            var balancingEarning = periodisedValues.SingleOrDefault(pv => pv.AttributeName == AttributeNames.PriceEpisodeBalancePayment.ToString());

            Assert.IsNotNull(balancingEarning);

            Assert.AreEqual(0.00m, balancingEarning.Period_1);
            Assert.AreEqual(0.00m, balancingEarning.Period_2);
            Assert.AreEqual(0.00m, balancingEarning.Period_3);
            Assert.AreEqual(0.00m, balancingEarning.Period_4);
            Assert.AreEqual(0.00m, balancingEarning.Period_5);
            Assert.AreEqual(0.00m, balancingEarning.Period_6);
            Assert.AreEqual(0.00m, balancingEarning.Period_7);
            Assert.AreEqual(0.00m, balancingEarning.Period_8);
            Assert.AreEqual(0.00m, balancingEarning.Period_9);
            Assert.AreEqual(0.00m, balancingEarning.Period_10);
            Assert.AreEqual(0.00m, balancingEarning.Period_11);
            Assert.AreEqual(0.00m, balancingEarning.Period_12);
        }

        [Test]
        public void ThenTwelveLeaarningDeliveryPeriodsAreAddedForALearnerThatFinishedEarly()
        {
            // Arrange
            TestDataHelper.ExecuteScript("IlrDataOneLearningDeliveryToProcessEarlyFinisher.sql");

            // Act
            _task.Execute(_context);

            // Assert
            var periods = TestDataHelper.GetApprenticeshipPriceEpisodePeriods();

            Assert.IsNotNull(periods);
            Assert.AreEqual(12, periods.Length);

            Assert.AreEqual(1, periods[0].Period);
            Assert.AreEqual(160.00m, periods[0].PriceEpisodeOnProgPayment);
            Assert.AreEqual(00.00m, periods[0].PriceEpisodeCompletionPayment);
            Assert.AreEqual(0.00m, periods[0].PriceEpisodeBalancePayment);

            var zeroValuesPeriods = new[] { 4, 5, 6, 7, 8, 9, 10, 11, 12 };

            foreach (var period in zeroValuesPeriods)
            {
                var periodEntity = periods.SingleOrDefault(p => p.Period == period);

                Assert.IsNotNull(periodEntity);
                Assert.AreEqual(0.00m, periodEntity.PriceEpisodeOnProgPayment);
                Assert.AreEqual(0.00m, periodEntity.PriceEpisodeCompletionPayment);
                Assert.AreEqual(0.00m, periodEntity.PriceEpisodeBalancePayment);
            }
        }

        [Test]
        public void ThenMultiplePriceEpisodesWithAssociatedPeriodisedValuesAreAdded()
        {
            // Arrange
            TestDataHelper.ExecuteScript("IlrDataOneLearningDeliveryWitnMultiplePriceEpisodesToProcess.sql");

            // Act
            _task.Execute(_context);

            // Assert
            var priceEpisodes = TestDataHelper.GetApprenticeshipPriceEpisodes();
            var periodisedValues = TestDataHelper.GetApprenticeshipPriceEpisodePeriodisedValues();
            var periodValues = TestDataHelper.GetApprenticeshipPriceEpisodePeriods();

            Assert.IsNotNull(priceEpisodes);
            Assert.IsNotNull(periodisedValues);
            Assert.IsNotNull(periodValues);

            Assert.AreEqual(2, priceEpisodes.Length);

            Assert.AreEqual(2, periodisedValues.Count(pv => pv.AttributeName == AttributeNames.PriceEpisodeOnProgPayment.ToString()));
            Assert.AreEqual(2, periodisedValues.Count(pv => pv.AttributeName == AttributeNames.PriceEpisodeCompletionPayment.ToString()));
            Assert.AreEqual(2, periodisedValues.Count(pv => pv.AttributeName == AttributeNames.PriceEpisodeBalancePayment.ToString()));

            Assert.AreEqual(3, periodisedValues.Count(pv => pv.PriceEpisodeIdentifier == priceEpisodes[0].PriceEpisodeIdentifier));
            Assert.AreEqual(3, periodisedValues.Count(pv => pv.PriceEpisodeIdentifier == priceEpisodes[1].PriceEpisodeIdentifier));

            Assert.AreEqual(24, periodValues.Length);
        }

        [Test]
        public void ThenNoEarningsAreCalculatedForPlannedBreak()
        {
            // Arrange
            TestDataHelper.ExecuteScript("IlrDataPlannedBreakLearningDeliveriesToProcess.sql");

            // Act
            _task.Execute(_context);

            // Assert
            var periodisedValues = TestDataHelper.GetApprenticeshipPriceEpisodePeriodisedValues();

            Assert.IsNotNull(periodisedValues);

            var monthlyEarnings = periodisedValues.Where(pv => pv.AttributeName == AttributeNames.PriceEpisodeOnProgPayment.ToString()).ToArray();

            Assert.IsNotNull(monthlyEarnings);
          

            Assert.AreEqual(1000.00m, monthlyEarnings[0].Period_2);
            Assert.AreEqual(1000.00m, monthlyEarnings[0].Period_3);

            Assert.AreEqual(0.00m, monthlyEarnings[1].Period_4);
            Assert.AreEqual(0.00m, monthlyEarnings[1].Period_5);

            Assert.AreEqual(909.09m, Math.Round( monthlyEarnings[1].Period_6,2));
            Assert.AreEqual(909.09m, Math.Round(monthlyEarnings[1].Period_7, 2));
            Assert.AreEqual(909.09m, Math.Round(monthlyEarnings[1].Period_8, 2));
            Assert.AreEqual(909.09m, Math.Round(monthlyEarnings[1].Period_9, 2));
            Assert.AreEqual(909.09m, Math.Round(monthlyEarnings[1].Period_10, 2));
            Assert.AreEqual(909.09m, Math.Round(monthlyEarnings[1].Period_11, 2));
            Assert.AreEqual(909.09m, Math.Round(monthlyEarnings[1].Period_12,2));
        }

    }
}