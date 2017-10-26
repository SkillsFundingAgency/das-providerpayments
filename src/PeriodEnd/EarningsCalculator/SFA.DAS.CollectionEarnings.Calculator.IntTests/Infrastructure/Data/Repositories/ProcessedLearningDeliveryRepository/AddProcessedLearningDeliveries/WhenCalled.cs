using System;
using System.Data.SqlClient;
using NUnit.Framework;
using SFA.DAS.CollectionEarnings.Calculator.Infrastructure.Data;
using SFA.DAS.CollectionEarnings.Calculator.Infrastructure.Data.Entities;
using SFA.DAS.CollectionEarnings.Calculator.IntegrationTests.Tools;
using SFA.DAS.CollectionEarnings.Calculator.UnitTests.Tools.Builders;

namespace SFA.DAS.CollectionEarnings.Calculator.IntegrationTests.Infrastructure.Data.Repositories.ProcessedLearningDeliveryRepository.AddProcessedLearningDeliveries
{
    public class WhenCalled
    {
        private static readonly object[] InvalidProcessedLearningDeliveriesWithExpectedExceptionTypes =
        {
            new object[] {
                // Duplicate PK
                new[]
                {
                    new ApprenticeshipPriceEpisodeEntityBuilder().Build(),
                    new ApprenticeshipPriceEpisodeEntityBuilder().Build()
                },
                typeof(SqlException)
            },
            new object[]
            {
                // Out of bounds InstallmentValue
                new[]
                {
                    new ApprenticeshipPriceEpisodeEntityBuilder().WithPriceEpisodeInstalmentValue(12345678901.00m).Build()
                },
                typeof(InvalidOperationException)
            },
            new object[]
            {
                // Out of bounds CompletionElement
                new[]
                {
                    new ApprenticeshipPriceEpisodeEntityBuilder().WithPriceEpisodeCompletionElement(12345678901.00m).Build()
                },
                typeof(InvalidOperationException)
            },
            new object[]
            {
                // Out of bounds TotalTnp
                new[]
                {
                    new ApprenticeshipPriceEpisodeEntityBuilder().WithPriceEpisodeTotalTnpPrice(12345678901.00m).Build()
                },
                typeof(InvalidOperationException)
            },
            new object[]
            {
                // Out of bounds Tnp1
                new[]
                {
                    new ApprenticeshipPriceEpisodeEntityBuilder().WithTnp1(12345678901.00m).Build()
                },
                typeof(InvalidOperationException)
            },
            new object[]
            {
                // Out of bounds Tnp2
                new[]
                {
                    new ApprenticeshipPriceEpisodeEntityBuilder().WithTnp2(12345678901.00m).Build()
                },
                typeof(InvalidOperationException)
            },
            new object[]
            {
                // Out of bounds Tnp3
                new[]
                {
                    new ApprenticeshipPriceEpisodeEntityBuilder().WithTnp3(12345678901.00m).Build()
                },
                typeof(InvalidOperationException)
            },
            new object[]
            {
                // Out of bounds Tnp4
                new[]
                {
                    new ApprenticeshipPriceEpisodeEntityBuilder().WithTnp4(12345678901.00m).Build()
                },
                typeof(InvalidOperationException)
            }
        };

        private readonly string _transientConnectionString = GlobalTestContext.Instance.ConnectionString;

        private IApprenticeshipPriceEpisodeRepository _repository;

        [SetUp]
        public void Arrange()
        {
            TestDataHelper.Clean();

            _repository = new Calculator.Infrastructure.Data.Repositories.ApprenticeshipPriceEpisodeRepository(_transientConnectionString);
        }

        [Test]
        public void ThenExpectingDatabaseEntriesForAddProcessedLearningDeliveriesWithValidInput()
        {
            // Arrange
            var deliveries = new[]
            {
                new ApprenticeshipPriceEpisodeEntityBuilder().WithLearnRefNumber("1").Build(),
                new ApprenticeshipPriceEpisodeEntityBuilder().WithLearnRefNumber("2").Build(),
                new ApprenticeshipPriceEpisodeEntityBuilder().WithLearnRefNumber("3").Build(),
                new ApprenticeshipPriceEpisodeEntityBuilder().WithLearnRefNumber("4").Build(),
                new ApprenticeshipPriceEpisodeEntityBuilder().WithLearnRefNumber("5").Build()
            };

            // Act
            _repository.AddApprenticeshipPriceEpisodes(deliveries);

            // Assert
            var rows = TestDataHelper.GetApprenticeshipPriceEpisodes();

            Assert.IsNotNull(rows);
            Assert.AreEqual(5, rows.Length);
        }

        [Test]
        [TestCaseSource(nameof(InvalidProcessedLearningDeliveriesWithExpectedExceptionTypes))]
        public void ThenExpectingExceptionForAddProcessedLearningDeliveriesWithInvalidInput(ApprenticeshipPriceEpisodeEntity[] entities, Type exceptionType)
        {
            // Assert
            Assert.Throws(exceptionType, () => _repository.AddApprenticeshipPriceEpisodes(entities));
        }
    }
}