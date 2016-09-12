using System;

namespace SFA.DAS.ProviderPayments.Calc.PaymentSchedule.Infrastructure.Data.Entities
{
    public class EarningEntity
    {
        public string CommitmentId { get; set; }
        public string LearnerRefNumber { get; set; }
        public int AimSequenceNumber { get; set; }
        public long Ukprn { get; set; }
        public DateTime LearningStartDate { get; set; }
        public DateTime LearningPlannedEndDate { get; set; }
        public DateTime? LearningActualEndDate { get; set; }
        public decimal MonthlyInstallment { get; set; }
        public decimal CompletionPayment { get; set; }
        public decimal MonthlyInstallmentCapped { get; set; }
        public decimal CompletionPaymentCapped { get; set; }
        public int TotalNumberOfPeriods { get; set; }
        public int CurrentPeriod { get; set; }
    }
}