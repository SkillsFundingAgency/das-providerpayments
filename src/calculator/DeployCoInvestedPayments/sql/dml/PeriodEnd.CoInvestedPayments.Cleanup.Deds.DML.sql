DELETE FROM ${DAS_PeriodEnd.FQ}.[Payments].[Payments]
    WHERE [Ukprn] IN (SELECT DISTINCT [Ukprn] FROM [CoInvestedPayments].[Payments])
        AND [CollectionPeriodName] IN (SELECT DISTINCT '${YearOfCollection}-' + [CollectionPeriodName] FROM [CoInvestedPayments].[Payments])
        AND [CollectionPeriodMonth] IN (SELECT DISTINCT [CollectionPeriodMonth] FROM [CoInvestedPayments].[Payments])
        AND [CollectionPeriodYear] IN (SELECT DISTINCT [CollectionPeriodYear] FROM [CoInvestedPayments].[Payments])
        AND [FundingSource] IN (2, 3)
GO
