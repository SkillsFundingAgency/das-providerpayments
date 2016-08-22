using AutoMapper;
using SFA.DAS.ProviderPayments.Api.Dto;
using SFA.DAS.ProviderPayments.Domain;
using SFA.DAS.ProviderPayments.Domain.Data.Entities;
using PeriodType = SFA.DAS.ProviderPayments.Domain.PeriodType;

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
                AddPaymentMappings(cfg);
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

            cfg.CreateMap<Period, PeriodDto>();
            cfg.CreateMap<PeriodEnd, PeriodEndDto>()
                .ForMember(dst => dst.Links, opt => opt.Ignore());
        }
        private static void AddAccountMappings(IMapperConfigurationExpression cfg)
        {
            cfg.CreateMap<AccountEntity, Account>();

            cfg.CreateMap<Account, AccountDto>()
                .ForMember(dst => dst.Links, opt => opt.Ignore());
        }
        private static void AddPaymentMappings(IMapperConfigurationExpression cfg)
        {
            cfg.CreateMap<PaymentEntity, Payment>()
                .ForMember(dst => dst.Account, opt => opt.Ignore())
                .ForMember(dst => dst.Provider, opt => opt.Ignore())
                .ForMember(dst => dst.Apprenticeship, opt => opt.Ignore())
                .ForMember(dst => dst.ReportedPeriod, opt => opt.Ignore())
                .ForMember(dst => dst.DeliveryPeriod, opt => opt.Ignore())
                .AfterMap((source, destination) =>
                {
                    destination.Account = new Account
                    {
                        Id = source.AccountId
                    };
                    destination.Provider = new Provider
                    {
                        Ukprn = source.Ukprn
                    };
                    destination.Apprenticeship = new Apprenticeship
                    {
                        Learner = new Learner
                        {
                            Uln = source.Uln
                        },
                        Course = new Course
                        {
                            StandardCode = source.StandardCode,
                            PathwayCode = source.PathwayCode,
                            FrameworkCode = source.FrameworkCode,
                            ProgrammeType = source.ProgrammeType
                        }
                    };
                    destination.ReportedPeriod = new Period
                    {
                        Code = source.ReportedPeriodCode,
                        PeriodType = PeriodType.CalendarMonth
                    };
                    destination.DeliveryPeriod = new Period
                    {
                        Code = source.DeliveryPeriodCode,
                        PeriodType = PeriodType.CalendarMonth
                    };
                });


            cfg.CreateMap<Payment, PaymentDto>();
            cfg.CreateMap<Provider, ProviderDto>();
            cfg.CreateMap<Apprenticeship, ApprenticeshipDto>();
            cfg.CreateMap<Learner, LearnerDto>();
            cfg.CreateMap<Course, CourseDto>();
            cfg.CreateMap<Period, PeriodDto>();
        }
    }
}
