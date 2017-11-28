IF NOT EXISTS(SELECT [schema_id] FROM sys.schemas WHERE [name]='Reference')
BEGIN
	EXEC('CREATE SCHEMA Reference')
END
GO

----------------------------------------------------------------------------------------------------------------------------------------------
-- PaymentsHistory
-----------------------------------------------------------------------------------------------------------------------------------------------
IF EXISTS(SELECT [object_id] FROM sys.tables WHERE [name]='LevyPaymentsHistory' AND [schema_id] = SCHEMA_ID('Reference'))
BEGIN
	DROP TABLE Reference.LevyPaymentsHistory
END
GO

CREATE TABLE Reference.LevyPaymentsHistory
(
	RequiredPaymentId uniqueidentifier NOT NULL,
	CommitmentId bigint NOT NULL,
	DeliveryMonth int NOT NULL,
	DeliveryYear int NOT NULL,
	TransactionType int NOT NULL,
	FundingSource int NOT NULL,
	Amount decimal(15,5)
)
GO

CREATE INDEX [IX_LevyPaymentsHistory_Commitment] ON Reference.LevyPaymentsHistory (CommitmentId, TransactionType, DeliveryYear, DeliveryMonth) INCLUDE (RequiredPaymentId, FundingSource, Amount)
GO