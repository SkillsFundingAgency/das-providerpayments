using System;
using NLog;
using StructureMap;

namespace SFA.DAS.ProviderPayments.Calculator.LevyPayments.DependencyResolution
{
    public class CalcRegistry : Registry
    {
        public CalcRegistry(Type taskType)
        {
            Scan(
               scan =>
               {
                   scan.AssembliesFromApplicationBaseDirectory(a => a.GetName().Name.StartsWith("SFA.DAS.ProviderPayments.Calculator"));

                   scan.RegisterConcreteTypesAgainstTheFirstInterface();
               });


            For<ILogger>().Use(() => LogManager.GetLogger(taskType.FullName));
        }
    }
}
