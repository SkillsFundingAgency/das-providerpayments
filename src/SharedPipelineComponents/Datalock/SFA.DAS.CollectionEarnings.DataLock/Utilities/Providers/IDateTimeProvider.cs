using System;

namespace SFA.DAS.CollectionEarnings.DataLock.Tools.Providers
{
    public interface IDateTimeProvider
    {
        DateTime YearOfCollectionStart { get; }
    }
}