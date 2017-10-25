using System;
using System.Collections.Generic;
using MediatR;
using SFA.DAS.EAS.Account.Api.Client;
using SFA.DAS.EAS.Account.Api.Types;

namespace SFA.DAS.Payments.Reference.Accounts.Application.GetPageOfAccountsQuery
{
    public class GetPageOfAccountsQueryHandler : IRequestHandler<GetPageOfAccountsQueryRequest, GetPageOfAccountsQueryResponse>
    {
        private readonly IAccountApiClient _accountApiClient;

        public GetPageOfAccountsQueryHandler(IAccountApiClient accountApiClient)
        {
            _accountApiClient = accountApiClient;
        }

        public GetPageOfAccountsQueryResponse Handle(GetPageOfAccountsQueryRequest message)
        {
            try
            {
                var pageOfAccounts = AsyncHelpers.RunSync(async () =>
                {
                    var result = await _accountApiClient.GetPageOfAccounts(message.PageNumber, 1000, message.CorrelationDate);
                    return result;
                });

                return new GetPageOfAccountsQueryResponse
                {
                    IsValid = true,
                    HasMorePages = pageOfAccounts.Page < pageOfAccounts.TotalPages,
                    Items = pageOfAccounts.Data.ToArray()
                };
            }
            catch (Exception ex)
            {
                return new GetPageOfAccountsQueryResponse
                {
                    IsValid = false,
                    Exception = ex
                };
            }
        }
    }
}