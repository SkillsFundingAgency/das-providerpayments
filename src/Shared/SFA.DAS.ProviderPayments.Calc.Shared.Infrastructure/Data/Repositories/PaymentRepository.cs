using System.Collections.Generic;
using SFA.DAS.Payments.DCFS.Infrastructure.Data;
using SFA.DAS.ProviderPayments.Calc.Shared.Infrastructure.Data.Entities;

namespace SFA.DAS.ProviderPayments.Calc.Shared.Infrastructure.Data.Repositories
{
    public class PaymentRepository : DcfsRepository, IPaymentRepository
    {
        public PaymentRepository(string connectionString) : base(connectionString)
        {
        }

        public void AddMany(List<PaymentEntity> payments, PaymentSchema schema)
        {
            ExecuteBatch(payments.ToArray(), $"{schema.ToString()}.Payments");
        }

        public IEnumerable<LearnerSummaryPaymentEntity> GetHistoricEmployerPaymentsEachRoundedDownForProvider(long ukprn)
        {
            const string sql = @"
            SELECT 
                LearnRefNumber,
                TransactionType,
                SUM(FLOOR(Amount)) As Amount
            FROM Reference.PaymentsHistory
            WHERE Ukprn = @ukprn AND FundingSource = 3
            GROUP BY LearnRefNumber, TransactionType" ;

            return Query<LearnerSummaryPaymentEntity>(sql, new { ukprn });
        }
    }
}
