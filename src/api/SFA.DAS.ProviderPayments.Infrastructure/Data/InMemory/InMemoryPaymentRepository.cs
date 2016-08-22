using System;
using System.Linq;
using System.Threading.Tasks;
using SFA.DAS.ProviderPayments.Domain;
using SFA.DAS.ProviderPayments.Domain.Data;
using SFA.DAS.ProviderPayments.Domain.Data.Entities;

namespace SFA.DAS.ProviderPayments.Infrastructure.Data.InMemory
{
    public class InMemoryPaymentRepository : IPaymentRepository
    {
        private Random _rdm = new Random();
        private PaymentEntity[] _payments;
        private const int PageSize = 10;

        public InMemoryPaymentRepository()
        {
            _payments = new[]
            {
                BuildPayment(),
                BuildPayment(),
                BuildPayment(),
                BuildPayment(),
                BuildPayment(),
                BuildPayment(),
                BuildPayment(),
                BuildPayment(fundingType: (int)FundingType.GovermentCoInvestment),
                BuildPayment(fundingType: (int)FundingType.EmployerCoInvestment),
                BuildPayment(),
                BuildPayment(),
                BuildPayment(),
                BuildPayment(),
                BuildPayment(),
                BuildPayment()
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

        private PaymentEntity BuildPayment(string account = null, string ukprn = null, long uln = 0,
            long standardCode = 0, int pathwayCode = 0, int frameworkCode = 0, int programmeType = 0,
            string reportedPeriodCode = null, string deliveryPeriodCode = null, decimal amount = 0,
            int transactionType = 0, int fundingType = 0)
        {
            if (standardCode == 0 && pathwayCode == 0)
            {
                standardCode = _rdm.Next();
            }

            return new PaymentEntity
            {
                AccountId = account ?? "DasAccount1",
                Ukprn = ukprn ?? "PROV1",
                Uln = uln > 0 ? uln : _rdm.Next(),
                StandardCode = standardCode,
                PathwayCode = pathwayCode,
                FrameworkCode = frameworkCode,
                ProgrammeType = programmeType,
                ReportedPeriodCode = reportedPeriodCode ?? "201704",
                DeliveryPeriodCode = deliveryPeriodCode ?? "201704",
                Amount = amount > 0 ? amount : (decimal)Math.Round(_rdm.NextDouble() * _rdm.Next(100, 1000), 2),
                TransactionType = 0,
                FundingType = 0
            };
        }
    }
}
