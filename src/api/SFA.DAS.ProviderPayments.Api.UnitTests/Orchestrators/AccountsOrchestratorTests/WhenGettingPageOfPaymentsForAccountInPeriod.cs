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
using SFA.DAS.ProviderPayments.Application.Account.Failures;
using SFA.DAS.ProviderPayments.Application.Account.GetPaymentsForAccountInPeriodQuery;
using SFA.DAS.ProviderPayments.Application.Validation;
using SFA.DAS.ProviderPayments.Application.Validation.Failures;
using SFA.DAS.ProviderPayments.Domain;
using SFA.DAS.ProviderPayments.Domain.Mapping;
using FundingType = SFA.DAS.ProviderPayments.Domain.FundingType;
using PeriodType = SFA.DAS.ProviderPayments.Domain.PeriodType;
using TransactionType = SFA.DAS.ProviderPayments.Domain.TransactionType;

namespace SFA.DAS.ProviderPayments.Api.UnitTests.Orchestrators.AccountsOrchestratorTests
{
    public class WhenGettingPageOfPaymentsForAccountInPeriod
    {
        private const string PeriodCode = "201704";
        private const string AccountId = "SomeAccount";
        private const string Ukprn = "Provider1";
        private const int Uln = 7824368;
        private const int StandardCode = 7863845;
        private const decimal Amount = 832.92m;
        private const TransactionType TransactionType = Domain.TransactionType.Learning;
        private const FundingType FundingType = Domain.FundingType.LevyCredit;

        private Payment _payment;
        private Mock<IMediator> _mediator;
        private Mock<ILinkBuilder> _linkBuilder;
        private Mock<ILogger> _logger;
        private AccountsOrchestrator _orchestrator;
        private Mock<IMapper> _mapper;

        [SetUp]
        public void Arrange()
        {
            _payment = new Payment
            {
                Account = new Account
                {
                    Id = AccountId
                },
                Provider = new Provider
                {
                    Ukprn = Ukprn
                },
                Apprenticeship = new Apprenticeship
                {
                    Learner = new Learner
                    {
                        Uln = Uln
                    },
                    Course = new Course
                    {
                        StandardCode = StandardCode
                    }
                },
                DeliveryPeriod = new Period
                {
                    Code = PeriodCode,
                    PeriodType = PeriodType.CalendarMonth
                },
                ReportedPeriod = new Period
                {
                    Code = PeriodCode,
                    PeriodType = PeriodType.CalendarMonth
                },
                Amount = Amount,
                TransactionType = TransactionType,
                FundingType = FundingType
            };

            _mediator = new Mock<IMediator>();
            _mediator.Setup(m => m.SendAsync(It.Is<GetPaymentsForAccountInPeriodQueryRequest>(r => r.PeriodCode == PeriodCode && r.AccountId == AccountId)))
                .Returns(Task.FromResult(new GetPaymentsForAccountInPeriodQueryResponse
                {
                    IsValid = true,
                    TotalNumberOfItems = 1,
                    TotalNumberOfPages = 1,
                    Items = new[]
                    {
                        _payment
                    }
                }));

            _mapper = new Mock<IMapper>();
            _mapper.Setup(m => m.Map<Payment, PaymentDto>(It.IsAny<IEnumerable<Payment>>()))
                .Returns(new[]
                {
                    new PaymentDto
                    {
                        Account = new AccountDto
                        {
                            Id = AccountId
                        },
                        Provider = new ProviderDto
                        {
                            Ukprn = Ukprn
                        },
                        Apprenticeship = new ApprenticeshipDto
                        {
                            Learner = new LearnerDto
                            {
                                Uln = Uln
                            },
                            Course = new CourseDto
                            {
                                StandardCode = StandardCode
                            }
                        },
                        DeliveryPeriod = new PeriodDto
                        {
                            Code = PeriodCode,
                            PeriodType = Dto.PeriodType.CalendarMonth
                        },
                        ReportedPeriod = new PeriodDto
                        {
                            Code = PeriodCode,
                            PeriodType = Dto.PeriodType.CalendarMonth
                        },
                        Amount = Amount,
                        TransactionType = Dto.TransactionType.Learning,
                        FundingType = Dto.FundingType.LevyCredit
                    }
                });

            _linkBuilder = new Mock<ILinkBuilder>();
            _linkBuilder.Setup(b => b.GetAccountPaymentsLink(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>()))
                .Returns((string periodCode, string accountId, int pageNumber) => $"/{pageNumber}");

            _logger = new Mock<ILogger>();

            _orchestrator = new AccountsOrchestrator(_mediator.Object, _mapper.Object, _linkBuilder.Object, _logger.Object);
        }

        [Test]
        public async Task ThenItShouldReturnAnInstanceOfAHalPage()
        {
            // Act
            var actual = await _orchestrator.GetPageOfPaymentsForAccountInPeriod(PeriodCode, AccountId, 1);

            // Assert
            Assert.IsNotNull(actual);
        }

