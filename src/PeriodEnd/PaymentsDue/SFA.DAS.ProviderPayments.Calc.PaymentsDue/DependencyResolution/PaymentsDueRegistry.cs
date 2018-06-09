using System;
using System.Collections.Generic;
using MediatR;
using NLog;
using SFA.DAS.Payments.DCFS.Context;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Domain;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data.Repositories;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Services;
using StructureMap;
using StructureMap.AutoFactory;

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
            For<ICollectionPeriodRepository>().Use<CollectionPeriodRepository>();
            For<IProviderRepository>().Use<ProviderRepository>();
            For<IRequiredPaymentRepository>().Use<RequiredPaymentRepository>();
            For<ICommitmentRepository>().Use<CommitmentRepository>();
            For<IDatalockOutputRepository>().Use<DatalockOutputRepository>();
            For<INonPayableEarningRepository>().Use<NonPayableEarningRepository>();
            For<IRequiredPaymentsHistoryRepository>().Use<RequiredPaymentsHistoryRepository>();

            For<ILogger>().Use(() => LogManager.GetLogger(taskType.FullName));

            For<SingleInstanceFactory>().Use<SingleInstanceFactory>(ctx => t => GetInstance(ctx, t));
            For<MultiInstanceFactory>().Use<MultiInstanceFactory>(ctx => t => GetAllInstances(ctx, t));
            For<IMediator>().Use<Mediator>();

            For<ILearner>().Use<Learner>();
            For<IIShouldBeInTheDataLockComponent>().Use<IShouldBeInTheDatalockComponent>();
            For<IRawEarningsRepository>().Use<RawEarningsRepository>();
            For<IRawEarningsMathsEnglishRepository>().Use<RawEarningsMathsEnglishRepository>();
            For<ILearnerFactory>().Use<LearnerFactory>();
            For<IDataLockComponentFactory>().Use<DatalockComponentFactory>();

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
