DECLARE @CollectionPeriodName varchar(8) = (SELECT [AcademicYear] FROM Reference.CollectionPeriods WHERE [Open] = 1) + '-' + (SELECT [Name] FROM Reference.CollectionPeriods WHERE [Open] = 1)

DELETE FROM ${DAS_PeriodEnd.FQ}.[Payments].[Payments]
    WHERE [RequiredPaymentId] IN (
		SELECT DISTINCT [Id] FROM [PaymentsDue].[RequiredPayments] WHERE [Ukprn] IN (
			SELECT DISTINCT [Ukprn] FROM [Reference].[Providers]
			)
		)
    AND [CollectionPeriodName] = @CollectionPeriodName
    AND [FundingSource] IN (2, 3, 4)
GO
