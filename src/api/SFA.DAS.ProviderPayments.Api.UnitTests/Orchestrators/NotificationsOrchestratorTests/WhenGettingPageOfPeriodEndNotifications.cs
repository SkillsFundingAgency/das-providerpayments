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
using SFA.DAS.ProviderPayments.Application.PeriodEnd.GetPageOfPeriodEndsQuery;
using SFA.DAS.ProviderPayments.Application.Validation;
using SFA.DAS.ProviderPayments.Application.Validation.Failures;
using SFA.DAS.ProviderPayments.Domain;
using SFA.DAS.ProviderPayments.Domain.Mapping;
using PeriodType = SFA.DAS.ProviderPayments.Domain.PeriodType;

namespace SFA.DAS.ProviderPayments.Api.UnitTests.Orchestrators.NotificationsOrchestratorTests
{
    public class WhenGettingPageOfPeriodEndNotifications
    {
        private PeriodEnd _periodEnd;
        private Mock<IMediator> _mediator;
        private Mock<ILinkBuilder> _linkBuilder;
        private Mock<ILogger> _logger;
        private NotificationsOrchestrator _orchestrator;
        private Mock<IMapper> _mapper;
        private PeriodEndDto _periodEndDto;

        [SetUp]
        public void Arrange()
        {
            _periodEnd = new PeriodEnd
            {
                Period = new Period
                {
                    Code = "201704",
                    PeriodType = PeriodType.CalendarMonth
                },
                TotalValue = 12345.67m,
                NumberOfProviders = 23,
                NumberOfEmployers = 92,
                PaymentRunDate = new DateTime(2017, 05, 05)
            };
            _periodEndDto = new PeriodEndDto
            {
                Period = new PeriodDto
                {
                    Code = _periodEnd.Period.Code,
                    PeriodType = Dto.PeriodType.CalendarMonth
                },
                TotalValue = _periodEnd.TotalValue,
                NumberOfProviders = _periodEnd.NumberOfProviders,
                NumberOfEmployers = _periodEnd.NumberOfEmployers,
                PaymentRunDate = _periodEnd.PaymentRunDate
            };

            _mediator = new Mock<IMediator>();
            _mediator.Setup(m => m.SendAsync(It.Is<GetPageOfPeriodEndsQueryRequest>(r => r.PageNumber == 1)))
                .Returns(Task.FromResult(new GetPageOfPeriodEndsQueryResponse
                {
                    IsValid = true,
                    TotalNumberOfItems = 1,
                    TotalNumberOfPages = 1,
                    Items = new[]
                    {
                        _periodEnd
                    }
                }));

            _mapper = new Mock<IMapper>();
            _mapper.Setup(m => m.Map<PeriodEnd, PeriodEndDto>(It.IsAny<IEnumerable<PeriodEnd>>()))
                .Returns(new[]
                {
                    _periodEndDto
                });

            _linkBuilder = new Mock<ILinkBuilder>();
            _linkBuilder.Setup(b => b.GetPeriodEndNotificationPageLink(It.IsAny<int>()))
                .Returns((int pageNumber) => $"/{pageNumber}");

            _logger = new Mock<ILogger>();

            _orchestrator = new NotificationsOrchestrator(_mediator.Object, _mapper.Object, _linkBuilder.Object, _logger.Object);
        }

        [Test]
        public async Task ThenItShouldReturnAnInstanceOfAHalPage()
        {
            // Act
            var actual = await _orchestrator.GetPageOfPeriodEndNotifications(1);

            // Assert
            Assert.IsNotNull(actual);
        }

        [Test]
        public async Task ThenItShouldReturnAPageWithTheCorrectItems()
        {
            // Act
            var actual = await _orchestrator.GetPageOfPeriodEndNotifications(1);

            // Assert
            Assert.AreEqual(1, actual.Count);
            Assert.AreEqual("201704", actual.Content.Items.ElementAt(0).Period.Code);
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
                    IsValid = true,
                    TotalNumberOfItems = 1,
                    TotalNumberOfPages = totalNumberOfPages,
                    Items = new[]
                    {
                        _periodEnd
                    }
                }));

            // Act
            var actual = await _orchestrator.GetPageOfPeriodEndNotifications(pageNumber);

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
            _mediator.Setup(m => m.SendAsync(It.IsAny<GetPageOfPeriodEndsQueryRequest>()))
                .Throws(actualException);

            // Act + assert
            var ex = Assert.ThrowsAsync<Exception>(async () => await _orchestrator.GetPageOfPeriodEndNotifications(1));
            Assert.AreEqual(actualException.Message, ex.Message);
            _logger.Verify(l => l.Error(actualException, actualException.Message), Times.Once);
        }

        [Test]
        public void WithAnInvalidRequestThenItShouldThrowABadRequestException()
        {
            // Arrange
            _mediator.Setup(m => m.SendAsync(It.IsAny<GetPageOfPeriodEndsQueryRequest>()))
                .Returns(Task.FromResult(new GetPageOfPeriodEndsQueryResponse
                {
                    IsValid = false,
                    ValidationFailures = new[]
                    {
                        new ValidationFailure { Code = "TEST", Description = "Unit testing" }
                    }
                }));

            // Act + Assert
            var ex = Assert.ThrowsAsync<BadRequestException>(async () => await _orchestrator.GetPageOfPeriodEndNotifications(1));
            Assert.AreEqual("Unit testing", ex.Message);
        }

        [Test]
        public void WithAnPeriodThatDoesNotExistThenItShouldThrowAPeriodNotFoundException()
        {
            // Arrange
            _mediator.Setup(m => m.SendAsync(It.IsAny<GetPageOfPeriodEndsQueryRequest>()))
                .Returns(Task.FromResult(new GetPageOfPeriodEndsQueryResponse
                {
                    IsValid = false,
                    ValidationFailures = new[]
                    {
                        new PeriodNotFoundFailure()
                    }
                }));

            // Act + Assert
            var ex = Assert.ThrowsAsync<PeriodNotFoundException>(async () => await _orchestrator.GetPageOfPeriodEndNotifications(1));
            Assert.AreEqual("The period requested does not exist", ex.Message);
        }

        [Test]
        public void WithAnPageThatDoesNotExistThenItShouldThrowAPageNotFoundException()
        {
            // Arrange
            _mediator.Setup(m => m.SendAsync(It.IsAny<GetPageOfPeriodEndsQueryRequest>()))
                .Returns(Task.FromResult(new GetPageOfPeriodEndsQueryResponse
                {
                    IsValid = false,
                    ValidationFailures = new[]
                    {
                        new PageNotFoundFailure()
                    }
                }));

            // Act + Assert
            var ex = Assert.ThrowsAsync<PageNotFoundException>(async () => await _orchestrator.GetPageOfPeriodEndNotifications(1));
            Assert.AreEqual("The page requested does not exist", ex.Message);
        }
    }
}
