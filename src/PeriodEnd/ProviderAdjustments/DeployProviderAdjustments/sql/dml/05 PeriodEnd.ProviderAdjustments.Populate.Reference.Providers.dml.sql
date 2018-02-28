TRUNCATE TABLE [Reference].[ProviderAdjustmentsProviders]
GO

INSERT INTO [Reference].[ProviderAdjustmentsProviders] WITH (TABLOCKX) (
        [Ukprn]
    )
    SELECT
        DISTINCT Ukprn
    FROM ${EAS_Deds.FQ}.dbo.EAS_Submission
GO
