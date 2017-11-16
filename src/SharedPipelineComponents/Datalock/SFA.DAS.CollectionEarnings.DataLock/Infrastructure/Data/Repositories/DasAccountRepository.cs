using SFA.DAS.CollectionEarnings.DataLock.Infrastructure.Data.Entities;
using SFA.DAS.Payments.DCFS.Infrastructure.Data;

namespace SFA.DAS.CollectionEarnings.DataLock.Infrastructure.Data.Repositories
{
    public class DasAccountRepository : DcfsRepository, IDasAccountRepository
    {
        public DasAccountRepository(string connectionString)
            : base(connectionString)
        {
        }

        public DasAccounEntity[] GetDasAccounts()
        {
            return QueryByProc<DasAccounEntity>("Reference.GetDasAccounts");
        }
    }
}