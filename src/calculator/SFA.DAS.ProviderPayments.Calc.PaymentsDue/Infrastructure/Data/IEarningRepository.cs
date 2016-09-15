using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data.Entities;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data
{
    public interface IEarningRepository
    {
        EarningEntity[] GetProviderEarnings(long ukprn);
    }
}