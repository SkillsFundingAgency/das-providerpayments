using System;
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
            throw new NotImplementedException();
        }

        public IEnumerable<TDestination> Map<TSource, TDestination>(IEnumerable<TSource> source)
        {
            throw new NotImplementedException();
        }
    }
}
