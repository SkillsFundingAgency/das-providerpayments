IF NOT EXISTS(SELECT [schema_id] FROM sys.schemas WHERE [name]='TransferPayments')
BEGIN
    EXEC('CREATE SCHEMA TransferPayments')
END

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
		TransferId bigint PRIMARY KEY identity(1,1),
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

	CREATE INDEX TransferPayments_AccountTransfers_RequiredPaymentId ON TransferPayments.AccountTransfers (RequiredPaymentId)
END

