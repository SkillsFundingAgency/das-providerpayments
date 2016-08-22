using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using SFA.DAS.ProviderPayments.Application.Account.Failures;
using SFA.DAS.ProviderPayments.Application.Account.GetPaymentsForAccountInPeriodQuery;
using SFA.DAS.ProviderPayments.Application.Validation;
using SFA.DAS.ProviderPayments.Application.Validation.Failures;
using SFA.DAS.ProviderPayments.Domain;
using SFA.DAS.ProviderPayments.Domain.Data;
using SFA.DAS.ProviderPayments.Domain.Data.Entities;
using SFA.DAS.ProviderPayments.Domain.Mapping;

namespace SFA.DAS.ProviderPayments.Application.UnitTests.Account.GetPaymentsForAccountInPeriodQuery.GetPaymentsForAccountInPeriodQueryHandlerTests
{
    public class WhenHandling
    {
        private const string PeriodCode = "201704";
        private const string AccountId = "TheAccount";
        private const int PageNumber = 1;

        private Payment _payment;
        private PaymentEntity _paymentEntity;
        private GetPaymentsForAccountInPeriodQueryRequest _request;
        private Mock<IPaymentRepository> _paymentRepository;
        private Mock<IMapper> _mapper;
        private Mock<IValidator<GetPaymentsForAccountInPeriodQueryRequest>> _validator;
        private GetPaymentsForAccountInPeriodQueryHandler _handler;

        [SetUp]
        public void Arrange()
        {
            _payment = new Payment();
            _paymentEntity = new PaymentEntity();
            _request = new GetPaymentsForAccountInPeriodQueryRequest
            {
                PeriodCode = PeriodCode,
                AccountId = AccountId,
                PageNumber = PageNumber
            };

            _paymentRepository = new Mock<IPaymentRepository>();
            _paymentRepository.Setup(r => r.GetPageOfPaymentsForAccountInPeriod(PeriodCode, AccountId, PageNumber))
                .Returns(Task.FromResult(new PageOfEntities<PaymentEntity>
                {
                    TotalNumberOfItems = 36,
                    TotalNumberOfPages = 12,
                    Items = new[]
                    {
                        _paymentEntity
                    }
                }));

            _mapper = new Mock<IMapper>();
            _mapper.Setup(m => m.Map<PaymentEntity, Payment>(It.Is<IEnumerable<PaymentEntity>>(x => x.Count() == 1 && x.Single() == _paymentEntity)))
                .Returns(new[]
                {
                    _payment
                });

            _validator = new Mock<IValidator<GetPaymentsForAccountInPeriodQueryRequest>>();
            _validator.Setup(v => v.ValidateAsync(_request))
                .Returns(Task.FromResult(new ValidationResult
                {
                    Failures = new ValidationFailure[0]
                }));

            _handler = new GetPaymentsForAccountInPeriodQueryHandler(_paymentRepository.Object, _mapper.Object, _validator.Object);

        }

        [Test]
        public async Task ThenItShouldReturnInstanceOfGetPaymentsForAccountInPeriodQueryResponse()
        {
            // Act
            var actual = await _handler.Handle(_request);

            // Assert
            Assert.IsNotNull(actual);
        }

        [Test]
        public async Task WithAValidAndExistingRequestThenItShouldReturnCorrectPayments()
        {
            // Act
            var actual = await _handler.Handle(_request);

            // Assert
            Assert.IsTrue(actual.IsValid);
            Assert.AreEqual(36, actual.TotalNumberOfItems);
            Assert.AreEqual(12, actual.TotalNumberOfPages);
            Assert.IsNotNull(actual.Items);
            Assert.AreEqual(1, actual.Items.Count());

            var actualItem = actual.Items.First();
            Assert.IsNotNull(actualItem);
            Assert.AreSame(_payment, actualItem);
        }

