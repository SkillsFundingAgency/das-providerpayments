IF NOT EXISTS(SELECT [schema_id] FROM sys.schemas WHERE [name]='TransfersPayments')
BEGIN
	EXEC('CREATE SCHEMA TransfersPayments')
END
GO

-----------------------------------------------------------------------------------------------------------------------------------------------
-- Payments
-----------------------------------------------------------------------------------------------------------------------------------------------
IF EXISTS(SELECT [object_id] FROM sys.tables WHERE [name]='Payments' AND [schema_id] = SCHEMA_ID('TransfersPayments'))
BEGIN
	DROP TABLE TransfersPayments.Payments
END
GO

CREATE TABLE TransfersPayments.Payments
(
	[PaymentId] uniqueidentifier PRIMARY KEY DEFAULT(NEWID()),
	[RequiredPaymentId] bigint NOT NULL,
	[DeliveryMonth] int NOT NULL,
	[DeliveryYear] int NOT NULL,
	[ColectionPeriodName] varchar(8) NOT NULL,
	[ColectionPeriodMonth] int NOT NULL,
	[ColectionPeriodYear] int NOT NULL,
	[FundingSource] int NOT NULL,
	[TransactionType] int NOT NULL,
	[Amount] decimal(15,5) NOT NULL,
	[FundingAccountId] bigint NOT NULL
)
GO

-----------------------------------------------------------------------------------------------------------------------------------------------
-- AccountTransfers
-----------------------------------------------------------------------------------------------------------------------------------------------

IF NOT EXISTS(SELECT NULL FROM 
	sys.tables t INNER JOIN sys.schemas s ON t.schema_id = s.schema_id
	WHERE t.name='AccountTransfers' AND s.name='TransferPayments'
)
BEGIN
	CREATE TABLE TransfersPayments.AccountTransfers
	(
		Id uniqueidentifier PRIMARY KEY DEFAULT(NEWID()),
		SendingAccountId bigint NOT NULL,
		RecievingAccountId bigint NOT NULL,
		RequiredPaymentId bigint NOT NULL,
		CommitmentId bigint NOT NULL,
		Amount decimal(15,5) NOT NULL,
		TransferType int NOT NULL,
		TransferDate DateTime NOT NULL,
		CollectionPeriodName varchar(8) NOT NULL
	)
END
GO