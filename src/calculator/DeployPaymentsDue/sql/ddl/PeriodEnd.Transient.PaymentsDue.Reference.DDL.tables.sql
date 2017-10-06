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
	[PriceEpisodeEndDate] date NOT NULL,
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
	[ApprenticeshipContractType] int NOT NULL,
	[PriceEpisodeFundLineType] varchar(60) NULL,
	[PriceEpisodeSfaContribPct] decimal(15,5) NOT NULL,
	[PriceEpisodeLevyNonPayInd] int NULL,
	[PriceEpisodeApplic1618FrameworkUpliftBalancing] decimal(15,5) NOT NULL,
	[PriceEpisodeApplic1618FrameworkUpliftCompletionPayment] decimal(15,5) NOT NULL,
	[PriceEpisodeApplic1618FrameworkUpliftOnProgPayment] decimal(15,5) NOT NULL,
	[PriceEpisodeFirstDisadvantagePayment] decimal(15,5) NOT NULL,
	[PriceEpisodeSecondDisadvantagePayment] decimal(15,5) NOT NULL,
	[LearningSupportPayment] decimal(15,5) NOT NULL,
	[EpisodeStartDate] date NOT NULL,
	[LearnAimRef] varchar(8),
	[LearningStartDate] datetime
)
GO

CREATE INDEX [IDX_Earnings_Learner] ON Reference.ApprenticeshipEarnings ([Ukprn], [LearnRefNumber], [LearnAimRef])
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
	CommitmentVersionId varchar(25),
	AccountId varchar(50),
	AccountVersionId varchar(50),
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
	PriceEpisodeIdentifier varchar(25),
	LearnAimRef varchar(8),
	LearningStartDate datetime,
	IlrSubmissionDateTime datetime,
	ApprenticeshipContractType int,
	SfaContributionPercentage decimal(15,5),
	FundingLineType varchar(100),
	UseLevyBalance bit
)
GO

CREATE INDEX [IDX_PaymentsHistory_Learner] ON Reference.RequiredPaymentsHistory ([Ukprn], [LearnRefNumber], [StandardCode], [TransactionType], [DeliveryYear], [DeliveryMonth], [AmountDue])
GO

CREATE INDEX [IDX_PaymentsHistory_Course] ON Reference.RequiredPaymentsHistory ([Ukprn], [Uln], [StandardCode], [ProgrammeType], [FrameworkCode], [PathwayCode])
GO


-----------------------------------------------------------------------------------------------------------------------------------------------
-- ApprenticeshipDeliveryEarnings
-----------------------------------------------------------------------------------------------------------------------------------------------
IF EXISTS(SELECT [object_id] FROM sys.tables WHERE [name]='ApprenticeshipDeliveryEarnings' AND [schema_id] = SCHEMA_ID('Reference'))
BEGIN
	DROP TABLE Reference.ApprenticeshipDeliveryEarnings
END
GO

CREATE TABLE Reference.ApprenticeshipDeliveryEarnings (
    [Ukprn] bigint NOT NULL,
	[Uln] bigint NOT NULL,
	[LearnRefNumber] varchar(12) NOT NULL,
	[AimSeqNumber] int NOT NULL,
	[Period] int NOT NULL,
	[MathEngOnProgPayment] decimal(15,5) NOT NULL,
	[MathEngBalPayment] decimal(15,5) NOT NULL,
	[LearningSupportPayment] decimal(15,5) NOT NULL,
	[StandardCode] bigint NULL,
	[ProgrammeType] int NULL,
	[FrameworkCode] int NULL,
	[PathwayCode] int NULL,
	[ApprenticeshipContractType] int NULL,
	[FundingLineType] varchar(100) NULL,
	[SfaContributionPercentage] decimal(15,5) NULL,
	[LevyNonPayIndicator] int NULL,
	LearnAimRef varchar(8),
	LearningStartDate datetime	
)
GO

CREATE INDEX [IDX_Earnings_Learner] ON Reference.ApprenticeshipDeliveryEarnings ([Ukprn], [LearnRefNumber], [LearnAimRef])
GO