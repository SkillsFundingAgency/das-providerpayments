TRUNCATE TABLE [Reference].[ProviderAdjustmentsCurrent]
GO

SELECT 
	Ukprn,
    Submission_Id,
    CollectionPeriod,
    Payment_Id,
    PaymentName,
    PaymentValue
FROM OPENQUERY(${DAS_PeriodEnd.servername}, '
	SELECT 
		s.Ukprn,
		s.Submission_Id,
		s.CollectionPeriod,
		p.Payment_Id,
		p.PaymentName,
		sv.PaymentValue,
		p.FM36
	FROM 
		${DAS_PeriodEnd.databasename}.dbo.EAS_Submission s
		INNER JOIN ${DAS_PeriodEnd.databasename}.dbo.EAS_Submission_Values sv ON s.Submission_Id = sv.Submission_Id
			AND s.CollectionPeriod = sv.CollectionPeriod
		INNER JOIN ${DAS_PeriodEnd.databasename}.dbo.Payment_Types p ON sv.Payment_Id = p.Payment_Id
	WHERE
		s.Submission_Id NOT IN (
			SELECT SubmissionId 
			FROM ${DAS_PeriodEnd.databasename}.ProviderAdjustments.Payments
		)
		AND p.FM36 = 1
		') as s
WHERE 
	s.Ukprn IN (SELECT Ukprn FROM Reference.ProviderAdjustmentsProviders)

GO
