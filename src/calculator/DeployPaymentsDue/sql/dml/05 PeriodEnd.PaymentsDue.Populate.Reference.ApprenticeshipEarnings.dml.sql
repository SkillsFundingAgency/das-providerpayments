TRUNCATE TABLE [Reference].[ApprenticeshipEarnings]
GO

INSERT INTO [Reference].[ApprenticeshipEarnings]
	SELECT
		pe.[Ukprn],
		l.[Uln],
		pe.[LearnRefNumber],
		pe.[PriceEpisodeAimSeqNumber],
		pe.[PriceEpisodeIdentifier],
		pv.[AttributeName],
		pv.Period_1,
		pv.Period_2,
		pv.Period_3,
		pv.Period_4,
		pv.Period_5,
		pv.Period_6,
		pv.Period_7,
		pv.Period_8,
		pv.Period_9,
		pv.Period_10,
		pv.Period_11,
		pv.Period_12,
		ld.[StdCode],
		ld.[ProgType],
		ld.[FworkCode],
		ld.[PwayCode],
		ldf.[LearnDelFAMCode]
	FROM ${ILR_Deds.FQ}.[Rulebase].[AEC_ApprenticeshipPriceEpisode] pe
		JOIN ${ILR_Deds.FQ}.[Rulebase].[AEC_ApprenticeshipPriceEpisode_PeriodisedValues] pv ON pe.[Ukprn] = pv.[Ukprn]
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
		AND pv.[AttributeName] IN ('PriceEpisodeOnProgPayment', 'PriceEpisodeCompletionPayment', 'PriceEpisodeBalancePayment')
		AND ldf.[LearnDelFAMType] = 'ACT'
        AND ldf.[LearnDelFAMDateFrom] <= pe.[EpisodeEffectiveTNPStartDate]
        AND ldf.[LearnDelFAMDateTo] >= COALESCE(pe.[PriceEpisodeActualEndDate], pe.[PriceEpisodePlannedEndDate])
GO
