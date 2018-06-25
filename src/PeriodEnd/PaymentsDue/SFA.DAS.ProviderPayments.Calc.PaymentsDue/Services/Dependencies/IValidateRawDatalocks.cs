using System.Collections.Generic;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Domain;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data.Entities;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.Services.Dependencies
{
    public interface IValidateRawDatalocks
    {
        List<DatalockOutput> ProcessDatalocks(
            List<DatalockOutputEntity> datalocks, 
            List<DatalockValidationError> datalockValidationErrors,
            List<Commitment> commitments);
    }
}
