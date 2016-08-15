using AutoMapper;
using SFA.DAS.ProdiverPayments.Domain;
using SFA.DAS.ProdiverPayments.Domain.Data.Entities;

namespace SFA.DAS.ProdiverPayments.Infrastructure.Mapping
{
    public static class AutoMapperConfiguration
    {
        public static MapperConfiguration Configure()
        {
            return new MapperConfiguration(cfg =>
            {
                AddPeriodEndMappings(cfg);
            });
        }

        private static void AddPeriodEndMappings(IMapperConfigurationExpression cfg)
        {
            cfg.CreateMap<PeriodEndEntity, PeriodEnd>();
        }
    }
}
