using System;
using System.Collections.Generic;
using MediatR;
using NLog;
using SFA.DAS.ProviderPayments.Calc.LevyPayments.Infrastructure.Data;
using SFA.DAS.ProviderPayments.Calc.LevyPayments.Infrastructure.Data.Repositories;
using StructureMap;

namespace SFA.DAS.ProviderPayments.Calc.LevyPayments.DependencyResolution
{
    public class CalcRegistry : Registry
    {
        public CalcRegistry(Type taskType)
        {
            Scan(
               scan =>
               {
                   scan.AssemblyContainingType<CalcRegistry>();

                   scan.RegisterConcreteTypesAgainstTheFirstInterface();
               });

            For<ILogger>().Use(() => LogManager.GetLogger(taskType.FullName));

            // TODO: Fix so can be registered with convention
            For<ICollectionPeriodRepository>().Use<DcfsCollectionPeriodRepository>();
            For<IAccountRepository>().Use<DcfsAccountRepository>();
            For<ICommitmentRepository>().Use<DcfsCommitmentRepository>();
            For<IPaymentDueRepository>().Use<DcfsPaymentDueRepository>();
            For<IPaymentRepository>().Use<DcfsPaymentRepository>();

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
