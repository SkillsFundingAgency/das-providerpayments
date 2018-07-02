
using System.Collections.Generic;
using SFA.DAS.ProviderPayments.Calc.Refunds.Dto;
using SFA.DAS.ProviderPayments.Calc.Refunds.Infrastructure.Data.Entities;

namespace SFA.DAS.ProviderPayments.Calc.Refunds.Services.Dependencies
{
    public interface IProviderProcessor
    {
        List<AccountLevyCredit> Process(ProviderEntity provider);
    }
}