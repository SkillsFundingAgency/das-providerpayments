DELETE FROM ${DAS_PeriodEnd.FQ}.[Payments].[Payments]
    WHERE [Ukprn] IN (SELECT DISTINCT [Ukprn] FROM [LevyPayments].[Payments])
        AND [CollectionPeriodName] IN (SELECT DISTINCT '${YearOfCollection}-' + [CollectionPeriodName] FROM [LevyPayments].[Payments])
        AND [CollectionPeriodMonth] IN (SELECT DISTINCT [CollectionPeriodMonth] FROM [LevyPayments].[Payments])
        AND [CollectionPeriodYear] IN (SELECT DISTINCT [CollectionPeriodYear] FROM [LevyPayments].[Payments])
        AND [FundingSource] = 1
GO
