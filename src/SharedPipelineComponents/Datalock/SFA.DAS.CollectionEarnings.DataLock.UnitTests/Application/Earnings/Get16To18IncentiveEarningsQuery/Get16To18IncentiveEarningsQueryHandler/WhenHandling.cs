using System;
using Moq;
using NUnit.Framework;
using SFA.DAS.CollectionEarnings.DataLock.Application.Earnings.Get16To18IncentiveEarningsQuery;
using SFA.DAS.CollectionEarnings.DataLock.Infrastructure.Data;
using SFA.DAS.CollectionEarnings.DataLock.UnitTests.Tools.Entities;
using SFA.DAS.CollectionEarnings.DataLock.UnitTests.Tools.Application;

namespace SFA.DAS.CollectionEarnings.DataLock.UnitTests.Application.Earnings.Get16To18IncentiveEarningsQuery.Get16To18IncentiveEarningsQueryHandler
{
    public class WhenHandling
    {
        private readonly long _ukprn = 10007459;

        private Mock<IIncentiveEarningsRepository> _incentiveEarningsRepository;

        private Get16To18IncentiveEarningsQueryRequest _request;
        private CollectionEarnings.DataLock.Application.Earnings.Get16To18IncentiveEarningsQuery.Get16To18IncentiveEarningsQueryHandler _handler;

        [SetUp]
        public void Arrange()
        {
            _incentiveEarningsRepository = new Mock<IIncentiveEarningsRepository>();

            _request = new Get16To18IncentiveEarningsQueryRequest
            {
                Ukprn = _ukprn
            };

            _handler = new CollectionEarnings.DataLock.Application.Earnings.Get16To18IncentiveEarningsQuery.Get16To18IncentiveEarningsQueryHandler(_incentiveEarningsRepository.Object);
        }

        [Test]
        public void ThenValidResponseReturnedForValidRepositoryResponse()
        {
            // Arrange
            _incentiveEarningsRepository
                .Setup(dlr => dlr.GetIncentiveEarnings(_ukprn))
                .Returns(new[] {new IncentiveEarningsEntityBuilder().Build()});

            // Act
            var response = _handler.Handle(_request);

            // Assert
            Assert.IsNotNull(response);
            Assert.IsTrue(response.IsValid);
            Assert.IsNull(response.Exception);
            Assert.AreEqual(1, response.Items.Length);
        }

        [Test]
        public void ThenInvalidResponseReturnedForInvalidRepositoryResponse()
        {
            // Arrange
            _incentiveEarningsRepository
                .Setup(dlr => dlr.GetIncentiveEarnings(_ukprn))
                .Throws<Exception>();

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