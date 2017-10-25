using ProviderPayments.TestStack.Domain;
using ProviderPayments.TestStack.Infrastructure.Mapping;
using ProviderPayments.TestStack.UI.Models;

namespace ProviderPayments.TestStack.UI.Plumbing.Mapping
{
    public static class MapperConfiguration
    {
        public static AutoMapper.MapperConfiguration Configure()
        {
            return AutoMapperConfiguration.Configure(AddUIMappings);
        }

        private static void AddUIMappings(AutoMapper.IMapperConfigurationExpression cfg)
        {
            cfg.CreateMap<CommitmentModel, Commitment>().ReverseMap();
            cfg.CreateMap<IlrSubmissionModel, IlrSubmission>();
            cfg.CreateMap<AccountModel, Account>().ReverseMap();
            cfg.CreateMap<IlrLearnerModel, IlrLearner>();
        }
    }
}