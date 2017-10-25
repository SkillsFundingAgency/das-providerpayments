using System;
using SFA.DAS.Payments.DCFS.Context;

namespace SFA.DAS.Payments.Reference.Commitments.Infrastructure.Data.Dcfs
{
    public class DcfsEventStreamPointerRepository : DcfsRepository, IEventStreamPointerRepository
    {
        private const string Source = "[dbo].[EventStreamPointer] ";
        private const string SelectLastEventIdCommand = "SELECT ISNULL(MAX(EventId),0) LastEventId "
                                                      + "FROM " + Source;
        private const string InsertEventIdCommand = "INSERT INTO " + Source
                                                  + "(EventId, ReadDate) "
                                                  + "VALUES "
                                                  + "(@eventId, @readDate)";

        public DcfsEventStreamPointerRepository(ContextWrapper context)
            : base(context.GetPropertyValue(ContextPropertyKeys.TransientDatabaseConnectionString))
        {
        }

        public long GetLastEventId()
        {
            return QuerySingle<long>(SelectLastEventIdCommand);
        }

        public void SetLastEventId(long eventId, DateTime readDate)
        {
            Execute(InsertEventIdCommand, new { eventId, readDate });
        }
    }
}
