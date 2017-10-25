using System;

namespace SFA.DAS.Payments.Reference.Commitments.Infrastructure.Data
{
    public interface IEventStreamPointerRepository
    {
        long GetLastEventId();

        void SetLastEventId(long eventId, DateTime readDate);
    }
}