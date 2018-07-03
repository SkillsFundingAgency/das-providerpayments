IF NOT EXISTS(SELECT [schema_id] FROM sys.schemas WHERE [name]='Refunds')
BEGIN
    EXEC('CREATE SCHEMA Refunds')
END
GO


-----------------------------------------------------------------------------------------------------------------------------------------------
-- TaskLog
-----------------------------------------------------------------------------------------------------------------------------------------------
IF EXISTS(SELECT [object_id] FROM sys.tables WHERE [name]='TaskLog' AND [schema_id] = SCHEMA_ID('Refunds'))
BEGIN
	DROP TABLE Refunds.TaskLog
END
GO

CREATE TABLE Refunds.TaskLog
(
	[TaskLogId] bigint identity(1,1) NOT NULL,
	[DateTime] datetime NOT NULL DEFAULT(GETDATE()),
	[Level] nvarchar(10) NOT NULL,
	[Logger] nvarchar(512) NOT NULL,
	[Message] nvarchar(1024) NOT NULL,
	[Exception] nvarchar(max) NULL
)
GO

