IF NOT EXISTS(SELECT [schema_id] FROM sys.schemas WHERE [name]='Reference')
BEGIN
	EXEC('CREATE SCHEMA Reference')
END
GO

----------------------------------------------------------------------------------------------------------------------------------------------
-- PaymentsHistory
-----------------------------------------------------------------------------------------------------------------------------------------------
IF EXISTS(SELECT [object_id] FROM sys.tables WHERE [name]='CoInvestedPaymentsHistory' AND [schema_id] = SCHEMA_ID('Reference'))
BEGIN
	DROP TABLE Reference.CoInvestedPaymentsHistory
END
GO

CREATE TABLE Reference.CoInvestedPaymentsHistory
(
	RequiredPaymentId uniqueidentifier NOT NULL,
	Uln bigint ,
	Ukprn bigint,
	AimSeqNumber int,
	StandardCode bigint,
	ProgrammeType int,
	FrameworkCode int,
	PathwayCode int,
	DeliveryMonth int NOT NULL,
	DeliveryYear int NOT NULL,
	TransactionType int NOT NULL,
	Amount decimal(15,5),
	FundingSource Int Not Null,
	CommitmentId bigint null
)
GO