using System;
using NLog;
using SFA.DAS.Payments.DCFS.Context;
using SFA.DAS.Payments.DCFS.StructureMap.Infrastructure;
using SFA.DAS.ProviderPayments.Calc.Refunds.Infrastructure.Repositories;
using SFA.DAS.ProviderPayments.Calc.Shared.Infrastructure.Data;
using SFA.DAS.ProviderPayments.Calc.Shared.Infrastructure.Data.Repositories;

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
                    scan.SingleImplementationsOfInterface();
                    scan.WithDefaultConventions();
                });

            For<ContextWrapper>().Use(contextWrapper);
            For<IHoldDcConfiguration>().Use<DcConfiguration>().Singleton();
            For<ILogger>().Use(() => LogManager.GetLogger(taskType.FullName));

            // these don't resolve by scan conventions above due to ctor params
            For<IDasAccountRepository>().Use<DasAccountRepository>();
            For<IPaymentRepository>().Use<PaymentRepository>();
            For<IProviderRepository>().Use<ProviderRepository>();
            For<IRequiredPaymentRepository>().Use<RequiredPaymentRepository>();
            For<IHistoricalPaymentsRepository>().Use<HistoricalPaymentsRepository>();
        }
    }
}
