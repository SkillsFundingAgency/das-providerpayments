﻿using System.Collections.Generic;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Domain;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data.Entities;
using SFA.DAS.ProviderPayments.Calc.Shared.Infrastructure.Data.Entities;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.Services.Dependencies
{
    public interface IValidateRawDatalocks
    {
        List<DatalockOutput> GetSuccessfulDatalocks(
            List<DatalockOutputEntity> datalocks, 
            List<DatalockValidationError> datalockValidationErrors,
            List<Commitment> commitments);
    }
}
