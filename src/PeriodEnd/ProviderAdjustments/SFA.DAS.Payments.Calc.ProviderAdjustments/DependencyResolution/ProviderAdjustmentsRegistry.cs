using System;
using System.Collections.Generic;
using MediatR;
using NLog;
using SFA.DAS.Payments.Calc.ProviderAdjustments.Infrastructure.Data;
using SFA.DAS.Payments.Calc.ProviderAdjustments.Infrastructure.Data.Repositories;
using SFA.DAS.Payments.Calc.ProviderAdjustments.Services;
using SFA.DAS.Payments.DCFS.Context;
using StructureMap;

namespace SFA.DAS.Payments.Calc.ProviderAdjustments.DependencyResolution
{
    public class ProviderAdjustmentsRegistry : Registry
    {
        public ProviderAdjustmentsRegistry(Type taskType, ContextWrapper contextWrapper)
        {
            Scan(
               scan =>
               {
                   scan.AssemblyContainingType<ProviderAdjustmentsRegistry>();

                   scan.RegisterConcreteTypesAgainstTheFirstInterface();
               });

            For<ContextWrapper>().Use(contextWrapper);

            // TODO: Fix so can be registered with convention
            For<IAdjustmentRepository>().Use<DcfsAdjustmentRepository>();
            For<ICollectionPeriodRepository>().Use<DcfsCollectionPeriodRepository>();
            For<IPaymentRepository>().Use<DcfsPaymentRepository>();
            For<ICalculateProviderPayments>().Use<ProviderPaymentsCalculator>();

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