using System;
using SFA.DAS.ProviderPayments.Calculator.Common;
using SFA.DAS.ProviderPayments.Calculator.Common.Context;
using SFA.DAS.ProviderPayments.Calculator.PaymentSchedule.DependencyResolution;

namespace SFA.DAS.ProviderPayments.Calculator.PaymentSchedule
{
    public class PaymentScheduleTask : DcfsTask
    {
        private readonly IDependencyResolver _dependencyResolver;

        public PaymentScheduleTask()
        {
            _dependencyResolver = new TaskDependencyResolver();
        }
        internal PaymentScheduleTask(IDependencyResolver dependencyResolver)
        {
            _dependencyResolver = dependencyResolver;
        }

        protected override void Execute(ContextWrapper context)
        {
            _dependencyResolver.Init(typeof(PaymentScheduleProcessor), context);

            var processor = _dependencyResolver.GetInstance<PaymentScheduleProcessor>();

            processor.Process();
        }
    }
}
