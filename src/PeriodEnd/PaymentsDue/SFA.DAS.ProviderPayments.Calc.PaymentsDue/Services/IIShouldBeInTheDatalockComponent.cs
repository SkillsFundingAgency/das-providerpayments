using System;
using System.Collections.Generic;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Domain;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data.Entities;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.Services
{
    public interface IIShouldBeInTheDataLockComponent
    {
        List<PriceEpisode> ValidatePriceEpisodes(
            List<Commitment> commitments,
            List<DatalockOutput> dataLocks,
            DateTime lastDayOfAcademicYear);
    }
}