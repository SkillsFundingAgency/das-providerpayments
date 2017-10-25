using ProviderPayments.TestStack.Domain;

namespace ProviderPayments.TestStack.UI.Models
{
    public class PaymentsReportModel
    {
        public PaymentReport[] Payments { get; set; }
        public Account[] Accounts { get; set; } 
    }
}