using System;
using MediatR;
using SFA.DAS.EAS.Account.Api.Client;

namespace SFA.DAS.Payments.Reference.Accounts.Application.GetPageOfAccountLegalEntitiesQuery
{
    public class GetPageOfAccountLegalEntitiesQueryHandler : 
        IRequestHandler<GetPageOfAccountLegalEntitiesQueryRequest, GetPageOfAccountLegalEntitiesQueryResponse>
    {
        private readonly IAccountApiClient _accountApiClient;

        public GetPageOfAccountLegalEntitiesQueryHandler(IAccountApiClient accountApiClient)
        {
            _accountApiClient = accountApiClient;
        }

        public GetPageOfAccountLegalEntitiesQueryResponse Handle(GetPageOfAccountLegalEntitiesQueryRequest message)
        {
            try
            {
                var pagedApiResponse = AsyncHelpers.RunSync(async () =>
                    {
                        var result = await _accountApiClient.GetPageOfAccountLegalEntities(message.PageNumber);
                        return result;
                    });

                return new GetPageOfAccountLegalEntitiesQueryResponse
                {
                    IsValid = true,
                    HasMorePages = pagedApiResponse.Page < pagedApiResponse.TotalPages,
                    Items = pagedApiResponse.Data.ToArray()
                };
            }
            catch (Exception ex)
            {
                return new GetPageOfAccountLegalEntitiesQueryResponse
                {
                    IsValid = false,
                    Exception = ex
                };
            }
        }
    }
}