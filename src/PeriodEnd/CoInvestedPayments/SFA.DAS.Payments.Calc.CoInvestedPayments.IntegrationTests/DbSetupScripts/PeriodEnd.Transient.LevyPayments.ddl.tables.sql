IF NOT EXISTS(SELECT [schema_id] FROM sys.schemas WHERE [name]='LevyPayments')
BEGIN
	EXEC('CREATE SCHEMA LevyPayments')
END
GO

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
GO

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
	AccountId bigint PRIMARY KEY,
	HasBeenProcessed bit NOT NULL DEFAULT(0),
	LevySpent decimal(15,2) NOT NULL DEFAULT(0)
)
GO

CREATE INDEX [IX_AccountProcessStatus_AccountId] ON LevyPayments.AccountProcessStatus (AccountId, HasBeenProcessed) INCLUDE (LevySpent)
GO

-----------------------------------------------------------------------------------------------------------------------------------------------
-- Payments
-----------------------------------------------------------------------------------------------------------------------------------------------
IF EXISTS(SELECT [object_id] FROM sys.tables WHERE [name]='Payments' AND [schema_id] = SCHEMA_ID('LevyPayments'))
BEGIN
	DROP TABLE LevyPayments.Payments
END
GO

CREATE TABLE LevyPayments.Payments
(
	PaymentId uniqueidentifier PRIMARY KEY DEFAULT(NEWID()),
	RequiredPaymentId uniqueidentifier NOT NULL,
	DeliveryMonth int NOT NULL,
	DeliveryYear int NOT NULL,
	CollectionPeriodName varchar(8) NOT NULL,
	CollectionPeriodMonth int NOT NULL,
	CollectionPeriodYear int NOT NULL,
	FundingSource int NOT NULL,
	TransactionType int NOT NULL,
	Amount decimal(15,5)
)
GO
