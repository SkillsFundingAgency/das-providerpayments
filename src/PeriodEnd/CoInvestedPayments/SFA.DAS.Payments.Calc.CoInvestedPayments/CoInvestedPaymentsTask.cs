using SFA.DAS.Payments.Calc.CoInvestedPayments.DependencyResolution;
using SFA.DAS.Payments.DCFS;
using SFA.DAS.Payments.DCFS.Context;
using SFA.DAS.Payments.DCFS.Infrastructure.DependencyResolution;
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

        protected override bool IsValidContext(ContextWrapper contextWrapper)
        {
            if (string.IsNullOrEmpty(contextWrapper.GetPropertyValue(ContextPropertyKeys.YearOfCollection)))
            {
                throw new PaymentsInvalidContextException(PaymentsInvalidContextException.ContextPropertiesNoYearOfCollectionMessage);
            }

            return IsValidYearOfCollection(contextWrapper.GetPropertyValue(ContextPropertyKeys.YearOfCollection))
                   && base.IsValidContext(contextWrapper);
        }

        private bool IsValidYearOfCollection(string yearOfCollection)
        {
            int year1;
            int year2;

            if (yearOfCollection.Length != 4 ||
                !int.TryParse(yearOfCollection.Substring(0, 2), out year1) ||
                !int.TryParse(yearOfCollection.Substring(2, 2), out year2) ||
                (year2 != year1 + 1))
            {
                throw new PaymentsInvalidContextException(PaymentsInvalidContextException.ContextPropertiesInvalidYearOfCollectionMessage);
            }

            return true;
        }
    }
}