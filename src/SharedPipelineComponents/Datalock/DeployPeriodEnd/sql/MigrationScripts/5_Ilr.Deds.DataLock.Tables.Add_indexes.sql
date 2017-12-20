IF EXISTS (SELECT * FROM sys.indexes i
JOIN sys.objects t ON i.object_id = t.object_id
WHERE t.name = 'DataLockEventPeriods'
AND i.name = 'IX_DataLockEventPeriods_DataLockEventId')
BEGIN
	DROP INDEX IX_DataLockEventPeriods_DataLockEventId ON DataLock.DataLockEventPeriods
END
GO

CREATE INDEX IX_DataLockEventPeriods_DataLockEventId ON DataLock.DataLockEventPeriods (DataLockEventId)
GO


IF EXISTS (SELECT * FROM sys.indexes i
JOIN sys.objects t ON i.object_id = t.object_id
WHERE t.name = 'DataLockEventCommitmentVersions'
AND i.name = 'IX_DataLockEventCommitmentVersions_DataLockEventId')
BEGIN
	DROP INDEX IX_DataLockEventCommitmentVersions_DataLockEventId ON DataLock.DataLockEventCommitmentVersions
END
GO

CREATE INDEX IX_DataLockEventCommitmentVersions_DataLockEventId ON DataLock.DataLockEventCommitmentVersions (DataLockEventId)



