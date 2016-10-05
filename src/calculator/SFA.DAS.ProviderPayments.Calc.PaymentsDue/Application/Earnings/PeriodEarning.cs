using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFA.DAS.ProviderPayments.Calc.Common.Application;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.Application.Earnings
{
    public class PeriodEarning
    {
        public string CommitmentId { get; set; }
        public long Ukprn { get; set; }
        public string LearnerReferenceNumber { get; set; }
        public int AimSequenceNumber { get; set; }

        public int CollectionPeriodNumber { get; set; }
        public string CollectionAcademicYear { get; set; }

        public int CalendarMonth { get; set; }
        public int CalendarYear { get; set; }

        public decimal EarnedValue { get; set; }
        public TransactionType Type { get; set; }
    }
}
