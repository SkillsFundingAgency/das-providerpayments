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
		ISNULL(pv.Period_1, 0),
		ISNULL(pv.Period_2, 0),
		ISNULL(pv.Period_3, 0),
		ISNULL(pv.Period_4, 0),
		ISNULL(pv.Period_5, 0),
		ISNULL(pv.Period_6, 0),
		ISNULL(pv.Period_7, 0),
		ISNULL(pv.Period_8, 0),
		ISNULL(pv.Period_9, 0),
		ISNULL(pv.Period_10, 0),
		ISNULL(pv.Period_11, 0),
		ISNULL(pv.Period_12, 0),
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
		AND pv.[AttributeName] IN ('PriceEpisodeOnProgPayment', 'PriceEpisodeCompletionPayment', 'PriceEpisodeBalancePayment',
		'PriceEpisodeFirstEmp1618Pay','PriceEpisodeFirstProv1618Pay','PriceEpisodeSecondEmp1618Pay','PriceEpisodeSecondProv1618Pay')
		AND ldf.[LearnDelFAMType] = 'ACT'
		AND ldf.[LearnDelFAMCode] IN ('1', '2')
        AND ldf.[LearnDelFAMDateFrom] <= pe.[EpisodeEffectiveTNPStartDate]
        AND ldf.[LearnDelFAMDateTo] >= COALESCE(pe.[PriceEpisodeActualEndDate], pe.[PriceEpisodePlannedEndDate])
GO
