TRUNCATE TABLE [Reference].[CollectionPeriods]
GO

IF EXISTS(SELECT 1 FROM sys.indexes WHERE name='IX_CollectionPeriods_Open' AND object_id = OBJECT_ID('Reference.CollectionPeriods'))
	DROP INDEX IX_CollectionPeriods_Open ON Reference.CollectionPeriods
GO

INSERT INTO [Reference].[CollectionPeriods]
    SELECT
        [Period_ID] AS [Id],
        [Return_Code] AS [Name],
        [Calendar_Month] AS [CalendarMonth],
        [Calendar_Year] AS [CalendarYear],
        [Collection_Open] AS [Open]
	FROM ${DAS_PeriodEnd.FQ}.[dbo].[Collection_Period_Mapping]
	WHERE  [Collection_Year] = ${YearOfCollection}
GO

CREATE INDEX IX_CollectionPeriods_Open ON Reference.CollectionPeriods ([Open])
GO
