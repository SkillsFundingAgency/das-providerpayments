---------------------------------------------------------------
-- DataLockEventCommitmentVersions
---------------------------------------------------------------
INSERT INTO Reference.DataLockEventCommitmentVersions
(DataLockEventId, CommitmentVersion, CommitmentStartDate, CommitmentStandardCode, CommitmentProgrammeType, 
CommitmentFrameworkCode, CommitmentPathwayCode, CommitmentNegotiatedPrice, CommitmentEffectiveDate)
SELECT dlecv.DataLockEventId,
    dlecv.CommitmentVersion,
    dlecv.CommitmentStartDate,
    dlecv.CommitmentStandardCode,
    dlecv.CommitmentProgrammeType,
    dlecv.CommitmentFrameworkCode,
    dlecv.CommitmentPathwayCode,
    dlecv.CommitmentNegotiatedPrice,
    dlecv.CommitmentEffectiveDate
FROM OPENQUERY(${DAS_ProviderEvents.servername}, '
		SELECT 
			dlecv.DataLockEventId, 
			dlecv.CommitmentVersion, 
			dlecv.CommitmentStartDate, 
			dlecv.CommitmentStandardCode, 
			dlecv.CommitmentProgrammeType, 
			dlecv.CommitmentFrameworkCode, 
			dlecv.CommitmentPathwayCode, 
			dlecv.CommitmentNegotiatedPrice, 
			dlecv.CommitmentEffectiveDate
		FROM ${DAS_ProviderEvents.databasename}.DataLock.DataLockEventCommitmentVersions dlecv'
    ) AS dlecv
INNER JOIN Reference.DataLockEvents dle ON dle.DataLockEventId = dlecv.DataLockEventId
