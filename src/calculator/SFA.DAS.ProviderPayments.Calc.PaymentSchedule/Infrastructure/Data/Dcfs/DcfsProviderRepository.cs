using SFA.DAS.ProviderPayments.Calc.Common.Infrastructure.Data;
using SFA.DAS.ProviderPayments.Calc.PaymentSchedule.Infrastructure.Data.Entities;

namespace SFA.DAS.ProviderPayments.Calc.PaymentSchedule.Infrastructure.Data.Dcfs
{
    public class DcfsProviderRepository : DcfsRepository, IProviderRepository
    {
        private const string ProviderSource = "PaymentSchedule.vw_Providers";
        private const string ProviderColumns = "UKPRN [Ukprn]";
        private const string SelectProviders = "SELECT " + ProviderColumns + " FROM " + ProviderSource;

        public DcfsProviderRepository(string connectionString)
            : base(connectionString)
        {
        }

        public ProviderEntity[] GetAllProviders()
        {
            return Query<ProviderEntity>(SelectProviders);
        }
    }
}