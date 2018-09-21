IF NOT EXISTS (
SELECT null FROM sys.columns c 
	INNER JOIN sys.tables t 
		ON c.object_id = t.object_id
	INNER JOIN sys.schemas s
		ON t.schema_id = s.schema_id
	WHERE t.name='DasCommitments'
	AND s.name='dbo'
	AND c.name='AccountLegalEntityPublicHashedId'
)
BEGIN
ALTER TABLE  dbo.DasCommitments
  Add AccountLegalEntityPublicHashedId char(6) null
END

IF NOT EXISTS (
SELECT null FROM sys.columns c 
	INNER JOIN sys.tables t 
		ON c.object_id = t.object_id
	INNER JOIN sys.schemas s
		ON t.schema_id = s.schema_id
	WHERE t.name='DasCommitmentsHistory'
	AND s.name='dbo'
	AND c.name='AccountLegalEntityPublicHashedId'
)
BEGIN
ALTER TABLE  dbo.DasCommitmentsHistory
  Add AccountLegalEntityPublicHashedId char(6) null
END
