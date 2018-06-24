using System.Collections.Generic;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Dto;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.Services.Dependencies
{
    public interface ISortProviderDataIntoLearnerData
    {
        List<LearnerData> Sort(long ukprn);
    }
}