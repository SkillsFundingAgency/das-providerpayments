using AutoMapper;

namespace ProviderPayments.TestStack.Infrastructure.Mapping
{
    public class AutoMapperMapper : Domain.Mapping.IMapper
    {
        private IMapper _mapper;

        public AutoMapperMapper(MapperConfiguration mapperConfiguration)
        {
            _mapper = mapperConfiguration.CreateMapper();
        }

        public TDestination Map<TDestination>(object source)
        {
            return _mapper.Map<TDestination>(source);
        }
    }
}
