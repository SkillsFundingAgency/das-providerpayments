---------------------------------------------------------------
-- DataLockEventErrors
---------------------------------------------------------------
INSERT INTO Reference.DataLockEventErrors
(DataLockEventId, ErrorCode, SystemDescription)
SELECT 
	dlee.DataLockEventId,
    dlee.ErrorCode,
    dlee.SystemDescription
FROM OPENQUERY(${DAS_ProviderEvents.servername}, '
	SELECT 
		dlee.DataLockEventId, 
		dlee.ErrorCode, 
		dlee.SystemDescription
	 FROM 
		${DAS_ProviderEvents.databasename}.DataLock.DataLockEventErrors dlee'
    ) AS dlee
INNER JOIN Reference.DataLockEvents dle ON dle.DataLockEventId = dlee.DataLockEventId
