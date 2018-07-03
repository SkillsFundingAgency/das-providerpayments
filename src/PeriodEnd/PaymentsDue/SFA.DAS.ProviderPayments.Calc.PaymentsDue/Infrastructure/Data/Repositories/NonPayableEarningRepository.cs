using System.Collections.Generic;
using SFA.DAS.Payments.DCFS.Infrastructure.Data;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data.Entities;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data.Repositories
{
    public interface INonPayableEarningRepository
    {
        void AddMany(List<NonPayableEarning> nonPayableEarnings);
    }

    public class NonPayableEarningRepository : DcfsRepository, INonPayableEarningRepository
    {
        public NonPayableEarningRepository(string connectionString) : base(connectionString)
        {
        }

        public void AddMany(List<NonPayableEarning> nonPayableEarnings)
        {
            ExecuteBatch(nonPayableEarnings.ToArray(), "PaymentsDue.NonPayableEarnings");
        }
    }
}