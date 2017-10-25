using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProviderPayments.TestStack.Domain.Data;
using ProviderPayments.TestStack.Domain.Data.Entities;

namespace ProviderPayments.TestStack.Infrastructure.Data
{
    public class SqlServerTrainingRepository : SqlServerRepository, ITrainingRepository
    {
        public SqlServerTrainingRepository()
            : base("TransientConnectionString")
        {
        }

        public async Task<IEnumerable<StandardEntity>> AllStandards()
        {
            return await Query<StandardEntity>(@"SELECT StandardCode [Code]
                                                       ,DisplayName
                                                 FROM TestStack.TrainingStandard");
        }
        public async Task<StandardEntity> SingleStandard(long standardCode)
        {
            return (await Query<StandardEntity>(@"SELECT StandardCode [Code]
                                                        ,DisplayName
                                                  FROM TestStack.TrainingStandard
                                                  WHERE StandardCode = @standardCode", new { standardCode })).SingleOrDefault();
        }

        public async Task<IEnumerable<FrameworkEntity>> AllFrameworks()
        {
            return await Query<FrameworkEntity>(@"SELECT PathwayCode
                                                        ,FrameworkCode
                                                        ,ProgrammeType
                                                        ,DisplayName
                                                  FROM TestStack.TrainingFramework");
        }
        public async Task<FrameworkEntity> SingleFramework(int pathwayCode, int frameworkCode, int programmeType)
        {
            return (await Query<FrameworkEntity>(@"SELECT PathwayCode
                                                         ,FrameworkCode
                                                         ,ProgrammeType
                                                         ,DisplayName
                                                   FROM TestStack.TrainingFramework
                                                   WHERE PathwayCode = @pathwayCode
                                                   AND FrameworkCode = @frameworkCode
                                                   AND ProgrammeType = @programmeType", 
                    new { pathwayCode, frameworkCode, programmeType })).SingleOrDefault();
        }

    }
}
