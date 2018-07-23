using JetBrains.Annotations;
using SFA.DAS.Payments.DCFS;
using SFA.DAS.Payments.DCFS.Infrastructure.DependencyResolution;
using SFA.DAS.ProviderPayments.Calc.Refunds.DependencyResolution;

namespace SFA.DAS.ProviderPayments.Calc.Refunds
{
    [UsedImplicitly]
    public class RefundsTask : DcfsTask
    {
        private const string DatabaseSchema = "Refunds";

        private readonly IDependencyResolver _dependencyResolver;

        public RefundsTask()
            : base(DatabaseSchema)
        {
            _dependencyResolver = new TaskDependencyResolver();
        }

        public RefundsTask(IDependencyResolver dependencyResolver)
            : base(DatabaseSchema)
        {
            _dependencyResolver = dependencyResolver;
        }

        protected override void Execute(Payments.DCFS.Context.ContextWrapper context)
        {
            _dependencyResolver.Init(typeof(RefundsProcessor), context);

            var processor = _dependencyResolver.GetInstance<RefundsProcessor>();

            processor.Process();
        }
    }
}
