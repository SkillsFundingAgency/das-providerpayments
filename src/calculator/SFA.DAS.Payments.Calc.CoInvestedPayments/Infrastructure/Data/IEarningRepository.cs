﻿using SFA.DAS.Payments.Calc.CoInvestedPayments.Infrastructure.Data.Entities;

namespace SFA.DAS.Payments.Calc.CoInvestedPayments.Infrastructure.Data
{
    public interface IEarningRepository
    {
        EarningEntity[] GetProviderEarnings(long ukprn);
    }
}