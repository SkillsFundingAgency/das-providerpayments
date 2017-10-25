using System.Collections.Generic;
using System.Threading.Tasks;
using ProviderPayments.TestStack.Domain.Data.Entities;

namespace ProviderPayments.TestStack.Domain.Data
{
    public interface ITrainingRepository : IRepository
    {
        Task<IEnumerable<StandardEntity>> AllStandards();
        Task<StandardEntity> SingleStandard(long standardCode);

        Task<IEnumerable<FrameworkEntity>> AllFrameworks();
        Task<FrameworkEntity> SingleFramework(int pathwayCode, int frameworkCode, int programmeType);
    }
}
