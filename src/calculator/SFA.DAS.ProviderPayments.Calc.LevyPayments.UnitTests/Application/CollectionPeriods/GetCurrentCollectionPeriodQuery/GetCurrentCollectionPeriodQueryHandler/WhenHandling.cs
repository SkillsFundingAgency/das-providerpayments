using System;
using Moq;
using NUnit.Framework;
using SFA.DAS.ProviderPayments.Calc.LevyPayments.Application.CollectionPeriods.GetCurrentCollectionPeriodQuery;
using SFA.DAS.ProviderPayments.Calc.LevyPayments.Infrastructure.Data;
using SFA.DAS.ProviderPayments.Calc.LevyPayments.Infrastructure.Data.Entities;

namespace SFA.DAS.ProviderPayments.Calc.LevyPayments.UnitTests.Application.CollectionPeriods.GetCurrentCollectionPeriodQuery.GetCurrentCollectionPeriodQueryHandler
{
    public class WhenHandling
    {
        private static readonly CollectionPeriodEntity PeriodEntity = new CollectionPeriodEntity
        {
            PeriodId = 1,
            Month = 8,
            Year = 2016
        };

        private static readonly object[] RepositoryResponses =
        {
            new object[] {PeriodEntity},
            new object[] {null}
        };

        private Mock<ICollectionPeriodRepository> _repository;
        private GetCurrentCollectionPeriodQueryRequest _request;
        private LevyPayments.Application.CollectionPeriods.GetCurrentCollectionPeriodQuery.GetCurrentCollectionPeriodQueryHandler _handler;

        [SetUp]
        public void Arrange()
        {
            _request = new GetCurrentCollectionPeriodQueryRequest();

            _repository = new Mock<ICollectionPeriodRepository>();

            _handler = new LevyPayments.Application.CollectionPeriods.GetCurrentCollectionPeriodQuery.GetCurrentCollectionPeriodQueryHandler(_repository.Object);
        }

        [Test]
        [TestCaseSource(nameof(RepositoryResponses))]
        public void ThenValidGetCurrentCollectionPeriodQueryResponseReturnedForValidRepositoryResponse(CollectionPeriodEntity period)
        {
            // Arrange
            _repository.Setup(r => r.GetCurrentCollectionPeriod())
                .Returns(period);

            // Act
            var response = _handler.Handle(_request);

            // Assert
            Assert.IsNotNull(response);
            Assert.IsTrue(response.IsValid);
        }

        [Test]
        public void ThenCurrentCollectionPeriodShouldBeInTheGetCurrentCollectionPeriodQueryResponse()
        {
            // Arrange
            _repository.Setup(r => r.GetCurrentCollectionPeriod())
                .Returns(PeriodEntity);

            // Act
            var response = _handler.Handle(_request);

            // Assert
            Assert.IsNotNull(response?.Period);
            Assert.AreEqual(PeriodEntity.PeriodId, response.Period.PeriodId);
            Assert.AreEqual(PeriodEntity.Month, response.Period.Month);
            Assert.AreEqual(PeriodEntity.Year, response.Period.Year);

        }

        [Test]
        public void ThenInvalidGetCurrentCollectionPeriodQueryResponseReturnedForInvalidRepositoryResponse()
        {
            // Arrange
            _repository.Setup(r => r.GetCurrentCollectionPeriod())
                .Throws<Exception>();

            // Act
            var response = _handler.Handle(_request);

            // Assert
            Assert.IsFalse(response.IsValid);
            Assert.IsNull(response.Period);
            Assert.IsNotNull(response.Exception);
        }
    }
}