using System;
using System.Collections.Generic;
using MediatR;
using Moq;
using NLog;
using NUnit.Framework;
using SFA.DAS.EAS.Account.Api.Types;
using SFA.DAS.Payments.Reference.Accounts.Application.AddAuditCommand;
using SFA.DAS.Payments.Reference.Accounts.Application.AddOrUpdateAccountCommand;
using SFA.DAS.Payments.Reference.Accounts.Application.GetPageOfAccountsQuery;
using SFA.DAS.Payments.Reference.Accounts.Processor;

namespace SFA.DAS.Payments.Reference.Accounts.UnitTests.Processor
{
    [TestFixture]
    public class GivenAnImportAccountsOrchestrator
    {
        [TestFixture]
        public class WhenCallingImportAccounts
        {
            private Mock<IMediator> _mediator;
            private Mock<ILogger> _logger;
            private ImportAccountsOrchestrator _sut;

            [SetUp]
            public void Arrange()
            {
                _mediator = new Mock<IMediator>();
                _mediator.Setup(m => m.Send(It.Is<GetPageOfAccountsQueryRequest>(r => r.PageNumber == 1)))
                    .Returns(new GetPageOfAccountsQueryResponse
                    {
                        IsValid = true,
                        Items = new[]
                        {
                        new AccountWithBalanceViewModel { AccountId = 1, AccountHashId = "AAA", AccountName = "Account 1", Balance = 10000 },
                        new AccountWithBalanceViewModel { AccountId = 2, AccountHashId = "BBB", AccountName = "Account 2", Balance = 20000 }
                        },
                        HasMorePages = false
                    });

                _logger = new Mock<ILogger>();

                _sut = new ImportAccountsOrchestrator(_mediator.Object, _logger.Object);
            }

            [Test]
            public void ThenItShouldKeepReadingPagesUntilNoMoreAreLeft()
            {
                // Arrange
                _mediator.Setup(m => m.Send(It.Is<GetPageOfAccountsQueryRequest>(r => r.PageNumber == 1)))
                    .Returns(new GetPageOfAccountsQueryResponse
                    {
                        IsValid = true,
                        Items = new[]
                        {
                        new AccountWithBalanceViewModel { AccountId = 1, AccountHashId = "AAA", AccountName = "Account 1", Balance = 10000 },
                        new AccountWithBalanceViewModel { AccountId = 2, AccountHashId = "BBB", AccountName = "Account 2", Balance = 20000 }
                        },
                        HasMorePages = true
                    });
                _mediator.Setup(m => m.Send(It.Is<GetPageOfAccountsQueryRequest>(r => r.PageNumber == 2)))
                    .Returns(new GetPageOfAccountsQueryResponse
                    {
                        IsValid = true,
                        Items = new[]
                        {
                        new AccountWithBalanceViewModel { AccountId = 3, AccountHashId = "CCC", AccountName = "Account 3", Balance = 30000 },
                        new AccountWithBalanceViewModel { AccountId = 4, AccountHashId = "DDD", AccountName = "Account 4", Balance = 40000 }
                        },
                        HasMorePages = true
                    });
                _mediator.Setup(m => m.Send(It.Is<GetPageOfAccountsQueryRequest>(r => r.PageNumber == 3)))
                    .Returns(new GetPageOfAccountsQueryResponse
                    {
                        IsValid = true,
                        Items = new[]
                        {
                        new AccountWithBalanceViewModel { AccountId = 5, AccountHashId = "EEE", AccountName = "Account 5", Balance = 50000 }
                        },
                        HasMorePages = false
                    });

                // Act
                _sut.ImportAccounts();

                // Assert
                _mediator.Verify(m => m.Send(It.IsAny<GetPageOfAccountsQueryRequest>()), Times.Exactly(3));
                _mediator.Verify(m => m.Send(It.Is<GetPageOfAccountsQueryRequest>(r => r.PageNumber == 1)), Times.Once);
                _mediator.Verify(m => m.Send(It.Is<GetPageOfAccountsQueryRequest>(r => r.PageNumber == 2)), Times.Once);
                _mediator.Verify(m => m.Send(It.Is<GetPageOfAccountsQueryRequest>(r => r.PageNumber == 3)), Times.Once);
            }

            [Test]
            public void ThenItShouldAddOrUpdateEachAccount()
            {
                // Act
                _sut.ImportAccounts();

                // Assert
                _mediator.Verify(m => m.Send(It.IsAny<AddOrUpdateAccountCommandRequest>()), Times.Exactly(2));
                _mediator.Verify(m => m.Send(It.Is<AddOrUpdateAccountCommandRequest>(r => r.Account.AccountId == 1)), Times.Once);
                _mediator.Verify(m => m.Send(It.Is<AddOrUpdateAccountCommandRequest>(r => r.Account.AccountId == 2)), Times.Once);
            }

            [Test]
            public void ThenItShouldUseTheSameCorrelationDateForAllGetAccountRequests()
            {
                // Arrange
                var correlationDates = new List<DateTime>();
                _mediator.Setup(m => m.Send(It.IsAny<GetPageOfAccountsQueryRequest>()))
                    .Returns((GetPageOfAccountsQueryRequest request) =>
                    {
                        correlationDates.Add(request.CorrelationDate);

                        return new GetPageOfAccountsQueryResponse
                        {
                            IsValid = true,
                            Items = new[]
                                {
                                new AccountWithBalanceViewModel { AccountId = 1, AccountHashId = "AAA", AccountName = "Account 1", Balance = 10000 },
                                new AccountWithBalanceViewModel { AccountId = 2, AccountHashId = "BBB", AccountName = "Account 2", Balance = 20000 }
                                },
                            HasMorePages = request.PageNumber < 2
                        };
                    });

                // Act
                _sut.ImportAccounts();

                // Assert
                Assert.AreEqual(2, correlationDates.Count);
                Assert.AreEqual(correlationDates[0], correlationDates[1]);
            }

            [Test]
            public void ThenItShouldSendTheCorrelationDateWithCreationCommands()
            {
                // Act
                _sut.ImportAccounts();

                // Assert
                _mediator.Verify(m => m.Send(It.Is<AddOrUpdateAccountCommandRequest>(r => r.CorrelationDate >= DateTime.Today)), Times.Exactly(2));
            }

            [Test]
            public void ThenItShouldWriteAnAuditRecordAtEndOfProcess()
            {
                // Act
                _sut.ImportAccounts();

                // Assert
                _mediator.Verify(m => m.Send(It.Is<AddAuditCommandRequest>(r => 
                    r.AccountsRead == 2 && 
                    r.CorrelationDate >= DateTime.Today && 
                    r.AuditType == AuditType.Account)), Times.Once());
            }
        }
    }
}
