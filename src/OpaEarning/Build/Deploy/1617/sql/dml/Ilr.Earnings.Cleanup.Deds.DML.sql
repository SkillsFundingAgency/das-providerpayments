DELETE FROM ${ILR_Deds.FQ}.[Rulebase].[AEC_Cases]
    WHERE Ukprn IN (SELECT Ukprn FROM [Valid].[LearningProvider])
GO
DELETE FROM ${ILR_Deds.FQ}.[Rulebase].[AEC_global]
    WHERE Ukprn IN (SELECT Ukprn FROM [Valid].[LearningProvider])
GO
DELETE FROM ${ILR_Deds.FQ}.[Rulebase].[AEC_LearningDelivery]
    WHERE Ukprn IN (SELECT Ukprn FROM [Valid].[LearningProvider])
GO
DELETE FROM ${ILR_Deds.FQ}.[Rulebase].[AEC_LearningDelivery_Period]
    WHERE Ukprn IN (SELECT Ukprn FROM [Valid].[LearningProvider])
GO
DELETE FROM ${ILR_Deds.FQ}.[Rulebase].[AEC_LearningDelivery_PeriodisedValues]
    WHERE Ukprn IN (SELECT Ukprn FROM [Valid].[LearningProvider])
GO
DELETE FROM ${ILR_Deds.FQ}.[Rulebase].[AEC_LearningDelivery_PeriodisedTextValues]
    WHERE Ukprn IN (SELECT Ukprn FROM [Valid].[LearningProvider])
GO
DELETE FROM ${ILR_Deds.FQ}.[Rulebase].[AEC_HistoricEarningOutput]
    WHERE Ukprn IN (SELECT Ukprn FROM [Valid].[LearningProvider])
GO
DELETE FROM ${ILR_Deds.FQ}.[Rulebase].[AEC_ApprenticeshipPriceEpisode]
    WHERE Ukprn IN (SELECT Ukprn FROM [Valid].[LearningProvider])
GO
DELETE FROM ${ILR_Deds.FQ}.[Rulebase].[AEC_ApprenticeshipPriceEpisode_Period]
    WHERE Ukprn IN (SELECT Ukprn FROM [Valid].[LearningProvider])
GO
DELETE FROM ${ILR_Deds.FQ}.[Rulebase].[AEC_ApprenticeshipPriceEpisode_PeriodisedValues]
    WHERE Ukprn IN (SELECT Ukprn FROM [Valid].[LearningProvider])
GO
DELETE FROM ${ILR_Deds.FQ}.dbo.AEC_EarningHistory
    WHERE Ukprn IN (SELECT Ukprn FROM Rulebase.vw_AEC_EarningHistory)
        AND CollectionYear IN (SELECT CollectionYear FROM Rulebase.vw_AEC_EarningHistory)
        AND CollectionReturnCode IN (SELECT CollectionReturnCode FROM Rulebase.vw_AEC_EarningHistory)
GO
UPDATE ${ILR_Deds.FQ}.dbo.AEC_EarningHistory
        SET LatestInYear = 0
    WHERE CollectionYear IN (SELECT CollectionYear FROM Rulebase.vw_AEC_EarningHistory)
        AND Ukprn IN (SELECT Ukprn FROM [Valid].[LearningProvider])
GO
