IF NOT EXISTS(SELECT [schema_id] FROM sys.schemas WHERE [name]='PaymentsDue')
BEGIN
	EXEC('CREATE SCHEMA PaymentsDue')
END
GO

-----------------------------------------------------------------------------------------------------------------------------------------------
-- RequiredPayments
-----------------------------------------------------------------------------------------------------------------------------------------------
IF EXISTS(SELECT [object_id] FROM sys.tables WHERE [name]='RequiredPayments' AND [schema_id] = SCHEMA_ID('PaymentsDue'))
BEGIN
	DROP TABLE PaymentsDue.RequiredPayments
END
GO

CREATE TABLE PaymentsDue.RequiredPayments
(
	Id uniqueidentifier PRIMARY KEY DEFAULT(NEWID()),
	CommitmentId varchar(50),
	LearnRefNumber varchar(12),
	AimSeqNumber int,
	Ukprn bigint,
	DeliveryMonth int,
	DeliveryYear int,
	CollectionPeriodMonth int NOT NULL,
	CollectionPeriodYear int NOT NULL,
	TransactionType int,
	AmountDue decimal(15,5)
)