        [Test]
        public async Task ThenItShouldReturnAPageWithTheCorrectItems()
        {
            // Act
            var actual = await _orchestrator.GetPageOfPaymentsForAccountInPeriod(PeriodCode, AccountId, 1);

            // Assert
            Assert.AreEqual(1, actual.Count);

            var actualAccount = actual.Content.Items.First();
            Assert.AreEqual(AccountId, actualAccount.Account?.Id);
            Assert.AreEqual(Ukprn, actualAccount.Provider?.Ukprn);
            Assert.AreEqual(Uln, actualAccount.Apprenticeship?.Learner?.Uln);
            Assert.AreEqual(StandardCode, actualAccount.Apprenticeship?.Course?.StandardCode);
            Assert.AreEqual(0, actualAccount.Apprenticeship?.Course?.PathwayCode);
            Assert.AreEqual(0, actualAccount.Apprenticeship?.Course?.FrameworkCode);
            Assert.AreEqual(0, actualAccount.Apprenticeship?.Course?.ProgrammeType);
            Assert.AreEqual(Amount, actualAccount.Amount);
            Assert.AreEqual(Dto.TransactionType.Learning, actualAccount.TransactionType);
            Assert.AreEqual(Dto.FundingType.LevyCredit, actualAccount.FundingType);
        }

        [TestCase(1, 1, null, null, "/1", "/1")]
        [TestCase(1, 2, "/2", null, "/1", "/2")]
        [TestCase(2, 2, null, "/1", "/1", "/2")]
        [TestCase(5, 10, "/6", "/4", "/1", "/10")]
        public async Task ThenItShouldReturnCorrectLinks(int pageNumber, int totalNumberOfPages,
            string expectedNextLink, string expectedPrevLink, string expectedFirstLink, string expectedLastLink)
        {
            // Arrange
            _mediator.Setup(m => m.SendAsync(It.Is<GetPaymentsForAccountInPeriodQueryRequest>(r => r.PageNumber == pageNumber)))
                .Returns(Task.FromResult(new GetPaymentsForAccountInPeriodQueryResponse
                {
                    IsValid = true,
                    TotalNumberOfItems = 1,
                    TotalNumberOfPages = totalNumberOfPages,
                    Items = new[]
                    {
                        _payment
                    }
                }));

            // Act
            var actual = await _orchestrator.GetPageOfPaymentsForAccountInPeriod(PeriodCode, AccountId, pageNumber);

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
            _mediator.Setup(m => m.SendAsync(It.IsAny<GetPaymentsForAccountInPeriodQueryRequest>()))
                .Throws(actualException);

            // Act + assert
            var ex = Assert.ThrowsAsync<Exception>(async () => await _orchestrator.GetPageOfPaymentsForAccountInPeriod(PeriodCode, AccountId, 1));
            Assert.AreEqual(actualException.Message, ex.Message);
            _logger.Verify(l => l.Error(actualException, actualException.Message), Times.Once);
        }

        [Test]
        public void WithAnInvalidRequestThenItShouldThrowABadRequestException()
        {
            // Arrange
            _mediator.Setup(m => m.SendAsync(It.IsAny<GetPaymentsForAccountInPeriodQueryRequest>()))
                .Returns(Task.FromResult(new GetPaymentsForAccountInPeriodQueryResponse
                {
                    IsValid = false,
                    ValidationFailures = new[]
                    {
                        new ValidationFailure { Code = "TEST", Description = "Unit testing" }
                    }
                }));

            // Act + Assert
            var ex = Assert.ThrowsAsync<BadRequestException>(async () => await _orchestrator.GetPageOfPaymentsForAccountInPeriod(PeriodCode, AccountId, 1));
            Assert.AreEqual("Unit testing", ex.Message);
        }

        [Test]
        public void WithAnPeriodThatDoesNotExistThenItShouldThrowAPeriodNotFoundException()
        {
            // Arrange
            _mediator.Setup(m => m.SendAsync(It.IsAny<GetPaymentsForAccountInPeriodQueryRequest>()))
                .Returns(Task.FromResult(new GetPaymentsForAccountInPeriodQueryResponse
                {
                    IsValid = false,
                    ValidationFailures = new[]
                    {
                        new PeriodNotFoundFailure()
                    }
                }));

            // Act + Assert
            var ex = Assert.ThrowsAsync<PeriodNotFoundException>(async () => await _orchestrator.GetPageOfPaymentsForAccountInPeriod(PeriodCode, AccountId, 1));
            Assert.AreEqual("The period requested does not exist", ex.Message);
        }

        [Test]
        public void WithAnAccountThatDoesNotExistThenItShouldThrowAnAccountNotFoundException()
        {
            // Arrange
            _mediator.Setup(m => m.SendAsync(It.IsAny<GetPaymentsForAccountInPeriodQueryRequest>()))
                .Returns(Task.FromResult(new GetPaymentsForAccountInPeriodQueryResponse
                {
                    IsValid = false,
                    ValidationFailures = new[]
                    {
                        new AccountNotFoundFailure()
                    }
                }));

            // Act + Assert
            var ex = Assert.ThrowsAsync<AccountNotFoundException>(async () => await _orchestrator.GetPageOfPaymentsForAccountInPeriod(PeriodCode, AccountId, 1));
            Assert.AreEqual("The account requested does not exist", ex.Message);
        }

        [Test]
        public void WithAnPageThatDoesNotExistThenItShouldThrowAPageNotFoundException()
        {
            // Arrange
            _mediator.Setup(m => m.SendAsync(It.IsAny<GetPaymentsForAccountInPeriodQueryRequest>()))
                .Returns(Task.FromResult(new GetPaymentsForAccountInPeriodQueryResponse
                {
                    IsValid = false,
                    ValidationFailures = new[]
                    {
                        new PageNotFoundFailure()
                    }
                }));

            // Act + Assert
            var ex = Assert.ThrowsAsync<PageNotFoundException>(async () => await _orchestrator.GetPageOfPaymentsForAccountInPeriod(PeriodCode, AccountId, 1));
            Assert.AreEqual("The page requested does not exist", ex.Message);
        }

    }
}
