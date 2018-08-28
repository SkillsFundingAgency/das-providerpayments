-- ValidationError
INSERT INTO ${ILR_Deds.FQ}.[DataLock].[ValidationError] (
	[Ukprn], 
	[LearnRefNumber], 
	[AimSeqNumber], 
	[RuleId],
    [CollectionPeriodName],
    [CollectionPeriodMonth],
    [CollectionPeriodYear],
	[PriceEpisodeIdentifier]
) VALUES (
	(SELECT TOP 1 [UKPRN] FROM [Valid].[LearningProvider]), 
	'Lrn001', 
	1, 
	'DLOCK_02',
	(SELECT TOP 1 '1617-' + [Name] FROM Reference.CollectionPeriods WHERE [Open] = 1),
	(SELECT TOP 1 [CalendarMonth] FROM Reference.CollectionPeriods WHERE [Open] = 1),
	(SELECT TOP 1 [CalendarYear] FROM Reference.CollectionPeriods WHERE [Open] = 1),
	'27-25-2016-09-01'
)
GO

INSERT INTO ${ILR_Deds.FQ}.[DataLock].[ValidationError] (
	[Ukprn], 
	[LearnRefNumber], 
	[AimSeqNumber], 
	[RuleId],
    [CollectionPeriodName],
    [CollectionPeriodMonth],
    [CollectionPeriodYear],
	[PriceEpisodeIdentifier]
) VALUES (
	(SELECT TOP 1 [UKPRN] FROM [Valid].[LearningProvider]), 
	'Lrn002', 
	1, 
	'DLOCK_07',
	(SELECT TOP 1 '1617-' + [Name] FROM Reference.CollectionPeriods WHERE [Open] = 1),
	(SELECT TOP 1 [CalendarMonth] FROM Reference.CollectionPeriods WHERE [Open] = 1),
	(SELECT TOP 1 [CalendarYear] FROM Reference.CollectionPeriods WHERE [Open] = 1),
	'27-25-2016-09-01'
)
GO

INSERT INTO ${ILR_Deds.FQ}.[DataLock].[ValidationError] (
	[Ukprn], 
	[LearnRefNumber], 
	[AimSeqNumber], 
	[RuleId],
    [CollectionPeriodName],
    [CollectionPeriodMonth],
    [CollectionPeriodYear],
	[PriceEpisodeIdentifier]
) VALUES (
	(SELECT TOP 1 [UKPRN] FROM [Valid].[LearningProvider]), 
	'Lrn003', 
	1, 
	'DLOCK_03',
	(SELECT TOP 1 '1617-' + [Name] FROM Reference.CollectionPeriods WHERE [Open] = 1),
	(SELECT TOP 1 [CalendarMonth] FROM Reference.CollectionPeriods WHERE [Open] = 1),
	(SELECT TOP 1 [CalendarYear] FROM Reference.CollectionPeriods WHERE [Open] = 1),
	'27-25-2016-09-01'
)
GO

-- PriceEpisodeMatch
INSERT INTO ${ILR_Deds.FQ}.[DataLock].[PriceEpisodeMatch] (
	[Ukprn], 
	[LearnRefNumber], 
	[AimSeqNumber], 
	[CommitmentId],
    [CollectionPeriodName],
    [CollectionPeriodMonth],
    [CollectionPeriodYear],
	[PriceEpisodeIdentifier],
	[IsSuccess]
) VALUES (
	(SELECT TOP 1 [UKPRN] FROM [Valid].[LearningProvider]), 
	'Lrn099', 
	1, 
	1,
	(SELECT TOP 1 '1617-' + [Name] FROM Reference.CollectionPeriods WHERE [Open] = 1),
	(SELECT TOP 1 [CalendarMonth] FROM Reference.CollectionPeriods WHERE [Open] = 1),
	(SELECT TOP 1 [CalendarYear] FROM Reference.CollectionPeriods WHERE [Open] = 1),
	'27-25-2016-09-01',
	1
)
GO

INSERT INTO ${ILR_Deds.FQ}.[DataLock].[PriceEpisodeMatch] (
	[Ukprn], 
	[LearnRefNumber], 
	[AimSeqNumber], 
	[CommitmentId],
    [CollectionPeriodName],
    [CollectionPeriodMonth],
    [CollectionPeriodYear],
	[PriceEpisodeIdentifier],
	[IsSuccess]
) VALUES (
	(SELECT TOP 1 [UKPRN] FROM [Valid].[LearningProvider]), 
	'Lrn098', 
	1, 
	1,
	(SELECT TOP 1 '1617-' + [Name] FROM Reference.CollectionPeriods WHERE [Open] = 1),
	(SELECT TOP 1 [CalendarMonth] FROM Reference.CollectionPeriods WHERE [Open] = 1),
	(SELECT TOP 1 [CalendarYear] FROM Reference.CollectionPeriods WHERE [Open] = 1),
	'27-25-2016-09-01',
	1
)
GO

INSERT INTO ${ILR_Deds.FQ}.[DataLock].[PriceEpisodeMatch] (
	[Ukprn], 
	[LearnRefNumber], 
	[AimSeqNumber], 
	[CommitmentId],
    [CollectionPeriodName],
    [CollectionPeriodMonth],
    [CollectionPeriodYear],
	[PriceEpisodeIdentifier],
	[IsSuccess]
) VALUES (
	(SELECT TOP 1 [UKPRN] FROM [Valid].[LearningProvider]), 
	'Lrn099', 
	1, 
	1,
	(SELECT TOP 1 '1617-' + [Name] FROM Reference.CollectionPeriods WHERE [Open] = 1),
	(SELECT TOP 1 [CalendarMonth] FROM Reference.CollectionPeriods WHERE [Open] = 1),
	(SELECT TOP 1 [CalendarYear] FROM Reference.CollectionPeriods WHERE [Open] = 1),
	'27-25-2016-09-01',
	1
)
GO
