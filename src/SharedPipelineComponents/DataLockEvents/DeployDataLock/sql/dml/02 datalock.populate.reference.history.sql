DECLARE @ProvidersToProcess TABLE (UKPRN bigint)
INSERT INTO @ProvidersToProcess
(UKPRN)
SELECT
	p.UKPRN
FROM DataLock.vw_Providers p

DECLARE @LastestDataLockEvents TABLE (EventId bigint)
INSERT INTO @LastestDataLockEvents
SELECT
	MAX(Id)
FROM ${DAS_ProviderEvents.FQ}.DataLock.DataLockEvents
WHERE UKPRN IN (SELECT UKPRN FROM @ProvidersToProcess)
AND Status <> 3 -- Do not read removed as anything from here will be new again
GROUP BY UKPRN, LearnRefNumber, PriceEpisodeIdentifier

---------------------------------------------------------------
-- DataLockEvents
---------------------------------------------------------------
INSERT INTO Reference.DataLockEvents
(Id,DataLockEventId, ProcessDateTime, Status, IlrFileName, SubmittedDateTime, AcademicYear, UKPRN, ULN, LearnRefNumber, AimSeqNumber, 
PriceEpisodeIdentifier, CommitmentId, EmployerAccountId, EventSource, HasErrors, IlrStartDate, IlrStandardCode, 
IlrProgrammeType, IlrFrameworkCode, IlrPathwayCode, IlrTrainingPrice, IlrEndpointAssessorPrice, IlrPriceEffectiveFromDate, IlrPriceEffectiveToDate)
SELECT
	dle.Id,
	dle.DataLockEventId,
	dle.ProcessDateTime, 
	dle.Status,
	dle.IlrFileName, 
    dle.SubmittedDateTime, 
    dle.AcademicYear, 
	dle.UKPRN, 
	dle.ULN, 
	dle.LearnRefNumber, 
	dle.AimSeqNumber, 
	dle.PriceEpisodeIdentifier, 
	dle.CommitmentId, 
	dle.EmployerAccountId, 
	dle.EventSource, 
	dle.HasErrors, 
	dle.IlrStartDate, 
	dle.IlrStandardCode, 
	dle.IlrProgrammeType, 
	dle.IlrFrameworkCode, 
	dle.IlrPathwayCode, 
	dle.IlrTrainingPrice, 
	dle.IlrEndpointAssessorPrice,
	dle.IlrPriceEffectiveFromDate,
	dle.IlrPriceEffectiveToDate
	
FROM ${DAS_ProviderEvents.FQ}.DataLock.DataLockEvents dle
INNER JOIN @LastestDataLockEvents le
	ON dle.Id = le.EventId
