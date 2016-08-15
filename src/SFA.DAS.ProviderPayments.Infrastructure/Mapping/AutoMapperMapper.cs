using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;

namespace SFA.DAS.ProdiverPayments.Infrastructure.Mapping
{
    public class AutoMapperMapper : Domain.Mapping.IMapper
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
