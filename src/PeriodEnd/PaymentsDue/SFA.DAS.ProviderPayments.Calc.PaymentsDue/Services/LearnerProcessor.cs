using System.Linq;
using NLog;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Dto;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Services.Dependencies;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.Services
{
    public class LearnerProcessor : ILearnerProcessor
    {
        private readonly ILogger _logger;
        private readonly IDataLockComponentFactory _dataLockComponentFactory;
        private readonly ILearnerFactory _learnerFactory;
        private readonly IValidateRawDatalocks _datalockCommitmentMatcher;

        public LearnerProcessor(ILogger logger, 
            IDataLockComponentFactory dataLockComponentFactory, 
            ILearnerFactory learnerFactory,
            IValidateRawDatalocks datalockCommitmentMatcher)
        {
            _logger = logger;
            _dataLockComponentFactory = dataLockComponentFactory;
            _learnerFactory = learnerFactory;
            _datalockCommitmentMatcher = datalockCommitmentMatcher;
        }

        public LearnerProcessResults Process(LearnerProcessParameters parameters, long ukprn)
        {
            _logger.Info($"Processing started for Learner LearnRefNumber: [{parameters.LearnRefNumber}] from provider UKPRN: [{ukprn}].");

            var processedDatalocks = _datalockCommitmentMatcher
                .ProcessDatalocks(parameters.DataLocks, 
                    parameters.DatalockValidationErrors, 
                    parameters.Commitments);
            
            var dataLock = _dataLockComponentFactory.CreateDataLockComponent();

            var validationResult = dataLock.ValidatePriceEpisodes(
                processedDatalocks,
                parameters.RawEarnings,
                parameters.RawEarningsMathsEnglish,
                parameters.FirstDayOfAcademicYear);

            var learner = _learnerFactory.CreateLearner(
                validationResult.Earnings,
                validationResult.PeriodsToIgnore,
                parameters.HistoricalPayments);

            var paymentsDue = learner.CalculatePaymentsDue();
            var results = new LearnerProcessResults(paymentsDue, validationResult.NonPayableEarnings);
            
            _logger.Info($"There are [{results.NonPayableEarnings.Count}] non-payable earnings for Learner LearnRefNumber: [{parameters.LearnRefNumber}] from provider UKPRN: [{ukprn}].");
            _logger.Info($"There are [{results.PayableEarnings.Count}] payable earnings for Learner LearnRefNumber: [{parameters.LearnRefNumber}] from provider UKPRN: [{ukprn}].");
            _logger.Info($"Processing finished for Learner LearnRefNumber: [{parameters.LearnRefNumber}] from provider UKPRN: [{ukprn}].");

            return results;
        }
    }
}