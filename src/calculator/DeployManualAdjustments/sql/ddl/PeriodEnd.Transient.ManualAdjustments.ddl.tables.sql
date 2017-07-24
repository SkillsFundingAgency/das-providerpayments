IF NOT EXISTS(SELECT [schema_id] FROM sys.schemas WHERE [name]='Adjustments')
BEGIN
    EXEC('CREATE SCHEMA Adjustments')
END
GO

-----------------------------------------------------------------------------------------------------------------------------------------------
-- TaskLog
-----------------------------------------------------------------------------------------------------------------------------------------------
IF EXISTS(SELECT [object_id] FROM sys.tables WHERE [name]='TaskLog' AND [schema_id] = SCHEMA_ID('Adjustments'))
BEGIN
	DROP TABLE Adjustments.TaskLog
END
GO

CREATE TABLE Adjustments.TaskLog
(
	[TaskLogId] uniqueidentifier NOT NULL DEFAULT(NEWID()),
	[DateTime] datetime NOT NULL DEFAULT(GETDATE()),
	[Level] nvarchar(10) NOT NULL,
	[Logger] nvarchar(512) NOT NULL,
	[Message] nvarchar(1024) NOT NULL,
	[Exception] nvarchar(max) NULL
)
GO

-----------------------------------------------------------------------------------------------------------------------------------------------
-- ManualAdjustments
-----------------------------------------------------------------------------------------------------------------------------------------------
IF EXISTS(SELECT [object_id] FROM sys.tables WHERE [name]='ManualAdjustments' AND [schema_id] = SCHEMA_ID('Adjustments'))
BEGIN
    DROP TABLE Adjustments.ManualAdjustments
END
GO
CREATE TABLE Adjustments.ManualAdjustments
(
    RequiredPaymentIdToReverse uniqueidentifier NOT NULL PRIMARY KEY,
    RequiredPaymentIdForReversal uniqueidentifier NULL,
)