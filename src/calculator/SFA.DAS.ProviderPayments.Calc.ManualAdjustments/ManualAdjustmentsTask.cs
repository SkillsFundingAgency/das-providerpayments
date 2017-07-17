using System;
using SFA.DAS.Payments.DCFS;
using SFA.DAS.Payments.DCFS.Context;
using SFA.DAS.Payments.DCFS.Infrastructure.DependencyResolution;
using SFA.DAS.ProviderPayments.Calc.ManualAdjustments.DependencyResolution;

namespace SFA.DAS.ProviderPayments.Calc.ManualAdjustments
{
    public class ManualAdjustmentsTask : DcfsTask
    {
        private const string DatabaseSchema = "ProviderAdjustments";

        private readonly IDependencyResolver _dependencyResolver;

        public ManualAdjustmentsTask() 
            : base(DatabaseSchema)
        {
            _dependencyResolver = new TaskDependencyResolver();
        }
        internal ManualAdjustmentsTask(IDependencyResolver dependencyResolver)
            : base(DatabaseSchema)
        {
            _dependencyResolver = dependencyResolver;
        }

        protected override void Execute(ContextWrapper context)
        {
            throw new NotImplementedException();
        }
    }
}
