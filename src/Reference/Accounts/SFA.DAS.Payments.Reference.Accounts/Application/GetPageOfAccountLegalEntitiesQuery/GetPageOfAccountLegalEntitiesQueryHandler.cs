using MediatR;
using SFA.DAS.EAS.Account.Api.Client;

namespace SFA.DAS.Payments.Reference.Accounts.Application.GetPageOfAccountLegalEntitiesQuery
{
    public class GetPageOfAccountLegalEntitiesQueryHandler : 
        IRequestHandler<GetPageOfAccountLegalEntitiesQueryRequest, GetPageOfAccountLegalEntitiesQueryResponse>
    {
        private const int PageSize = 1000;
        private readonly IAccountApiClient _accountApiClient;

        public GetPageOfAccountLegalEntitiesQueryHandler(IAccountApiClient accountApiClient)
        {
            _accountApiClient = accountApiClient;
        }

        public GetPageOfAccountLegalEntitiesQueryResponse Handle(GetPageOfAccountLegalEntitiesQueryRequest message)
        {
            var pagedApiResponse = AsyncHelpers.RunSync(async () =>
            {
                var result = await _accountApiClient.GetPageOfAccountLegalEntities(message.PageNumber, PageSize);
                return result;
            });

            return new GetPageOfAccountLegalEntitiesQueryResponse
            {
                /*IsValid = true,
                HasMorePages = pageOfAccounts.Page < pageOfAccounts.TotalPages,*/
                Items = pagedApiResponse.Data.ToArray()
            };
        }
    }
}