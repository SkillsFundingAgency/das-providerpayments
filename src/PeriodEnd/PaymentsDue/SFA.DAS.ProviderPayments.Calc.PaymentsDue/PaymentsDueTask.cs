using SFA.DAS.Payments.DCFS;
using SFA.DAS.Payments.DCFS.Infrastructure.DependencyResolution;
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

        protected override void Execute(Payments.DCFS.Context.ContextWrapper context)
        {
            _dependencyResolver.Init(typeof(PaymentsDueProcessorV2), context);

            var processor = _dependencyResolver.GetInstance<PaymentsDueProcessorV2>();

            processor.Process();
        }
    }
}
