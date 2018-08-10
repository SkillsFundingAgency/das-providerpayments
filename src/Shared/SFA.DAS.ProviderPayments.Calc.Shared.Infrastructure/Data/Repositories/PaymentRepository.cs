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

        public IEnumerable<PaymentEntity> GetAllHistoricPaymentsForProvider(long ukprn)
        {
            const string sql = @"
            SELECT 
                [LearnRefNumber]
                ,[DeliveryMonth]
                ,[DeliveryYear]
                ,[Amount]
                ,[RequiredPaymentId]
                ,[CollectionPeriodName]
                ,[CollectionPeriodMonth]
                ,[CollectionPeriodYear]
                ,[FundingSource]
                ,[TransactionType]
            FROM Reference.PaymentsHistory
            WHERE Ukprn = @ukprn";

            return Query<PaymentEntity>(sql, new {ukprn});
        }
    }
}
