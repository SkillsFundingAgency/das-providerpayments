IF NOT EXISTS(SELECT [schema_id] FROM sys.schemas WHERE [name]='LevyPayments')
BEGIN
	EXEC('CREATE SCHEMA LevyPayments')
END

-----------------------------------------------------------------------------------------------------------------------------------------------
-- TaskLog
-----------------------------------------------------------------------------------------------------------------------------------------------
IF EXISTS(SELECT [object_id] FROM sys.tables WHERE [name]='TaskLog' AND [schema_id] = SCHEMA_ID('LevyPayments'))
BEGIN
	DROP TABLE LevyPayments.TaskLog
END
GO

CREATE TABLE LevyPayments.TaskLog
(
	[TaskLogId] uniqueidentifier NOT NULL DEFAULT(NEWID()),
	[DateTime] datetime NOT NULL DEFAULT(GETDATE()),
	[Level] nvarchar(10) NOT NULL,
	[Logger] nvarchar(512) NOT NULL,
	[Message] nvarchar(1024) NOT NULL,
	[Exception] nvarchar(max) NULL
)


-----------------------------------------------------------------------------------------------------------------------------------------------
-- AccountProcessStatus
-----------------------------------------------------------------------------------------------------------------------------------------------
IF EXISTS(SELECT [object_id] FROM sys.tables WHERE [name]='AccountProcessStatus' AND [schema_id] = SCHEMA_ID('LevyPayments'))
BEGIN
	DROP TABLE LevyPayments.AccountProcessStatus
END
GO

CREATE TABLE LevyPayments.AccountProcessStatus
(
	AccountId varchar(50) PRIMARY KEY,
	HasBeenProcessed bit,
	LevySpent decimal(15,2)
)