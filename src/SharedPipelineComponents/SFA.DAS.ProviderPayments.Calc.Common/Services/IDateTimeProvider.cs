using System;

namespace SFA.DAS.ProviderPayments.Calc.Common.Services
{
    public interface IDateTimeProvider
    {
        DateTime YearOfCollectionStart { get; }
    }
}