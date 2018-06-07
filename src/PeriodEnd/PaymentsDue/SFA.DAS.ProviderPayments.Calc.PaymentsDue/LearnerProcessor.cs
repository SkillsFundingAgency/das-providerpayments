using NLog;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Domain;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Services;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue
{
    public class LearnerProcessor : ILearnerProcessor
    {
        private readonly ILogger _logger;
        private readonly IDataLockComponentFactory _dataLockComponentFactory;
        private readonly ILearnerFactory _learnerFactory;

        public LearnerProcessor(ILogger logger, IDataLockComponentFactory dataLockComponentFactory, ILearnerFactory learnerFactory)
        {
            _logger = logger;
            _dataLockComponentFactory = dataLockComponentFactory;
            _learnerFactory = learnerFactory;
        }

        public LearnerProcessResults Process(LearnerProcessParameters parameters)
        {
            var dataLock = _dataLockComponentFactory.CreateDataLockComponent();
            var learner = _learnerFactory.CreateLearner();

            dataLock.ValidatePriceEpisodes(parameters.Commitments, parameters.DataLocks);

            learner.CalculatePaymentsDue();

            return new LearnerProcessResults();
        }
    }
}