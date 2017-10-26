using System.Collections.Concurrent;
using System.Collections.Generic;

namespace SFA.DAS.CollectionEarnings.DataLock.Tools.Extensions
{
    public static class ConcurrentBagExtensions
    {
        public static void AddRange<T>(this ConcurrentBag<T> @this, IEnumerable<T> toAdd)
        {
            foreach (var element in toAdd)
            {
                @this.Add(element);
            }
        }
    }
}