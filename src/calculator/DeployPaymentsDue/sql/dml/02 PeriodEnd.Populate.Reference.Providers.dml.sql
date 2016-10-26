TRUNCATE TABLE [Reference].[Providers]
GO

INSERT INTO [Reference].[Providers]
    SELECT
        [UKPRN] AS [Ukprn]
	FROM ${ILR_Deds.FQ}.[Valid].[LearningProvider]
GO
