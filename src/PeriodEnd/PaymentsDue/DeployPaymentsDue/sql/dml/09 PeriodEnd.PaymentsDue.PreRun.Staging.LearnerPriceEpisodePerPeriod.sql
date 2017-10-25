TRUNCATE TABLE Staging.LearnerPriceEpisodePerPeriod
GO

CREATE TABLE #MaxStartDateForEpisodes
(
	Ukprn bigint,
	LearnRefNumber varchar(12),
	AimSeqNumber int,
	Period int,
	MaxEpisodeStartDate date
)
CREATE TABLE #MaxStartDateForEpisodesInPeriod
(
	Ukprn bigint,
	LearnRefNumber varchar(12),
	AimSeqNumber int,
	Period int,
	MaxEpisodeStartDate date
)

INSERT INTO #MaxStartDateForEpisodes
SELECT 
	ae1.Ukprn,
    ae1.LearnRefNumber,
    ae1.AimSeqNumber,
    ae1.Period,
    MAX(ae1.EpisodeStartDate) MaxEpisodeStartDate
FROM Reference.ApprenticeshipEarnings ae1
JOIN Staging.CollectionPeriods cp
    ON ae1.Period = cp.PeriodNumber
GROUP BY 
	ae1.Ukprn,
    ae1.LearnRefNumber,
    ae1.AimSeqNumber,
    ae1.Period

INSERT INTO #MaxStartDateForEpisodesInPeriod
SELECT 
	ae1.Ukprn,
    ae1.LearnRefNumber,
    ae1.AimSeqNumber,
    ae1.Period,
    MAX(ae1.EpisodeStartDate) MaxEpisodeStartDate
FROM Reference.ApprenticeshipEarnings ae1
JOIN Staging.CollectionPeriods cp
    ON ae1.Period = cp.PeriodNumber
WHERE ae1.EpisodeStartDate < DATEADD(MM, 1, DATEFROMPARTS(cp.CalendarYear, cp.CalendarMonth, 1))
AND ae1.PriceEpisodeEndDate >= DATEFROMPARTS(cp.CalendarYear, cp.CalendarMonth, 1)
GROUP BY 
	ae1.Ukprn,
    ae1.LearnRefNumber,
    ae1.AimSeqNumber,
    ae1.Period

INSERT INTO Staging.LearnerPriceEpisodePerPeriod
SELECT
	x.Ukprn,
    x.LearnRefNumber, 
    x.AimSeqNumber, 
    x.Period, 
    COALESCE(y.MaxEpisodeStartDate, x.MaxEpisodeStartDate) MaxEpisodeStartDate
FROM #MaxStartDateForEpisodes x
LEFT JOIN #MaxStartDateForEpisodesInPeriod y
	ON x.Ukprn = y.Ukprn
	AND x.LearnRefNumber = y.LearnRefNumber
	AND x.AimSeqNumber = y.AimSeqNumber
	AND x.Period = y.Period	

DROP TABLE #MaxStartDateForEpisodes
DROP TABLE #MaxStartDateForEpisodesInPeriod