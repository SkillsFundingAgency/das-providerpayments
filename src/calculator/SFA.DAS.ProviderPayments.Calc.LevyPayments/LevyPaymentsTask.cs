using SFA.DAS.ProviderPayments.Calc.Common;
using SFA.DAS.ProviderPayments.Calc.Common.Context;
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

        protected override bool IsValidContext(ContextWrapper contextWrapper)
        {
            if (string.IsNullOrEmpty(contextWrapper.GetPropertyValue(ContextPropertyKeys.YearOfCollection)))
            {
                throw new InvalidContextException(InvalidContextException.ContextPropertiesNoYearOfCollectionMessage);
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
                throw new InvalidContextException(InvalidContextException.ContextPropertiesInvalidYearOfCollectionMessage);
            }

            return true;
        }
    }
}
