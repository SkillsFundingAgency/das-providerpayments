using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProviderPayments.TestStack.Domain.Data;
using ProviderPayments.TestStack.Domain.Data.Entities;

namespace ProviderPayments.TestStack.Infrastructure.Data
{
    public class SqlServerProviderRepository : SqlServerRepository, IProviderRepository
    {
        public SqlServerProviderRepository()
            : base("TransientConnectionString")
        {
        }

        public async Task<IEnumerable<ProviderEntity>> All()
        {
            return await Query<ProviderEntity>(@"SELECT Ukprn
                                                        ,ProviderName [Name]
                                                  FROM TestStack.Provider");
        }

        public async Task<ProviderEntity> Single(long id)
        {
            return (await Query<ProviderEntity>(@"SELECT Ukprn
                                                        ,ProviderName [Name]
                                                  FROM TestStack.Provider
                                                  WHERE Ukprn = @id",new { id })).SingleOrDefault();
        }
    }
}
