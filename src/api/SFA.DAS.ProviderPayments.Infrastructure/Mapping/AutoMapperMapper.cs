using System.Collections.Generic;
using AutoMapper;

namespace SFA.DAS.ProviderPayments.Infrastructure.Mapping
{
    public class AutoMapperMapper : ProviderPayments.Domain.Mapping.IMapper
    {
        private IMapper _mapper;

        public AutoMapperMapper(MapperConfiguration mapperConfiguration)
        {
            _mapper = mapperConfiguration.CreateMapper();
        }

        public TDestination Map<TSource, TDestination>(TSource source)
        {
            return _mapper.Map<TDestination>(source);
        }

        public IEnumerable<TDestination> Map<TSource, TDestination>(IEnumerable<TSource> source)
        {
            return _mapper.Map<TDestination[]>(source);
        }
    }
}
