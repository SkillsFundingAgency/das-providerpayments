using SFA.DAS.ProviderPayments.Calc.Common;
using SFA.DAS.ProviderPayments.Calc.LevyPayments.DependencyResolution;
using ContextWrapper = SFA.DAS.ProviderPayments.Calc.Common.Context.ContextWrapper;

namespace SFA.DAS.ProviderPayments.Calc.LevyPayments
{
    public class LevyPaymentsTask : DcfsTask
    {
        private const string DatabaseSchema = "LevyPayments";

        private readonly IDependencyResolver _dependencyResolver;

        public LevyPaymentsTask()
            : base(DatabaseSchema)
        {
            _dependencyResolver = new TaskDependencyResolver();
        }

        internal LevyPaymentsTask(IDependencyResolver dependencyResolver)
            : base(DatabaseSchema)
        {
            _dependencyResolver = dependencyResolver;
        }


        protected override void Execute(ContextWrapper context)
        {
            _dependencyResolver.Init(typeof(LevyPaymentsTask), context);

            var processor = _dependencyResolver.GetInstance<LevyPaymentsProcessor>();

            processor.Process();
        }
    }
}
