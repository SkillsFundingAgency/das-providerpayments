using System.Collections.Generic;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data.Entities;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.Domain
{
    public interface ILearnerFactory
    {
        ILearner CreateLearner(
            IEnumerable<RawEarning> rawEarnings, 
            IEnumerable<RawEarningForMathsOrEnglish> mathsAndEnglishEarnings,
            IEnumerable<PriceEpisode> priceEpisodes, 
            IEnumerable<RequiredPaymentEntity> pastPayments);
    }
}