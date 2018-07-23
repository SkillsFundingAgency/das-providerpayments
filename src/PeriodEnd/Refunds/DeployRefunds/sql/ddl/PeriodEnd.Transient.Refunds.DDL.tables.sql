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

-----------------------------------------------------------------------------------------------------------------------------------------------
-- LevyAccountActivity
-----------------------------------------------------------------------------------------------------------------------------------------------
IF EXISTS(SELECT [object_id] FROM sys.tables WHERE [name]='LevyAccountActivity' AND [schema_id] = SCHEMA_ID('Refunds'))
BEGIN
	DROP TABLE Refunds.LevyAccountActivity
END
GO

CREATE TABLE Refunds.LevyAccountActivity
(
    CollectionPeriodName varchar(8) NOT NULL,
    AccountId bigint NOT NULL,
    LevyAdjustment decimal(15,5) NOT NULL,

    CONSTRAINT PK_LevyAccountActivity PRIMARY KEY NONCLUSTERED (CollectionPeriodName, AccountId)
)
GO

-----------------------------------------------------------------------------------------------------------------------------------------------
-- Payments
-----------------------------------------------------------------------------------------------------------------------------------------------
IF EXISTS(SELECT [object_id] FROM sys.tables WHERE [name]='Payments' AND [schema_id] = SCHEMA_ID('Refunds'))
BEGIN
	DROP TABLE Refunds.Payments
END
GO

CREATE TABLE Refunds.Payments
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
	
	CONSTRAINT PK_Refunds_Payments_RequiredPaymentId_FundingSource PRIMARY KEY (RequiredPaymentId, FundingSource, DeliveryYear, DeliveryMonth)
)
GO

CREATE INDEX IX_Refunds_Payments_RequiredPaymentId ON Refunds.Payments (RequiredPaymentId) INCLUDE (Amount)
