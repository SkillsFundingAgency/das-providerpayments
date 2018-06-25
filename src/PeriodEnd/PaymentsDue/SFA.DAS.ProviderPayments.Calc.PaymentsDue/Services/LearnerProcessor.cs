using NLog;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Dto;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Services.Dependencies;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.Services
{
    public class LearnerProcessor : ILearnerProcessor
    {
        private readonly ILogger _logger;
        private readonly IIDetermineWhichEarningsShouldBePaid _determinePayableEarnings;
        private readonly IValidateRawDatalocks _datalockCommitmentMatcher;
        private readonly ICalculatePaymentsDue _paymentsDueCalc;

        public LearnerProcessor(ILogger logger,
            IIDetermineWhichEarningsShouldBePaid determinePayableEarnings, 
            IValidateRawDatalocks datalockCommitmentMatcher, 
            ICalculatePaymentsDue paymentsDueCalc)
        {
            _logger = logger;
            _determinePayableEarnings = determinePayableEarnings;
            _datalockCommitmentMatcher = datalockCommitmentMatcher;
            _paymentsDueCalc = paymentsDueCalc;
        }

        public PaymentsDueResult Process(LearnerData parameters, long ukprn)
        {
            _logger.Info($"Processing started for Learner LearnRefNumber: [{parameters.LearnRefNumber}] from provider UKPRN: [{ukprn}].");

            var processedDatalocks = _datalockCommitmentMatcher
                .ProcessDatalocks(parameters.DataLocks, 
                    parameters.DatalockValidationErrors, 
                    parameters.Commitments);
            
            var validationResult = _determinePayableEarnings.DeterminePayableEarnings(
                processedDatalocks,
                parameters.RawEarnings,
                parameters.RawEarningsMathsEnglish);

            var paymentsDue = _paymentsDueCalc.Calculate(validationResult.Earnings,
                validationResult.PeriodsToIgnore,
                parameters.HistoricalPayments);
            var results = new PaymentsDueResult(paymentsDue, validationResult.NonPayableEarnings);
            
            _logger.Info($"There are [{results.NonPayableEarnings.Count}] non-payable earnings for Learner LearnRefNumber: [{parameters.LearnRefNumber}] from provider UKPRN: [{ukprn}].");
            _logger.Info($"There are [{results.PayableEarnings.Count}] payable earnings for Learner LearnRefNumber: [{parameters.LearnRefNumber}] from provider UKPRN: [{ukprn}].");
            _logger.Info($"Processing finished for Learner LearnRefNumber: [{parameters.LearnRefNumber}] from provider UKPRN: [{ukprn}].");

            return results;
        }
    }
}