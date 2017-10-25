using SFA.DAS.Payments.Calc.ProviderAdjustments.DependencyResolution;
using SFA.DAS.Payments.DCFS;
using SFA.DAS.Payments.DCFS.Context;
using SFA.DAS.Payments.DCFS.Infrastructure.DependencyResolution;
using SFA.DAS.ProviderPayments.Calc.Common.Context;

namespace SFA.DAS.Payments.Calc.ProviderAdjustments
{
    public class ProviderAdjustmentsTask : DcfsTask
    {
        private const string DatabaseSchema = "ProviderAdjustments";

        private readonly IDependencyResolver _dependencyResolver;

        public ProviderAdjustmentsTask()
            : base(DatabaseSchema)
        {
            _dependencyResolver = new TaskDependencyResolver();
        }

        internal ProviderAdjustmentsTask(IDependencyResolver dependencyResolver)
            : base(DatabaseSchema)
        {
            _dependencyResolver = dependencyResolver;
        }


        protected override void Execute(ContextWrapper context)
        {
            _dependencyResolver.Init(typeof(ProviderAdjustmentsTask), context);

            var processor = _dependencyResolver.GetInstance<ProviderAdjustmentsProcessor>();

            processor.Process();
        }

        protected override bool IsValidContext(ContextWrapper contextWrapper)
        {
            if (string.IsNullOrEmpty(contextWrapper.GetPropertyValue(PaymentsContextPropertyKeys.YearOfCollection)))
            {
                throw new PaymentsInvalidContextException(PaymentsInvalidContextException.ContextPropertiesNoYearOfCollectionMessage);
            }

            return IsValidYearOfCollection(contextWrapper.GetPropertyValue(PaymentsContextPropertyKeys.YearOfCollection))
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