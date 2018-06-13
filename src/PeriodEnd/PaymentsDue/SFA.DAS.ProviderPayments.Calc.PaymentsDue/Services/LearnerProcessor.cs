using NLog;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Dto;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.Services
{
    public class LearnerProcessor : ILearnerProcessor
    {
        private readonly ILogger _logger;
        private readonly IDataLockComponentFactory _dataLockComponentFactory;
        private readonly ILearnerFactory _learnerFactory;

        public LearnerProcessor(
            ILogger logger, 
            IDataLockComponentFactory dataLockComponentFactory, 
            ILearnerFactory learnerFactory)
        {
            _logger = logger;
            _dataLockComponentFactory = dataLockComponentFactory;
            _learnerFactory = learnerFactory;
        }

        public LearnerProcessResults Process(LearnerProcessParameters parameters)
        {
            _logger.Info($"Processing started for Learner LearnRefNumber: [{parameters.LearnRefNumber}].");
            
            var dataLock = _dataLockComponentFactory.CreateDataLockComponent();
            
            var priceEpisodes = dataLock.ValidatePriceEpisodes(parameters.Commitments, parameters.DataLocks, parameters.YearAcademicYearStarted);

            var learner = _learnerFactory.CreateLearner(
                parameters.RawEarnings, 
                parameters.RawEarningsMathsEnglish, 
                priceEpisodes, 
                parameters.HistoricalPayments);
            
            var results = learner.CalculatePaymentsDue();

            _logger.Info($"There are [{results.NonPayableEarnings.Count}] non-payable earnings for Learner LearnRefNumber: [{parameters.LearnRefNumber}].");
            _logger.Info($"There are [{results.PayableEarnings.Count}] payable earnings for Learner LearnRefNumber: [{parameters.LearnRefNumber}].");
            _logger.Info($"Processing finished for Learner LearnRefNumber: [{parameters.LearnRefNumber}].");

            return results;
        }
    }
}