TRUNCATE TABLE [Reference].[CollectionPeriods]
GO

INSERT INTO [Reference].[CollectionPeriods]
    SELECT
        [Period_ID] AS [Id],
        [Collection_Period] AS [Name],
        [Period] AS [CalendarMonth],
        [Calendar_Year] AS [CalendarYear],
        [Collection_Open] AS [Open]
	FROM ${ILR_Summarisation.FQ}.[dbo].[Collection_Period_Mapping]
	WHERE [Collection_Open] = 1
GO
