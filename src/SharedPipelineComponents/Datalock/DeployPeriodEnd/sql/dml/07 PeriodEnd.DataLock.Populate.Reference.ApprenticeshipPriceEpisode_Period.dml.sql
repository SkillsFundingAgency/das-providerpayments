TRUNCATE TABLE [Reference].[ApprenticeshipPriceEpisode_Period]
GO

INSERT INTO [Reference].[ApprenticeshipPriceEpisode_Period] (
	[Ukprn] ,
	[LearnRefNumber] ,
	[PriceEpisodeIdentifier] ,
	[Period] ,
	[PriceEpisodeFirstEmp1618Pay],
	[PriceEpisodeSecondEmp1618Pay]
	)
	SELECT
	ape.[Ukprn] ,
	ape.[LearnRefNumber] ,
	ape.[PriceEpisodeIdentifier] ,
	[Period] ,
	[PriceEpisodeFirstEmp1618Pay],
	[PriceEpisodeSecondEmp1618Pay]

	
	FROM ${ILR_Deds.FQ}.[Rulebase].[AEC_ApprenticeshipPriceEpisode] ape
	JOIN ${ILR_Deds.FQ}.[Rulebase].[AEC_ApprenticeshipPriceEpisode_Period] p
		On p.Ukprn = ape.Ukprn
		And p.LearnRefNumber = ape.LearnRefNumber 
		And p.PriceEpisodeIdentifier = ape.PriceEpisodeIdentifier

    WHERE ape.PriceEpisodeContractType = 'Levy Contract'
	And ape.UKPRN IN (SELECT DISTINCT [Ukprn] FROM [Reference].[Providers])
	And (IsNull(p.PriceEpisodeFirstEmp1618Pay,0) <> 0 OR IsNull(p.PriceEpisodeSecondEmp1618Pay,0) <> 0)
	
	
	 
