using System;
using System.Linq;
using MediatR;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.Application.Providers.GetProvidersQuery
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
                            Ukprn = p.Ukprn,
                            IlrSubmissionDateTime = p.IlrSubmissionDateTime
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