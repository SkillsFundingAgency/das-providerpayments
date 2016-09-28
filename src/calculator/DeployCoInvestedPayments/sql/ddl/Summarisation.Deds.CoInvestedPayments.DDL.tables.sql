IF NOT EXISTS(SELECT [schema_id] FROM sys.schemas WHERE [name]='CoInvestedPayments')
BEGIN
	EXEC('CREATE SCHEMA CoInvestedPayments')
END
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
	RequiredPaymentId uniqueidentifier NOT NULL,
	CommitmentId varchar(50) NOT NULL,
	LearnRefNumber varchar(12) NOT NULL,
	AimSeqNumber int NOT NULL,
	Ukprn bigint NOT NULL,
	DeliveryMonth int NOT NULL,
	DeliveryYear int NOT NULL,
	CollectionPeriodMonth int NOT NULL,
	CollectionPeriodYear int NOT NULL,
	FundingSource int NOT NULL,
	TransactionType int NOT NULL,
	Amount decimal(15,5)
)
GO