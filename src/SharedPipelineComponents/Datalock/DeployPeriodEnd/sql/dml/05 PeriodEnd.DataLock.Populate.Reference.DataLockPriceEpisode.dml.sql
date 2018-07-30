IF EXISTS (SELECT * FROM sys.indexes i
JOIN sys.objects t ON i.object_id = t.object_id
WHERE t.name = 'DataLockPriceEpisode'
AND i.name = 'IDX_DataLockPriceEpisode_Ukprn')
BEGIN
	DROP INDEX IDX_DataLockPriceEpisode_Ukprn ON Reference.DataLockPriceEpisode
END
GO


IF EXISTS (SELECT * FROM sys.indexes i
JOIN sys.objects t ON i.object_id = t.object_id
WHERE t.name = 'DataLockPriceEpisode'
AND i.name = 'IX_DataLockPriceEpisode_DataLockEvents')
BEGIN
	DROP INDEX IX_DataLockPriceEpisode_DataLockEvents ON Reference.DataLockPriceEpisode
END
GO


IF EXISTS (SELECT NULL FROM sys.indexes WHERE name='IX_DataLockPriceEpisode_Uln')
BEGIN
	DROP INDEX IX_DataLockPriceEpisode_Uln ON Reference.DataLockPriceEpisode
END

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

GO

CREATE CLUSTERED INDEX [IDX_DataLockPriceEpisode_Ukprn] ON [Reference].[DataLockPriceEpisode]
(
	[Ukprn] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO

CREATE INDEX IX_DataLockPriceEpisode_DataLockEvents ON Reference.DataLockPriceEpisode (UKPRN, LearnRefNumber, PriceEpisodeIdentifier)
GO

CREATE INDEX IX_DataLockPriceEpisode_Uln ON Reference.DataLockPriceEpisode (Uln)
