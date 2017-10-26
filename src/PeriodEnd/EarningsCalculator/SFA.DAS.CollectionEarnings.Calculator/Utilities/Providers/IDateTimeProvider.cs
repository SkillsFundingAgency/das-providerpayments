using System;

namespace SFA.DAS.CollectionEarnings.Calculator.Tools.Providers
{
    public interface IDateTimeProvider
    {
        DateTime Now { get; }
        DateTime Today { get; }
        DateTime YearOfCollectionStart { get; }
    }
}