using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SFA.DAS.Payments.AcceptanceTests.ResultsDataModels
{
    public class LearnerResults
    {
        public LearnerResults()
        {
            Earnings = new List<EarningsResult>();
            Payments = new List<PaymentResult>();
            LevyAccountBalanceResults = new List<LevyAccountBalanceResult>();
            SubmissionDataLockResults = new List<SubmissionDataLockPeriodResults>();
        }
        public string ProviderId { get; set; }
        public string LearnerReferenceNumber { get; set; }
        public List<EarningsResult> Earnings { get; set; }
        public List<PaymentResult> Payments { get; set; }
        public List<LevyAccountBalanceResult> LevyAccountBalanceResults { get; set; }
        public DataLockEventResult[] DataLockEvents { get; set; }
        public List<SubmissionDataLockPeriodResults> SubmissionDataLockResults { get; set; }

        public override string ToString()
        {
            var stringBuilder = new StringBuilder();

            stringBuilder.AppendLine($"Provider: [{ProviderId}], Learner Ref: [{LearnerReferenceNumber}]");
            Earnings.ForEach(result => stringBuilder.AppendLine($"Earning: value:[{result.Value}], calc period:[{result.CalculationPeriod}], delivery period:[{result.DeliveryPeriod}]"));
            Payments.ForEach(result => stringBuilder.AppendLine($"Payment: amount:[{result.Amount}], calc period:[{result.CalculationPeriod}], delivery period:[{result.DeliveryPeriod}]"));
            LevyAccountBalanceResults.ForEach(result => stringBuilder.AppendLine($"Levy Account: amount:[{result.Amount}], calc period:[{result.CalculationPeriod}], delivery period:[{result.DeliveryPeriod}]"));
            if (DataLockEvents != null)
                DataLockEvents.ToList().ForEach(result => stringBuilder.AppendLine($"Datalock Events: Id:[{result.Id}], has errors:[{result.HasErrors}], event source:[{result.EventSource}]"));
            SubmissionDataLockResults.ForEach(result => stringBuilder.AppendLine($"Datalock Results: match period:[{result.MatchPeriod}], calc period:[{result.CalculationPeriod}], matches count:[{result.Matches.Count}]"));
            
            return stringBuilder.ToString();
        }
    }
}
