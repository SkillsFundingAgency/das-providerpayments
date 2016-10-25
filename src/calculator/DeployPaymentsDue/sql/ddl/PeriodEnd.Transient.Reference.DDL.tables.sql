IF NOT EXISTS(SELECT [schema_id] FROM sys.schemas WHERE [name]='Reference')
BEGIN
	EXEC('CREATE SCHEMA Reference')
END
GO

-----------------------------------------------------------------------------------------------------------------------------------------------
-- CollectionPeriods
-----------------------------------------------------------------------------------------------------------------------------------------------
IF EXISTS(SELECT [object_id] FROM sys.tables WHERE [name]='CollectionPeriods' AND [schema_id] = SCHEMA_ID('Reference'))
BEGIN
	DROP TABLE Reference.CollectionPeriods
END
GO

CREATE TABLE Reference.CollectionPeriods (
	[Id] int NOT NULL,
	[Name] varchar(3) NOT NULL,
	[CalendarMonth] int NOT NULL,
	[CalendarYear] int NOT NULL,
	[Open] bit NOT NULL,
	CONSTRAINT [PK_CollectionPeriods] PRIMARY KEY CLUSTERED ([Id] ASC)
)
GO

-----------------------------------------------------------------------------------------------------------------------------------------------
-- Providers
-----------------------------------------------------------------------------------------------------------------------------------------------
IF EXISTS(SELECT [object_id] FROM sys.tables WHERE [name]='Providers' AND [schema_id] = SCHEMA_ID('Reference'))
BEGIN
	DROP TABLE Reference.Providers
END
GO

CREATE TABLE Reference.Providers (
	[Ukprn] bigint NOT NULL,
	CONSTRAINT [PK_Providers] PRIMARY KEY CLUSTERED ([Ukprn] ASC)
)
GO

-----------------------------------------------------------------------------------------------------------------------------------------------
-- ApprenticeshipEarnings
-----------------------------------------------------------------------------------------------------------------------------------------------
IF EXISTS(SELECT [object_id] FROM sys.tables WHERE [name]='ApprenticeshipEarnings' AND [schema_id] = SCHEMA_ID('Reference'))
BEGIN
	DROP TABLE Reference.ApprenticeshipEarnings
END
GO

CREATE TABLE Reference.ApprenticeshipEarnings(
    [Ukprn] bigint NOT NULL,
	[LearnRefNumber] varchar(12) NOT NULL,
	[AimSeqNumber] int NOT NULL,
    [MonthlyInstallment] decimal(15,5) NOT NULL,
    [CompletionPayment] decimal(15,5) NOT NULL,
	[Period_1] decimal(15,5) NOT NULL,
	[Period_2] decimal(15,5) NOT NULL,
	[Period_3] decimal(15,5) NOT NULL,
	[Period_4] decimal(15,5) NOT NULL,
	[Period_5] decimal(15,5) NOT NULL,
	[Period_6] decimal(15,5) NOT NULL,
	[Period_7] decimal(15,5) NOT NULL,
	[Period_8] decimal(15,5) NOT NULL,
	[Period_9] decimal(15,5) NOT NULL,
	[Period_10] decimal(15,5) NOT NULL,
	[Period_11] decimal(15,5) NOT NULL,
	[Period_12] decimal(15,5) NOT NULL
    
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

CREATE TABLE Reference.RequiredPaymentsHistory
(
	Id uniqueidentifier,
	CommitmentId bigint,
	LearnRefNumber varchar(12),
	AimSeqNumber int,
	Ukprn bigint,
	DeliveryMonth int,
	DeliveryYear int,
	CollectionPeriodName varchar(3) NOT NULL,
	CollectionPeriodMonth int NOT NULL,
	CollectionPeriodYear int NOT NULL,
	TransactionType int,
	AmountDue decimal(15,5)
)
GO

CREATE INDEX [IDX_PaymentsHistory_Learner] ON Reference.RequiredPaymentsHistory ([Ukprn], [LearnRefNumber], [AimSeqNumber])
GO

CREATE INDEX [IDX_PaymentsHistory_Commitment] ON Reference.RequiredPaymentsHistory ([CommitmentId])
GO
