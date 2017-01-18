IF NOT EXISTS(SELECT [schema_id] FROM sys.schemas WHERE [name]='Reference')
BEGIN
	EXEC('CREATE SCHEMA Reference')
END
GO

-----------------------------------------------------------------------------------------------------------------------------------------------
-- ApprenticeshipEarnings
-----------------------------------------------------------------------------------------------------------------------------------------------
IF EXISTS(SELECT [object_id] FROM sys.tables WHERE [name]='ApprenticeshipEarnings' AND [schema_id] = SCHEMA_ID('Reference'))
BEGIN
	DROP TABLE Reference.ApprenticeshipEarnings
END
GO

CREATE TABLE Reference.ApprenticeshipEarnings (
    [Ukprn] bigint NOT NULL,
	[Uln] bigint NOT NULL,
	[LearnRefNumber] varchar(12) NOT NULL,
	[AimSeqNumber] int NOT NULL,
	[PriceEpisodeIdentifier] varchar(25) NOT NULL,
	[Period] int NOT NULL,
	[PriceEpisodeOnProgPayment] decimal(15,5) NOT NULL,
	[PriceEpisodeCompletionPayment] decimal(15,5) NOT NULL,
	[PriceEpisodeBalancePayment] decimal(15,5) NOT NULL,
	[PriceEpisodeFirstEmp1618Pay] decimal(15,5) NOT NULL,
	[PriceEpisodeFirstProv1618Pay] decimal(15,5) NOT NULL,
	[PriceEpisodeSecondEmp1618Pay] decimal(15,5) NOT NULL,
	[PriceEpisodeSecondProv1618Pay] decimal(15,5) NOT NULL,
	[StandardCode] bigint NULL,
	[ProgrammeType] int NULL,
	[FrameworkCode] int NULL,
	[PathwayCode] int NULL,
	[ApprenticeshipContractType] varchar(5) NOT NULL
)
GO

CREATE INDEX [IDX_Earnings_Learner] ON Reference.ApprenticeshipEarnings ([Ukprn], [LearnRefNumber], [AimSeqNumber])
GO

-----------------------------------------------------------------------------------------------------------------------------------------------
-- RequiredPaymentsHistory
-----------------------------------------------------------------------------------------------------------------------------------------------
IF EXISTS(SELECT [object_id] FROM sys.tables WHERE [name]='RequiredPaymentsHistory' AND [schema_id] = SCHEMA_ID('Reference'))
BEGIN
	DROP TABLE Reference.RequiredPaymentsHistory
END
GO

CREATE TABLE Reference.RequiredPaymentsHistory (
	Id uniqueidentifier,
	CommitmentId bigint,
	LearnRefNumber varchar(12),
	Uln bigint null,
	AimSeqNumber int,
	Ukprn bigint,
	DeliveryMonth int,
	DeliveryYear int,
	CollectionPeriodName varchar(8) NOT NULL,
	CollectionPeriodMonth int NOT NULL,
	CollectionPeriodYear int NOT NULL,
	TransactionType int,
	AmountDue decimal(15,5),
	StandardCode bigint null,
	ProgrammeType int null,
	FrameworkCode int null,
	PathwayCode int null,
	PriceEpisodeIdentifier varchar(25)
)
GO

CREATE INDEX [IDX_PaymentsHistory_Learner] ON Reference.RequiredPaymentsHistory ([Ukprn], [Uln])
GO

CREATE INDEX [IDX_PaymentsHistory_Course] ON Reference.RequiredPaymentsHistory ([Ukprn], [Uln], [StandardCode], [ProgrammeType], [FrameworkCode], [PathwayCode])
GO
