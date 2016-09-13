﻿using SFA.DAS.ProviderPayments.Calc.Common;
using SFA.DAS.ProviderPayments.Calc.Common.Context;
using SFA.DAS.ProviderPayments.Calc.PaymentSchedule.DependencyResolution;

namespace SFA.DAS.ProviderPayments.Calc.PaymentSchedule
{
    public class PaymentScheduleTask : DcfsTask
    {
        private const string DatabaseSchema = "PaymentSchedule";

        private readonly IDependencyResolver _dependencyResolver;

        public PaymentScheduleTask()
            : base(DatabaseSchema)
        {
            _dependencyResolver = new TaskDependencyResolver();
        }

        internal PaymentScheduleTask(IDependencyResolver dependencyResolver)
            : base(DatabaseSchema)
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
