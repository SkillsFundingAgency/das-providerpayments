﻿using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data.Entities;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data
{
    public interface IProviderRepository
    {
        ProviderEntity[] GetAllProviders();
        ProviderEntity GetProvider(long ukprn);
    }
}