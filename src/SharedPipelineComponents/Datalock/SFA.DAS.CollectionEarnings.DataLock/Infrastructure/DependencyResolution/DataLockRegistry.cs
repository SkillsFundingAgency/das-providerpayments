using System;
using System.Collections.Generic;
using MediatR;
using NLog;
using SFA.DAS.CollectionEarnings.DataLock.Infrastructure.Data;
using SFA.DAS.CollectionEarnings.DataLock.Infrastructure.Data.Repositories;
using SFA.DAS.CollectionEarnings.DataLock.Tools.Providers;
using StructureMap;
using SFA.DAS.CollectionEarnings.DataLock.Application.DataLock.Matcher;

namespace SFA.DAS.CollectionEarnings.DataLock.Infrastructure.DependencyResolution
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
            For<IValidationErrorRepository>().Use<ValidationErrorRepository>();
            For<IPriceEpisodeMatchRepository>().Use<PriceEpisodeMatchRepository>();
            For<IProviderRepository>().Use<ProviderRepository>();
            For<IPriceEpisodePeriodMatchRepository>().Use<PriceEpisodePeriodMatchRepository>();
            For<IDateTimeProvider>().Use<DateTimeProvider>();
            For<IDasAccountRepository>().Use<DasAccountRepository>();
            For<IIncentiveEarningsRepository>().Use<IncentiveEarningsRepository>();


            For<IMatcher>().Use(() => MatcherFactory.CreateMatcher());

            For<SingleInstanceFactory>().Use<SingleInstanceFactory>(ctx => t => GetInstance(ctx, t));
            For<MultiInstanceFactory>().Use<MultiInstanceFactory>(ctx => t => GetAllInstances(ctx, t));
            For<IMediator>().Use<Mediator>();
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