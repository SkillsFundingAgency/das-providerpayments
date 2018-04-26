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
	[PaymentId] uniqueidentifier PRIMARY KEY DEFAULT(NEWID()),
	[RequiredPaymentId] bigint NOT NULL,
	[DeliveryMonth] int NOT NULL,
	[DeliveryYear] int NOT NULL,
	[CollectionPeriodName] varchar(8) NOT NULL,
	[CollectionPeriodMonth] int NOT NULL,
	[CollectionPeriodYear] int NOT NULL,
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
	CREATE TABLE TransferPayments.AccountTransfers
	(
		Id uniqueidentifier PRIMARY KEY DEFAULT(NEWID()),
		SendingAccountId bigint NOT NULL,
		ReceivingAccountId bigint NOT NULL,
		RequiredPaymentId uniqueidentifier NOT NULL,
		CommitmentId bigint NOT NULL,
		Amount decimal(15,5) NOT NULL,
		TransferType int NOT NULL,
		TransferDate DateTime NOT NULL,
		CollectionPeriodName varchar(8) NOT NULL
	)
END
GO