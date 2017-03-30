-----------------------------------------------------------------------------------------------------------------------------------------------
-- DasAccounts
-----------------------------------------------------------------------------------------------------------------------------------------------
IF EXISTS(SELECT [object_id] FROM sys.tables WHERE [name]='DasAccounts' AND [schema_id] = SCHEMA_ID('dbo'))
BEGIN
	DROP TABLE dbo.DasAccounts
END
GO

CREATE TABLE [dbo].[DasAccounts](
	[AccountId] [varchar](50) NOT NULL PRIMARY KEY,
	[AccountName] [varchar](125) NOT NULL,
	[Balance] [decimal](15, 2) NULL
)
GO