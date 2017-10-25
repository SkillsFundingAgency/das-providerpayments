using System;
using System.Collections.Generic;
using MediatR;
using NLog;
using StructureMap;

namespace SFA.DAS.CollectionEarnings.Calculator.DependencyResolution
{
    public class EarningsCalculatorRegistry : Registry
    {
        public EarningsCalculatorRegistry(Type taskType)
        {
            Scan(
                scan =>
                {
                    scan.AssemblyContainingType<EarningsCalculatorRegistry>();

                    scan.RegisterConcreteTypesAgainstTheFirstInterface();
                });

            For<ILogger>().Use(() => LogManager.GetLogger(taskType.FullName));

            For<SingleInstanceFactory>().Use<SingleInstanceFactory>(ctx => t => GetInstance(ctx, t));
            For<MultiInstanceFactory>().Use<MultiInstanceFactory>(ctx => t => GetAllInstances(ctx, t));
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