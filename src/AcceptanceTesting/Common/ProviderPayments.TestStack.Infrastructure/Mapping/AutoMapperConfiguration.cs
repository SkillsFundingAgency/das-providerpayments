using System;
using System.Linq;
using AutoMapper;
using ProviderPayments.TestStack.Domain;
using ProviderPayments.TestStack.Domain.Data.Entities;

namespace ProviderPayments.TestStack.Infrastructure.Mapping
{
    public static class AutoMapperConfiguration
    {
        public static MapperConfiguration Configure(Action<IMapperConfigurationExpression> externalMappingsAction)
        {
            return new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<CommitmentEntity, Commitment>()
                    .ForMember(dst => dst.Account, opt => opt.Ignore())
                    .ForMember(dst => dst.Provider, opt => opt.Ignore())
                    .ForMember(dst => dst.Learner, opt => opt.Ignore())
                    .ForMember(dst => dst.Course, opt => opt.Ignore())
                    .ForMember(dst => dst.Status, opt => opt.Ignore())
                    .AfterMap((src, dst) =>
                    {
                        dst.Account = new Account
                        {
                            Id = long.Parse(src.AccountId)
                        };
                        dst.Provider = new Provider
                        {
                            Ukprn = src.Ukprn
                        };
                        dst.Learner = new Learner
                        {
                            Uln = src.Uln
                        };
                        dst.Course = new Course
                        {
                            StandardCode = src.StandardCode ?? 0,
                            PathwayCode = src.PathwayCode ?? 0,
                            FrameworkCode = src.FrameworkCode ?? 0,
                            ProgrammeType = src.ProgrammeType ?? 0
                        };

                        dst.Status = (PaymentStatus) src.PaymentStatus;
                    });
                cfg.CreateMap<Commitment, CommitmentEntity>()
                    .ForMember(dst => dst.AccountId, opt => opt.MapFrom(src => src.Account.Id))
                    .ForMember(dst => dst.Ukprn, opt => opt.MapFrom(src => src.Provider.Ukprn))
                    .ForMember(dst => dst.Uln, opt => opt.MapFrom(src => src.Learner.Uln))
                    .ForMember(dst => dst.StandardCode, opt => opt.MapFrom(src => src.Course.StandardCode))
                    .ForMember(dst => dst.PathwayCode, opt => opt.MapFrom(src => src.Course.PathwayCode))
                    .ForMember(dst => dst.FrameworkCode, opt => opt.MapFrom(src => src.Course.FrameworkCode))
                    .ForMember(dst => dst.ProgrammeType, opt => opt.MapFrom(src => src.Course.ProgrammeType))
                    .AfterMap((src, dst) =>
                    {
                        dst.PaymentStatus = (int) src.Status;
                        dst.PaymentStatusDescription = src.Status.ToString();
                    });


                cfg.CreateMap<AccountEntity, Account>().ReverseMap();

                cfg.CreateMap<ProviderEntity, Provider>();

                cfg.CreateMap<LearnerEntity, Learner>();

                cfg.CreateMap<StandardEntity, Standard>();

                cfg.CreateMap<FrameworkEntity, Framework>();

                cfg.CreateMap<ComponentEntity, Component>()
                    .ForMember(dst => dst.CurrentVersion, opt => opt.MapFrom(src => src.Version));

                cfg.CreateMap<PaymentReportEntity, PaymentReport>();

                cfg.CreateMap<ProcessStepEntity, ProcessStep>();
                cfg.CreateMap<ProcessStatusEntity, ProcessStatus>()
                    .AfterMap((src, dst) =>
                    {
                        dst.Steps = dst.Steps.OrderBy(s => s.Index).ToArray();
                    });

                externalMappingsAction?.Invoke(cfg);
            });
        }
    }
}
