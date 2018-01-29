IF NOT EXISTS (SELECT NULL 
FROM sys.indexes 
WHERE name='IX_DataLockEventPeriods_DataLockId' )
BEGIN
	CREATE INDEX IX_DataLockEventPeriods_DataLockId ON DataLock.DataLockEventPeriods (DataLockEventId)
END


IF NOT EXISTS (SELECT NULL 
FROM sys.indexes 
WHERE name='IX_DataLockEventCommitmentVersions_DataLockId' )
BEGIN
	CREATE INDEX IX_DataLockEventCommitmentVersions_DataLockId ON DataLock.DataLockEventCommitmentVersions (DataLockEventId)
END


IF NOT EXISTS (SELECT NULL 
FROM sys.indexes 
WHERE name='IX_DataLockEvents_DataLockId' )
BEGIN
	CREATE INDEX IX_DataLockEvents_DataLockId ON DataLock.DataLockEvents (DataLockEventId)
END

