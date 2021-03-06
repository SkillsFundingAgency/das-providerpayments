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

CREATE TABLE [Reference].[DasAccounts]
(
    [AccountId] bigint NOT NULL PRIMARY KEY,
	[AccountHashId] [varchar](50) NOT NULL,
    [AccountName] [varchar](125) NOT NULL,
    [Balance] [decimal](15, 2) NULL,
	[VersionId] [varchar](50) NOT NULL,
	[IsLevyPayer] bit NOT NULL,
	[TransferAllowance] decimal(15,2) null
)
GO

