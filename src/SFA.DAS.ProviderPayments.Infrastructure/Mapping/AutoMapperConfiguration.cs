using AutoMapper;
using SFA.DAS.ProviderPayments.Domain;
using SFA.DAS.ProviderPayments.Domain.Data.Entities;

namespace SFA.DAS.ProviderPayments.Infrastructure.Mapping
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
