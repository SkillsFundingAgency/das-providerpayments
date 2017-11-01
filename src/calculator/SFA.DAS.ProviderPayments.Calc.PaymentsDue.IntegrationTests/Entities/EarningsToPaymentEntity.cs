using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.IntegrationTests.Entities
{
    public class EarningsToPaymentEntity
    {
        public Guid Id { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime PlannedEndDate { get; set; }
        public DateTime? ActualEndDate { get; set; }
        public int CompletionStatus { get; set; }
        public decimal CompletionAmount { get; set; }
        public int TotalInstallments { get; set; }
        public decimal MonthlyInstallment { get; set; }

    }
}
