using System.Threading.Tasks;
using System.Web.Http.Results;
using Moq;
using NUnit.Framework;
using SFA.DAS.ProviderPayments.Api.Controllers.Api;
using SFA.DAS.ProviderPayments.Api.Dto;
using SFA.DAS.ProviderPayments.Api.Orchestrators;

namespace SFA.DAS.ProviderPayments.Api.UnitTests.Controllers.Api.NotificationsControllerTests
{
    public class WhenGettingPeriodEnd
    {
        private PageOfPeriodEnds _result;
        private Mock<NotificationsOrchestrator> _notificationsOrchestrator;
        private NotificationsController _controller;

        [SetUp]
        public void Arrange()
        {
            _result = new PageOfPeriodEnds();

            _notificationsOrchestrator = new Mock<NotificationsOrchestrator>();
            _notificationsOrchestrator.Setup(o => o.GetPageOfPeriodEndNotifications(It.IsAny<int>()))
                .Returns(Task.FromResult(_result));

            _controller = new NotificationsController(_notificationsOrchestrator.Object);
        }

        [Test]
        public async Task ThenItShouldReturnOkResult()
        {
            // Act
            var actual = await _controller.PeriodEnd();

            // Assert
            Assert.IsNotNull(actual);
            Assert.IsInstanceOf<OkNegotiatedContentResult<PageOfPeriodEnds>>(actual);
        }

        [Test]
        public async Task ThenItShouldReturnPageFromOrchestrator()
        {
            // Act
            var actual = (await _controller.PeriodEnd()) as OkNegotiatedContentResult<PageOfPeriodEnds>;

            // Assert
            Assert.AreSame(_result, actual.Content);
        }

        [TestCase(1)]
        [TestCase(2)]
        [TestCase(10)]
        [TestCase(100)]
        [TestCase(int.MaxValue)]
        public async Task ThenItShouldCallOrcestratorWithCorrectPageNumber(int pageNumber)
        {
            // Act
            await _controller.PeriodEnd(pageNumber);

            // Assert
            _notificationsOrchestrator.Verify(o => o.GetPageOfPeriodEndNotifications(pageNumber), Times.Once);
        }
    }
}
