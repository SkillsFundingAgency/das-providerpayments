DELETE FROM ${DAS_PeriodEnd.FQ}.[Payments].[Payments]
    WHERE [Ukprn] IN (SELECT DISTINCT [Ukprn] FROM [Reference].[Providers])
        AND [CollectionPeriodName] IN (SELECT '${YearOfCollection}-' + [Collection_Period] FROM [LevyPayments].[vw_CollectionPeriods] WHERE [Collection_Open] = 1)
        AND [CollectionPeriodMonth] IN (SELECT [Period] FROM [LevyPayments].[vw_CollectionPeriods] WHERE [Collection_Open] = 1)
        AND [CollectionPeriodYear] IN (SELECT [Calendar_Year] FROM [LevyPayments].[vw_CollectionPeriods] WHERE [Collection_Open] = 1)
        AND [FundingSource] = 1
GO
