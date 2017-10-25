using System;
using System.Linq;
using MediatR;
using SFA.DAS.CollectionEarnings.DataLock.Infrastructure.Data;

namespace SFA.DAS.CollectionEarnings.DataLock.Application.Provider.GetProvidersQuery
{
    public class GetProvidersQueryHandler : IRequestHandler<GetProvidersQueryRequest, GetProvidersQueryResponse>
    {
        private readonly IProviderRepository _providerRepository;

        public GetProvidersQueryHandler(IProviderRepository providerRepository)
        {
            _providerRepository = providerRepository;
        }

        public GetProvidersQueryResponse Handle(GetProvidersQueryRequest message)
        {
            try
            {
                var providerEntities = _providerRepository.GetAllProviders();

                return new GetProvidersQueryResponse
                {
                    IsValid = true,
                    Items = providerEntities == null
                        ? null
                        : providerEntities.Select(p =>
                            new Provider
                            {
                                Ukprn = p.Ukprn
                            }).ToArray()
                };
            }
            catch (Exception ex)
            {
                return new GetProvidersQueryResponse
                {
                    IsValid = false,
                    Exception = ex
                };
            }
        }
    }
}