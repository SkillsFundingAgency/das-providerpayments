using System.Threading.Tasks;
using MediatR;
using SFA.DAS.ProdiverPayments.Application.Validation;
using SFA.DAS.ProdiverPayments.Domain.Data;
using SFA.DAS.ProdiverPayments.Domain.Data.Entities;
using SFA.DAS.ProdiverPayments.Domain.Mapping;

namespace SFA.DAS.ProdiverPayments.Application.PeriodEnd.GetPageOfPeriodEndsQuery
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

            return new GetPageOfPeriodEndsQueryResponse
            {
                TotalNumberOfItems = entitiesPage.TotalNumberOfItems,
                TotalNumberOfPages = entitiesPage.TotalNumberOfPages,
                Items = _mapper.Map<PeriodEndEntity, Domain.PeriodEnd>(entitiesPage.Items)
            };
        }
    }
}
