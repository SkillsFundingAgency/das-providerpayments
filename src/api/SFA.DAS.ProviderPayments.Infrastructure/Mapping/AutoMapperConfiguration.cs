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
                AddAccountMappings(cfg);
            });
        }

        private static void AddPeriodEndMappings(IMapperConfigurationExpression cfg)
        {
            cfg.CreateMap<PeriodEndEntity, PeriodEnd>()
                .ForMember(dst => dst.Period, opt => opt.Ignore())
                .AfterMap((source, destination) =>
                {
                    destination.Period = new Period
                    {
                        Code = source.PeriodCode,
                        PeriodType = (PeriodType)source.PeriodType
                    };
                });
        }
        private static void AddAccountMappings(IMapperConfigurationExpression cfg)
        {
            cfg.CreateMap<AccountEntity, Account>();
        }
    }
}
