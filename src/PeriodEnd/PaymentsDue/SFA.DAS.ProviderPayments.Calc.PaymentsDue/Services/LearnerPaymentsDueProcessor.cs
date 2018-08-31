using NLog;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Domain;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Dto;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Services.Dependencies;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.Services
{
    public class LearnerPaymentsDueProcessor : ILearnerPaymentsDueProcessor
    {
        private readonly ILogger _logger;
        private readonly IDetermineWhichEarningsShouldBePaid _determinePayableEarnings;
        private readonly IValidateRawDatalocks _rawDatalocksValidator;
        private readonly ICalculatePaymentsDue _paymentsDueCalculator;

        public LearnerPaymentsDueProcessor(ILogger logger,
            IDetermineWhichEarningsShouldBePaid determinePayableEarnings, 
            IValidateRawDatalocks rawDatalocksValidator, 
            ICalculatePaymentsDue paymentsDueCalculator)
        {
            _logger = logger;
            _determinePayableEarnings = determinePayableEarnings;
            _rawDatalocksValidator = rawDatalocksValidator;
            _paymentsDueCalculator = paymentsDueCalculator;
        }

        public PaymentsDueResult GetPayableAndNonPayableEarnings(LearnerData parameters, long ukprn)
        {
            _logger.Info($"Processing started for Learner LearnRefNumber: [{parameters.LearnRefNumber}] from provider UKPRN: [{ukprn}].");

            var successfulDatalocks = _rawDatalocksValidator
                .GetSuccessfulDatalocks(parameters.DataLocks, 
                    parameters.DatalockValidationErrors, 
                    parameters.Commitments);
            
            var earnings = _determinePayableEarnings.DeterminePayableEarnings(
                successfulDatalocks,
                parameters.RawEarnings,
                parameters.RawEarningsMathsEnglish, 
                parameters.CompletionPaymentEvidence);

            var paymentsDue = _paymentsDueCalculator.Calculate(earnings.PayableEarnings,
                earnings.PeriodsToIgnore,
                parameters.HistoricalPayments);
            var results = new PaymentsDueResult(paymentsDue, earnings.NonPayableEarnings);
            
            _logger.Info($"There are [{results.NonPayableEarnings.Count}] non-payable earnings for Learner LearnRefNumber: [{parameters.LearnRefNumber}] from provider UKPRN: [{ukprn}].");
            _logger.Info($"There are [{results.PayableEarnings.Count}] payable earnings for Learner LearnRefNumber: [{parameters.LearnRefNumber}] from provider UKPRN: [{ukprn}].");
            _logger.Info($"Processing finished for Learner LearnRefNumber: [{parameters.LearnRefNumber}] from provider UKPRN: [{ukprn}].");

            return results;
        }
    }
}