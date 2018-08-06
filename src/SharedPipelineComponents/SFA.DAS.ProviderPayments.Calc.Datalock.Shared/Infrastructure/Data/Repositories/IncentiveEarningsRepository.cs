using System.Collections.Generic;
using SFA.DAS.Payments.DCFS.Infrastructure.Data;
using SFA.DAS.ProviderPayments.Calc.Datalock.Shared.Infrastructure.Data.Entities;

namespace SFA.DAS.ProviderPayments.Calc.Datalock.Shared.Infrastructure.Data.Repositories
{
    public interface IIncentiveEarningsRepository
    {
        IEnumerable<IncentiveEarningsEntity> GetIncentiveEarnings(long ukprn);
    }

    public class IncentiveEarningsRepository : DcfsRepository, IIncentiveEarningsRepository
    {
        private const string IncentiveEarningsSource = "DataLock.vw_16To18IncentiveEarnings";
        private const string IncentiveEarningsColumns = "Ukprn," +
                                              "LearnRefNumber," +
                                              "Period," +
                                              "PriceEpisodeFirstEmp1618Pay," +
                                              "PriceEpisodeSecondEmp1618Pay," +
                                              "PriceEpisodeIdentifier";
                                              
        private const string SelectIncentiveEarnings = "SELECT " + IncentiveEarningsColumns + 
                                                       " FROM " + IncentiveEarningsSource + 
                                                       " WHERE Ukprn = @Ukprn";
        

        public IncentiveEarningsRepository(string connectionString)
            : base(connectionString)
        {
        }

        public IEnumerable<IncentiveEarningsEntity> GetIncentiveEarnings(long ukprn)
        {
            return Query<IncentiveEarningsEntity>(SelectIncentiveEarnings, new { ukprn });
        }
    }
}