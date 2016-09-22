using SFA.DAS.Payments.Calc.CoInvestedPayments.Infrastructure.Data.Entities;
using SFA.DAS.ProviderPayments.Calc.Common.Infrastructure.Data;

namespace SFA.DAS.Payments.Calc.CoInvestedPayments.Infrastructure.Data.Repositories
{
    public class ProviderRepository : DcfsRepository, IProviderRepository
    {
        private const string ProviderSource = "PaymentsDue.RequiredPayments";
        private const string ProviderColumns = "UKPRN [Ukprn]";
        private const string SelectProviders = "SELECT " + ProviderColumns + " FROM " + ProviderSource;

        public ProviderRepository(string connectionString)
            : base(connectionString)
        {
        }

        public ProviderEntity[] GetAllProviders()
        {
            return Query<ProviderEntity>(SelectProviders);
        }
    }
}