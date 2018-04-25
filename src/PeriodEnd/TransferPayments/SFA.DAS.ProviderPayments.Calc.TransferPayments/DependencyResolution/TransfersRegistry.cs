using System;
using System.Collections.Generic;
using MediatR;
using NLog;
using SFA.DAS.Payments.DCFS.Context;
using SFA.DAS.ProviderPayments.Calc.TransferPayments.Dal.Repositories;
using SFA.DAS.ProviderPayments.Calc.TransferPayments.Dependencies;
using SFA.DAS.ProviderPayments.Calc.TransferPayments.Services;
using StructureMap;

namespace SFA.DAS.ProviderPayments.Calc.TransferPayments.DependencyResolution
{
    public class TransfersRegistry : Registry
    {
        public TransfersRegistry(Type taskType, ContextWrapper contextWrapper)
        {
            Scan(
                scan =>
                {
                    scan.AssemblyContainingType<TransfersRegistry>();

                    scan.RegisterConcreteTypesAgainstTheFirstInterface();
                });

            For<ContextWrapper>().Use(contextWrapper);

           
            For<ILogger>().Use(() => LogManager.GetLogger(taskType.FullName));

            For<SingleInstanceFactory>().Use<SingleInstanceFactory>(ctx => t => GetInstance(ctx, t));
            For<MultiInstanceFactory>().Use<MultiInstanceFactory>(ctx => t => GetAllInstances(ctx, t));
            For<IMediator>().Use<Mediator>();

            For<IAmAnAccountRepository>().Use<AccountRepository>();
            For<IAmATransferRepository>().Use<TransferRepository>();
            For<IProcessLevyTransfers>().Use<LevyTransferService>();
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
