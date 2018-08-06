using System;
using System.Collections.Generic;
using NLog;
using SFA.DAS.ProviderPayments.Calc.Common.Services;
using SFA.DAS.ProviderPayments.Calc.Datalock.Shared.Infrastructure.Data.Repositories;
using SFA.DAS.ProviderPayments.Calc.Datalock.Shared.Matcher;
using SFA.DAS.ProviderPayments.Calc.Shared.Infrastructure.Data;
using SFA.DAS.ProviderPayments.Calc.Shared.Infrastructure.Data.Repositories;
using StructureMap;

namespace SFA.DAS.ProviderPayments.Calc.Datalock.PeriodEnd.DependencyResolution
{
    public class DataLockRegistry : Registry
    {
        public DataLockRegistry(Type taskType)
        {
            Scan(
                scan =>
                {
                    scan.AssemblyContainingType<DataLockRegistry>();

                    scan.RegisterConcreteTypesAgainstTheFirstInterface();
                });

            For<ILogger>().Use(() => LogManager.GetLogger(taskType.FullName));

            // TODO: Fix so can be registered with convention
            For<ICommitmentRepository>().Use<CommitmentRepository>();
            For<IPriceEpisodeRepository>().Use<PriceEpisodeRepository>();
            For<IValidationErrorRepository>().Use<DatalockRepository>();
            For<IPriceEpisodeMatchRepository>().Use<DatalockRepository>();
            For<IProviderRepository>().Use<ProviderRepository>();
            For<IPriceEpisodePeriodMatchRepository>().Use<DatalockRepository>();
            For<IDateTimeProvider>().Use<DateTimeProvider>();
            For<IDasAccountRepository>().Use<DasAccountRepository>();
            For<IIncentiveEarningsRepository>().Use<IncentiveEarningsRepository>();
            
            For<IMatcher>().Use(() => MatcherFactory.CreateMatcher());
        }

        private static IEnumerable<object> GetAllInstances(IContext ctx, Type t)
        {
            return ctx.GetAllInstances(t);
        }

        private static object GetInstance(IContext ctx, Type t)
        {
            return ctx.GetInstance(t);
        }
    }
}