using System.Threading.Tasks;
using SFA.DAS.ProviderPayments.Application.Validation;
using SFA.DAS.ProviderPayments.Domain.Data;
using SFA.DAS.ProviderPayments.Domain.Data.Entities;
using SFA.DAS.ProviderPayments.Domain.Mapping;

namespace SFA.DAS.ProviderPayments.Application.PeriodEnd.GetPageOfPeriodEndsQuery
{
    public class GetPageOfPeriodEndsQueryHandler : PagedQueryHandler<GetPageOfPeriodEndsQueryRequest, GetPageOfPeriodEndsQueryResponse, PeriodEndEntity, Domain.PeriodEnd>
    {
        private readonly IPeriodEndRepository _periodEndRepository;

        public GetPageOfPeriodEndsQueryHandler(IPeriodEndRepository periodEndRepository, IMapper mapper, IValidator<GetPageOfPeriodEndsQueryRequest> validator)
            : base(mapper, validator)
        {
            _periodEndRepository = periodEndRepository;
        }

        protected override async Task<PageOfEntities<PeriodEndEntity>> GetData(GetPageOfPeriodEndsQueryRequest request)
        {
            return await _periodEndRepository.GetPageAsync(request.PageNumber);
        }
    }
}
