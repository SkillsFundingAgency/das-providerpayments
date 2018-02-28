TRUNCATE TABLE [Reference].[CollectionPeriods]
GO

INSERT INTO [Reference].[CollectionPeriods]
 SELECT
        [Period_ID] AS [Id],
        [Return_Code] AS [Name],
        [Calendar_Month] AS [CalendarMonth],
        [Calendar_Year] AS [CalendarYear],
        [Collection_Open] AS [Open]
	FROM ${DAS_PeriodEnd.FQ}.[dbo].[Collection_Period_Mapping]
	WHERE [Collection_Year] = ${YearOfCollection}
	ORDER BY [Period_ID] 
GO
