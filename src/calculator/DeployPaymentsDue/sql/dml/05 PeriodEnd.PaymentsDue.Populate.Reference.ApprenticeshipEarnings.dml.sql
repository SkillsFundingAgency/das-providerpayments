TRUNCATE TABLE [Reference].[ApprenticeshipEarnings]
GO

INSERT INTO [Reference].[ApprenticeshipEarnings]
	SELECT
		pe.[Ukprn],
		l.[Uln],
		pe.[LearnRefNumber],
		pe.[AimSeqNumber],
		pe.[EpisodeStartDate],
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
		pv.Period_12
	FROM ${ILR_Deds.FQ}.[Rulebase].[AEC_ApprenticeshipPriceEpisode] pe
		JOIN ${ILR_Deds.FQ}.[Rulebase].[AEC_ApprenticeshipPriceEpisode_PeriodisedValues] pv ON pe.[Ukprn] = pv.[Ukprn]
			AND pe.[LearnRefNumber] = pv.[LearnRefNumber]
			AND pe.[EpisodeStartDate] = pv.[EpisodeStartDate]
			AND pe.[PriceEpisodeIdentifier] = pv.[PriceEpisodeIdentifier]
		JOIN ${ILR_Deds.FQ}.[Valid].[Learner] l ON l.[Ukprn] = pe.[Ukprn]
			AND l.[LearnRefNumber] = pe.[LearnRefNumber]
	WHERE pe.[Ukprn] IN (SELECT DISTINCT [Ukprn] FROM [Reference].[Providers])
		AND pv.[AttributeName] IN ('PriceEpisodeOnProgPayment', 'PriceEpisodeCompletionPayment', 'PriceEpisodeBalancePayment')
GO
