using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data.Entities
{
    public class LearnerFAMEntity
    {
        public string LearnRefNumber { get; set; }
        public string LearnFAMType { get; set; }
        public int LearnFAMCode { get; set; }
    }
}
