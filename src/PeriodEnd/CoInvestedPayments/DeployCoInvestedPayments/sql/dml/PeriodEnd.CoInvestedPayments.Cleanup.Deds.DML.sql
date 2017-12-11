DELETE FROM ${DAS_PeriodEnd.FQ}.[Payments].[Payments]
    WHERE [RequiredPaymentId] IN (
		SELECT DISTINCT [Id] FROM [PaymentsDue].[RequiredPayments] WHERE [Ukprn] IN (
			SELECT DISTINCT [Ukprn] FROM [Reference].[Providers]
			)
		)
    AND [CollectionPeriodName] IN (SELECT '${YearOfCollection}-' + [Collection_Period] FROM [CoInvestedPayments].[vw_CollectionPeriods] WHERE [Collection_Open] = 1)
    AND [CollectionPeriodMonth] IN (SELECT [Period] FROM [CoInvestedPayments].[vw_CollectionPeriods] WHERE [Collection_Open] = 1)
    AND [CollectionPeriodYear] IN (SELECT [Calendar_Year] FROM [CoInvestedPayments].[vw_CollectionPeriods] WHERE [Collection_Open] = 1)
    AND [FundingSource] IN (2, 3, 4)
GO
