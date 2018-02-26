﻿using SFA.DAS.Payments.DCFS.Infrastructure.Data;
using SFA.DAS.Provider.Events.DataLock.Domain.Data;
using SFA.DAS.Provider.Events.DataLock.Domain.Data.Entities;
using System;

namespace SFA.DAS.Provider.Events.DataLock.Infrastructure.Data
{
    public class DcfsDataLockEventPeriodRepository : DcfsRepository, IDataLockEventPeriodRepository
    {
        private const string Source = "Reference.DataLockEventPeriods";
        private const string Columns = "DataLockEventId," +
                                       "CollectionPeriodName," +
                                       "CollectionPeriodMonth," +
                                       "CollectionPeriodYear," +
                                       "CommitmentVersion," +
                                       "IsPayable," +
                                       "TransactionType," +
                                        "TransactionTypesFlag";
        private const string SelectEventPeriods = "SELECT " + Columns + " FROM " + Source + " WHERE DataLockEventId = @eventId";

        public DcfsDataLockEventPeriodRepository(string connectionString)
            : base(connectionString)
        {
        }

        public DataLockEventPeriodEntity[] GetDataLockEventPeriods(Guid eventId)
        {
            return Query<DataLockEventPeriodEntity>(SelectEventPeriods, new { eventId });
        }

        public void BulkWriteDataLockEventPeriods(DataLockEventPeriodEntity[] periods)
        {
            ExecuteBatch(periods, "DataLockEvents.DataLockEventPeriods");
        }
    }
}