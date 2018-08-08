IF NOT EXISTS (
SELECT null FROM sys.columns c 
	INNER JOIN sys.tables t 
		ON c.object_id = t.object_id
	INNER JOIN sys.schemas s
		ON t.schema_id = s.schema_id
	WHERE t.name='DasCommitments'
	AND s.name='dbo'
	AND c.name='PausedOnDate'
)
BEGIN
ALTER TABLE  dbo.DasCommitments
  Add PausedOnDate datetime2 null
END

IF NOT EXISTS (
SELECT null FROM sys.columns c 
	INNER JOIN sys.tables t 
		ON c.object_id = t.object_id
	INNER JOIN sys.schemas s
		ON t.schema_id = s.schema_id
	WHERE t.name='DasCommitments'
	AND s.name='dbo'
	AND c.name='WithdrawnOnDate'
)
BEGIN
ALTER TABLE  dbo.DasCommitments
  Add WithdrawnOnDate datetime2 null
END

IF NOT EXISTS (
SELECT null FROM sys.columns c 
	INNER JOIN sys.tables t 
		ON c.object_id = t.object_id
	INNER JOIN sys.schemas s
		ON t.schema_id = s.schema_id
	WHERE t.name='DasCommitmentsHistory'
	AND s.name='dbo'
	AND c.name='PausedOnDate'
)
BEGIN
ALTER TABLE  dbo.DasCommitmentsHistory
  Add PausedOnDate datetime2 null
END

IF NOT EXISTS (
SELECT null FROM sys.columns c 
	INNER JOIN sys.tables t 
		ON c.object_id = t.object_id
	INNER JOIN sys.schemas s
		ON t.schema_id = s.schema_id
	WHERE t.name='DasCommitmentsHistory'
	AND s.name='dbo'
	AND c.name='WithdrawnOnDate'
)
BEGIN
ALTER TABLE  dbo.DasCommitmentsHistory
  Add WithdrawnOnDate datetime2 null
END