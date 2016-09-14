using System;

namespace SFA.DAS.ProviderPayments.Calc.PaymentSchedule.Application.Earnings
{
    public class PeriodEarning
    {
        public string CommitmentId { get; set; }
        public string LearnerRefNumber { get; set; }
        public int AimSequenceNumber { get; set; }
        public long Ukprn { get; set; }
        public DateTime LearningStartDate { get; set; }
        public DateTime LearningPlannedEndDate { get; set; }
        public DateTime? LearningActualEndDate { get; set; }
        public decimal MonthlyInstallment { get; set; }
        public decimal MonthlyInstallmentUncapped { get; set; }
        public decimal CompletionPayment { get; set; }
        public decimal CompletionPaymentUncapped { get; set; }
    }
}