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
INNER JOIN ${DAS_ProviderEvents.FQ}.DataLock.DataLockEventPeriods dlep
On dle.DataLockEventId = dlep.DataLockEventId 