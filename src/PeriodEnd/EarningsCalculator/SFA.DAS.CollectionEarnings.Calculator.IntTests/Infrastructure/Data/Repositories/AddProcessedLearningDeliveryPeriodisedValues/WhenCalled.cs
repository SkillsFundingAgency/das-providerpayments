using System;
using System.Data.SqlClient;
using NUnit.Framework;
using SFA.DAS.CollectionEarnings.Calculator.Infrastructure.Data;
using SFA.DAS.CollectionEarnings.Calculator.Infrastructure.Data.Entities;
using SFA.DAS.CollectionEarnings.Calculator.IntegrationTests.Tools;
using SFA.DAS.CollectionEarnings.Calculator.UnitTests.Tools.Builders;

namespace SFA.DAS.CollectionEarnings.Calculator.IntegrationTests.Infrastructure.Data.Repositories.ProcessedLearningDeliveryPeriodisedValuesRepository.AddProcessedLearningDeliveryPeriodisedValues
{
    public class WhenCalled
    {
        private static readonly object[] InvalidProcessedLearningDeliveryPeriodisedValuesWithExpectedExceptionTypes =
        {
            new object[] {
                // Duplicate PK
                new[]
                {
                    new ApprenticeshipPriceEpisodePeriodisedValuesEntityBuilder().Build(),
                    new ApprenticeshipPriceEpisodePeriodisedValuesEntityBuilder().Build()
                },
                typeof(SqlException)
            },
            new object[]
            {
                // Out of bounds Period_1 -> Period_12 values
                new[]
                {
                    new ApprenticeshipPriceEpisodePeriodisedValuesEntityBuilder().WithPeriodValue(12345678901.00m).Build()
                },
                typeof(InvalidOperationException)
            }
        };

        private readonly string _transientConnectionString = GlobalTestContext.Instance.ConnectionString;

        private IApprenticeshipPriceEpisodePeriodisedValuesRepository _repository;

        [SetUp]
        public void Arrange()
        {
            TestDataHelper.Clean();

            _repository = new Calculator.Infrastructure.Data.Repositories.ApprenticeshipPriceEpisodePeriodisedValuesRepository(_transientConnectionString);
        }

        [Test]
        public void ThenExpectingDatabaseEntriesForAddProcessedLearningDeliveryPeriodisedValuesWithValidInput()
        {
            // Arrange
            var periodisedValues = new[]
            {
                new ApprenticeshipPriceEpisodePeriodisedValuesEntityBuilder().WithLearnRefNumber("1").Build(),
                new ApprenticeshipPriceEpisodePeriodisedValuesEntityBuilder().WithLearnRefNumber("2").Build(),
                new ApprenticeshipPriceEpisodePeriodisedValuesEntityBuilder().WithLearnRefNumber("3").Build(),
                new ApprenticeshipPriceEpisodePeriodisedValuesEntityBuilder().WithLearnRefNumber("4").Build(),
                new ApprenticeshipPriceEpisodePeriodisedValuesEntityBuilder().WithLearnRefNumber("5").Build()
            };

            // Act
            _repository.AddApprenticeshipPriceEpisodePeriodisedValues(periodisedValues);

            // Assert
            var rows = TestDataHelper.GetApprenticeshipPriceEpisodePeriodisedValues();

            Assert.IsNotNull(rows);
            Assert.AreEqual(5, rows.Length);
        }

        [Test]
        [TestCaseSource(nameof(InvalidProcessedLearningDeliveryPeriodisedValuesWithExpectedExceptionTypes))]
        public void ThenExpectingExceptionForAddProcessedLearningDeliveryPeriodisedValuesWithInvalidInput(ApprenticeshipPriceEpisodePeriodisedValuesEntity[] periodisedValues, Type exceptionType)
        {
            // Assert
            Assert.Throws(exceptionType, () => _repository.AddApprenticeshipPriceEpisodePeriodisedValues(periodisedValues));
        }
    }
}