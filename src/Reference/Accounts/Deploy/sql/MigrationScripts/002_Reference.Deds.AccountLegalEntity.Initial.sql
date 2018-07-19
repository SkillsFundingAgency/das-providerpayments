-----------------------------------------------------------------------------------------------------------------------------------------------
-- AccountLegalEntity
-----------------------------------------------------------------------------------------------------------------------------------------------

IF NOT EXISTS(SELECT NULL FROM 
	sys.tables t INNER JOIN sys.schemas s ON t.schema_id = s.schema_id
	WHERE t.name='AccountLegalEntity' AND s.name='dbo'
)
BEGIN
	CREATE TABLE dbo.AccountLegalEntity
	(
		[Id] BIGINT NOT NULL PRIMARY KEY,
		[PublicHashedId] CHAR(6) NOT NULL, 
		[AccountId] BIGINT NOT NULL,
		[LegalEntityId] BIGINT NOT NULL
	)
END
