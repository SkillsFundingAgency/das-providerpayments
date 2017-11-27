TRUNCATE TABLE [Reference].[DataLockPriceEpisode]
GO

INSERT INTO [Reference].[DataLockPriceEpisode] (
		[Ukprn],
		[LearnRefNumber],
		[Uln],
		[NiNumber],
		[AimSeqNumber],
		[StandardCode],
		[ProgrammeType],
		[FrameworkCode],
		[PathwayCode],
		[StartDate],
		[NegotiatedPrice],
		[PriceEpisodeIdentifier],
		[EndDate],
		[PriceEpisodeFirstAdditionalPaymentThresholdDate],
		[PriceEpisodeSecondAdditionalPaymentThresholdDate],
		[Tnp1],
		[Tnp2],
		[Tnp3],
		[Tnp4],
		[LearningStartDate],
		[EffectiveToDate]
	)
	SELECT
		l.[Ukprn],
		l.[LearnRefNumber] AS [LearnRefNumber],
		l.[ULN] AS [Uln],
		l.[NINumber] AS [NiNumber],
		ld.[AimSeqNumber] AS [AimSeqNumber],
		ld.[StdCode] AS [StandardCode],
		ld.[ProgType] AS [ProgrammeType],
		ld.[FworkCode] AS [FrameworkCode],
		ld.[PwayCode] AS [PathwayCode],
		ape.[EpisodeEffectiveTNPStartDate] AS [StartDate],
		ape.[PriceEpisodeTotalTNPPrice] AS [NegotiatedPrice],
		ape.[PriceEpisodeIdentifier],
		COALESCE(ape.[PriceEpisodeActualEndDate], ape.[PriceEpisodePlannedEndDate]) AS [EndDate],
		ape.[PriceEpisodeFirstAdditionalPaymentThresholdDate] AS FirstAdditionalPaymentThresholdDate,
		ape.[PriceEpisodeSecondAdditionalPaymentThresholdDate] as SecondAdditionalPaymentThresholdDate,
		ape.[TNP1],
		ape.[TNP2],
		ape.[TNP3],
		ape.[TNP4],
		ld.LearnStartDate AS LearningStartDate,
		et.EffectiveTo
	FROM ${ILR_Deds.FQ}.[Rulebase].[AEC_ApprenticeshipPriceEpisode] ape
		JOIN ${ILR_Deds.FQ}.[Valid].[Learner] l ON ape.[Ukprn] = l.[Ukprn]
			AND ape.[LearnRefNumber] = l.[LearnRefNumber]
		JOIN ${ILR_Deds.FQ}.[Valid].[LearningDelivery] ld ON ape.[Ukprn] = ld.[Ukprn]
			AND ape.[LearnRefNumber] = ld.[LearnRefNumber]
			AND ape.[PriceEpisodeAimSeqNumber] = ld.[AimSeqNumber]
		LEFT JOIN (
			SELECT x.Ukprn, x.PriceEpisodeIdentifier, x.LearnRefNumber, x.PriceEpisodeAimSeqNumber, DATEADD(DD,-1,MIN(y.EpisodeEffectiveTNPStartDate)) EffectiveTo
			FROM ${ILR_Deds.FQ}.[Rulebase].[AEC_ApprenticeshipPriceEpisode] x
			LEFT OUTER JOIN ${ILR_Deds.FQ}.[Rulebase].[AEC_ApprenticeshipPriceEpisode] y
				ON x.LearnRefNumber = y.LearnRefNumber
				AND y.EpisodeEffectiveTNPStartDate > x.EpisodeEffectiveTNPStartDate
			GROUP BY x.ukprn, x.PriceEpisodeIdentifier,x.LearnRefNumber, x.PriceEpisodeAimSeqNumber, x.EpisodeEffectiveTNPStartDate
		) et
			ON ape.Ukprn = et.Ukprn
			AND ape.PriceEpisodeIdentifier = et.PriceEpisodeIdentifier
			AND ape.LearnRefNumber = et.LearnRefNumber
            AND ape.PriceEpisodeAimSeqNumber = et.PriceEpisodeAimSeqNumber
	WHERE ape.PriceEpisodeContractType = 'Levy Contract'
