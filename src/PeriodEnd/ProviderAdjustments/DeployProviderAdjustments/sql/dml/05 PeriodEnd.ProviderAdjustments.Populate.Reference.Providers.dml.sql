TRUNCATE TABLE [Reference].[ProviderAdjustmentsProviders]
GO

INSERT INTO [Reference].[ProviderAdjustmentsProviders] WITH (TABLOCKX) (
        [Ukprn]
    )
    SELECT
        DISTINCT Ukprn
    FROM ${DAS_PeriodEnd.FQ}.dbo.EAS_Submission
GO
