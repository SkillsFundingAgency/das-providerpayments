using System.Threading.Tasks;
using MediatR;
using SFA.DAS.ProviderPayments.Application.Account.GetAccountsAffectedInPeriodQuery;
using SFA.DAS.ProviderPayments.Application.Validation;
using SFA.DAS.ProviderPayments.Application.Validation.Failures;
using SFA.DAS.ProviderPayments.Domain.Data;
using SFA.DAS.ProviderPayments.Domain.Data.Entities;
using SFA.DAS.ProviderPayments.Domain.Mapping;

namespace SFA.DAS.ProviderPayments.Application.PeriodEnd.GetPageOfPeriodEndsQuery
{
    public class GetPageOfPeriodEndsQueryHandler : IAsyncRequestHandler<GetPageOfPeriodEndsQueryRequest, GetPageOfPeriodEndsQueryResponse>
    {
        private readonly IPeriodEndRepository _periodEndRepository;
        private readonly IMapper _mapper;
        private readonly IValidator<GetPageOfPeriodEndsQueryRequest> _validator;

        public GetPageOfPeriodEndsQueryHandler(IPeriodEndRepository periodEndRepository, IMapper mapper, IValidator<GetPageOfPeriodEndsQueryRequest> validator)
        {
            _periodEndRepository = periodEndRepository;
            _mapper = mapper;
            _validator = validator;
        }

        public async Task<GetPageOfPeriodEndsQueryResponse> Handle(GetPageOfPeriodEndsQueryRequest message)
        {
            var validationResult = await _validator.ValidateAsync(message);
            if (!validationResult.IsValid())
            {
                return new GetPageOfPeriodEndsQueryResponse
                {
                    IsValid = false,
                    ValidationFailures = validationResult.Failures
                };
            }

            var entitiesPage = await _periodEndRepository.GetPageAsync(message.PageNumber);
            if (entitiesPage == null)
            {
                return new GetPageOfPeriodEndsQueryResponse
                {
                    IsValid = false,
                    ValidationFailures = new[] { new PageNotFoundFailure() }
                };
            }

            return new GetPageOfPeriodEndsQueryResponse
            {
                IsValid = true,
                TotalNumberOfItems = entitiesPage.TotalNumberOfItems,
                TotalNumberOfPages = entitiesPage.TotalNumberOfPages,
                Items = _mapper.Map<PeriodEndEntity, Domain.PeriodEnd>(entitiesPage.Items)
            };
        }
    }
}
