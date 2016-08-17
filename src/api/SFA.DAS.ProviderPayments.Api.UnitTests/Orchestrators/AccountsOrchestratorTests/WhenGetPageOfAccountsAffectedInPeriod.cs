using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Moq;
using NUnit.Framework;
using SFA.DAS.ProviderPayments.Api.Orchestrators;
using SFA.DAS.ProviderPayments.Api.Plumbing.WebApi;
using SFA.DAS.ProviderPayments.Application.Account.GetAccountsAffectedInPeriodQuery;
using SFA.DAS.ProviderPayments.Domain;

namespace SFA.DAS.ProviderPayments.Api.UnitTests.Orchestrators.AccountsOrchestratorTests
{
    public class WhenGetPageOfAccountsAffectedInPeriod
    {
        private const string PeriodCode = "201704";
        private const string AccountId = "SomeAccount";
        private const string AccountPaymentsLink = "/201704/SomeAccount/payments";

        private Account _account;
        private Mock<IMediator> _mediator;
        private Mock<ILinkBuilder> _linkBuilder;
        private AccountsOrchestrator _orchestrator;

        [SetUp]
        public void Arrange()
        {
            _account = new Account
            {
                Id = AccountId
            };

            _mediator = new Mock<IMediator>();
            _mediator.Setup(m => m.SendAsync(It.Is<GetAccountsAffectedInPeriodQueryRequest>(r => r.PeriodCode == PeriodCode && r.PageNumber == 1)))
                .Returns(Task.FromResult(new GetAccountsAffectedInPeriodQueryResponse
                {
                    TotalNumberOfItems = 1,
                    TotalNumberOfPages = 1,
                    Items = new[]
                    {
                        _account
                    }
                }));

            _linkBuilder = new Mock<ILinkBuilder>();
            _linkBuilder.Setup(b => b.GetPeriodEndAccountsPageLink(It.IsAny<int>()))
                .Returns((int pageNumber) => $"/{pageNumber}");
            _linkBuilder.Setup(b => b.GetAccountPaymentsLink(PeriodCode, AccountId))
                .Returns(AccountPaymentsLink);

            _orchestrator = new AccountsOrchestrator(_mediator.Object, _linkBuilder.Object);
        }

        [Test]
        public async Task ThenItShouldReturnAnInstanceOfAHalPage()
        {
            // Act
            var actual = await _orchestrator.GetPageOfAccountsAffectedInPeriod(PeriodCode, 1);

            // Assert
            Assert.IsNotNull(actual);
        }

        [Test]
        public async Task ThenItShouldReturnAPageWithTheCorrectItems()
        {
            // Act
            var actual = await _orchestrator.GetPageOfAccountsAffectedInPeriod(PeriodCode, 1);

            // Assert
            Assert.AreEqual(1, actual.Count);

            var actualAccount = actual.Content.Items.First();
            Assert.AreEqual(AccountId, actualAccount.Id);
            Assert.AreEqual(AccountPaymentsLink, actualAccount.Links.Payments?.Href);
        }

        [TestCase(1, 1, null, null, "/1", "/1")]
        [TestCase(1, 2, "/2", null, "/1", "/2")]
        [TestCase(2, 2, null, "/1", "/1", "/2")]
        [TestCase(5, 10, "/6", "/4", "/1", "/10")]
        public async Task ThenItShouldReturnCorrectLinks(int pageNumber, int totalNumberOfPages,
            string expectedNextLink, string expectedPrevLink, string expectedFirstLink, string expectedLastLink)
        {
            // Arrange
            _mediator.Setup(m => m.SendAsync(It.Is<GetAccountsAffectedInPeriodQueryRequest>(r => r.PeriodCode == PeriodCode && r.PageNumber == pageNumber)))
                .Returns(Task.FromResult(new GetAccountsAffectedInPeriodQueryResponse
                {
                    TotalNumberOfItems = 1,
                    TotalNumberOfPages = totalNumberOfPages,
                    Items = new[]
                    {
                        _account
                    }
                }));

            // Act
            var actual = await _orchestrator.GetPageOfAccountsAffectedInPeriod(PeriodCode, pageNumber);

            // Assert
            Assert.AreEqual(expectedNextLink, actual.Links.Next?.Href);
            Assert.AreEqual(expectedPrevLink, actual.Links.Prev?.Href);
            Assert.AreEqual(expectedFirstLink, actual.Links.First?.Href);
            Assert.AreEqual(expectedLastLink, actual.Links.Last?.Href);
        }

        
    }
}
