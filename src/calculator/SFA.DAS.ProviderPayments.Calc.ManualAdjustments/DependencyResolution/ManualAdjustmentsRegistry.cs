using System;
using System.Collections.Generic;
using MediatR;
using NLog;
using SFA.DAS.Payments.DCFS.Context;
using SFA.DAS.ProviderPayments.Calc.ManualAdjustments.Infrastructure;
using SFA.DAS.ProviderPayments.Calc.ManualAdjustments.Infrastructure.Dcfs;
using StructureMap;

namespace SFA.DAS.ProviderPayments.Calc.ManualAdjustments.DependencyResolution
{
    public class ManualAdjustmentsRegistry : Registry
    {
        public ManualAdjustmentsRegistry(Type taskType, ContextWrapper contextWrapper)
        {
            Scan(
               scan =>
               {
                   scan.AssemblyContainingType<ManualAdjustmentsRegistry>();

                   scan.RegisterConcreteTypesAgainstTheFirstInterface();
               });

            For<IAccountRepository>().Use<DcfsAccountRepository>();
            For<ICollectionPeriodRepository>().Use<DcfsCollectionPeriodRepository>();
            For<IManualAdjustmentRepository>().Use<DcfsManualAdjustmentRepository>();
            For<IPaymentRepository>().Use<DcfsPaymentRepository>();
            For<IRequiredPaymentRepository>().Use<DcfsRequiredPaymentRepository>();

            For<ContextWrapper>().Use(contextWrapper);

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