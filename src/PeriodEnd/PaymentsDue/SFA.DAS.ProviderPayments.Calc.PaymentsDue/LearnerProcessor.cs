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
            _logger.Info($"Processing started for Learner LearnRefNumber: [{parameters.LearnRefNumber}].");

            var dataLock = _dataLockComponentFactory.CreateDataLockComponent();
            var learner = _learnerFactory.CreateLearner();

            dataLock.ValidatePriceEpisodes(parameters.Commitments, parameters.DataLocks);

            learner.CalculatePaymentsDue();

            var results = new LearnerProcessResults();

            _logger.Info($"There are [{results.NonPayableEarnings.Count}] non-payable earnings for Learner LearnRefNumber: [{parameters.LearnRefNumber}].");
            _logger.Info($"There are [{results.PayableEarnings.Count}] payable earnings for Learner LearnRefNumber: [{parameters.LearnRefNumber}].");
            _logger.Info($"Processing finished for Learner LearnRefNumber: [{parameters.LearnRefNumber}].");

            return results;
        }
    }
}