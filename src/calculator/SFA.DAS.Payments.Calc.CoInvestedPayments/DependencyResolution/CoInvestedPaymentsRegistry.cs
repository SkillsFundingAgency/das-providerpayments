using System;
using System.Collections.Generic;
using MediatR;
using NLog;
using SFA.DAS.Payments.Calc.CoInvestedPayments.Infrastructure.Data;
using SFA.DAS.Payments.Calc.CoInvestedPayments.Infrastructure.Data.Repositories;
using SFA.DAS.ProviderPayments.Calc.Common.Context;
using StructureMap;

namespace SFA.DAS.Payments.Calc.CoInvestedPayments.DependencyResolution
{
    public class CoInvestedPaymentsRegistry : Registry
    {
        public CoInvestedPaymentsRegistry(Type taskType)
        {
            Scan(
               scan =>
               {
                   scan.AssemblyContainingType<CoInvestedPaymentsRegistry>();

                   scan.RegisterConcreteTypesAgainstTheFirstInterface();
               });

            For<ILogger>().Use(() => LogManager.GetLogger(taskType.FullName));

            // TODO: Fix so can be registered with convention
            For<ICollectionPeriodRepository>().Use<CollectionPeriodRepository>();
            For<IPaymentDueRepository>().Use<PaymentDueRepository>();
            For<IPaymentRepository>().Use<PaymentRepository>();
            For<IProviderRepository>().Use<ProviderRepository>();

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
