using System;
using System.Collections.Generic;
using MediatR;
using NLog;
using SFA.DAS.ProviderPayments.Calc.Common.Context;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data.Repositories;
using StructureMap;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.DependencyResolution
{
    public class PaymentsDueRegistry : Registry
    {
        public PaymentsDueRegistry(Type taskType, ContextWrapper contextWrapper)
        {
            Scan(
               scan =>
               {
                   scan.AssemblyContainingType<PaymentsDueRegistry>();

                   scan.RegisterConcreteTypesAgainstTheFirstInterface();
               });

            For<ContextWrapper>().Use(contextWrapper);

            // TODO: Fix so can be registered with convention
            For<ICollectionPeriodRepository>().Use<DcfsCollectionPeriodRepository>();
            For<IProviderRepository>().Use<DcfsProviderRepository>();
            For<IEarningRepository>().Use<DcfsEarningRepository>();
            For<IRequiredPaymentRepository>().Use<DcfsRequiredPaymentRepository>();

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
