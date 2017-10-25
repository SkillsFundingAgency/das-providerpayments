using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProviderPayments.TestStack.Domain;
using ProviderPayments.TestStack.Domain.Data;
using ProviderPayments.TestStack.Domain.Mapping;

namespace ProviderPayments.TestStack.Application
{
    public interface ITrainingService
    {
        Task<IEnumerable<Standard>> GetAllStandards();
        Task<IEnumerable<Framework>> GetAllFrameworks();
    }

    public class TrainingService : ITrainingService
    {
        private readonly ITrainingRepository _trainingRepository;
        private readonly IMapper _mapper;

        public TrainingService(ITrainingRepository trainingRepository, IMapper mapper)
        {
            _trainingRepository = trainingRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<Standard>> GetAllStandards()
        {
            var entities = await _trainingRepository.AllStandards();
            var standards = _mapper.Map<IEnumerable<Standard>>(entities);
            return standards.OrderBy(l => l.DisplayName);
        }
        public async Task<IEnumerable<Framework>> GetAllFrameworks()
        {
            var entities = await _trainingRepository.AllFrameworks();
            var frameworks = _mapper.Map<IEnumerable<Framework>>(entities);
            return frameworks.OrderBy(l => l.DisplayName);
        }
    }
}