        [Test]
        public async Task WithAPageThatDoesNotExistThenItShouldReturnAPageNotFoundFailure()
        {
            // Arrange
            _paymentRepository.Setup(r => r.GetPageOfPaymentsForAccountInPeriod(PeriodCode, AccountId, PageNumber))
                .Returns(Task.FromResult<PageOfEntities<PaymentEntity>>(null));

            // Act
            var actual = await _handler.Handle(_request);

            // Assert
            Assert.IsFalse(actual.IsValid);
            Assert.IsNotNull(actual.ValidationFailures);
            Assert.AreEqual(1, actual.ValidationFailures.Count());
            Assert.IsInstanceOf<PageNotFoundFailure>(actual.ValidationFailures.First());
        }

        [Test]
        public async Task WithAnInvalidPageNumberThenItShouldReturnAnInvalidPageNumberFailure()
        {
            // Arrange
            _validator.Setup(v => v.ValidateAsync(It.IsAny<GetPaymentsForAccountInPeriodQueryRequest>()))
                .Returns(Task.FromResult(new ValidationResult
                {
                    Failures = new[]
                    {
                        new InvalidPageNumberFailure()
                    }
                }));

            // Act
            var actual = await _handler.Handle(_request);

            // Assert
            Assert.IsFalse(actual.IsValid);
            Assert.IsNotNull(actual.ValidationFailures);
            Assert.AreEqual(1, actual.ValidationFailures.Count());
            Assert.IsInstanceOf<InvalidPageNumberFailure>(actual.ValidationFailures.First());
        }

        [Test]
        public async Task WithAnInvalidPeriodCodeThenItShouldReturnAnInvalidPeriodCodeFailure()
        {
            // Arrange
            _validator.Setup(v => v.ValidateAsync(It.IsAny<GetPaymentsForAccountInPeriodQueryRequest>()))
                .Returns(Task.FromResult(new ValidationResult
                {
                    Failures = new[]
                    {
                        new InvalidPeriodCodeFailure()
                    }
                }));

            // Act
            var actual = await _handler.Handle(_request);

            // Assert
            Assert.IsFalse(actual.IsValid);
            Assert.IsNotNull(actual.ValidationFailures);
            Assert.AreEqual(1, actual.ValidationFailures.Count());
            Assert.IsInstanceOf<InvalidPeriodCodeFailure>(actual.ValidationFailures.First());
        }

        [Test]
        public async Task WithAPeriodCodeThatDoesNotExistThenItShouldReturnAPeriodNotFoundFailure()
        {
            // Arrange
            _validator.Setup(v => v.ValidateAsync(It.IsAny<GetPaymentsForAccountInPeriodQueryRequest>()))
                .Returns(Task.FromResult(new ValidationResult
                {
                    Failures = new[]
                    {
                        new PeriodNotFoundFailure()
                    }
                }));

            // Act
            var actual = await _handler.Handle(_request);

            // Assert
            Assert.IsFalse(actual.IsValid);
            Assert.IsNotNull(actual.ValidationFailures);
            Assert.AreEqual(1, actual.ValidationFailures.Count());
            Assert.IsInstanceOf<PeriodNotFoundFailure>(actual.ValidationFailures.First());
        }

        [Test]
        public async Task WithAnAccountIdThatDoesNotExistThenItShouldReturnAnAccountNotFoundFailure()
        {
            // Arrange
            _validator.Setup(v => v.ValidateAsync(It.IsAny<GetPaymentsForAccountInPeriodQueryRequest>()))
                .Returns(Task.FromResult(new ValidationResult
                {
                    Failures = new[]
                    {
                        new AccountNotFoundFailure()
                    }
                }));

            // Act
            var actual = await _handler.Handle(_request);

            // Assert
            Assert.IsFalse(actual.IsValid);
            Assert.IsNotNull(actual.ValidationFailures);
            Assert.AreEqual(1, actual.ValidationFailures.Count());
            Assert.IsInstanceOf<AccountNotFoundFailure>(actual.ValidationFailures.First());
        }
    }
}
