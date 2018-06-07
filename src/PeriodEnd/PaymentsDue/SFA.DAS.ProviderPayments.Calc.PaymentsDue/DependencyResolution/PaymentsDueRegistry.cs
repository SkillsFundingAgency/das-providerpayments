﻿using System;
using System.Collections.Generic;
using MediatR;
using NLog;
using SFA.DAS.Payments.DCFS.Context;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Domain;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data.Repositories;
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
            For<IRequiredPaymentRepository>().Use<DcfsRequiredPaymentRepository>();

            For<ILogger>().Use(() => LogManager.GetLogger(taskType.FullName));

            For<SingleInstanceFactory>().Use<SingleInstanceFactory>(ctx => t => GetInstance(ctx, t));
            For<MultiInstanceFactory>().Use<MultiInstanceFactory>(ctx => t => GetAllInstances(ctx, t));
            For<IMediator>().Use<Mediator>();

            // todo: For<ILearner>().Use<Learner>();
            For<IIShouldBeInTheDatalockComponent>().Use<IShouldBeInTheDatalockComponent>();
            For<ILearnerFactory>().CreateFactory();
            For<IDataLockComponentFactory>().CreateFactory();

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
