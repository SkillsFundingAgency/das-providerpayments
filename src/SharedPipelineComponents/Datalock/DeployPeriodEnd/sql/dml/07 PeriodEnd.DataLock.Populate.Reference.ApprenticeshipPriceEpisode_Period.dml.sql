TRUNCATE TABLE [Reference].[ApprenticeshipPriceEpisode_Period]
GO

INSERT INTO [Reference].[ApprenticeshipPriceEpisode_Period] (
	[Ukprn] ,
	[LearnRefNumber] ,
	[PriceEpisodeIdentifier] ,
	[Period] ,
	[PriceEpisodeApplic1618FrameworkUpliftBalancing] ,
	[PriceEpisodeApplic1618FrameworkUpliftCompletionPayment],
	[PriceEpisodeApplic1618FrameworkUpliftOnProgPayment],
	[PriceEpisodeBalancePayment],
	[PriceEpisodeBalanceValue],
	[PriceEpisodeCompletionPayment],
	[PriceEpisodeFirstDisadvantagePayment],
	[PriceEpisodeFirstEmp1618Pay],
	[PriceEpisodeFirstProv1618Pay],
	[PriceEpisodeFundLineType] ,
	[PriceEpisodeInstalmentsThisPeriod] ,
	[PriceEpisodeLSFCash],
	[PriceEpisodeOnProgPayment],
	[PriceEpisodeSecondDisadvantagePayment],
	[PriceEpisodeSecondEmp1618Pay],
	[PriceEpisodeSecondProv1618Pay]
	)
	SELECT
	[Ukprn] ,
	[LearnRefNumber] ,
	[PriceEpisodeIdentifier] ,
	[Period] ,
	[PriceEpisodeApplic1618FrameworkUpliftBalancing] ,
	[PriceEpisodeApplic1618FrameworkUpliftCompletionPayment],
	[PriceEpisodeApplic1618FrameworkUpliftOnProgPayment],
	[PriceEpisodeBalancePayment],
	[PriceEpisodeBalanceValue],
	[PriceEpisodeCompletionPayment],
	[PriceEpisodeFirstDisadvantagePayment],
	[PriceEpisodeFirstEmp1618Pay],
	[PriceEpisodeFirstProv1618Pay],
	[PriceEpisodeFundLineType] ,
	[PriceEpisodeInstalmentsThisPeriod] ,
	[PriceEpisodeLSFCash],
	[PriceEpisodeOnProgPayment],
	[PriceEpisodeSecondDisadvantagePayment],
	[PriceEpisodeSecondEmp1618Pay],
	[PriceEpisodeSecondProv1618Pay]

	FROM ${ILR_Deds.FQ}.[Rulebase].[AEC_ApprenticeshipPriceEpisode_Period]
		
	WHERE UKPRN IN (SELECT DISTINCT [Ukprn] FROM [Reference].[Providers])
