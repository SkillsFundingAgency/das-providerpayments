using System;

namespace SFA.DAS.ProviderPayments.Calc.Common.Interfaces
{
    public interface IDateTimeProvider
    {
        DateTime Now { get; }
    }
}
