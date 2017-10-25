using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProviderPayments.TestStack.Domain;
using ProviderPayments.TestStack.Domain.Data;
using ProviderPayments.TestStack.Domain.Mapping;

namespace ProviderPayments.TestStack.Application
{
    public interface IProviderService
    {
        Task<IEnumerable<Provider>> GetAllProviders();
    }

    public class ProviderService : IProviderService
    {
        private readonly IProviderRepository _providerRepository;
        private readonly IMapper _mapper;

        public ProviderService(IProviderRepository providerRepository, IMapper mapper)
        {
            _providerRepository = providerRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<Provider>> GetAllProviders()
        {
            var entities = await _providerRepository.All();
            var providers = _mapper.Map<IEnumerable<Provider>>(entities);
            return providers.OrderBy(p => p.Name);
        }
    }
}
