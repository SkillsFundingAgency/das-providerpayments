using System;
using System.Linq;
using MediatR;
using SFA.DAS.CollectionEarnings.DataLock.Infrastructure.Data;

namespace SFA.DAS.CollectionEarnings.DataLock.Application.DasAccount.GetDasAccountsQuery
{
    public class GetDasAccountsQueryHandler : IRequestHandler<GetDasAccountsQueryRequest, GetDasAccountsQueryResponse>
    {
        private readonly IDasAccountRepository _dasAccountRepository;

        public GetDasAccountsQueryHandler(IDasAccountRepository dasAccountRepository)
        {
            _dasAccountRepository = dasAccountRepository;
        }

        public GetDasAccountsQueryResponse Handle(GetDasAccountsQueryRequest message)
        {
            try
            {
                var accountEntities = _dasAccountRepository.GetDasAccounts();

                return new GetDasAccountsQueryResponse
                {
                    IsValid = true,
                    Items = accountEntities?.Select(c =>
                        new DasAccount
                        {
                           AccountId = c.AccountId,
                           IsLevyPayer =c.IsLevyPayer
                        }).ToArray()
                };
            }
            catch (Exception ex)
            {
                return new GetDasAccountsQueryResponse
                {
                    IsValid = false,
                    Exception = ex
                };
            }
        }

    }
}
