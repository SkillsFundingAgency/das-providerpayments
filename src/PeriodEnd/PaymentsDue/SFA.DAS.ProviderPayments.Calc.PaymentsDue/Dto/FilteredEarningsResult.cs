using System.Collections.Generic;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data.Entities;
using SFA.DAS.ProviderPayments.Calc.Shared.Infrastructure.Data.Entities;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.Dto
{
    public class FilteredEarningsResult
    {
        public List<RawEarning> RawEarnings { get; set; } = new List<RawEarning>();
        public List<NonPayableEarning> NonPayableEarnings { get; set; } = new List<NonPayableEarning>();
    }
}
