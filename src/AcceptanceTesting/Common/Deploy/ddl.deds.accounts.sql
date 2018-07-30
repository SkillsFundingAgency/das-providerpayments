-----------------------------------------------------------------------------------------------------------------------------------------------
-- DasAccounts
-----------------------------------------------------------------------------------------------------------------------------------------------
IF EXISTS(SELECT [object_id] FROM sys.tables WHERE [name]='DasAccounts' AND [schema_id] = SCHEMA_ID('dbo'))
BEGIN
	DROP TABLE [dbo].[DasAccounts]
END
GO
	
CREATE TABLE [dbo].[DasAccounts](
	[AccountId] bigint NOT NULL,
	[AccountHashId] varchar(125) NOT NULL,
	[AccountName] varchar(255) NOT NULL,
	[Balance] decimal(18,4) NOT NULL,
	[VersionId] varchar(50) NOT NULL,
	PRIMARY KEY CLUSTERED (
		[AccountId]
	)
)
GO
