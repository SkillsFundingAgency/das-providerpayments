using System.Threading.Tasks;
using MediatR;
using SFA.DAS.ProviderPayments.Application.Validation;
using SFA.DAS.ProviderPayments.Application.Validation.Failures;
using SFA.DAS.ProviderPayments.Domain.Data.Entities;
using SFA.DAS.ProviderPayments.Domain.Mapping;

namespace SFA.DAS.ProviderPayments.Application
{
    public abstract class PagedQueryHandler<TRequest, TResponse, TEntity, TDomain> : IAsyncRequestHandler<TRequest, TResponse> 
        where TRequest : IAsyncRequest<TResponse>
        where TResponse : PagedQueryResponse<TDomain>, new()
    {
        private readonly IMapper _mapper;
        private readonly IValidator<TRequest> _validator;

        protected PagedQueryHandler(IMapper mapper, IValidator<TRequest> validator)
        {
            _mapper = mapper;
            _validator = validator;
        }

        public async Task<TResponse> Handle(TRequest message)
        {
            var validationResult = await _validator.ValidateAsync(message);
            if (!validationResult.IsValid())
            {
                return new TResponse
                {
                    IsValid = false,
                    ValidationFailures = validationResult.Failures
                };
            }

            var entities = await GetData(message);
            if (entities == null)
            {
                return new TResponse
                {
                    IsValid = false,
                    ValidationFailures = new[] { new PageNotFoundFailure() }
                };
            }

            return new TResponse
            {
                IsValid = true,
                TotalNumberOfItems = entities.TotalNumberOfItems,
                TotalNumberOfPages = entities.TotalNumberOfPages,
                Items = _mapper.Map<TEntity, TDomain>(entities.Items)
            };
        }

        protected abstract Task<PageOfEntities<TEntity>> GetData(TRequest request);
    }
}
