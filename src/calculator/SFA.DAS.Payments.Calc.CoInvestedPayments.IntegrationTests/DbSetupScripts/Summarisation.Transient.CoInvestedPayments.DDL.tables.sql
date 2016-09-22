IF NOT EXISTS(SELECT [schema_id] FROM sys.schemas WHERE [name]='CoInvestedPayments')
BEGIN
	EXEC('CREATE SCHEMA CoInvestedPayments')
END
GO

-----------------------------------------------------------------------------------------------------------------------------------------------
-- TaskLog
-----------------------------------------------------------------------------------------------------------------------------------------------
IF EXISTS(SELECT [object_id] FROM sys.tables WHERE [name]='TaskLog' AND [schema_id] = SCHEMA_ID('CoInvestedPayments'))
BEGIN
	DROP TABLE CoInvestedPayments.TaskLog
END
GO

CREATE TABLE CoInvestedPayments.TaskLog
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
IF EXISTS(SELECT [object_id] FROM sys.tables WHERE [name]='AccountProcessStatus' AND [schema_id] = SCHEMA_ID('CoInvestedPayments'))
BEGIN
	DROP TABLE CoInvestedPayments.AccountProcessStatus
END
GO

CREATE TABLE CoInvestedPayments.AccountProcessStatus
(
	AccountId varchar(50) PRIMARY KEY,
	HasBeenProcessed bit NOT NULL DEFAULT(0),
	LevySpent decimal(15,2) NOT NULL DEFAULT(0)
)
GO

-----------------------------------------------------------------------------------------------------------------------------------------------
-- Payments
-----------------------------------------------------------------------------------------------------------------------------------------------
IF EXISTS(SELECT [object_id] FROM sys.tables WHERE [name]='Payments' AND [schema_id] = SCHEMA_ID('CoInvestedPayments'))
BEGIN
	DROP TABLE CoInvestedPayments.Payments
END
GO

CREATE TABLE CoInvestedPayments.Payments
(
	PaymentId uniqueidentifier PRIMARY KEY DEFAULT(NEWID()),
	CommitmentId varchar(50) NOT NULL,
	LearnRefNumber varchar(12) NOT NULL,
	AimSeqNumber int NOT NULL,
	Ukprn bigint NOT NULL,
	DeliveryMonth int NOT NULL,
	DeliveryYear int NOT NULL,
	CollectionPeriodMonth int NOT NULL,
	CollectionPeriodYear int NOT NULL,
	[Source] int NOT NULL,
	TransactionType int NOT NULL,
	Amount decimal(15,2)
)
GO