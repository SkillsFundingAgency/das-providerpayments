
IF EXISTS(SELECT 1 FROM sys.columns 
          WHERE Name = N'HistoricEmpIdEndWithinYear'
          AND Object_ID = Object_ID(N'dbo.AEC_EarningHistory'))
BEGIN
    ALTER TABLE [dbo].[AEC_EarningHistory]
		ALTER COLUMN [HistoricEmpIdEndWithinYear] INT NULL
END
ELSE
BEGIN
	  ALTER TABLE [dbo].[AEC_EarningHistory]
		ADD [HistoricEmpIdEndWithinYear] INT NULL
END
GO

IF EXISTS(SELECT 1 FROM sys.columns 
          WHERE Name = N'HistoricEmpIdStartWithinYear'
          AND Object_ID = Object_ID(N'dbo.AEC_EarningHistory'))
BEGIN
    ALTER TABLE [dbo].[AEC_EarningHistory]
		ALTER COLUMN [HistoricEmpIdStartWithinYear] INT NULL
END
ELSE
BEGIN
	  ALTER TABLE [dbo].[AEC_EarningHistory]
		ADD [HistoricEmpIdStartWithinYear] INT NULL
END
GO

ALTER TABLE [dbo].[AEC_EarningHistory]
   ALTER COLUMN [HistoricTotal1618UpliftPaymentsInTheYearInput] [decimal](11, 5) NULL

GO
ALTER TABLE [dbo].[AEC_EarningHistory]
   ALTER COLUMN    [HistoricTNP1Input] [decimal](12, 5) NULL

GO
ALTER TABLE [dbo].[AEC_EarningHistory]
   ALTER COLUMN 	[HistoricTNP2Input] [decimal](12, 5) NULL

GO
ALTER TABLE [dbo].[AEC_EarningHistory]
   ALTER COLUMN 	[HistoricTNP3Input] [decimal](12, 5) NULL

GO
ALTER TABLE [dbo].[AEC_EarningHistory]
   ALTER COLUMN 	[HistoricTNP4Input] [decimal](12, 5) NULL
GO

ALTER TABLE [dbo].[AEC_EarningHistory]
   ALTER COLUMN 	[TotalProgAimPaymentsInTheYear] [decimal](11, 5) NULL
GO

ALTER TABLE [dbo].[AEC_EarningHistory]
   ALTER COLUMN 		[HistoricVirtualTNP3EndOfTheYearInput] [decimal](12, 5) NULL
GO

ALTER TABLE [dbo].[AEC_EarningHistory]
   ALTER COLUMN 		[HistoricVirtualTNP4EndOfTheYearInput] [decimal](12, 5) NULL

	

	