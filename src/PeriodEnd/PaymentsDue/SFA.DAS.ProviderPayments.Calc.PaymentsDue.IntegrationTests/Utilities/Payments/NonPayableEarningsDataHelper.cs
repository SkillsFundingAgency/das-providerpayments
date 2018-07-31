using System.Collections.Generic;
using System.Linq;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data.Entities;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.IntegrationTests.Tools;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.IntegrationTests.Utilities
{
    internal static class NonPayableEarningsDataHelper
    {
        internal static List<NonPayableEarning> GetAll()
        {
            const string sql = @"
            SELECT *
            FROM PaymentsDue.NonPayableEarnings";

            return TestDataHelper
                .Query<NonPayableEarning>(sql)
                .ToList();
        }

        internal static void Truncate()
        {
            const string sql = "TRUNCATE TABLE PaymentsDue.NonPayableEarnings";
            TestDataHelper.Execute(sql);
        }
    }
}