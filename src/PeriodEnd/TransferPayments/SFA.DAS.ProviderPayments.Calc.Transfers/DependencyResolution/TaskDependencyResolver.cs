using System;
using SFA.DAS.Payments.DCFS.StructureMap.Infrastructure.DependencyResolution;
using StructureMap;
using SFA.DAS.Payments.DCFS.Context;

namespace SFA.DAS.ProviderPayments.Calc.Transfers.DependencyResolution
{
    public class TaskDependencyResolver : StructureMapDependencyResolver
    {
        protected override Registry CreateRegistry(Type taskType, ContextWrapper contextWrapper)
        {
            return new TransfersRegistry(taskType, contextWrapper);
        }
    }
}
