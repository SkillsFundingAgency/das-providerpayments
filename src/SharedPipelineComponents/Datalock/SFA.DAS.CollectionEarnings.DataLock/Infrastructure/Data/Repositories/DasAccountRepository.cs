using SFA.DAS.CollectionEarnings.DataLock.Infrastructure.Data.Entities;
using SFA.DAS.Payments.DCFS.Infrastructure.Data;

namespace SFA.DAS.CollectionEarnings.DataLock.Infrastructure.Data.Repositories
{
    public class DasAccountRepository : DcfsRepository, IDasAccountRepository
    {
        private const string DasAccountSource = "Reference.DasAccounts";
        private const string DasAccountColumns = "AccountId," +
                                                 "IslevyPayer";
        private const string SelectDasAccounts = "SELECT " + DasAccountColumns + " FROM " + DasAccountSource;
        

        public DasAccountRepository(string connectionString)
            : base(connectionString)
        {
        }

        public DasAccounEntity[] GetDasAccounts()
        {
            return Query<DasAccounEntity>(SelectDasAccounts);
        }
    }
}