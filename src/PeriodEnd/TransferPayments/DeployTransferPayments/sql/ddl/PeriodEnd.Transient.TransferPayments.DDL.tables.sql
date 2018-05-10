IF NOT EXISTS(SELECT [schema_id] FROM sys.schemas WHERE [name]='TransferPayments')
BEGIN
    EXEC('CREATE SCHEMA TransferPayments')
END
GO


-----------------------------------------------------------------------------------------------------------------------------------------------
-- Payments
-----------------------------------------------------------------------------------------------------------------------------------------------
IF EXISTS(SELECT [object_id] FROM sys.tables WHERE [name]='Payments' AND [schema_id] = SCHEMA_ID('TransferPayments'))
BEGIN
	DROP TABLE TransferPayments.Payments
END
GO

CREATE TABLE TransferPayments.Payments
(
	[PaymentId] uniqueidentifier DEFAULT(NEWID()),
	[RequiredPaymentId] uniqueidentifier NOT NULL,
	[DeliveryMonth] int NOT NULL,
	[DeliveryYear] int NOT NULL,
	[CollectionPeriodName] varchar(8) NOT NULL,
	[CollectionPeriodMonth] int NOT NULL,
	[CollectionPeriodYear] int NOT NULL,
	[FundingSource] int NOT NULL,
	[TransactionType] int NOT NULL,
	[Amount] decimal(15,5) NOT NULL,
	[FundingAccountId] bigint NOT NULL,
	CONSTRAINT PK_TransferPayments_Payments_RequiredPaymentId_FundingSource PRIMARY KEY (RequiredPaymentId, FundingSource)
)
GO

IF EXISTS(SELECT NULL FROM sys.indexes WHERE Name = 'IX_TransferPayments_Payments_RequiredPaymentId')
BEGIN
	DROP INDEX IX_TransferPayments_Payments_RequiredPaymentId ON TransferPayments.Payments
END
CREATE INDEX IX_TransferPayments_Payments_RequiredPaymentId ON TransferPayments.Payments (RequiredPaymentId)
GO


-----------------------------------------------------------------------------------------------------------------------------------------------
-- AccountTransfers
-----------------------------------------------------------------------------------------------------------------------------------------------

IF EXISTS(SELECT NULL FROM 
	sys.tables t INNER JOIN sys.schemas s ON t.schema_id = s.schema_id
	WHERE t.name='AccountTransfers' AND s.name='TransferPayments'
) 
BEGIN
	DROP TABLE TransferPayments.AccountTransfers
END

CREATE TABLE TransferPayments.AccountTransfers
(
	SendingAccountId bigint NOT NULL,
	ReceivingAccountId bigint NOT NULL,
	RequiredPaymentId uniqueidentifier NOT NULL,
	CommitmentId bigint NOT NULL,
	Amount decimal(15,5) NOT NULL,
	TransferType int NOT NULL,
	CollectionPeriodName varchar(8) NOT NULL,
	CollectionPeriodMonth int NOT NULL,
	CollectionPeriodYear int NOT NULL
)

IF EXISTS (SELECT NULL FROM sys.indexes WHERE Name = 'IX_TransferPayments_AccountTransfers_RequiredPaymentId')
BEGIN
	DROP INDEX IX_TransferPayments_AccountTransfers_RequiredPaymentId ON TransferPayments.AccountTransfers
END

CREATE INDEX IX_TransferPayments_AccountTransfers_RequiredPaymentId ON TransferPayments.AccountTransfers (RequiredPaymentId)


-----------------------------------------------------------------------------------------------------------------------------------------------
-- TaskLog
-----------------------------------------------------------------------------------------------------------------------------------------------
IF EXISTS(SELECT [object_id] FROM sys.tables WHERE [name]='TaskLog' AND [schema_id] = SCHEMA_ID('TransferPayments'))
BEGIN
	DROP TABLE TransferPayments.TaskLog
END
GO

CREATE TABLE TransferPayments.TaskLog
(
	[TaskLogId] uniqueidentifier NOT NULL DEFAULT(NEWID()),
	[DateTime] datetime NOT NULL DEFAULT(GETDATE()),
	[Level] nvarchar(10) NOT NULL,
	[Logger] nvarchar(512) NOT NULL,
	[Message] nvarchar(1024) NOT NULL,
	[Exception] nvarchar(max) NULL
)
GO