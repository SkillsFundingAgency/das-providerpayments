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
	CommitmentId bigint,
	CommitmentVersionId varchar(50),
	AccountId varchar(50),
	AccountVersionId varchar(50),
	Uln bigint,
	LearnRefNumber varchar(12),
	AimSeqNumber int,
	Ukprn bigint,
	IlrSubmissionDateTime datetime,
	PriceEpisodeIdentifier varchar(25),
	StandardCode bigint,
	ProgrammeType int,
	FrameworkCode int,
	PathwayCode int,
	ApprenticeshipContractType varchar(5),
	DeliveryMonth int,
	DeliveryYear int,
	CollectionPeriodName varchar(8) NOT NULL,
	CollectionPeriodMonth int NOT NULL,
	CollectionPeriodYear int NOT NULL,
	TransactionType int,
	AmountDue decimal(15,5)
)