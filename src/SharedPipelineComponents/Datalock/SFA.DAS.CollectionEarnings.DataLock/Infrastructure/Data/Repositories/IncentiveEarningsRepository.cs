using SFA.DAS.CollectionEarnings.DataLock.Infrastructure.Data.Entities;
using SFA.DAS.Payments.DCFS.Infrastructure.Data;

namespace SFA.DAS.CollectionEarnings.DataLock.Infrastructure.Data.Repositories
{
    public class IncentiveEarningsRepository : DcfsRepository, IIncentiveEarningsRepository
    {
        private const string IncentiveEarningsSource = "DataLock.vw_16To18IncentiveEarnings";
        private const string IncentiveEarningsColumns = "Ukprn," +
                                              "LearnRefNumber," +
                                              "Period," +
                                              "PriceEpisodeFirstEmp1618Pay," +
                                              "PriceEpisodeSecondEmp1618Pay," +
                                              "PriceEpisodeIdentifier";
                                              
        private const string SelectIncentiveEarnings = "SELECT " + IncentiveEarningsColumns + " FROM " + IncentiveEarningsSource + " WHERE Ukprn = @Ukprn";
        

        public IncentiveEarningsRepository(string connectionString)
            : base(connectionString)
        {
        }

        public IncentiveEarningsEntity[] GetIncentiveEarnings(long ukprn)
        {
            return Query<IncentiveEarningsEntity>(SelectIncentiveEarnings, new { ukprn });
        }
    }
}