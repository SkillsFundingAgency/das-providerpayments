
IF NOT EXISTS (SELECT NULL FROM sys.indexes WHERE [name] = 'IX_PriceEpisodePeriodMatch_2')
BEGIN
	CREATE NONCLUSTERED INDEX [IX_PriceEpisodePeriodMatch_2]
		ON [DataLock].[PriceEpisodePeriodMatch] ([Ukprn],[PriceEpisodeIdentifier],[LearnRefNumber],[VersionId])
		INCLUDE ([AimSeqNumber],[Period],[Payable],[TransactionType])
END
GO



IF EXISTS (SELECT * FROM sys.indexes i
JOIN sys.objects t ON i.object_id = t.object_id
WHERE t.name = 'DataLockEventsData'
AND i.name = 'IX_DataLockEventsData_Query')
BEGIN
	DROP INDEX IX_DataLockEventsData_Query ON DataLockEvents.DataLockEventsData
END
GO

TRUNCATE TABLE DataLockEvents.DataLockEventsData
GO

INSERT INTO DataLockEvents.DataLockEventsData
(
	Ukprn ,PriceEpisodeIdentifier ,LearnRefNumber ,AimSeqNumber ,CommitmentId ,IsSuccess ,
	IlrFilename,SubmittedTime,ULN,IlrStartDate,IlrStandardCode,IlrProgrammeType,IlrFrameworkCode,IlrPathwayCode,IlrTrainingPrice,IlrEndpointAssessorPrice,IlrPriceEffectiveFromDate,IlrPriceEffectiveToDate,
	CommitmentVersionId ,Period ,Payable,TransactionType,TransactionTypesFlag  ,EmployerAccountId ,CommitmentStartDate,CommitmentStandardCode ,CommitmentProgrammeType,
	CommitmentFrameworkCode,CommitmentPathwayCode ,CommitmentNegotiatedPrice ,CommitmentEffectiveDate ,RuleId 
)

SELECT
		pem.Ukprn,
		pem.PriceEpisodeIdentifier,
		pem.LearnRefNumber,
		pem.AimSeqNumber,
		pem.CommitmentId,
		pem.IsSuccess,

		ilr.IlrFilename,
		ilr.SubmittedTime,
		ilr.Uln,
		ilr.IlrStartDate,
		ilr.IlrStandardCode,
		ilr.IlrProgrammeType,
		ilr.IlrFrameworkCode,
		ilr.IlrPathwayCode,
		ilr.IlrTrainingPrice,
		ilr.IlrEndpointAssessorPrice,
		ilr.IlrPriceEffectiveFromDate,
		ilr.IlrPriceEffectiveToDate,

		pepm.VersionId CommitmentVersionId,
		pepm.Period,
		pepm.Payable,
		pepm.TransactionType,
		pepm.TransactionTypesFlag,

		c.AccountId  EmployerAccountId,
		c.StartDate CommitmentStartDate,
		c.StandardCode CommitmentStandardCode,
		c.ProgrammeType CommitmentProgrammeType,
		c.FrameworkCode CommitmentFrameworkCode,
		c.PathwayCode CommitmentPathwayCode,
		c.AgreedCost AS CommitmentNegotiatedPrice,
		c.EffectiveFrom AS CommitmentEffectiveDate,

		err.RuleId
	FROM DataLock.PriceEpisodeMatch pem
	JOIN DataLockEvents.vw_IlrPriceEpisodes ilr
		ON pem.Ukprn = ilr.Ukprn
		AND pem.LearnRefNumber = ilr.LearnRefnumber
		AND pem.PriceEpisodeIdentifier = ilr.PriceEpisodeIdentifier
	JOIN DataLockEvents.vw_PriceEpisodePeriodMatch pepm
		ON pem.Ukprn = pepm.Ukprn
		AND pem.LearnRefNumber = pepm.LearnRefnumber
		AND pem.PriceEpisodeIdentifier = pepm.PriceEpisodeIdentifier
	JOIN Reference.DasCommitments c
		ON pem.CommitmentId = c.CommitmentId
		AND pepm.VersionId = c.VersionId
	LEFT JOIN DataLock.ValidationError err
		ON pem.Ukprn = err.Ukprn
		AND pem.LearnRefNumber = err.LearnRefNumber
		AND pem.PriceEpisodeIdentifier = err.PriceEpisodeIdentifier

GO


CREATE CLUSTERED INDEX [IX_DataLockEventsData_Query] ON [DataLockEvents].[DataLockEventsData] (UKPRN, LearnRefNumber, AimSeqNumber, RuleId)
GO
