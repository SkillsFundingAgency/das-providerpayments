﻿using SFA.DAS.Payments.DCFS.Infrastructure.Data;
using SFA.DAS.Provider.Events.DataLock.Domain.Data;
using SFA.DAS.Provider.Events.DataLock.Domain.Data.Entities;
using System;

namespace SFA.DAS.Provider.Events.DataLock.Infrastructure.Data
{
    public class DcfsDataLockEventCommitmentVersionRepository : DcfsRepository, IDataLockEventCommitmentVersionRepository
    {
        private const string Source = "Reference.DataLockEventCommitmentVersions";
        private const string Columns = "DataLockEventId," +
                                       "CommitmentVersion," +
                                       "CommitmentStartDate," +
                                       "CommitmentStandardCode," +
                                       "CommitmentProgrammeType," +
                                       "CommitmentFrameworkCode," +
                                       "CommitmentPathwayCode," +
                                       "CommitmentNegotiatedPrice," +
                                       "CommitmentEffectiveDate";
        private const string SelectEventCommitmentVersions = "SELECT " + Columns + " FROM " + Source + " WHERE DataLockEventId = @eventId";

        public DcfsDataLockEventCommitmentVersionRepository(string connectionString)
            : base(connectionString)
        {
        }

        public DataLockEventCommitmentVersionEntity[] GetDataLockEventCommitmentVersions(Guid eventId)
        {
            return Query<DataLockEventCommitmentVersionEntity>(SelectEventCommitmentVersions, new { eventId });
        }

        public void BulkWriteDataLockEventCommitmentVersion(DataLockEventCommitmentVersionEntity[] versions)
        {
            ExecuteBatch(versions, "DataLockEvents.DataLockEventCommitmentVersions");
        }
    }
}