﻿using System;
using System.Collections.Generic;
using MediatR;
using NLog;
using SFA.DAS.Provider.Events.DataLock.Application.GetCurrentProviderEvents;
using SFA.DAS.Provider.Events.DataLock.Domain.Data;
using SFA.DAS.Provider.Events.DataLock.Infrastructure.Data;
using StructureMap;

namespace SFA.DAS.Provider.Events.DataLock.Infrastructure.DependencyResolution
{
    public class DataLockEventsRegistry : Registry
    {
        public DataLockEventsRegistry(Type taskType)
        {
            Scan(
                scan =>
                {
                    scan.AssemblyContainingType<DataLockEventsRegistry>();

                    scan.RegisterConcreteTypesAgainstTheFirstInterface();
                });

            For<ILogger>().Use(() => LogManager.GetLogger(taskType.FullName));

            For<ICommitmentRepository>().Use<DcfsCommitmentRepository>();
            For<IDataLockEventCommitmentVersionRepository>().Use<DcfsDataLockEventCommitmentVersionRepository>();
            For<IDataLockEventErrorRepository>().Use<DcfsDataLockEventErrorRepository>();
            For<IDataLockEventPeriodRepository>().Use<DcfsDataLockEventPeriodRepository>();
            For<IDataLockEventRepository>().Use<DcfsDataLockEventRepository>();
            For<IIlrPriceEpisodeRepository>().Use<DcfsIlrPriceEpisodeRepository>();
            For<IPriceEpisodeMatchRepository>().Use<DcfsPriceEpisodeMatchRepository>();
            For<IPriceEpisodePeriodMatchRepository>().Use<DcfsPriceEpisodePeriodMatchRepository>();
            For<IValidationErrorRepository>().Use<DcfsValidationErrorRepository>();
            For<IProviderRepository>().Use<DcfsProviderRepository>();
            For<IDataLockEventDataRepository>().Use<DcfsDataLockEventDataRepository>();
            For<ICollectionPeriodRepository>().Use<DcfsCollectionPeriodRepository>();

            For<IRequestHandler<GetCurrentProviderEventsRequest, GetCurrentProviderEventsResponse>>().Use<GetCurrentProviderEventsHandler>();

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
