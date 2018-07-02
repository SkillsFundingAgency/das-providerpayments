using System;
using System.Collections.Generic;
using MediatR;
using NLog;
using SFA.DAS.Payments.DCFS.Context;
using SFA.DAS.ProviderPayments.Calc.Refunds.Infrastructure.Data;
using SFA.DAS.ProviderPayments.Calc.Refunds.Infrastructure.Repositories;
using SFA.DAS.ProviderPayments.Calc.Refunds.Services;
using SFA.DAS.ProviderPayments.Calc.Refunds.Services.Dependencies;
using StructureMap;

namespace SFA.DAS.ProviderPayments.Calc.Refunds.DependencyResolution
{
    public class RefundsRegistry : Registry
    {
        public RefundsRegistry(Type taskType, ContextWrapper contextWrapper)
        {
            Scan(
                scan =>
                {
                    scan.AssemblyContainingType<RefundsRegistry>();

                    scan.RegisterConcreteTypesAgainstTheFirstInterface();
                });

            For<ContextWrapper>().Use(contextWrapper);

            For<IProviderRepository>().Use<ProviderRepository>();
            For<IProviderProcessor>().Use<ProviderProcessor>();
            //For<ISortProviderDataIntoLearnerData>().Use<ISortProviderDataIntoLearnerData>();
            For<ILogger>().Use(() => LogManager.GetLogger(taskType.FullName));

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
