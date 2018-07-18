using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SFA.DAS.EAS.Account.Api.Client;
using SFA.DAS.EAS.Account.Api.Types;

namespace SFA.DAS.Payments.Reference.Accounts.IntegrationTests.StubbedInfrastructure
{
    public class StubbedApiClient : IAccountApiClient
    {
        internal static List<AccountWithBalanceViewModel> Accounts { get; } = new List<AccountWithBalanceViewModel>();

        public Task<AccountDetailViewModel> GetAccount(string hashedAccountId)
        {
            throw new NotImplementedException();
        }

        public Task<AccountDetailViewModel> GetAccount(long accountId)
        {
            throw new NotImplementedException();
        }

        public Task<PagedApiResponseViewModel<AccountLegalEntityViewModel>> GetPageOfAccountLegalEntities(int pageNumber = 1, int pageSize = 1000)
        {
            throw new NotImplementedException();
        }

        public Task<PagedApiResponseViewModel<AccountWithBalanceViewModel>> GetPageOfAccounts(int pageNumber = 1, int pageSize = 1000, DateTime? toDate = null)
        {
            var skip = (pageNumber - 1) * pageSize;
            var accounts = Accounts.Skip(skip).Take(pageSize).ToList();
            var result = new PagedApiResponseViewModel<AccountWithBalanceViewModel>
            {
                Page = pageNumber,
                TotalPages = (int)Math.Ceiling(Accounts.Count / (float)pageSize),
                Data = accounts
            };
            return Task.FromResult(result);
        }

        Task<ICollection<TeamMemberViewModel>> IAccountApiClient.GetAccountUsers(string accountId)
        {
            throw new NotImplementedException();
        }

        public Task<ICollection<TeamMemberViewModel>> GetAccountUsers(long accountId)
        {
            throw new NotImplementedException();
        }

        public Task<ICollection<TransactionSummaryViewModel>> GetTransactionSummary(string accountId)
        {
            throw new NotImplementedException();
        }

        Task<T> IAccountApiClient.GetResource<T>(string uri)
        {
            throw new NotImplementedException();
        }

        public Task<StatisticsViewModel> GetStatistics()
        {
            throw new NotImplementedException();
        }

        public Task<ICollection<TransferConnectionViewModel>> GetTransferConnections(string accountId)
        {
            throw new NotImplementedException();
        }

        Task<ICollection<AccountDetailViewModel>> IAccountApiClient.GetUserAccounts(string userId)
        {
            throw new NotImplementedException();
        }

        public Task<LegalEntityViewModel> GetLegalEntity(string accountId, long id)
        {
            throw new NotImplementedException();
        }

        public Task<ICollection<ResourceViewModel>> GetLegalEntitiesConnectedToAccount(string accountId)
        {
            throw new NotImplementedException();
        }

        public Task<ICollection<ResourceViewModel>> GetPayeSchemesConnectedToAccount(string accountId)
        {
            throw new NotImplementedException();
        }

        public Task<ICollection<LevyDeclarationViewModel>> GetLevyDeclarations(string accountId)
        {
            throw new NotImplementedException();
        }

        public Task<EmployerAgreementView> GetEmployerAgreement(string accountId, string legalEntityId, string agreementId)
        {
            throw new NotImplementedException();
        }

        public Task<TransactionsViewModel> GetTransactions(string accountId, int year, int month)
        {
            throw new NotImplementedException();
        }
    }
}
