using System.Collections.Generic;
using SFA.DAS.ProviderPayments.Calc.Refunds.Domain;
using SFA.DAS.ProviderPayments.Calc.Refunds.Dto;
using SFA.DAS.ProviderPayments.Calc.Shared.Infrastructure.Data.Entities;

namespace SFA.DAS.ProviderPayments.Calc.Refunds.Services.Dependencies
{
    public interface IProcessLearnerRefunds
    {
        List<Refund> ProcessRefundsForLearner(List<RequiredPaymentEntity> refunds, List<HistoricalPayment> previousPayments);
    }
}
