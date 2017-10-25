using System;
using Moq;
using NUnit.Framework;
using SFA.DAS.CollectionEarnings.DataLock.Infrastructure.Data;
using SFA.DAS.CollectionEarnings.DataLock.Infrastructure.Data.Entities;
using SFA.DAS.CollectionEarnings.DataLock.Application.DasAccount.GetDasAccountsQuery;

namespace SFA.DAS.CollectionEarnings.DataLock.UnitTests.Application.DasAccount.GetDasAccountsQuery.GetDasAccountsQueryHandler
{
    public class WhenHandling
    {
        private static readonly DasAccounEntity[] DasAccountEntities =
        {
            new DasAccounEntity {AccountId = 123, IsLevyPayer =true},
            new DasAccounEntity {AccountId= 456, IsLevyPayer =false}
        };

        private static readonly object[] RepositoryResponses =
        {
            new object[] {DasAccountEntities},
            new object[] {null}
        };

        private Mock<IDasAccountRepository> _repository;
        private GetDasAccountsQueryRequest _request;
        private SFA.DAS.CollectionEarnings.DataLock.Application.DasAccount.GetDasAccountsQuery.GetDasAccountsQueryHandler _handler;

        [SetUp]
        public void Arrange()
        {
            _request = new GetDasAccountsQueryRequest();

            _repository = new Mock<IDasAccountRepository>();

            _handler = new CollectionEarnings.DataLock.Application.DasAccount.GetDasAccountsQuery.GetDasAccountsQueryHandler(_repository.Object);
        }

        [Test]
        [TestCaseSource(nameof(RepositoryResponses))]
        public void ThenValidGetDasAccountQueryResponseReturnedForValidRepositoryResponse(DasAccounEntity[] DasAccount)
        {
            // Arrange
            _repository = new Mock<IDasAccountRepository>();
            _repository.Setup(r => r.GetDasAccounts())
                .Returns(DasAccount);

            // Act
            var response = _handler.Handle(_request);

            // Assert
            Assert.IsNotNull(response);
            Assert.IsTrue(response.IsValid);
        }

        [Test]
        public void ThenDasAccountShouldBeInTheGetDasAccountQueryResponse()
        {
            // Arrange
            _repository.Setup(r => r.GetDasAccounts())
                .Returns(DasAccountEntities);

            // Act
            var response = _handler.Handle(_request);

            // Assert
            Assert.IsNotNull(response?.Items);
            Assert.AreEqual(DasAccountEntities[0].AccountId, response.Items[0].AccountId);
            Assert.AreEqual(DasAccountEntities[1].AccountId, response.Items[1].AccountId);

            Assert.AreEqual(DasAccountEntities[0].IsLevyPayer, response.Items[0].IsLevyPayer);
            Assert.AreEqual(DasAccountEntities[1].IsLevyPayer, response.Items[1].IsLevyPayer);
        }
    

        [Test]
        public void ThenInvalidGetDasAccountQueryResponseReturnedForInvalidRepositoryResponse()
        {
            // Arrange
            _repository.Setup(r => r.GetDasAccounts())
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