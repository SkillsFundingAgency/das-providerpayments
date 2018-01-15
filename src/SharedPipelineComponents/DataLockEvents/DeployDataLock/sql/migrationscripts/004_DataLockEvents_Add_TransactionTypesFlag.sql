IF NOT EXISTS(SELECT [column_id] FROM sys.columns WHERE [name] = 'TransactionTypesFlag' AND [object_id] = OBJECT_ID('DataLock.DataLockEventPeriods'))
	BEGIN
		ALTER TABLE [DataLock].[DataLockEventPeriods]
		ADD [TransactionTypesFlag] int NULL
	END
GO
