using System;
using Moq;
using NUnit.Framework;
using SFA.DAS.Payments.Calc.ProviderAdjustments.Application.Adjustments.GetCurrentAdjustmentsQuery;
using SFA.DAS.Payments.Calc.ProviderAdjustments.Infrastructure.Data;
using SFA.DAS.Payments.Calc.ProviderAdjustments.Infrastructure.Data.Entities;

namespace SFA.DAS.Payments.Calc.ProviderAdjustments.UnitTests.Application.Adjustments.GetCurrentAdjustmentsQuery.GetCurrentAdjustmentsQueryHandler
{
    public class WhenHandling
    {
        private static readonly long Ukprn = 10007459;

        private static readonly AdjustmentEntity[] AdjustmentEntities =
        {
            new AdjustmentEntity
            {
                Ukprn = Ukprn,
                SubmissionId = "abc",
                SubmissionCollectionPeriod = 1,
                PaymentType = 1,
                PaymentTypeName = "type",
                Amount = 123.45m
            }
        };

        private static readonly object[] RepositoryResponses =
        {
            new object[] { AdjustmentEntities },
            new object[] { null }
        };

        private Mock<IAdjustmentRepository> _repository;
        private GetCurrentAdjustmentsQueryRequest _request;
        private ProviderAdjustments.Application.Adjustments.GetCurrentAdjustmentsQuery.GetCurrentAdjustmentsQueryHandler _handler;

        [SetUp]
        public void Arrange()
        {
            _request = new GetCurrentAdjustmentsQueryRequest
            {
                Ukprn = Ukprn
            };

            _repository = new Mock<IAdjustmentRepository>();

            _handler = new ProviderAdjustments.Application.Adjustments.GetCurrentAdjustmentsQuery.GetCurrentAdjustmentsQueryHandler(_repository.Object);
        }

        [Test]
        [TestCaseSource(nameof(RepositoryResponses))]
        public void ThenValidGetPreviousAdjustmentsQueryResponseReturnedForValidRepositoryResponse(AdjustmentEntity[] entities)
        {
            // Arrange
            _repository.Setup(r => r.GetCurrentProviderAdjustments(Ukprn))
                .Returns(entities);

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
            _repository.Setup(r => r.GetCurrentProviderAdjustments(Ukprn))
                .Returns(AdjustmentEntities);

            // Act
            var response = _handler.Handle(_request);

            // Assert
            Assert.IsNotNull(response?.Items);
            Assert.AreEqual(1, response.Items.Length);
            Assert.AreEqual(AdjustmentEntities[0].Ukprn, response.Items[0].Ukprn);
            Assert.AreEqual(AdjustmentEntities[0].SubmissionId, response.Items[0].SubmissionId);
            Assert.AreEqual(AdjustmentEntities[0].SubmissionCollectionPeriod, response.Items[0].SubmissionCollectionPeriod);
            Assert.AreEqual(AdjustmentEntities[0].PaymentType, response.Items[0].PaymentType);
            Assert.AreEqual(AdjustmentEntities[0].PaymentTypeName, response.Items[0].PaymentTypeName);
            Assert.AreEqual(AdjustmentEntities[0].Amount, response.Items[0].Amount);
        }

        [Test]
        public void ThenInvalidGetCurrentCollectionPeriodQueryResponseReturnedForInvalidRepositoryResponse()
        {
            // Arrange
            _repository.Setup(r => r.GetCurrentProviderAdjustments(Ukprn))
                .Throws<Exception>();

            // Act
            var response = _handler.Handle(_request);

            // Assert
            Assert.IsFalse(response.IsValid);
            Assert.IsNull(response.Items);
            Assert.IsNotNull(response.Exception);
        }
    }
}