using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using SFA.DAS.ProviderPayments.Application.PeriodEnd.GetPageOfPeriodEndsQuery;
using SFA.DAS.ProviderPayments.Application.Validation;
using SFA.DAS.ProviderPayments.Domain.Data;
using SFA.DAS.ProviderPayments.Domain.Data.Entities;
using SFA.DAS.ProviderPayments.Domain.Mapping;

namespace SFA.DAS.ProviderPayments.Application.UnitTests.PeriodEnd.GetPageOfPeriodEndsQuery.GetPageOfPeriodEndsQueryHandlerTests
{
    public class WhenHandling
    {
        private const long ExpectedNumberOfItems = 18;
        private const int ExpectedNumberOfPages = 2;
        private readonly Domain.PeriodEnd ExpectedPeriodEnd1 = new Domain.PeriodEnd
        {
            PeriodCode = "201704"
        };

        private Mock<IValidator<GetPageOfPeriodEndsQueryRequest>> _validator;
        private Mock<IPeriodEndRepository> _repository;
        private Mock<IMapper> _mapper;
        private GetPageOfPeriodEndsQueryHandler _handler;

        [SetUp]
        public void Arrange()
        {
            _repository = new Mock<IPeriodEndRepository>();
            _repository.Setup(r => r.GetPageAsync(It.IsAny<int>()))
                .Returns(Task.FromResult(new PageOfEntities<PeriodEndEntity>
                {
                    TotalNumberOfItems = ExpectedNumberOfItems,
                    TotalNumberOfPages = ExpectedNumberOfPages,
                    Items = new[]
                    {
                        new PeriodEndEntity { PeriodCode = ExpectedPeriodEnd1.PeriodCode }
                    }
                }));

            _mapper = new Mock<IMapper>();
            _mapper.Setup(m => m.Map<PeriodEndEntity, Domain.PeriodEnd>(It.IsAny<IEnumerable<PeriodEndEntity>>()))
                .Returns(new[]
                {
                    ExpectedPeriodEnd1
                });

            _validator = new Mock<IValidator<GetPageOfPeriodEndsQueryRequest>>();
            _validator.Setup(v => v.ValidateAsync(It.IsAny<GetPageOfPeriodEndsQueryRequest>()))
                .Returns(Task.FromResult(new ValidationResult()));

            _handler = new GetPageOfPeriodEndsQueryHandler(_repository.Object, _mapper.Object, _validator.Object);
        }

        [Test]
        public async Task ThenItShouldReturnInstanceOfGetPageOfPeriodEndsQueryResponse()
        {
            // Act
            var actual = await _handler.Handle(new GetPageOfPeriodEndsQueryRequest { PageNumber = 1 });

            // Assert
            Assert.IsNotNull(actual);
        }

        [Test]
        public async Task WithInvalidRequestThenItShouldReturnInvalidResponse()
        {
            // Validator
            _validator.Setup(v => v.ValidateAsync(It.IsAny<GetPageOfPeriodEndsQueryRequest>()))
                .Returns(Task.FromResult(new ValidationResult
                {
                    Failures = new[]
                    {
                        new ValidationFailure {Code = "TEST", Description = "Unit test failure"}
                    }
                }));

            // Act
            var actual = await _handler.Handle(new GetPageOfPeriodEndsQueryRequest { PageNumber = 0 });

            // Assert
            Assert.IsFalse(actual.IsValid);
            Assert.IsNotNull(actual.ValidationFailures);
            Assert.IsTrue(actual.ValidationFailures.Any(f => f.Code == "TEST" && f.Description == "Unit test failure"));
        }

        [Test]
        public async Task ThenItShouldGetPageFromRepository()
        {
            // Act
            await _handler.Handle(new GetPageOfPeriodEndsQueryRequest { PageNumber = 2 });

            // Assert
            _repository.Verify(r => r.GetPageAsync(2), Times.Once);
        }

        [Test]
        public async Task ThenItShouldReturnMappedResultsFromRepository()
        {
            // Act
            var actual = await _handler.Handle(new GetPageOfPeriodEndsQueryRequest { PageNumber = 2 });

            // Assert
            Assert.AreEqual(ExpectedNumberOfItems, actual.TotalNumberOfItems);
            Assert.AreEqual(ExpectedNumberOfPages, actual.TotalNumberOfPages);
            Assert.IsNotNull(actual.Items);
            Assert.IsTrue(actual.Items.Any(x => x.PeriodCode == ExpectedPeriodEnd1.PeriodCode));
        }
    }
}
