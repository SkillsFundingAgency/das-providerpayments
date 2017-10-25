using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using ProviderPayments.TestStack.Domain.Data.Entities;

namespace ProviderPayments.TestStack.Domain.Data
{
    public interface IPaymentReportRepository : IRepository
    {
        Task<IEnumerable<PaymentReportEntity>> All();
    }
}