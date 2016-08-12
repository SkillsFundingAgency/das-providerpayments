using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Moq;
using NUnit.Framework;
using SFA.DAS.ProdiverPayments.Application.PeriodEnd.GetPageOfPeriodEndsQuery;
using SFA.DAS.ProdiverPayments.Domain;
using SFA.DAS.ProviderPayments.Api.Orchestrators;
using SFA.DAS.ProviderPayments.Api.Plumbing.WebApi;

namespace SFA.DAS.ProviderPayments.Api.UnitTests.Orchestrators.NotificationsOrchestratorTests
{
    public class WhenGettingPageOfPeriodEndNotifications
    {
        private Mock<IMediator> _mediator;
        private Mock<ILinkBuilder> _linkBuilder;
        private NotificationsOrchestrator _notificationsOrchestrator;

        [SetUp]
        public void Arrange()
        {
            _mediator = new Mock<IMediator>();
            _mediator.Setup(m => m.SendAsync(It.Is<GetPageOfPeriodEndsQueryRequest>(r => r.PageNumber == 1)))
                .Returns(Task.FromResult(new GetPageOfPeriodEndsQueryResponse
                {
                    TotalNumberOfItems = 1,
                    TotalNumberOfPages = 1,
                    Items = new[]
                    {
                        new PeriodEnd { PeriodCode = "201704" }
                    }
                }));

            _linkBuilder = new Mock<ILinkBuilder>();
            _linkBuilder.Setup(b => b.GetPeriodEndNotificationPageLink(It.IsAny<int>()))
                .Returns((int pageNumber) => $"/{pageNumber}");

            _notificationsOrchestrator = new NotificationsOrchestrator(_mediator.Object, _linkBuilder.Object);
        }

        [Test]
        public async Task ThenItShouldReturnAnInstanceOfAHalPage()
        {
            // Act
            var actual = await _notificationsOrchestrator.GetPageOfPeriodEndNotifications(1);

            // Assert
            Assert.IsNotNull(actual);
        }

        [Test]
        public async Task ThenItShouldReturnAPageWithTheCorrectItems()
        {
            // Act
            var actual = await _notificationsOrchestrator.GetPageOfPeriodEndNotifications(1);

            // Assert
            Assert.AreEqual(1, actual.Count);
            Assert.AreEqual("201704", actual.PageItems.Items.ElementAt(0).PeriodCode);
        }

        [TestCase(1, 1, null, null, "/1", "/1")]
        [TestCase(1, 2, "/2", null, "/1", "/2")]
        [TestCase(2, 2, null, "/1", "/1", "/2")]
        [TestCase(5, 10, "/6", "/4", "/1", "/10")]
        public async Task ThenItShouldReturnCorrectLinks(int pageNumber, int totalNumberOfPages, 
            string expectedNextLink, string expectedPrevLink, string expectedFirstLink, string expectedLastLink)
        {
            // Arrange
            _mediator.Setup(m => m.SendAsync(It.Is<GetPageOfPeriodEndsQueryRequest>(r => r.PageNumber == pageNumber)))
                .Returns(Task.FromResult(new GetPageOfPeriodEndsQueryResponse
                {
                    TotalNumberOfItems = 1,
                    TotalNumberOfPages = totalNumberOfPages,
                    Items = new[]
                    {
                        new PeriodEnd { PeriodCode = "201704" }
                    }
                }));

            // Act
            var actual = await _notificationsOrchestrator.GetPageOfPeriodEndNotifications(pageNumber);

            // Assert
            Assert.AreEqual(expectedNextLink, actual.Links.Next?.Href);
            Assert.AreEqual(expectedPrevLink, actual.Links.Prev?.Href);
            Assert.AreEqual(expectedFirstLink, actual.Links.First?.Href);
            Assert.AreEqual(expectedLastLink, actual.Links.Last?.Href);
        }
    }
}
