using System;
using NLog;
using SFA.DAS.Payments.DCFS.Context;
using SFA.DAS.ProviderPayments.Calc.Refunds.Infrastructure.Repositories;
using SFA.DAS.ProviderPayments.Calc.Refunds.Services;
using SFA.DAS.ProviderPayments.Calc.Refunds.Services.Dependencies;
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
                    scan.AssemblyContainingType<IProviderRepository>();
                    
                    scan.RegisterConcreteTypesAgainstTheFirstInterface();
                    scan.SingleImplementationsOfInterface();
                });

            For<ContextWrapper>().Use(contextWrapper);

            For<ILogger>().Use(() => LogManager.GetLogger(taskType.FullName));

            For<IDasAccountRepository>().Use<DasAccountRepository>();
            For<IPaymentRepository>().Use<PaymentRepository>();
            For<IProviderRepository>().Use<ProviderRepository>();
            For<IRequiredPaymentRepository>().Use<RequiredPaymentRepository>();

            For<IHistoricalPaymentsRepository>().Use<HistoricalPaymentsRepository>();
        }
    }
}
