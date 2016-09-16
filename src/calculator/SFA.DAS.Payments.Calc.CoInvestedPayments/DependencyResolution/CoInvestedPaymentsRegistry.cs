using System;
using NLog;
using StructureMap;

namespace SFA.DAS.Payments.Calc.CoInvestedPayments.DependencyResolution
{
    public class CoInvestedPaymentsRegistry : Registry
    {
        public CoInvestedPaymentsRegistry(Type taskType)
        {
            Scan(
               scan =>
               {
                   scan.AssemblyContainingType<CoInvestedPaymentsRegistry>();

                   scan.RegisterConcreteTypesAgainstTheFirstInterface();
               });

            For<ILogger>().Use(() => LogManager.GetLogger(taskType.FullName));
        }
    }
}
