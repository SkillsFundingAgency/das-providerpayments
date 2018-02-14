TRUNCATE TABLE [Reference].[ProviderAdjustmentsCurrent]
GO

DECLARE @AlreadyProcessedSubmissions TABLE (SubmissionId UNIQUEIDENTIFIER)

INSERT INTO @AlreadyProcessedSubmissions (SubmissionId)
SELECT DISTINCT SubmissionId
FROM ${DAS_PeriodEnd.FQ}.ProviderAdjustments.Payments

INSERT INTO [Reference].[ProviderAdjustmentsCurrent] (
    [Ukprn],
    [SubmissionId],
    [SubmissionCollectionPeriod],
    [PaymentType],
    [PaymentTypeName],
    [Amount]
    )
SELECT 
	s.Ukprn,
    Submission_Id,
    CollectionPeriod,
    Payment_Id,
    PaymentName,
    PaymentValue
FROM 
	OPENQUERY(${DS_EAS1718_Collection.servername}, '
	SELECT
		s.Submission_Id,
		s.CollectionPeriod,
		p.Payment_Id,
		p.PaymentName,
		sv.PaymentValue,
		s.Ukprn
	FROM 
		${DS_EAS1718_Collection.databasename}.dbo.EAS_Submission s
		INNER JOIN ${DS_EAS1718_Collection.databasename}.dbo.EAS_Submission_Values sv ON s.Submission_Id = sv.Submission_Id
			AND s.CollectionPeriod = sv.CollectionPeriod
		INNER JOIN ${DS_EAS1718_Collection.databasename}.dbo.Payment_Types p ON sv.Payment_Id = p.Payment_Id
	WHERE
		p.FM36 = 1') as s
WHERE 
	s.Ukprn IN (SELECT Ukprn FROM Reference.ProviderAdjustmentsProviders)
    AND s.Submission_Id NOT IN (SELECT SubmissionId FROM @AlreadyProcessedSubmissions)
