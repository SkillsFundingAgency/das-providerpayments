using SFA.DAS.Payments.Automation.Infrastructure.PaymentResults;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SFA.DAS.Payments.Automation.Infrastructure.Data
{
    public interface IPaymentsClient
    {
        Task<List<PaymentResult>> GetPayments(long ukprn);
    }
}
