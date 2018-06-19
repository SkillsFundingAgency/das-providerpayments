using System.Collections.Generic;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Domain;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Dto;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data.Entities;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.Services.Dependencies
{
    public interface IIShouldBeInTheDataLockComponent
    {
        DatalockValidationResult ValidatePriceEpisodes(
            List<Commitment> commitments,
            List<DatalockOutput> datalockOutput, 
            List<DatalockValidationError> datalockValidationErrors,
            List<RawEarning> earnings,
            List<RawEarningForMathsOrEnglish> mathsAndEnglishEarnings);
    }
}