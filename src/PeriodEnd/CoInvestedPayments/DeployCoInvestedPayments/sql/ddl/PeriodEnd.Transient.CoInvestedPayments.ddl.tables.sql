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
	[TaskLogId] bigint IDENTITY(1,1) NOT NULL,
	[DateTime] datetime NOT NULL DEFAULT(GETDATE()),
	[Level] nvarchar(10) NOT NULL,
	[Logger] nvarchar(512) NOT NULL,
	[Message] nvarchar(1024) NOT NULL,
	[Exception] nvarchar(max) NULL
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
	CommitmentId bigint null,
	CONSTRAINT PK_CoInvestedPayments_Payments_RequiredPaymentId_FundingSource PRIMARY KEY (RequiredPaymentId, FundingSource, DeliveryYear, DeliveryMonth)
)
GO

CREATE INDEX IX_CoInvested_Payments_RequiredPaymentId ON CoInvestedPayments.Payments (RequiredPaymentId) INCLUDE (Amount)
