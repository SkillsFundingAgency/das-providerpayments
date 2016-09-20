using System;
using System.Linq;
using MediatR;
using SFA.DAS.Payments.Calc.CoInvestedPayments.Infrastructure.Data;

namespace SFA.DAS.Payments.Calc.CoInvestedPayments.Application.Providers.GetProvidersQuery
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
                    Items = providerEntities?.Select(p =>
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