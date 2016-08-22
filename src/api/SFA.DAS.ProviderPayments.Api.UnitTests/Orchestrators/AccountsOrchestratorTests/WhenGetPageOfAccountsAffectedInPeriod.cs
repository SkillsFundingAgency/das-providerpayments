using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Moq;
using NLog;
using NUnit.Framework;
using SFA.DAS.ProviderPayments.Api.Dto;
using SFA.DAS.ProviderPayments.Api.Orchestrators;
using SFA.DAS.ProviderPayments.Api.Orchestrators.OrchestratorExceptions;
using SFA.DAS.ProviderPayments.Api.Plumbing.WebApi;
using SFA.DAS.ProviderPayments.Application.Account.GetAccountsAffectedInPeriodQuery;
using SFA.DAS.ProviderPayments.Application.Validation;
using SFA.DAS.ProviderPayments.Application.Validation.Failures;
using SFA.DAS.ProviderPayments.Domain;
using SFA.DAS.ProviderPayments.Domain.Mapping;

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
        private Mock<ILogger> _logger;
        private AccountsOrchestrator _orchestrator;
        private Mock<IMapper> _mapper;

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
                    IsValid = true,
                    TotalNumberOfItems = 1,
                    TotalNumberOfPages = 1,
                    Items = new[]
                    {
                        _account
                    }
                }));

            _mapper = new Mock<IMapper>();
            _mapper.Setup(m => m.Map<Account, AccountDto>(It.IsAny<IEnumerable<Account>>()))
                .Returns(new[]
                {
                    new AccountDto { Id = AccountId }
                });

            _linkBuilder = new Mock<ILinkBuilder>();
            _linkBuilder.Setup(b => b.GetPeriodEndAccountsPageLink(It.IsAny<string>(), It.IsAny<int>()))
                .Returns((string periodCode, int pageNumber) => $"/{pageNumber}");
            _linkBuilder.Setup(b => b.GetAccountPaymentsLink(PeriodCode, AccountId, It.IsAny<int>()))
                .Returns(AccountPaymentsLink);

            _logger = new Mock<ILogger>();

            _orchestrator = new AccountsOrchestrator(_mediator.Object, _mapper.Object, _linkBuilder.Object, _logger.Object);
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
                    IsValid = true,
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

        [Test]
        public void AndAnExceptionIsThrownThenItShouldLogErrorAndThrowException()
        {
            // Arrange
            var actualException = new Exception("Unit test");
            _mediator.Setup(m => m.SendAsync(It.IsAny<GetAccountsAffectedInPeriodQueryRequest>()))
                .Throws(actualException);

            // Act + assert
            var ex = Assert.ThrowsAsync<Exception>(async () => await _orchestrator.GetPageOfAccountsAffectedInPeriod(PeriodCode, 1));
            Assert.AreEqual(actualException.Message, ex.Message);
            _logger.Verify(l => l.Error(actualException, actualException.Message), Times.Once);
        }

        [Test]
        public void WithAnInvalidRequestThenItShouldThrowABadRequestException()
        {
            // Arrange
            _mediator.Setup(m => m.SendAsync(It.IsAny<GetAccountsAffectedInPeriodQueryRequest>()))
                .Returns(Task.FromResult(new GetAccountsAffectedInPeriodQueryResponse
                {
                    IsValid = false,
                    ValidationFailures = new[]
                    {
                        new ValidationFailure { Code = "TEST", Description = "Unit testing" }
                    }
                }));

            // Act + Assert
            var ex = Assert.ThrowsAsync<BadRequestException>(async () => await _orchestrator.GetPageOfAccountsAffectedInPeriod(PeriodCode, 1));
            Assert.AreEqual("Unit testing", ex.Message);
        }

        [Test]
        public void WithAnPeriodThatDoesNotExistThenItShouldThrowAPeriodNotFoundException()
        {
            // Arrange
            _mediator.Setup(m => m.SendAsync(It.IsAny<GetAccountsAffectedInPeriodQueryRequest>()))
                .Returns(Task.FromResult(new GetAccountsAffectedInPeriodQueryResponse
                {
                    IsValid = false,
                    ValidationFailures = new[]
                    {
                        new PeriodNotFoundFailure()
                    }
                }));

            // Act + Assert
            var ex = Assert.ThrowsAsync<PeriodNotFoundException>(async () => await _orchestrator.GetPageOfAccountsAffectedInPeriod(PeriodCode, 1));
            Assert.AreEqual("The period requested does not exist", ex.Message);
        }

        [Test]
        public void WithAnPageThatDoesNotExistThenItShouldThrowAPageNotFoundException()
        {
            // Arrange
            _mediator.Setup(m => m.SendAsync(It.IsAny<GetAccountsAffectedInPeriodQueryRequest>()))
                .Returns(Task.FromResult(new GetAccountsAffectedInPeriodQueryResponse
                {
                    IsValid = false,
                    ValidationFailures = new[]
                    {
                        new PageNotFoundFailure()
                    }
                }));

            // Act + Assert
            var ex = Assert.ThrowsAsync<PageNotFoundException>(async () => await _orchestrator.GetPageOfAccountsAffectedInPeriod(PeriodCode, 1));
            Assert.AreEqual("The page requested does not exist", ex.Message);
        }
    }
}
