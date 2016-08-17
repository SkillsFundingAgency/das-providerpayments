using System.Threading.Tasks;
using MediatR;
using SFA.DAS.ProviderPayments.Application.Validation;
using SFA.DAS.ProviderPayments.Application.Validation.Failures;
using SFA.DAS.ProviderPayments.Domain.Data;
using SFA.DAS.ProviderPayments.Domain.Data.Entities;
using SFA.DAS.ProviderPayments.Domain.Mapping;

namespace SFA.DAS.ProviderPayments.Application.Account.GetAccountsAffectedInPeriodQuery
{
    public class GetAccountsAffectedInPeriodQueryHandler : IAsyncRequestHandler<GetAccountsAffectedInPeriodQueryRequest, GetAccountsAffectedInPeriodQueryResponse>
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IMapper _mapper;
        private readonly IValidator<GetAccountsAffectedInPeriodQueryRequest> _validator;

        public GetAccountsAffectedInPeriodQueryHandler(IAccountRepository accountRepository, IMapper mapper, IValidator<GetAccountsAffectedInPeriodQueryRequest> validator)
        {
            _accountRepository = accountRepository;
            _mapper = mapper;
            _validator = validator;
        }

        public async Task<GetAccountsAffectedInPeriodQueryResponse> Handle(GetAccountsAffectedInPeriodQueryRequest message)
        {
            var validationResult = await _validator.ValidateAsync(message);
            if (!validationResult.IsValid())
            {
                return new GetAccountsAffectedInPeriodQueryResponse
                {
                    IsValid = false,
                    ValidationFailures = validationResult.Failures
                };
            }

            var entities = await _accountRepository.GetPageOfAccountsAffectedInPeriodAsync(message.PeriodCode, message.PageNumber);
            if (entities == null)
            {
                return new GetAccountsAffectedInPeriodQueryResponse
                {
                    IsValid = false,
                    ValidationFailures = new[] { new PageNotFoundFailure() }
                };
            }

            return new GetAccountsAffectedInPeriodQueryResponse
            {
                IsValid = true,
                TotalNumberOfItems = entities.TotalNumberOfItems,
                TotalNumberOfPages = entities.TotalNumberOfPages,
                Items = _mapper.Map<AccountEntity, Domain.Account>(entities.Items)
            };
        }
    }
}
