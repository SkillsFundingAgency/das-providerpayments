using System;
using System.Collections.Generic;
using CS.Common.External.Interfaces;
using MediatR;
using NLog;
using SFA.DAS.Payments.DCFS.Context;
using StructureMap;

namespace SFA.DAS.Payments.Reference.Commitments.Infrastructure.DependencyResolution
{
    public class CommitmentsRegistry : Registry
    {
        public CommitmentsRegistry(Type taskType, ContextWrapper contextWrapper)
        {
            Scan(scan =>
            {
                scan.AssemblyContainingType<CommitmentsRegistry>();

                scan.RegisterConcreteTypesAgainstTheFirstInterface();
            });

            For<ILogger>().Use(() => LogManager.GetLogger(taskType.FullName));
            For<IExternalContext>().Use(contextWrapper.Context);

            For<Events.Api.Client.IEventsApi>().Use(() => ApiClientFactory.Instance.CreateClient(contextWrapper.Context));

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