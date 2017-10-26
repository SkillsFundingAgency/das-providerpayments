using System;
using System.Data.SqlClient;
using NUnit.Framework;
using SFA.DAS.CollectionEarnings.Calculator.Infrastructure.Data;
using SFA.DAS.CollectionEarnings.Calculator.Infrastructure.Data.Entities;
using SFA.DAS.CollectionEarnings.Calculator.IntegrationTests.Tools;
using SFA.DAS.CollectionEarnings.Calculator.UnitTests.Tools.Builders;

namespace SFA.DAS.CollectionEarnings.Calculator.IntegrationTests.Infrastructure.Data.Repositories.ProcessedLearningDeliveryPeriodRepository.AddProcessedLearningDeliveryPeriod
{
    public class WhenCalled
    {
        private static readonly object[] InvalidProcessedLearningDeliveryPeriodWithExpectedExceptionTypes =
        {
            new object[] {
                // Duplicate PK
                new[]
                {
                    new ApprenticeshipPriceEpisodePeriodEntityBuilder().Build(),
                    new ApprenticeshipPriceEpisodePeriodEntityBuilder().Build()
                },
                typeof(SqlException)
            },
            new object[]
            {
                // Out of bounds values
                new[]
                {
                    new ApprenticeshipPriceEpisodePeriodEntityBuilder().WithProgrammeAimBalPayment(12345678901.00m).WithProgrammeAimCompletionPayment(12345678901.00m).WithProgrammeAimOnProgPayment(12345678901.00m).Build()
                },
                typeof(InvalidOperationException)
            }
        };

        private readonly string _transientConnectionString = GlobalTestContext.Instance.ConnectionString;

        private IApprenticeshipPriceEpisodePeriodRepository _repository;

        [SetUp]
        public void Arrange()
        {
            TestDataHelper.Clean();

            _repository = new Calculator.Infrastructure.Data.Repositories.ApprenticeshipPriceEpisodePeriodRepository(_transientConnectionString);
        }

        [Test]
        public void ThenExpectingDatabaseEntriesForAddProcessedLearningDeliveryPeriodisedValuesWithValidInput()
        {
            // Arrange
            var periodValues = new[]
            {
                new ApprenticeshipPriceEpisodePeriodEntityBuilder().WithLearnRefNumber("1").Build(),
                new ApprenticeshipPriceEpisodePeriodEntityBuilder().WithLearnRefNumber("2").Build(),
                new ApprenticeshipPriceEpisodePeriodEntityBuilder().WithLearnRefNumber("3").Build(),
                new ApprenticeshipPriceEpisodePeriodEntityBuilder().WithLearnRefNumber("4").Build(),
                new ApprenticeshipPriceEpisodePeriodEntityBuilder().WithLearnRefNumber("5").Build()
            };

            // Act
            _repository.AddApprenticeshipPriceEpisodePeriod(periodValues);

            // Assert
            var rows = TestDataHelper.GetApprenticeshipPriceEpisodePeriods();

            Assert.IsNotNull(rows);
            Assert.AreEqual(5, rows.Length);
        }

        [Test]
        [TestCaseSource(nameof(InvalidProcessedLearningDeliveryPeriodWithExpectedExceptionTypes))]
        public void ThenExpectingExceptionForAddProcessedLearningDeliveryPeriodisedValuesWithInvalidInput(ApprenticeshipPriceEpisodePeriodEntity[] periodValues, Type exceptionType)
        {
            // Assert
            Assert.Throws(exceptionType, () => _repository.AddApprenticeshipPriceEpisodePeriod(periodValues));
        }
    }
}