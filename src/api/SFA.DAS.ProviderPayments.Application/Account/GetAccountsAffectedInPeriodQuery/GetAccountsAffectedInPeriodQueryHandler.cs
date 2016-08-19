using System.Threading.Tasks;
using SFA.DAS.ProviderPayments.Application.Validation;
using SFA.DAS.ProviderPayments.Domain.Data;
using SFA.DAS.ProviderPayments.Domain.Data.Entities;
using SFA.DAS.ProviderPayments.Domain.Mapping;

namespace SFA.DAS.ProviderPayments.Application.Account.GetAccountsAffectedInPeriodQuery
{
    public class GetAccountsAffectedInPeriodQueryHandler : PagedQueryHandler<GetAccountsAffectedInPeriodQueryRequest, GetAccountsAffectedInPeriodQueryResponse, AccountEntity, Domain.Account>
    {
        private readonly IAccountRepository _accountRepository;

        public GetAccountsAffectedInPeriodQueryHandler(IAccountRepository accountRepository, IMapper mapper, IValidator<GetAccountsAffectedInPeriodQueryRequest> validator)
            : base(mapper, validator)
        {
            _accountRepository = accountRepository;
        }

        protected override async Task<PageOfEntities<AccountEntity>> GetData(GetAccountsAffectedInPeriodQueryRequest request)
        {
            return await _accountRepository.GetPageOfAccountsAffectedInPeriodAsync(request.PeriodCode, request.PageNumber);
        }
    }
}
