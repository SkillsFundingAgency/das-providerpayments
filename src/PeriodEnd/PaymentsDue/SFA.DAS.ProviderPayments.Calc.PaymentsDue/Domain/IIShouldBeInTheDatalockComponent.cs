using System.Collections.Generic;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data.Entities;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.Domain
{
    public interface IIShouldBeInTheDataLockComponent
    {
        List<PriceEpisode> ValidatePriceEpisodes(
            List<Commitment> commitments,
            List<DataLockPriceEpisodePeriodMatchEntity> dataLocks);
    }
}