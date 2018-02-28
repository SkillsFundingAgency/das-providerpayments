TRUNCATE TABLE Staging.CollectionPeriods
GO

INSERT INTO Staging.CollectionPeriods WITH (TABLOCKX)
SELECT
	Id,
	Name,
	CalendarMonth,
	CalendarYear,
	[Open],
	CAST(RIGHT(Name,2) as int) AS PeriodNumber,
	CASE
		WHEN  [Name] = 'R01' OR [Name] = 'R02' OR [Name] = 'R03' OR [Name] = 'R04' OR [Name] = 'R05' THEN DATEFROMPARTS(CalendarYear,8,1)
		ELSE DATEFROMPARTS(CalendarYear - 1,8,1)
	END AS FirstDayOfAcademicYear
FROM Reference.CollectionPeriods
ORDER BY
	Id