using SFA.DAS.Payments.DCFS.Infrastructure.Data;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data.Entities;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data.Repositories
{
    public class DcfsProviderRepository : DcfsRepository, IProviderRepository
    {
        private const string ProviderSource = "Reference.Providers";
        private const string ProviderColumns = "UKPRN, "
                                             + "IlrSubmissionDateTime";
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