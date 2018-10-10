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
	Ukprn,
	SubmissionId,
	SubmissionCollectionPeriod,
	SubmissionAcademicYear,
	PaymentType,
	PaymentTypeName,
	Amount,
	CollectionPeriodName,
	CollectionPeriodMonth,
	CollectionPeriodYear
FROM ${DAS_PeriodEnd.FQ}.ProviderAdjustments.Payments
WHERE SubmissionAcademicYear = ${YearOfCollection}
GO
