using System.Collections.Generic;
using SFA.DAS.ProviderPayments.Calc.Shared.Infrastructure.Data.Entities;

namespace SFA.DAS.ProviderPayments.Calc.Refunds.Services.Dependiencies
{
    public interface IProcessLearnerRefunds
    {
        List<PaymentEntity> ProcessRefundsForLearner(List<RequiredPaymentEntity> refunds, List<HistoricalPaymentEntity> previousPayments);
    }
}
