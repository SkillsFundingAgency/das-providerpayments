using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using SFA.DAS.ProviderPayments.Application.Account.GetAccountsAffectedInPeriodQuery;
using SFA.DAS.ProviderPayments.Application.Validation;
using SFA.DAS.ProviderPayments.Domain.Data;
using SFA.DAS.ProviderPayments.Domain.Data.Entities;
using SFA.DAS.ProviderPayments.Domain.Mapping;

namespace SFA.DAS.ProviderPayments.Application.UnitTests.Account.GetAccountsAffectedInPeriodQuery.GetAccountsAffectedInPeriodQueryHandlerTests
{
    public class WhenHandlingRequest
    {
        private const string PeriodCode = "201704";
        private const string AccountId = "TheAccount";

        private Mock<IAccountRepository> _accountRepository;
        private Mock<IMapper> _mapper;
        private Mock<IValidator<GetAccountsAffectedInPeriodQueryRequest>> _validator;
        private GetAccountsAffectedInPeriodQueryHandler _handler;
        private GetAccountsAffectedInPeriodQueryRequest _request;

        [SetUp]
        public void Arrange()
        {
            _accountRepository = new Mock<IAccountRepository>();
            _accountRepository.Setup(r => r.GetPageOfAccountsAffectedInPeriodAsync(PeriodCode, It.IsAny<int>()))
                .Returns(Task.FromResult(new PageOfEntities<AccountEntity>
                {
                    TotalNumberOfItems = 5,
                    TotalNumberOfPages = 3,
                    Items = new[]
                    {
                        new AccountEntity { Id = AccountId }
                    }
                }));

            _mapper = new Mock<IMapper>();
            _mapper.Setup(m => m.Map<AccountEntity, Domain.Account>(It.IsAny<IEnumerable<AccountEntity>>()))
                .Returns(new[]
                {
                    new Domain.Account
                    {
                        Id = AccountId
                    }
                });

            _validator = new Mock<IValidator<GetAccountsAffectedInPeriodQueryRequest>>();
            _validator.Setup(v => v.ValidateAsync(It.IsAny<GetAccountsAffectedInPeriodQueryRequest>()))
                .Returns(Task.FromResult(new ValidationResult()));

            _handler = new GetAccountsAffectedInPeriodQueryHandler(_accountRepository.Object, _mapper.Object, _validator.Object);

            _request = new GetAccountsAffectedInPeriodQueryRequest
            {
                PeriodCode = PeriodCode,
                PageNumber = 1
            };
        }


        [Test]
        public async Task ThenItShouldReturnAnInstanceOfGetAccountsAffectedInPeriodQueryResponse()
        {
            // Act
            var actual = await _handler.Handle(_request);

            // Assert
            Assert.IsNotNull(actual);
        }

        [Test]
        public async Task ThenItShouldReturnValidResponse()
        {
            // Act
            var actual = await _handler.Handle(_request);

            // Assert
            Assert.IsTrue(actual.IsValid);
            Assert.AreEqual(5, actual.TotalNumberOfItems);
            Assert.AreEqual(3, actual.TotalNumberOfPages);
            Assert.IsNotNull(actual.Items);
            Assert.AreEqual(1, actual.Items.Count());
            Assert.AreEqual(AccountId, actual.Items.First().Id);
        }

        [Test]
        public async Task WithAnInvalidRequestThenItShouldReturnAnInvalidResponse()
        {
            // Arrange
            var failure = new ValidationFailure();
            _validator.Setup(v => v.ValidateAsync(It.IsAny<GetAccountsAffectedInPeriodQueryRequest>()))
                .Returns(Task.FromResult(new ValidationResult
                {
                    Failures = new[]
                    {
                        failure
                    }
                }));

            // Act
            var actual = await _handler.Handle(_request);

            // Assert
            Assert.IsFalse(actual.IsValid);
            Assert.IsNotNull(actual.ValidationFailures);
            Assert.AreEqual(1, actual.ValidationFailures.Count());
            Assert.AreSame(failure, actual.ValidationFailures.First());
        }
    }
}
