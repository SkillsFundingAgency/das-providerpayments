using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http.Results;
using Moq;
using NUnit.Framework;
using SFA.DAS.ProviderPayments.Api.Controllers.Api;
using SFA.DAS.ProviderPayments.Api.Dto;
using SFA.DAS.ProviderPayments.Api.Dto.Hal;
using SFA.DAS.ProviderPayments.Api.Orchestrators;
using SFA.DAS.ProviderPayments.Api.Orchestrators.OrchestratorExceptions;
using SFA.DAS.ProviderPayments.Application.Validation;

namespace SFA.DAS.ProviderPayments.Api.UnitTests.Controllers.Api.NotificationsControllerTests
{
    public class WhenGettingPeriodEnd
    {
        private static readonly IEnumerable<string[]> BadRequestFailureMessageCases = new[]
        {
            new[] {"Invalid PeriodCode"},
            new[] { "Invalid PeriodCode", "Invalid Page Number"}
        };

        private HalPage<PeriodEndDto> _result;
        private Mock<NotificationsOrchestrator> _orchestrator;
        private NotificationsController _controller;

        [SetUp]
        public void Arrange()
        {
            _result = new HalPage<PeriodEndDto>();

            _orchestrator = new Mock<NotificationsOrchestrator>();
            _orchestrator.Setup(o => o.GetPageOfPeriodEndNotifications(It.IsAny<int>()))
                .Returns(Task.FromResult(_result));

            _controller = new NotificationsController(_orchestrator.Object);
        }

        [Test]
        public async Task ThenItShouldReturnOkResult()
        {
            // Act
            var actual = await _controller.PeriodEnd();

            // Assert
            Assert.IsNotNull(actual);
            Assert.IsInstanceOf<OkNegotiatedContentResult<HalPage<PeriodEndDto>>>(actual);
        }

        [Test]
        public async Task ThenItShouldReturnPageFromOrchestrator()
        {
            // Act
            var actual = (await _controller.PeriodEnd()) as OkNegotiatedContentResult<HalPage<PeriodEndDto>>;

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
            _orchestrator.Verify(o => o.GetPageOfPeriodEndNotifications(pageNumber), Times.Once);
        }

        [Test]
        [TestCaseSource("BadRequestFailureMessageCases")]
        public async Task AndABadRequestExceptionIsThrowThenItShouldReturnABadRequestResultWithErrorMessage(string[] failureMessages)
        {
            // Arrange
            _orchestrator.Setup(o => o.GetPageOfPeriodEndNotifications(1))
                .Throws(new BadRequestException(failureMessages.Select(m => new ValidationFailure { Description = m })));

            // Act
            var actual = await _controller.PeriodEnd();

            // Assert
            var expectedMessage = failureMessages.Aggregate((x, y) => $"{x}\n{y}");
            Assert.IsNotNull(actual);
            Assert.IsInstanceOf<BadRequestErrorMessageResult>(actual);
            Assert.AreEqual(expectedMessage, ((BadRequestErrorMessageResult)actual).Message);
        }

        [Test]
        public async Task AndAPageNotFoundExceptionIsThrowThenItShouldReturnANotFoundResult()
        {
            // Arrange
            _orchestrator.Setup(o => o.GetPageOfPeriodEndNotifications(1))
                .Throws(new PageNotFoundException());

            // Act
            var actual = await _controller.PeriodEnd();

            // Assert
            Assert.IsNotNull(actual);
            Assert.IsInstanceOf<NotFoundResult>(actual);
        }

        [Test]
        public async Task AndAPeriodNotFoundExceptionIsThrowThenItShouldReturnANotFoundResult()
        {
            // Arrange
            _orchestrator.Setup(o => o.GetPageOfPeriodEndNotifications(1))
                .Throws(new PeriodNotFoundException());

            // Act
            var actual = await _controller.PeriodEnd();

            // Assert
            Assert.IsNotNull(actual);
            Assert.IsInstanceOf<NotFoundResult>(actual);
        }

        [Test]
        public async Task AndAUnhandledExceptionIsThrowThenItShouldReturnAInternalServerErrorResult()
        {
            // Arrange
            _orchestrator.Setup(o => o.GetPageOfPeriodEndNotifications(1))
                .Throws(new System.Exception());

            // Act
            var actual = await _controller.PeriodEnd();

            // Assert
            Assert.IsNotNull(actual);
            Assert.IsInstanceOf<InternalServerErrorResult>(actual);
        }
    }
}
