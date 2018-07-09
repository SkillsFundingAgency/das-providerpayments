﻿
using System.Collections.Generic;
using SFA.DAS.ProviderPayments.Calc.Refunds.Dto;
using SFA.DAS.ProviderPayments.Calc.Shared.Infrastructure.Data.Entities;

namespace SFA.DAS.ProviderPayments.Calc.Refunds.Services.Dependencies
{
    public interface ILearnerProcessor
    {
        IEnumerable<Refund> Process(LearnerData learnerData);
    }
}