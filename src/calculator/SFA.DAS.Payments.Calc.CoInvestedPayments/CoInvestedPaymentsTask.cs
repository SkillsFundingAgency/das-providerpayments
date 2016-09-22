using SFA.DAS.Payments.Calc.CoInvestedPayments.DependencyResolution;
using SFA.DAS.ProviderPayments.Calc.Common;
using SFA.DAS.ProviderPayments.Calc.Common.Context;

namespace SFA.DAS.Payments.Calc.CoInvestedPayments
{
    public class CoInvestedPaymentsTask : DcfsTask
    {
        private const string DatabaseSchema = "CoInvestedPayments";

        private readonly IDependencyResolver _dependencyResolver;

        public CoInvestedPaymentsTask()
            : base(DatabaseSchema)
        {
            _dependencyResolver = new TaskDependencyResolver();
        }

        internal CoInvestedPaymentsTask(IDependencyResolver dependencyResolver)
            : base(DatabaseSchema)
        {
            _dependencyResolver = dependencyResolver;
        }

        protected override void Execute(ContextWrapper context)
        {
            _dependencyResolver.Init(typeof(CoInvestedPaymentsProcessor), context);

            var processor = _dependencyResolver.GetInstance<CoInvestedPaymentsProcessor>();

            processor.Process();
        }
    }
}