IF NOT EXISTS(SELECT [column_id] FROM sys.columns WHERE [name] = 'TransactionTypes' AND [object_id] = OBJECT_ID('DataLock.PriceEpisodePeriodMatch'))
	BEGIN
		ALTER TABLE [DataLock].[PriceEpisodePeriodMatch]
		ADD [TransactionTypes] int NULL
	END
GO
