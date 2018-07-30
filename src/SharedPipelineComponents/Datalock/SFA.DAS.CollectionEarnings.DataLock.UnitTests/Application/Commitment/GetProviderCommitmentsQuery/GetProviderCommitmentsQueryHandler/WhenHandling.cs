using System;
using Moq;
using NUnit.Framework;
using SFA.DAS.CollectionEarnings.DataLock.Application.Commitment.GetProviderCommitmentsQuery;
using SFA.DAS.CollectionEarnings.DataLock.UnitTests.Tools.Entities;
using SFA.DAS.CollectionEarnings.DataLock.UnitTests.Utilities.Entities;
using SFA.DAS.ProviderPayments.Calc.Shared.Infrastructure.Data.Entities;
using SFA.DAS.ProviderPayments.Calc.Shared.Infrastructure.Data.Repositories;

namespace SFA.DAS.CollectionEarnings.DataLock.UnitTests.Application.Commitment.GetProviderCommitmentsQuery.GetProviderCommitmentsQueryHandler
{
    public class WhenHandling
    {
        private readonly long _ukprn = 10007459;

        private Mock<ICommitmentRepository> _commitmentRepository;

        private GetProviderCommitmentsQueryRequest _request;
        private CollectionEarnings.DataLock.Application.Commitment.GetProviderCommitmentsQuery.GetProviderCommitmentsQueryHandler _handler;

        private readonly CommitmentEntity _commitmentEntity = new CommitmentEntityBuilder().Build();

        [SetUp]
        public void Arrange()
        {
            _commitmentRepository = new Mock<ICommitmentRepository>();

            _commitmentRepository
                .Setup(cr => cr.GetProviderCommitments(_ukprn))
                .Returns(new[] { _commitmentEntity });

            _request = new GetProviderCommitmentsQueryRequest
            {
                Ukprn = _ukprn
            };

            _handler = new CollectionEarnings.DataLock.Application.Commitment.GetProviderCommitmentsQuery.GetProviderCommitmentsQueryHandler(_commitmentRepository.Object);
        }

        [Test]
        public void ThenValidResponseReturnedForValidRepositoryResponse()
        {
            // Act
            var response = _handler.Handle(_request);

            // Assert
            Assert.IsNotNull(response);
            Assert.IsTrue(response.IsValid);
            Assert.IsNull(response.Exception);
            Assert.AreEqual(1, response.Items.Length);
        }

        [Test]
        public void ThenItShouldReturnTheCorrectCommitmentInformation()
        {
            // Act
            var response = _handler.Handle(_request);

            // Assert
            Assert.IsNotNull(response);
            Assert.IsTrue(response.IsValid);

            Assert.AreEqual(_commitmentEntity.CommitmentId, response.Items[0].CommitmentId);
            Assert.AreEqual(_commitmentEntity.VersionId, response.Items[0].VersionId);
            Assert.AreEqual(_commitmentEntity.Uln, response.Items[0].Uln);
            Assert.AreEqual(_commitmentEntity.Ukprn, response.Items[0].Ukprn);
            Assert.AreEqual(_commitmentEntity.AccountId, response.Items[0].AccountId);
            Assert.AreEqual(_commitmentEntity.StartDate, response.Items[0].StartDate);
            Assert.AreEqual(_commitmentEntity.EndDate, response.Items[0].EndDate);
            Assert.AreEqual(_commitmentEntity.AgreedCost, response.Items[0].NegotiatedPrice);
            Assert.AreEqual(_commitmentEntity.StandardCode, response.Items[0].StandardCode);
            Assert.AreEqual(_commitmentEntity.ProgrammeType, response.Items[0].ProgrammeType);
            Assert.AreEqual(_commitmentEntity.FrameworkCode, response.Items[0].FrameworkCode);
            Assert.AreEqual(_commitmentEntity.PathwayCode, response.Items[0].PathwayCode);
            Assert.AreEqual(_commitmentEntity.PaymentStatus, response.Items[0].PaymentStatus);
            Assert.AreEqual(_commitmentEntity.PaymentStatusDescription, response.Items[0].PaymentStatusDescription);
            Assert.AreEqual(_commitmentEntity.Priority, response.Items[0].Priority);
            Assert.AreEqual(_commitmentEntity.EffectiveFrom, response.Items[0].EffectiveFrom);
            Assert.AreEqual(_commitmentEntity.EffectiveTo, response.Items[0].EffectiveTo);
        }

        [Test]
        public void ThenInvalidResponseReturnedForInvalidRepositoryResponse()
        {
            // Arrange
            _commitmentRepository
                .Setup(cr => cr.GetProviderCommitments(_ukprn))
                .Throws(new Exception("Exception while reading commitments."));

            // Act
            var response = _handler.Handle(_request);

            // Assert
            Assert.IsNotNull(response);
            Assert.IsFalse(response.IsValid);
            Assert.IsNotNull(response.Exception);
            Assert.IsNull(response.Items);
        }
    }
}