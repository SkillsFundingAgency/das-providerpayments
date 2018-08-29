IF NOT EXISTS (
SELECT null FROM sys.columns c 
	INNER JOIN sys.tables t 
		ON c.object_id = t.object_id
	INNER JOIN sys.schemas s
		ON t.schema_id = s.schema_id
	WHERE t.name='DasCommitments'
	AND s.name='dbo'
	AND c.name='TransferSendingEmployerAccountId'
)
BEGIN
ALTER TABLE  dbo.DasCommitments
  Add TransferSendingEmployerAccountId bigint null
END

IF NOT EXISTS (
SELECT null FROM sys.columns c 
	INNER JOIN sys.tables t 
		ON c.object_id = t.object_id
	INNER JOIN sys.schemas s
		ON t.schema_id = s.schema_id
	WHERE t.name='DasCommitments'
	AND s.name='dbo'
	AND c.name='TransferApprovalDate'
)
BEGIN
ALTER TABLE  dbo.DasCommitments
  Add TransferApprovalDate datetime null
END

IF NOT EXISTS (
SELECT null FROM sys.columns c 
	INNER JOIN sys.tables t 
		ON c.object_id = t.object_id
	INNER JOIN sys.schemas s
		ON t.schema_id = s.schema_id
	WHERE t.name='DasCommitmentsHistory'
	AND s.name='dbo'
	AND c.name='TransferSendingEmployerAccountId'
)
BEGIN
ALTER TABLE  dbo.DasCommitmentsHistory
  Add TransferSendingEmployerAccountId bigint null
END

IF NOT EXISTS (
SELECT null FROM sys.columns c 
	INNER JOIN sys.tables t 
		ON c.object_id = t.object_id
	INNER JOIN sys.schemas s
		ON t.schema_id = s.schema_id
	WHERE t.name='DasCommitmentsHistory'
	AND s.name='dbo'
	AND c.name='TransferApprovalDate'
)
BEGIN
ALTER TABLE  dbo.DasCommitmentsHistory
  Add TransferApprovalDate datetime null
END
