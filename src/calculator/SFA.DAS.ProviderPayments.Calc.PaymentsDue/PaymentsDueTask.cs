using SFA.DAS.ProviderPayments.Calc.Common;
using SFA.DAS.ProviderPayments.Calc.Common.Context;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.DependencyResolution;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue
{
    public class PaymentsDueTask : DcfsTask
    {
        private const string DatabaseSchema = "PaymentsDue";

        private readonly IDependencyResolver _dependencyResolver;

        public PaymentsDueTask()
            : base(DatabaseSchema)
        {
            _dependencyResolver = new TaskDependencyResolver();
        }

        internal PaymentsDueTask(IDependencyResolver dependencyResolver)
            : base(DatabaseSchema)
        {
            _dependencyResolver = dependencyResolver;
        }

        protected override void Execute(ContextWrapper context)
        {
            _dependencyResolver.Init(typeof(PaymentsDuePassThroughProcessor), context);

            var processor = _dependencyResolver.GetInstance<PaymentsDuePassThroughProcessor>();

            processor.Process();
        }
    }
}
