-----------------------------------------------------------------------------------------------------------------------------------------------
-- TaskLog
-----------------------------------------------------------------------------------------------------------------------------------------------
IF EXISTS(SELECT [object_id] FROM sys.tables WHERE [name]='TaskLog' AND [schema_id] = SCHEMA_ID('dbo'))
BEGIN
	DROP TABLE [dbo].[TaskLog]
END
GO
	
CREATE TABLE [dbo].[TaskLog](
	[TaskLogId] [uniqueidentifier] NOT NULL DEFAULT(NEWID()),
	[DateTime] [datetime] NOT NULL DEFAULT(GETDATE()),
	[Level] [nvarchar](10) NOT NULL,
	[Logger] [nvarchar](512) NOT NULL,
	[Message] [nvarchar](1024) NOT NULL,
	[Exception] [nvarchar](max) NULL
)
GO

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
	[IsLevyPayer] bit NOT NULL,
	[TransferAllowance] decimal(15, 5) null,
	PRIMARY KEY CLUSTERED (
		[AccountId]
	)
)
GO

-----------------------------------------------------------------------------------------------------------------------------------------------
-- DasAccountsAudit
-----------------------------------------------------------------------------------------------------------------------------------------------
IF EXISTS(SELECT [object_id] FROM sys.tables WHERE [name]='DasAccountsAudit' AND [schema_id] = SCHEMA_ID('dbo'))
BEGIN
	DROP TABLE [dbo].[DasAccountsAudit]
END
GO
	
CREATE TABLE [dbo].[DasAccountsAudit](
	[ReadDateTime] datetime NOT NULL,
	[AccountsRead] bigint NOT NULL,
	[AuditType] tinyint NOT NULL,
	[CompletedSuccessfully] bit
)
GO

-----------------------------------------------------------------------------------------------------------------------------------------------
-- AccountLegalEntity
-----------------------------------------------------------------------------------------------------------------------------------------------
IF EXISTS(SELECT [object_id] FROM sys.tables WHERE [name]='AccountLegalEntity' AND [schema_id] = SCHEMA_ID('dbo'))
BEGIN
	DROP TABLE [dbo].[AccountLegalEntity]
END
GO
	
CREATE TABLE [dbo].[AccountLegalEntity](
	[Id] BIGINT NOT NULL PRIMARY KEY,
	[PublicHashedId] CHAR(6) NOT NULL, 
	[AccountId] BIGINT NOT NULL,
	[LegalEntityId] BIGINT NOT NULL
)
GO

