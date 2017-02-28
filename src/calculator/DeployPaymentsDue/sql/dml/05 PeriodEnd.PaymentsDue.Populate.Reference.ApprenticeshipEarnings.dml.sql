TRUNCATE TABLE [Reference].[ApprenticeshipEarnings]
GO

INSERT INTO [Reference].[ApprenticeshipEarnings]
	SELECT
		pe.[Ukprn],
		l.[Uln],
		pe.[LearnRefNumber],
		pe.[PriceEpisodeAimSeqNumber],
		pe.[PriceEpisodeIdentifier],
		pv.[Period],
		ISNULL(pv.[PriceEpisodeOnProgPayment], 0),
		ISNULL(pv.[PriceEpisodeCompletionPayment], 0),
		ISNULL(pv.[PriceEpisodeBalancePayment], 0),
		ISNULL(pv.[PriceEpisodeFirstEmp1618Pay], 0),
		ISNULL(pv.[PriceEpisodeFirstProv1618Pay], 0),
		ISNULL(pv.[PriceEpisodeSecondEmp1618Pay], 0),
		ISNULL(pv.[PriceEpisodeSecondProv1618Pay], 0),
		ld.[StdCode],
		ld.[ProgType],
		ld.[FworkCode],
		ld.[PwayCode],
		Case pe.[PriceEpisodeContractType] When 'Levy Contract' Then 1 Else 2 END,
		pe.[PriceEpisodeFundLineType],
		pv.[PriceEpisodeSFAContribPct],
		pv.[PriceEpisodeLevyNonPayInd],
		IsNull(pv.[PriceEpisodeApplic1618FrameworkUpliftBalancing],0),
        IsNull(pv.[PriceEpisodeApplic1618FrameworkUpliftCompletionPayment],0),
        IsNull(pv.[PriceEpisodeApplic1618FrameworkUpliftOnProgPayment],0),
		IsNull(pv.[PriceEpisodeFirstDisadvantagePayment],0),
		IsNull(pv.[PriceEpisodeSecondDisadvantagePayment],0)
	FROM ${ILR_Deds.FQ}.[Rulebase].[AEC_ApprenticeshipPriceEpisode] pe
		JOIN ${ILR_Deds.FQ}.[Rulebase].[AEC_ApprenticeshipPriceEpisode_Period] pv ON pe.[Ukprn] = pv.[Ukprn]
			AND pe.[LearnRefNumber] = pv.[LearnRefNumber]
			AND pe.[PriceEpisodeIdentifier] = pv.[PriceEpisodeIdentifier]
		JOIN ${ILR_Deds.FQ}.[Valid].[Learner] l ON l.[Ukprn] = pe.[Ukprn]
			AND l.[LearnRefNumber] = pe.[LearnRefNumber]
		JOIN ${ILR_Deds.FQ}.[Valid].[LearningDelivery] ld ON pe.[Ukprn] = ld.[Ukprn]
			AND pe.[LearnRefNumber] = ld.[LearnRefNumber]
			AND pe.[PriceEpisodeAimSeqNumber] = ld.[AimSeqNumber]
	WHERE pe.[Ukprn] IN (SELECT DISTINCT [Ukprn] FROM [Reference].[Providers])
		
GO
