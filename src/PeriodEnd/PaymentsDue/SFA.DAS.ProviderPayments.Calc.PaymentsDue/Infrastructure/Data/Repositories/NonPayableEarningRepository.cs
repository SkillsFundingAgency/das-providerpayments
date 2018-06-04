using System.Collections.Generic;
using SFA.DAS.Payments.DCFS.Infrastructure.Data;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data.Entities;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data.Repositories
{
    public interface INonPayableEarningRepository
    {
        void AddMany(List<NonPayableEarningEntity> entities);
    }

    public class NonPayableEarningRepository : DcfsRepository, INonPayableEarningRepository
    {
        public NonPayableEarningRepository(string connectionString) : base(connectionString)
        {
        }

        public void AddMany(List<NonPayableEarningEntity> entities)
        {
            throw new System.NotImplementedException();
        }
    }
}