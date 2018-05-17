DECLARE @CollectionPeriodMonth int = (SELECT [CalendarMonth] FROM Reference.CollectionPeriods WHERE [Open] = 1)
DECLARE @CollectionPeriodYear int = (SELECT [CalendarYear] FROM Reference.CollectionPeriods WHERE [Open] = 1)

DELETE FROM ${DAS_PeriodEnd.FQ}.[Payments].[Payments]
    WHERE 1=1
        AND [CollectionPeriodMonth] = @CollectionPeriodMonth
        AND [CollectionPeriodYear] = @CollectionPeriodYear
        AND [FundingSource] = 5
GO
