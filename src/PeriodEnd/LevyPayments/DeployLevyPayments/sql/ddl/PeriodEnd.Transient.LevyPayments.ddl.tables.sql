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
	[TaskLogId] bigint IDENTITY(1,1) NOT NULL,
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
	AccountId bigint NOT NULL,
	HasBeenProcessed bit NOT NULL DEFAULT(0),
	LevySpent decimal(15,2) NOT NULL DEFAULT(0)
)
GO

CREATE INDEX IX_AccountProcessStatus_AccountId_HasBeenProcessed ON LevyPayments.AccountProcessStatus (AccountId, HasBeenProcessed)

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
	PaymentId uniqueidentifier DEFAULT(NEWID()),
	RequiredPaymentId uniqueidentifier NOT NULL,
	DeliveryMonth int NOT NULL,
	DeliveryYear int NOT NULL,
	CollectionPeriodName varchar(8) NOT NULL,
	CollectionPeriodMonth int NOT NULL,
	CollectionPeriodYear int NOT NULL,
	FundingSource int NOT NULL,
	TransactionType int NOT NULL,
	Amount decimal(15,5),
	CONSTRAINT PK_CoInvestedPayments_Payments_RequiredPaymentId_FundingSource PRIMARY KEY (RequiredPaymentId, FundingSource, DeliveryYear, DeliveryMonth)
)
GO

CREATE INDEX IX_LevyPayments_Payments_RequiredPaymentId ON LevyPayments.Payments (RequiredPaymentId) INCLUDE (Amount)
