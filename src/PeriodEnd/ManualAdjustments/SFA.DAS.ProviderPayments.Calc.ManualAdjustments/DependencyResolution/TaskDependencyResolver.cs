using System;
using SFA.DAS.Payments.DCFS.Context;
using SFA.DAS.Payments.DCFS.StructureMap.Infrastructure.DependencyResolution;
using SFA.DAS.ProviderPayments.Calc.Common.DependencyResolution;
using StructureMap;

namespace SFA.DAS.ProviderPayments.Calc.ManualAdjustments.DependencyResolution
{
    public class TaskDependencyResolver : StructureMapDependencyResolver
    {
        protected override Registry CreateRegistry(Type taskType, ContextWrapper contextWrapper)
        {
            return new ManualAdjustmentsRegistry(taskType, contextWrapper);
        }

        protected override void AddPolicies(ConfigurationExpression config, Type taskType, ContextWrapper contextWrapper)
        {
            base.AddPolicies(config, taskType, contextWrapper);
            config.Policies.Add(new YearOfCollectionPolicy(contextWrapper));
        }
    }
}