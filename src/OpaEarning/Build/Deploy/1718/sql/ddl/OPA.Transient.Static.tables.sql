IF NOT EXISTS (
SELECT  schema_name
FROM    information_schema.schemata
WHERE   schema_name = 'Static' ) 

BEGIN
EXEC sp_executesql N'CREATE SCHEMA Static'
END



IF OBJECT_ID('Static.ErrorMessageLookUp') IS NOT NULL
BEGIN
    DROP TABLE [Static].[ErrorMessageLookUp]
END;

GO

CREATE TABLE [Static].[ErrorMessageLookUp](
	[RuleName]				VARCHAR(50) PRIMARY KEY,
	[Severity]				VARCHAR(2) NULL,
	[ErrorMessage]			VARCHAR(MAX) NULL	
) ON [PRIMARY]
