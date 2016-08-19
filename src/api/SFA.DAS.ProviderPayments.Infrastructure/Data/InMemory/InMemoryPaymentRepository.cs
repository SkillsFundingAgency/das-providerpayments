using System;
using System.Linq;
using System.Threading.Tasks;
using SFA.DAS.ProviderPayments.Domain.Data;
using SFA.DAS.ProviderPayments.Domain.Data.Entities;

namespace SFA.DAS.ProviderPayments.Infrastructure.Data.InMemory
{
    public class InMemoryPaymentRepository : IPaymentRepository
    {
        private PaymentEntity[] _payments;
        private const int PageSize = 10;

        public InMemoryPaymentRepository()
        {
            _payments = new[]
            {
                new PaymentEntity
                {
                    AccountId = "DasAccount1",
                    Ukprn = "PROV1",
                    Uln = 12344532,
                    StandardCode = 8348957394,
                    ReportedPeriodCode = "201704",
                    DeliveryPeriodCode = "201704",
                    Amount = 134.31m,
                    TransactionType = 0,
                    FundingType = 0
                }
            };
        }

        public Task<PageOfEntities<PaymentEntity>> GetPageOfPaymentsForAccountInPeriod(string periodCode, string accountId, int pageNumber)
        {
            var skip = (pageNumber - 1) * PageSize;
            var payments = _payments.Where(p => p.ReportedPeriodCode == periodCode && p.AccountId.Equals(accountId, StringComparison.OrdinalIgnoreCase))
                .ToArray();
            var pageOfPayments = payments.Skip(skip).Take(PageSize).ToArray();
            if (pageOfPayments == null || !pageOfPayments.Any())
            {
                return Task.FromResult<PageOfEntities<PaymentEntity>>(null);
            }

            return Task.FromResult(new PageOfEntities<PaymentEntity>
            {
                TotalNumberOfItems = payments.Length,
                TotalNumberOfPages = (int)Math.Ceiling(payments.Length / (float)PageSize),
                Items = pageOfPayments
            });
        }
    }
}
