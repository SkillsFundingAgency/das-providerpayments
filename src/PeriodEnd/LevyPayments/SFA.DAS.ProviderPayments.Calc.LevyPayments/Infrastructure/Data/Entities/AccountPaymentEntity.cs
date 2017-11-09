using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFA.DAS.ProviderPayments.Calc.LevyPayments.Infrastructure.Data.Entities
{
    public class AccountPaymentEntity
    {
        public Guid Id { get; set; }
        public long CommitmentId { get; set; }
        public string LearnRefNumber { get; set; }
        public int AimSeqNumber { get; set; }
        public long Ukprn { get; set; }
        public int DeliveryMonth { get; set; }
        public int DeliveryYear { get; set; }
        public int TransactionType { get; set; }
        public decimal AmountDue { get; set; }
        public string AccountId { get; set; }
        public string AccountName { get; set; }
        public decimal Balance { get; set; }
        public bool IsRefund { get; set; }
    }
}
