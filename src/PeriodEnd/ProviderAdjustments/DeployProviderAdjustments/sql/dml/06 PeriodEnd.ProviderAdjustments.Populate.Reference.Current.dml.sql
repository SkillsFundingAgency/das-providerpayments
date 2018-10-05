TRUNCATE TABLE [Reference].[ProviderAdjustmentsCurrent]
GO

INSERT INTO [Reference].[ProviderAdjustmentsCurrent] (
    [Ukprn],
    [SubmissionId],
    [SubmissionCollectionPeriod],
    [PaymentType],
    [PaymentTypeName],
    [Amount]
    )
SELECT 
	S.Ukprn,
	S.Submission_Id [SubmissionId],
	S.CollectionPeriod [SubmissionCollectionPeriod],
	V.Payment_Id [PaymentType],
	P.PaymentName [PaymentTypeName],
	V.PaymentValue [Amount]
	
FROM ${EAS_Deds.FQ}.dbo.EAS_Submission_Values V
JOIN ${EAS_Deds.FQ}.dbo.EAS_Submission S
	ON V.Submission_Id = S.Submission_Id
	AND V.CollectionPeriod = S.CollectionPeriod
JOIN ${EAS_Deds.FQ}.dbo.Payment_Types P
	ON V.Payment_Id = P.Payment_Id
WHERE P.FM36 = 1
