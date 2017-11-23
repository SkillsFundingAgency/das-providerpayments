TRUNCATE TABLE [Reference].[ProviderAdjustmentsCurrent]
GO

DECLARE @AlreadyProcessedSubmissions TABLE (SubmissionId uniqueidentifier)
INSERT INTO @AlreadyProcessedSubmissions (
        SubmissionId
    )
    SELECT
        DISTINCT SubmissionId
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
        s.Submission_Id,
        s.CollectionPeriod,
        p.Payment_Id,
        p.PaymentName,
        sv.PaymentValue
    FROM ${DAS_PeriodEnd.FQ}.dbo.EAS_Submission s
        JOIN ${DAS_PeriodEnd.FQ}.dbo.EAS_Submission_Values sv ON s.Submission_Id = sv.Submission_Id
            AND s.CollectionPeriod = sv.CollectionPeriod
        JOIN ${DAS_PeriodEnd.FQ}.dbo.Payment_Types p ON sv.Payment_Id = p.Payment_Id
    WHERE s.Ukprn IN (SELECT DISTINCT Ukprn FROM Reference.ProviderAdjustmentsProviders)
        AND s.Submission_Id NOT IN (SELECT SubmissionId FROM @AlreadyProcessedSubmissions)
        AND p.FM36 = 1
GO
