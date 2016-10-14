DELETE FROM ${DAS_PeriodEnd.FQ}.[PaymentsDue].[RequiredPayments]
    WHERE [Ukprn] IN (SELECT [Ukprn] FROM ${ILR_Deds.FQ}.[Valid].[LearningProvider])
        AND [CollectionPeriodName] IN (SELECT [Collection_Period] FROM [PaymentsDue].[vw_CollectionPeriods] WHERE [Collection_Open] = 1)
        AND [CollectionPeriodMonth] IN (SELECT [Period] FROM [PaymentsDue].[vw_CollectionPeriods] WHERE [Collection_Open] = 1)
        AND [CollectionPeriodYear] IN (SELECT [Calendar_Year] FROM [PaymentsDue].[vw_CollectionPeriods] WHERE [Collection_Open] = 1)
GO
