IF NOT EXISTS(SELECT [schema_id] FROM sys.schemas WHERE [name]='Reference')
BEGIN
	EXEC('CREATE SCHEMA Reference')
END
GO

-----------------------------------------------------------------------------------------------------------------------------------------------
-- DasAccounts
-----------------------------------------------------------------------------------------------------------------------------------------------
IF EXISTS(SELECT [object_id] FROM sys.tables WHERE [name]='DasAccounts' AND [schema_id] = SCHEMA_ID('Reference'))
BEGIN
	DROP TABLE Reference.DasAccounts
END
GO

CREATE TABLE [Reference].[DasAccounts](
	[AccountId] [varchar](50) NOT NULL PRIMARY KEY,
	[AccountName] [varchar](125) NOT NULL,
	[LevyBalance] [decimal](15, 2) NULL
)
GO