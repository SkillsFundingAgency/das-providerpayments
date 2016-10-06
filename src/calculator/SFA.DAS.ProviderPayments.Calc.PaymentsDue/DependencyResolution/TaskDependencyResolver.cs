using System;
using SFA.DAS.Payments.DCFS.StructureMap.Infrastructure.DependencyResolution;
using StructureMap;
using ContextWrapper = SFA.DAS.Payments.DCFS.Context.ContextWrapper;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.DependencyResolution
{
    public class TaskDependencyResolver : StructureMapDependencyResolver
    {
        protected override Registry CreateRegistry(Type taskType, ContextWrapper contextWrapper)
        {
            return new PaymentsDueRegistry(taskType, contextWrapper);
        }
    }
}
