---------------------------------------------------------------
-- DataLockEventPeriods
---------------------------------------------------------------
INSERT INTO Reference.DataLockEventPeriods
(DataLockEventId, CollectionPeriodName, CollectionPeriodMonth, CollectionPeriodYear, CommitmentVersion, IsPayable, TransactionType,TransactionTypesFlag)
SELECT
	dlep.DataLockEventId, 
	dlep.CollectionPeriodName, 
	dlep.CollectionPeriodMonth, 
	dlep.CollectionPeriodYear, 
	dlep.CommitmentVersion, 
	dlep.IsPayable,
    dlep.TransactionType,
	dlep.TransactionTypesFlag
FROM Reference.DataLockEvents dle 
INNER JOIN OPENQUERY(${DAS_ProviderEvents.servername}, '
		SELECT * FROM ${DAS_ProviderEvents.databasename}.DataLock.DataLockEventPeriods'
	) AS dlep ON dle.DataLockEventId = dlep.DataLockEventId
