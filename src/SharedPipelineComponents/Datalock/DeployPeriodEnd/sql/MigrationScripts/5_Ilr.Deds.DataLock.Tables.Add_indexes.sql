
CREATE INDEX IX_DataLockEventPeriods_DataLockEventId ON DataLock.DataLockEventPeriods (DataLockEventId)

CREATE INDEX IX_DataLockEventCommitmentVersions_DataLockEventId ON DataLock.DataLockEventCommitmentVersions (DataLockEventId)

CREATE INDEX IX_FileDetails_Id_Ukprn ON dbo.FileDetails (Id, Ukprn)
CREATE INDEX IX_FileDetails_Ukprn_Id ON dbo.FileDetails (Ukprn, Id)

