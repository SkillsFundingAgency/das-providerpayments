DELETE FROM ${ILR_Deds.FQ}.[DataLock].[ValidationError]
    WHERE [Ukprn] IN (SELECT DISTINCT lp.[Ukprn] FROM [Valid].[LearningProvider] lp)
GO

DELETE FROM ${ILR_Deds.FQ}.[DataLock].[PriceEpisodeMatch]
    WHERE [Ukprn] IN (SELECT DISTINCT lp.[Ukprn] FROM [Valid].[LearningProvider] lp)
GO

DELETE FROM ${ILR_Deds.FQ}.[DataLock].[PriceEpisodePeriodMatch]
    WHERE [Ukprn] IN (SELECT DISTINCT lp.[Ukprn] FROM [Valid].[LearningProvider] lp)
GO