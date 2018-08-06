TRUNCATE TABLE [Reference].[ApprenticeshipPriceEpisode_Period]
GO

INSERT INTO [Reference].[ApprenticeshipPriceEpisode_Period] (
	[Ukprn] ,
	[LearnRefNumber] ,
	[PriceEpisodeIdentifier] ,
	[Period] ,
	[PriceEpisodeFirstEmp1618Pay],
	[PriceEpisodeSecondEmp1618Pay]
	)
SELECT 
	[Ukprn],
    [LearnRefNumber],
    [PriceEpisodeIdentifier],
    [Period],
    [PriceEpisodeFirstEmp1618Pay],
    [PriceEpisodeSecondEmp1618Pay]
FROM 
	OPENQUERY(${DS_SILR1718_Collection.servername}, '
	SELECT 
		ape.[Ukprn],
		ape.[LearnRefNumber],
		ape.[PriceEpisodeIdentifier],
		[Period],
		p.[PriceEpisodeFirstEmp1618Pay],
		p.[PriceEpisodeSecondEmp1618Pay],
		ape.PriceEpisodeContractType
	FROM
		${DS_SILR1718_Collection.databasename}.[Rulebase].[AEC_ApprenticeshipPriceEpisode] ape
		INNER JOIN ${DS_SILR1718_Collection.databasename}.[Rulebase].[AEC_ApprenticeshipPriceEpisode_Period] p 
			ON p.Ukprn = ape.Ukprn
			AND p.LearnRefNumber = ape.LearnRefNumber
			AND p.PriceEpisodeIdentifier = ape.PriceEpisodeIdentifier
	WHERE
		ape.PriceEpisodeContractType = ''Levy Contract''
		AND (IsNull(p.PriceEpisodeFirstEmp1618Pay, 0) <> 0
			OR IsNull(p.PriceEpisodeSecondEmp1618Pay, 0) <> 0)') as ape
WHERE 
    UKPRN IN (SELECT DISTINCT [Ukprn] FROM [Reference].[Providers])
