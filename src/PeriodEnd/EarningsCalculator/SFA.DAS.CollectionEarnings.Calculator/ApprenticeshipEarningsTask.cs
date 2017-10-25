using SFA.DAS.CollectionEarnings.Calculator.Context;
using SFA.DAS.CollectionEarnings.Calculator.DependencyResolution;
using SFA.DAS.Payments.DCFS;
using SFA.DAS.Payments.DCFS.Context;
using SFA.DAS.Payments.DCFS.Infrastructure.DependencyResolution;

namespace SFA.DAS.CollectionEarnings.Calculator
{
    public class ApprenticeshipEarningsTask :  DcfsTask
    {
        private IDependencyResolver _dependencyResolver;
        private const string EarningsSchema = "Earnings";

        public ApprenticeshipEarningsTask()
            : base(EarningsSchema)
        {
            _dependencyResolver = new TaskDependencyResolver();
        }

        public ApprenticeshipEarningsTask(IDependencyResolver dependencyResolver)
            : base(EarningsSchema)
        {
            _dependencyResolver = dependencyResolver;
        }

        protected override void Execute(ContextWrapper context)
        {
            _dependencyResolver.Init(typeof(ApprenticeshipEarningsProcessor), context);

            var processor = _dependencyResolver.GetInstance<ApprenticeshipEarningsProcessor>();

            processor.Process();
        }

        protected override bool IsValidContext(ContextWrapper contextWrapper)
        {
            if (string.IsNullOrEmpty(contextWrapper.GetPropertyValue(ContextPropertyKeys.TransientDatabaseConnectionString)))
            {
                throw new InvalidContextException(InvalidContextException.ContextPropertiesNoConnectionStringMessage);
            }

            if (string.IsNullOrEmpty(contextWrapper.GetPropertyValue(ContextPropertyKeys.LogLevel)))
            {
                throw new InvalidContextException(InvalidContextException.ContextPropertiesNoLogLevelMessage);
            }

            if (string.IsNullOrEmpty(contextWrapper.GetPropertyValue(EarningsContextPropertyKeys.YearOfCollection)))
            {
                throw new InvalidContextException(EarningsCalculatorException.ContextPropertiesNoYearOfCollectionMessage);
            }

            return ValidateYearOfCollection(contextWrapper.GetPropertyValue(EarningsContextPropertyKeys.YearOfCollection));
        }

        private bool ValidateYearOfCollection(string yearOfCollection)
        {
            int year1;
            int year2;

            if (yearOfCollection.Length != 4 ||
                !int.TryParse(yearOfCollection.Substring(0, 2), out year1) ||
                !int.TryParse(yearOfCollection.Substring(2, 2), out year2) ||
                (year2 != year1 + 1))
            {
                throw new InvalidContextException(EarningsCalculatorException.ContextPropertiesInvalidYearOfCollectionMessage);
            }

            return true;
        }
    }
}