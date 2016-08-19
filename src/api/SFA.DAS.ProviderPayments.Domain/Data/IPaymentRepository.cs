using System.Threading.Tasks;
using SFA.DAS.ProviderPayments.Domain.Data.Entities;

namespace SFA.DAS.ProviderPayments.Domain.Data
{
    public interface IPaymentRepository
    {
        Task<PageOfEntities<PaymentEntity>> GetPageOfPaymentsForAccountInPeriod(string periodCode, string accountId, int pageNumber);
    }
}
