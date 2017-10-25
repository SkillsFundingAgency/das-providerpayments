using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProviderPayments.TestStack.Domain;
using ProviderPayments.TestStack.Domain.Data;
using ProviderPayments.TestStack.Domain.Mapping;

namespace ProviderPayments.TestStack.Application
{
    public interface ILearnerService
    {
        Task<IEnumerable<Learner>> GetAllLearners();
    }

    public class LearnerService : ILearnerService
    {
        private readonly ILearnerRepository _learnerRepository;
        private readonly IMapper _mapper;

        public LearnerService(ILearnerRepository learnerRepository, IMapper mapper)
        {
            _learnerRepository = learnerRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<Learner>> GetAllLearners()
        {
            var entities = await _learnerRepository.All();
            var learners = _mapper.Map<IEnumerable<Learner>>(entities);
            return learners.OrderBy(l => l.Name);
        }
    }
}
