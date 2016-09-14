﻿using SFA.DAS.ProviderPayments.Calc.PaymentSchedule.Infrastructure.Data.Entities;

namespace SFA.DAS.ProviderPayments.Calc.PaymentSchedule.Infrastructure.Data
{
    public interface IProviderRepository
    {
        ProviderEntity[] GetAllProviders();
    }
}