using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFA.DAS.ProviderPayments.Domain
{
    public class Payment
    {
        public Account Account { get; set; }
        public Provider Provider { get; set; }
        public Apprenticeship Apprenticeship { get; set; }
        public Period ReportedPeriod { get; set; }
        public Period DeliveryPeriod { get; set; }
        public decimal Amount { get; set; }
        public TransactionType TransactionType { get; set; }
        public FundingType FundingType { get; set; }
    }

    public class Apprenticeship
    {
        public Learner Learner { get; set; }
        public Course Course { get; set; }
    }
}
