using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using SFA.DAS.EAS.Account.Api.Client;
using SFA.DAS.EAS.Account.Api.Types;
using SFA.DAS.Payments.Reference.Accounts.Application.GetPageOfAccountsQuery;

namespace SFA.DAS.Payments.Reference.Accounts.UnitTests.Application.GetPageOfAccountsQuery.GetPageOfAccountsQueryHandler
{
    public class WhenHandlingRequest
    {
        private GetPageOfAccountsQueryRequest _request;
        private Mock<IAccountApiClient> _accountApiClient;
        private Accounts.Application.GetPageOfAccountsQuery.GetPageOfAccountsQueryHandler _handler;

        [SetUp]
        public void Arrange()
        {
            _request = new GetPageOfAccountsQueryRequest
            {
                PageNumber = 1,
                CorrelationDate = new DateTime(2017, 1, 2)
            };

            _accountApiClient = new Mock<IAccountApiClient>();
            _accountApiClient.Setup(c => c.GetPageOfAccounts(1, 1000, _request.CorrelationDate))
                .Returns(Task.FromResult(
                    new PagedApiResponseViewModel<AccountWithBalanceViewModel>
                    {
                        Page = 1,
                        TotalPages = 1,
                        Data = new List<AccountWithBalanceViewModel>
                        {
                            new AccountWithBalanceViewModel
                            {
                                AccountId = 1,
                                AccountHashId = "1",
                                AccountName = "Account 1",
                                Balance = 1234567.89m
                            }
                        }
                    }));

            _handler = new Accounts.Application.GetPageOfAccountsQuery.GetPageOfAccountsQueryHandler(_accountApiClient.Object);
        }

        [Test]
        public void ThenShouldReturnAccountsReturnedByApi()
        {
            // Act
            var actual = _handler.Handle(_request);

            // Assert
            Assert.IsNotNull(actual);
            Assert.IsTrue(actual.IsValid);
            Assert.IsNotNull(actual.Items);
            Assert.AreEqual(1, actual.Items.Length);
            Assert.AreEqual(1, actual.Items[0].AccountId);
            Assert.AreEqual("1", actual.Items[0].AccountHashId);
            Assert.AreEqual("Account 1", actual.Items[0].AccountName);
            Assert.AreEqual(1234567.89m, actual.Items[0].Balance);
        }

        [TestCase(1, 1)]
        [TestCase(10, 10)]
        [TestCase(11, 10)]
        public void ThenItShouldReturnThatThereAreNoMorePagesWhenPageNumberGreaterOrEqualToTotalPages(int pageNumber, int totalNumberOfPages)
        {
            // Arrange
            _request.PageNumber = pageNumber;

            _accountApiClient.Setup(c => c.GetPageOfAccounts(pageNumber, 1000, _request.CorrelationDate))
                .Returns(Task.FromResult(
                    new PagedApiResponseViewModel<AccountWithBalanceViewModel>
                    {
                        Page = pageNumber,
                        TotalPages = totalNumberOfPages,
                        Data = new List<AccountWithBalanceViewModel>
                        {
                            new AccountWithBalanceViewModel
                            {
                                AccountId = 1,
                                AccountHashId = "1",
                                AccountName = "Account 1",
                                Balance = 1234567.89m
                            }
                        }
                    }));

            // Act
            var actual = _handler.Handle(_request);

            // Assert
            Assert.IsNotNull(actual);
            Assert.IsTrue(actual.IsValid);
            Assert.IsFalse(actual.HasMorePages);
        }

        [TestCase(1, 2)]
        public void ThenItShouldReturnThatThereAreMorePagesWhenPageNumberLessThanTotalPages(int pageNumber, int totalNumberOfPages)
        {
            // Arrange
            _request.PageNumber = pageNumber;

            _accountApiClient.Setup(c => c.GetPageOfAccounts(pageNumber, 1000, _request.CorrelationDate))
                .Returns(Task.FromResult(
                    new PagedApiResponseViewModel<AccountWithBalanceViewModel>
                    {
                        Page = pageNumber,
                        TotalPages = totalNumberOfPages,
                        Data = new List<AccountWithBalanceViewModel>
                        {
                            new AccountWithBalanceViewModel
                            {
                                AccountId = 1,
                                AccountHashId = "1",
                                AccountName = "Account 1",
                                Balance = 1234567.89m
                            }
                        }
                    }));

            // Act
            var actual = _handler.Handle(_request);

            // Assert
            Assert.IsNotNull(actual);
            Assert.IsTrue(actual.IsValid);
            Assert.IsTrue(actual.HasMorePages);
        }

        [Test]
        public void ThenItShouldReturnInvalidResponseWhenApiErrors()
        {
            // Arrange
            _accountApiClient.Setup(c => c.GetPageOfAccounts(1, 1000, _request.CorrelationDate))
                .Throws<Exception>();

            // Act
            var actual = _handler.Handle(_request);

            // Assert
            Assert.IsNotNull(actual);
            Assert.IsFalse(actual.IsValid);
            Assert.IsNotNull(actual.Exception);
            Assert.IsInstanceOf<Exception>(actual.Exception);
        }
    }
}
