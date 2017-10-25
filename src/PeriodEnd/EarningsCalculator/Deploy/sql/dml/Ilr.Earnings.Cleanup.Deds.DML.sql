DELETE FROM ${ILR_Deds.FQ}.Rulebase.AEC_ApprenticeshipPriceEpisode
    WHERE Ukprn IN (SELECT Ukprn FROM [Valid].[LearningProvider])
GO

DELETE FROM ${ILR_Deds.FQ}.Rulebase.AEC_ApprenticeshipPriceEpisode_PeriodisedValues
    WHERE Ukprn IN (SELECT Ukprn FROM [Valid].[LearningProvider])
GO

DELETE FROM ${ILR_Deds.FQ}.Rulebase.AEC_ApprenticeshipPriceEpisode_Period
    WHERE Ukprn IN (SELECT Ukprn FROM [Valid].[LearningProvider])
GO
