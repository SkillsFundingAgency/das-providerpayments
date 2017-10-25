using SFA.DAS.Payments.Automation.Domain.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFA.DAS.Payments.Automation.Infrastructure.PaymentResults
{
    public class PaymentResult
    {
        public string Id { get; set; }
        public long Ukprn { get; set; }
        public long Uln { get; set; }
        public string EmployerAccountId { get; set; }
        public long? ApprenticeshipId { get; set; }
        public int DeliveryMonth { get; set; }
        public int DeliveryYear { get; set; }
        public string DeliveryPeriod { get; set; }

        public int CollectionPeriodMonth { get; set; }
        public int CollectionPeriodYear { get; set; }
        public string CalculationPeriod { get; set; }

        public FundingSource FundingSource { get; set; }
        public TransactionType TransactionType { get; set; }
        public decimal Amount { get; set; }
        public  ContractType ContractType { get; set; }
    }
}
