IF NOT EXISTS(SELECT 1 FROM sys.columns 
          WHERE Name = N'HistoricLearnDelProgEarliestACT2DateInput'
          AND Object_ID = Object_ID(N'dbo.AEC_EarningHistory'))
BEGIN
    ALTER TABLE [dbo].[AEC_EarningHistory]
		 ADD [HistoricLearnDelProgEarliestACT2DateInput] DATE NULL
END