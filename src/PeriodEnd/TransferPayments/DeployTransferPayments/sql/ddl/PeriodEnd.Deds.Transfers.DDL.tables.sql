IF NOT EXISTS(SELECT [schema_id] FROM sys.schemas WHERE [name]='TransferPayments')
BEGIN
    EXEC('CREATE SCHEMA TransferPayments')
END
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
		RecievingAccountId bigint NOT NULL,
		RequiredPaymentId uniqueidentifier NOT NULL,
		CommitmentId bigint NOT NULL,
		Amount decimal(15,5) NOT NULL,
		TransferType int NOT NULL,
		TransferDate DateTime NOT NULL,
		CollectionPeriodName varchar(8) NOT NULL
	)
END
GO