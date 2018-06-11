DELETE FROM ${DAS_PeriodEnd.FQ}.[PaymentsDue].[Earnings] E
	WHERE EXISTS (
		SELECT Id FROM PaymentsDue.RequiredPayments R
		WHERE [Ukprn] IN (SELECT DISTINCT [Ukprn] FROM [Reference].[Providers])
        AND [CollectionPeriodName] IN (SELECT '${YearOfCollection}-' + [Collection_Period] FROM [PaymentsDue].[vw_CollectionPeriods] WHERE [Collection_Open] = 1)
		AND E.RequiredPaymentId = R.Id
    )
GO

DELETE FROM ${DAS_PeriodEnd.FQ}.[PaymentsDue].[RequiredPayments]
    WHERE [Ukprn] IN (SELECT DISTINCT [Ukprn] FROM [Reference].[Providers])
    AND [CollectionPeriodName] IN (SELECT '${YearOfCollection}-' + [Collection_Period] FROM [PaymentsDue].[vw_CollectionPeriods] WHERE [Collection_Open] = 1)
GO

DELETE FROM ${DAS_PeriodEnd.FQ}.[PaymentsDue].[NonPayableEarnings]
    WHERE [Ukprn] IN (SELECT DISTINCT [Ukprn] FROM [Reference].[Providers])
    AND [CollectionPeriodName] IN (SELECT '${YearOfCollection}-' + [Collection_Period] FROM [PaymentsDue].[vw_CollectionPeriods] WHERE [Collection_Open] = 1)
GO