IF NOT EXISTS(SELECT [schema_id] FROM sys.schemas WHERE [name]='Payments')
BEGIN
	EXEC('CREATE SCHEMA Payments')
END
GO

-----------------------------------------------------------------------------------------------------------------------------------------------
-- Payments
-----------------------------------------------------------------------------------------------------------------------------------------------
IF EXISTS(SELECT [object_id] FROM sys.tables WHERE [name]='Payments' AND [schema_id] = SCHEMA_ID('Payments'))
BEGIN
	DROP TABLE Payments.Payments
END
GO

CREATE TABLE Payments.Payments
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
