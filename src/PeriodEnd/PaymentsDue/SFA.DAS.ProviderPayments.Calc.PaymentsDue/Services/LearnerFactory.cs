using System.Collections.Generic;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Domain;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data.Entities;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Services.Dependencies;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.Services
{
    public class LearnerFactory : ILearnerFactory
    {
        public ILearner CreateLearner(
            List<FundingDue> earnings,
            List<int> periodsToIgnore,
            List<RequiredPaymentEntity> pastPayments)
        {
            return new Learner(earnings, periodsToIgnore, pastPayments);
        }
    }
}
