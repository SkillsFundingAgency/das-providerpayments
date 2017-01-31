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
		ldf.[LearnDelFAMCode],
		pv.[PriceEpisodeFundLineType],
		pv.[PriceEpisodeSFAContribPct]
	FROM ${ILR_Deds.FQ}.[Rulebase].[AEC_ApprenticeshipPriceEpisode] pe
		JOIN ${ILR_Deds.FQ}.[Rulebase].[AEC_ApprenticeshipPriceEpisode_Period] pv ON pe.[Ukprn] = pv.[Ukprn]
			AND pe.[LearnRefNumber] = pv.[LearnRefNumber]
			AND pe.[PriceEpisodeIdentifier] = pv.[PriceEpisodeIdentifier]
		JOIN ${ILR_Deds.FQ}.[Valid].[Learner] l ON l.[Ukprn] = pe.[Ukprn]
			AND l.[LearnRefNumber] = pe.[LearnRefNumber]
		JOIN ${ILR_Deds.FQ}.[Valid].[LearningDelivery] ld ON pe.[Ukprn] = ld.[Ukprn]
			AND pe.[LearnRefNumber] = ld.[LearnRefNumber]
			AND pe.[PriceEpisodeAimSeqNumber] = ld.[AimSeqNumber]
		JOIN ${ILR_Deds.FQ}.[Valid].[LearningDeliveryFAM] ldf ON pe.[Ukprn] = ldf.[Ukprn]
			AND pe.[LearnRefNumber] = ldf.[LearnRefNumber]
			AND pe.[PriceEpisodeAimSeqNumber] = ldf.[AimSeqNumber]
	WHERE pe.[Ukprn] IN (SELECT DISTINCT [Ukprn] FROM [Reference].[Providers])
		AND ldf.[LearnDelFAMType] = 'ACT'
		AND ldf.[LearnDelFAMCode] IN ('1', '2')
        AND ldf.[LearnDelFAMDateFrom] <= pe.[EpisodeEffectiveTNPStartDate]
        AND ldf.[LearnDelFAMDateTo] >= COALESCE(pe.[PriceEpisodeActualEndDate], pe.[PriceEpisodePlannedEndDate])
GO
