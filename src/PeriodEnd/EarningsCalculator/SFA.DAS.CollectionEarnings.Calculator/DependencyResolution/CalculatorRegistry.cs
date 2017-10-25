using System;
using System.Collections.Generic;
using MediatR;
using NLog;
using SFA.DAS.CollectionEarnings.Calculator.Infrastructure.Data;
using SFA.DAS.CollectionEarnings.Calculator.Infrastructure.Data.Repositories;
using SFA.DAS.CollectionEarnings.Calculator.Tools.Providers;
using StructureMap;

namespace SFA.DAS.CollectionEarnings.Calculator.DependencyResolution
{
    public class CalculatorRegistry : Registry
    {
        public CalculatorRegistry(Type taskType)
        {
            Scan(
                scan =>
                {
                    scan.AssemblyContainingType<CalculatorRegistry>();

                    scan.RegisterConcreteTypesAgainstTheFirstInterface();
                });

            For<ILogger>().Use(() => LogManager.GetLogger(taskType.FullName));

            // TODO: Fix so can be registered with convention
            For<IFinancialRecordRepository>().Use<FinancialRecordRepository>();
            For<ILearningDeliveryToProcessRepository>().Use<LearningDeliveryToProcessRepository>();
            For<IApprenticeshipPriceEpisodeRepository>().Use<ApprenticeshipPriceEpisodeRepository>();
            For<IApprenticeshipPriceEpisodePeriodisedValuesRepository>().Use<ApprenticeshipPriceEpisodePeriodisedValuesRepository>();
            For<IApprenticeshipPriceEpisodePeriodRepository>().Use<ApprenticeshipPriceEpisodePeriodRepository>();
            For<IDateTimeProvider>().Use<DateTimeProvider>();

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
