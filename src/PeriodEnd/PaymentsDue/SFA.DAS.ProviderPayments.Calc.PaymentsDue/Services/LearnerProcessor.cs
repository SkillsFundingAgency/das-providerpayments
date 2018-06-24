using NLog;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Dto;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Services.Dependencies;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.Services
{
    public class LearnerProcessor : ILearnerProcessor
    {
        private readonly ILogger _logger;
        private readonly IIDetermineWhichEarningsShouldBePaid _determinePayableEarnings;
        private readonly ILearnerFactory _learnerFactory;
        private readonly IValidateRawDatalocks _datalockCommitmentMatcher;

        public LearnerProcessor(ILogger logger,
            IIDetermineWhichEarningsShouldBePaid determinePayableEarnings, 
            ILearnerFactory learnerFactory,
            IValidateRawDatalocks datalockCommitmentMatcher)
        {
            _logger = logger;
            _determinePayableEarnings = determinePayableEarnings;
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
            
            var validationResult = _determinePayableEarnings.ValidatePriceEpisodes(
                processedDatalocks,
                parameters.RawEarnings,
                parameters.RawEarningsMathsEnglish,
                parameters.FirstDayOfAcademicYear);

            var learner = _learnerFactory.CreateLearner(
                validationResult.Earnings,
                validationResult.PeriodsToIgnore,
                parameters.HistoricalPayments);

            var paymentsDue = learner.Calculate();
            var results = new LearnerProcessResults(paymentsDue, validationResult.NonPayableEarnings);
            
            _logger.Info($"There are [{results.NonPayableEarnings.Count}] non-payable earnings for Learner LearnRefNumber: [{parameters.LearnRefNumber}] from provider UKPRN: [{ukprn}].");
            _logger.Info($"There are [{results.PayableEarnings.Count}] payable earnings for Learner LearnRefNumber: [{parameters.LearnRefNumber}] from provider UKPRN: [{ukprn}].");
            _logger.Info($"Processing finished for Learner LearnRefNumber: [{parameters.LearnRefNumber}] from provider UKPRN: [{ukprn}].");

            return results;
        }
    }
}