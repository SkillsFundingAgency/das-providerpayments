using JetBrains.Annotations;
using SFA.DAS.Payments.DCFS;
using SFA.DAS.Payments.DCFS.Infrastructure.DependencyResolution;
using SFA.DAS.ProviderPayments.Calc.TransferPayments.DependencyResolution;

namespace SFA.DAS.ProviderPayments.Calc.TransferPayments
{
    [UsedImplicitly]
    public class TransfersTask : DcfsTask
    {
        private const string DatabaseSchema = "TransferPayments";

        private readonly IDependencyResolver _dependencyResolver;

        public TransfersTask()
            : base(DatabaseSchema)
        {
            _dependencyResolver = new TaskDependencyResolver();
        }

        public TransfersTask(IDependencyResolver dependencyResolver)
            : base(DatabaseSchema)
        {
            _dependencyResolver = dependencyResolver;
        }

        protected override void Execute(Payments.DCFS.Context.ContextWrapper context)
        {
            _dependencyResolver.Init(typeof(TransfersProcessor), context);

            var processor = _dependencyResolver.GetInstance<TransfersProcessor>();

            processor.Process();
        }
    }
}
