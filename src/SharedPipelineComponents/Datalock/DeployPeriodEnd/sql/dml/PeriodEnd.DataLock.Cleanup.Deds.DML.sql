DELETE FROM ${DAS_PeriodEnd.FQ}.[DataLock].[ValidationError]
    WHERE [Ukprn] IN (SELECT DISTINCT p.[Ukprn] FROM [DataLock].[vw_Providers] p)
        AND [CollectionPeriodName] IN (SELECT '${YearOfCollection}-' + [Name] FROM Reference.CollectionPeriods WHERE [Open] = 1)
        AND [CollectionPeriodMonth] IN (SELECT [CalendarMonth] FROM Reference.CollectionPeriods WHERE [Open] = 1)
        AND [CollectionPeriodYear] IN (SELECT [CalendarYear] FROM Reference.CollectionPeriods WHERE [Open] = 1)
GO

DELETE FROM ${DAS_PeriodEnd.FQ}.[DataLock].[PriceEpisodeMatch]
    WHERE [Ukprn] IN (SELECT DISTINCT p.[Ukprn] FROM [DataLock].[vw_Providers] p)
        AND [CollectionPeriodName] IN (SELECT '${YearOfCollection}-' + [Name] FROM Reference.CollectionPeriods WHERE [Open] = 1)
        AND [CollectionPeriodMonth] IN (SELECT [CalendarMonth] FROM Reference.CollectionPeriods WHERE [Open] = 1)
        AND [CollectionPeriodYear] IN (SELECT [CalendarYear] FROM Reference.CollectionPeriods WHERE [Open] = 1)
GO

DELETE FROM ${DAS_PeriodEnd.FQ}.[DataLock].[PriceEpisodePeriodMatch]
    WHERE [Ukprn] IN (SELECT DISTINCT p.[Ukprn] FROM [DataLock].[vw_Providers] p)
        AND [CollectionPeriodName] IN (SELECT '${YearOfCollection}-' + [Name] FROM Reference.CollectionPeriods WHERE [Open] = 1)
        AND [CollectionPeriodMonth] IN (SELECT [CalendarMonth] FROM Reference.CollectionPeriods WHERE [Open] = 1)
        AND [CollectionPeriodYear] IN (SELECT [CalendarYear] FROM Reference.CollectionPeriods WHERE [Open] = 1)
GO
