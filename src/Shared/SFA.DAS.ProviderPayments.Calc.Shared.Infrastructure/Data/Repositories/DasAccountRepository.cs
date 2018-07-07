using System.Collections.Generic;
using SFA.DAS.Payments.DCFS.Infrastructure.Data;
using SFA.DAS.ProviderPayments.Calc.Shared.Infrastructure.Data.Entities;

namespace SFA.DAS.ProviderPayments.Calc.Shared.Infrastructure.Data.Repositories
{
    public class DasAccountRepository : DcfsRepository, IDasAccountRepository
    {
        public DasAccountRepository(string transientConnectionString) 
            : base(transientConnectionString) { }

        public void AddMany(List<DasAccountEntity> dasAccounts)
        {
            ExecuteBatch(dasAccounts.ToArray(), "Reference.DasAccounts");
        }
    }
}