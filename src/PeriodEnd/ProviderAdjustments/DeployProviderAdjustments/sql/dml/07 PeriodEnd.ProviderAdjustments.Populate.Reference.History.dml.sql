TRUNCATE TABLE [Reference].[ProviderAdjustmentsHistory]
GO

INSERT INTO [Reference].[ProviderAdjustmentsHistory] (
        [Ukprn],
        [SubmissionId],
        [SubmissionCollectionPeriod],
        [SubmissionAcademicYear],
        [PaymentType],
        [PaymentTypeName],
        [Amount],
        [CollectionPeriodName],
        [CollectionPeriodMonth],
        [CollectionPeriodYear]
    )
SELECT 
	[Ukprn],
    [SubmissionId],
    [SubmissionCollectionPeriod],
    [SubmissionAcademicYear],
    [PaymentType],
    [PaymentTypeName],
    [Amount],
    [CollectionPeriodName],
    [CollectionPeriodMonth],
    [CollectionPeriodYear]
FROM OPENQUERY(${DAS_PeriodEnd.servername}, '
		SELECT
			[Ukprn],
			[SubmissionId],
			[SubmissionCollectionPeriod],
			[SubmissionAcademicYear],
			[PaymentType],
			[PaymentTypeName],
			[Amount],
			[CollectionPeriodName],
			[CollectionPeriodMonth],
			[CollectionPeriodYear]
		FROM 
			${DAS_PeriodEnd.databasename}.ProviderAdjustments.Payments
		WHERE
			SubmissionAcademicYear = ${YearOfCollection}'
    ) AS r
WHERE Ukprn IN (
        SELECT Ukprn
        FROM Reference.ProviderAdjustmentsProviders
      )      
GO
