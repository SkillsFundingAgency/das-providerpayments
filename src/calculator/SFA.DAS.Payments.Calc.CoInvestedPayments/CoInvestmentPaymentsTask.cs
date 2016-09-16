using SFA.DAS.Payments.Calc.CoInvestedPayments.DependencyResolution;
using SFA.DAS.ProviderPayments.Calc.Common;
using SFA.DAS.ProviderPayments.Calc.Common.Context;

namespace SFA.DAS.Payments.Calc.CoInvestedPayments
{
    public class CoInvestmentPaymentsTask : DcfsTask
    {
        private const string DatabaseSchema = "CoInvestedPayments";

        private readonly IDependencyResolver _dependencyResolver;

        public CoInvestmentPaymentsTask()
            : base(DatabaseSchema)
        {
            _dependencyResolver = new TaskDependencyResolver();
        }

        internal CoInvestmentPaymentsTask(IDependencyResolver dependencyResolver)
            : base(DatabaseSchema)
        {
            _dependencyResolver = dependencyResolver;
        }

        protected override void Execute(ContextWrapper context)
        {
            _dependencyResolver.Init(typeof(CoInvestmentPaymentsPassThroughProcessor), context);

            var processor = _dependencyResolver.GetInstance<CoInvestmentPaymentsPassThroughProcessor>();

            processor.Process();
        }
    }
}