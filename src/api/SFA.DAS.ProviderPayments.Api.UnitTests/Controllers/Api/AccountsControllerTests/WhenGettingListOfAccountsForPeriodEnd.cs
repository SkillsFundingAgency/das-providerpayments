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

namespace SFA.DAS.ProviderPayments.Api.UnitTests.Controllers.Api.AccountsControllerTests
{
    public class WhenGettingListOfAccountsForPeriodEnd
    {
        private const string PeriodCode = "201704";
        private static readonly IEnumerable<string[]> BadRequestFailureMessageCases = new[]
        {
            new[] {"Invalid PeriodCode"},
            new[] { "Invalid PeriodCode", "Invalid Page Number"}
        };

        private AccountsController _controller;
        private Mock<AccountsOrchestrator> _orchestrator;

        [SetUp]
        public void Arrange()
        {
            _orchestrator = new Mock<AccountsOrchestrator>();
            _orchestrator.Setup(o => o.GetPageOfAccountsAffectedInPeriod(PeriodCode, 1))
                .Returns(Task.FromResult(new HalPage<AccountDto>
                {
                    Count = 17,
                    Content = new HalPageItems<AccountDto>
                    {
                        Items = new[]
                        {
                            new AccountDto
                            {
                                Id = "MyAccount",
                                Links = new PaymentEntityLinks
                                {
                                    Current = new HalLink {Href = "/Current"},
                                    Self = new HalLink {Href = "/Self"},
                                    Payments = new HalLink {Href = "/Payments"}
                                }
                            }
                        }
                    },
                    Links = new HalPageLinks
                    {
                        Next = new HalLink { Href = "/next" },
                        Prev = new HalLink { Href = "/prev" },
                        First = new HalLink { Href = "/first" },
                        Last = new HalLink { Href = "/last" }
                    }
                }));

            _controller = new AccountsController(_orchestrator.Object);
        }

        [Test]
        public async Task ThenItShouldReturnOkResultContainingAHalPageOfAccounts()
        {
            // Act
            var actual = await _controller.Get(PeriodCode);

            // Assert
            Assert.IsNotNull(actual);
            Assert.IsInstanceOf<OkNegotiatedContentResult<HalPage<AccountDto>>>(actual);
        }

        [Test]
        public async Task ThenThePageShouldHaveTheCorrectCount()
        {
            // Act
            var actual = (await _controller.Get(PeriodCode)) as OkNegotiatedContentResult<HalPage<AccountDto>>;

            // Assert
            Assert.AreEqual(17, actual.Content.Count);
        }

        [Test]
        public async Task ThenThePageShouldHaveTheCorrectContent()
        {
            // Act
            var actual = (await _controller.Get(PeriodCode)) as OkNegotiatedContentResult<HalPage<AccountDto>>;

            // Assert
            Assert.IsNotNull(actual.Content.Content);
            Assert.IsNotNull(actual.Content.Content.Items);
            Assert.AreEqual(1, actual.Content.Content.Items.Count());

            var actualAccount = actual.Content.Content.Items.First();
            Assert.AreEqual("MyAccount", actualAccount.Id);
            Assert.AreEqual("/Current", actualAccount.Links?.Current?.Href);
            Assert.AreEqual("/Self", actualAccount.Links?.Self?.Href);
            Assert.AreEqual("/Payments", actualAccount.Links?.Payments?.Href);
        }

        [Test]
        public async Task ThenThePageShouldHaveTheCorrectLinks()
        {
            // Act
            var actual = (await _controller.Get(PeriodCode)) as OkNegotiatedContentResult<HalPage<AccountDto>>;

            // Assert
            Assert.IsNotNull(actual.Content.Links);
            Assert.AreEqual("/next", actual.Content.Links.Next?.Href);
            Assert.AreEqual("/prev", actual.Content.Links.Prev?.Href);
            Assert.AreEqual("/first", actual.Content.Links.First?.Href);
            Assert.AreEqual("/last", actual.Content.Links.Last?.Href);
        }

        [Test]
        [TestCaseSource("BadRequestFailureMessageCases")]
        public async Task AndABadRequestExceptionIsThrowThenItShouldReturnABadRequestResultWithErrorMessage(string[] failureMessages)
        {
            // Arrange
            _orchestrator.Setup(o => o.GetPageOfAccountsAffectedInPeriod(PeriodCode, 1))
                .Throws(new BadRequestException(failureMessages.Select(m => new ValidationFailure { Description = m })));

            // Act
            var actual = await _controller.Get(PeriodCode);

            // Assert
            var expectedMessage = failureMessages.Aggregate((x, y) => $"{x}\n{y}");
            Assert.IsNotNull(actual);
            Assert.IsInstanceOf<BadRequestErrorMessageResult>(actual);
            Assert.AreEqual(expectedMessage, ((BadRequestErrorMessageResult) actual).Message);
        }

        [Test]
        public async Task AndAPageNotFoundExceptionIsThrowThenItShouldReturnANotFoundResult()
        {
            // Arrange
            _orchestrator.Setup(o => o.GetPageOfAccountsAffectedInPeriod(PeriodCode, 1))
                .Throws(new PageNotFoundException());

            // Act
            var actual = await _controller.Get(PeriodCode);

            // Assert
            Assert.IsNotNull(actual);
            Assert.IsInstanceOf<NotFoundResult>(actual);
        }

        [Test]
        public async Task AndAPeriodNotFoundExceptionIsThrowThenItShouldReturnANotFoundResult()
        {
            // Arrange
            _orchestrator.Setup(o => o.GetPageOfAccountsAffectedInPeriod(PeriodCode, 1))
                .Throws(new PeriodNotFoundException());

            // Act
            var actual = await _controller.Get(PeriodCode);

            // Assert
            Assert.IsNotNull(actual);
            Assert.IsInstanceOf<NotFoundResult>(actual);
        }

        [Test]
        public async Task AndAUnhandledExceptionIsThrowThenItShouldReturnAInternalServerErrorResult()
        {
            // Arrange
            _orchestrator.Setup(o => o.GetPageOfAccountsAffectedInPeriod(PeriodCode, 1))
                .Throws(new System.Exception());

            // Act
            var actual = await _controller.Get(PeriodCode);

            // Assert
            Assert.IsNotNull(actual);
            Assert.IsInstanceOf<InternalServerErrorResult>(actual);
        }
    }
}
