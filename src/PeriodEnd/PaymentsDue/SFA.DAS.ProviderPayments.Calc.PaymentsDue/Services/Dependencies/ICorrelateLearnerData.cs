using System.Collections.Generic;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Dto;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.Services.Dependencies
{
    public interface ICorrelateLearnerData
    {
        List<LearnerData> Correlate(long ukprn);
    }
}