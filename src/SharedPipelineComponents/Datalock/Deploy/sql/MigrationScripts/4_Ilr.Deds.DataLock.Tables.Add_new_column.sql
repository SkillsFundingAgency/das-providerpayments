IF NOT EXISTS(SELECT [column_id] FROM sys.columns WHERE [name] = 'TransactionTypesFlag' AND [object_id] = OBJECT_ID('DataLock.PriceEpisodePeriodMatch'))
	BEGIN
		ALTER TABLE [DataLock].[PriceEpisodePeriodMatch]
		ADD [TransactionTypesFlag] int NULL
	END
GO
