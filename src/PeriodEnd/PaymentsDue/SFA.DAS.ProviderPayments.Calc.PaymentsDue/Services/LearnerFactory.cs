using System.Collections.Generic;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Domain;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data.Entities;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.Services
{
    public class LearnerFactory : ILearnerFactory
    {
        public ILearner CreateLearner(
            IEnumerable<RawEarning> rawEarnings, 
            IEnumerable<RawEarningForMathsOrEnglish> mathsAndEnglishEarnings, 
            IEnumerable<PriceEpisode> priceEpisodes,
            IEnumerable<RequiredPaymentEntity> pastPayments)
        {
            return new Learner(rawEarnings, mathsAndEnglishEarnings, priceEpisodes, pastPayments);
        }
    }
}
