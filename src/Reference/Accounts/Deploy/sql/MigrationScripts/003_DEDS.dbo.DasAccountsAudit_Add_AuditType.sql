 
IF NOT EXISTS (
SELECT null FROM sys.columns c 
	INNER JOIN sys.tables t 
		ON c.object_id = t.object_id
	INNER JOIN sys.schemas s
		ON t.schema_id = s.schema_id
	WHERE t.name='DasAccountsAudit'
	AND s.name='dbo'
	AND c.name='AuditType'
)
BEGIN
ALTER TABLE dbo.DasAccountsAudit
	Add AuditType tinyint NOT NULL DEFAULT(0)
END
