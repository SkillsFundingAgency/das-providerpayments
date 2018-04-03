using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using NLog;
using NUnit.Framework;
using SFA.DAS.Events.Api.Client;
using SFA.DAS.Events.Api.Types;
using SFA.DAS.Payments.Reference.Commitments.Application.GetNextBatchOfCommitmentEventsQuery;

namespace SFA.DAS.Payments.Reference.Commitments.UnitTests.Application.GetNextBatchOfCommitmentEventsQuery.GetNextBatchOfCommitmentEventsQueryHandler
{
    public class WhenHandlingRequest
    {
        private GetNextBatchOfCommitmentEventsQueryRequest _request;

        private Mock<IEventsApi> _eventsApiClient;

        private Mock<ILogger> _logger;

        private Commitments.Application.GetNextBatchOfCommitmentEventsQuery.GetNextBatchOfCommitmentEventsQueryHandler _handler;

        [SetUp]
        public void Arrange()
        {
            _request = new GetNextBatchOfCommitmentEventsQueryRequest
            {
                LastSeenEventId = 123
            };

            _eventsApiClient = new Mock<IEventsApi>();
            _eventsApiClient.Setup(c => c.GetApprenticeshipEventsById(123, 1000, 1))
                .Returns(Task.FromResult(new List<ApprenticeshipEventView>
                {
                    new ApprenticeshipEventView()
                }));

            _logger = new Mock<ILogger>();

            _handler = new Commitments.Application.GetNextBatchOfCommitmentEventsQuery.GetNextBatchOfCommitmentEventsQueryHandler(
                _eventsApiClient.Object, _logger.Object);
        }

        [Test]
        public void ThenItShouldRequestPageFromApiStartingAtLastEventId()
        {
            // Act
            _handler.Handle(_request);

            // Assert
            _eventsApiClient.Verify(c => c.GetApprenticeshipEventsById(123, 1000, 1), Times.Once);
        }

        [Test]
        public void ThenItShouldReturnAValidResponseWithTheQueryResults()
        {
            // Act
            var actual = _handler.Handle(_request);

            // Assert
            Assert.IsNotNull(actual);
            Assert.IsTrue(actual.IsValid);
            Assert.IsNotNull(actual.Items);
            Assert.AreEqual(1, actual.Items.Length);
        }

        [Test]
        public void ThenItShouldReturnAnInvalidResponseIfAnExceptionOccurs()
        {
            // Arrange
            _eventsApiClient.Setup(c => c.GetApprenticeshipEventsById(123, 1000, 1))
                .ThrowsAsync(new System.Exception("Api is broken"));

            // Act
            var actual = _handler.Handle(_request);

            // Assert
            Assert.IsNotNull(actual);
            Assert.IsFalse(actual.IsValid);
            Assert.IsNotNull(actual.Exception);
            Assert.AreEqual("Api is broken", actual.Exception.Message);
        }

        [Test]
        public void ThenItShouldExcludeEventsWhereTheEffectiveFromIsAfterTheEffectiveTo()
        {
            // Arrange
            var effectiveDate = DateTime.Today;
            _eventsApiClient.Setup(c => c.GetApprenticeshipEventsById(123, 1000, 1))
                .Returns(Task.FromResult(new List<ApprenticeshipEventView>
                {
                    new ApprenticeshipEventView
                    {
                        PriceHistory = new List<PriceHistory>
                        {
                            new PriceHistory
                            {
                                EffectiveFrom = effectiveDate,
                                EffectiveTo = effectiveDate.AddDays(-1)
                            }
                        }
                    }
                }));

            // Act
            var actual = _handler.Handle(_request);

            // Assert
            Assert.IsNotNull(actual);
            Assert.IsTrue(actual.IsValid);
            Assert.IsNull(actual.Exception);
            Assert.AreEqual(1, actual.Items.Length);
            var item = actual.Items.First();
            Assert.AreEqual(0, item.PriceHistory.Count());
        }

        [Test]
        public void ThenItShouldIncludeEventsWhereTheEffectiveFromIsBeforeTheEffectiveTo()
        {
            // Arrange
            var effectiveDate = DateTime.Today;
            _eventsApiClient.Setup(c => c.GetApprenticeshipEventsById(123, 1000, 1))
                .Returns(Task.FromResult(new List<ApprenticeshipEventView>
                {
                    new ApprenticeshipEventView
                    {
                        PriceHistory = new List<PriceHistory>
                        {
                            new PriceHistory
                            {
                                EffectiveFrom = effectiveDate.AddDays(-1),
                                EffectiveTo = effectiveDate
                            }
                        }
                    }
                }));

            // Act
            var actual = _handler.Handle(_request);

            // Assert
            Assert.IsNotNull(actual);
            Assert.IsTrue(actual.IsValid);
            Assert.IsNull(actual.Exception);
            Assert.AreEqual(1, actual.Items.Length);
            var item = actual.Items.First();
            Assert.AreEqual(1, item.PriceHistory.Count());
        }
    }
}