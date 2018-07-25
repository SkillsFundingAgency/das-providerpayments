using System.Collections.Generic;
using SFA.DAS.ProviderPayments.Calc.Shared.Infrastructure.Data;
using SFA.DAS.ProviderPayments.Calc.Shared.Infrastructure.Data.Entities;

namespace SFA.DAS.ProviderPayments.Calc.Shared.IntegrationTests.Helpers
{
    public class PaymentDataHelper
    {
        internal static IEnumerable<PaymentEntity> GetAll(PaymentSchema paymentSchema)
        {
            string sql = $@"
            select *
            from {paymentSchema.ToString()}.Payments;
            ";

            return TestDataHelper.Query<PaymentEntity>(sql);
        }

        internal static void Truncate(PaymentSchema paymentSchema)
        {
            var sql = $"TRUNCATE TABLE {paymentSchema.ToString()}.Payments";
            TestDataHelper.Execute(sql);
        }
    }
}