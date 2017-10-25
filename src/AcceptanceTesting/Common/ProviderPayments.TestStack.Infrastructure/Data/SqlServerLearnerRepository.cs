using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProviderPayments.TestStack.Domain.Data;
using ProviderPayments.TestStack.Domain.Data.Entities;

namespace ProviderPayments.TestStack.Infrastructure.Data
{
    public class SqlServerLearnerRepository : SqlServerRepository, ILearnerRepository
    {
        public SqlServerLearnerRepository()
            : base("TransientConnectionString")
        {
        }

        public async Task<IEnumerable<LearnerEntity>> All()
        {
            return await Query<LearnerEntity>(@"SELECT Uln
	                                                  ,LearnerName [Name]
                                                FROM TestStack.Learner");
        }

        public async Task<LearnerEntity> Single(long id)
        {
            return (await Query<LearnerEntity>(@"SELECT Uln
	                                                   ,LearnerName [Name]
                                                 FROM TestStack.Learner
                                                 WHERE Uln = @id", new { id })).SingleOrDefault();
        }
    }
}
