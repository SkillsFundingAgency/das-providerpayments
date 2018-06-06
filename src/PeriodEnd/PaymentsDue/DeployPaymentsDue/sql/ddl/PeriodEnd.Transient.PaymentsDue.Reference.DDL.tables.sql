IF NOT EXISTS(SELECT [schema_id] FROM sys.schemas WHERE [name]='Reference')
BEGIN
	EXEC('CREATE SCHEMA Reference')
END
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
	AccountId bigint,
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

CREATE INDEX [IDX_PaymentsHistory_Learner] ON Reference.RequiredPaymentsHistory ([Ukprn], [LearnRefNumber])
GO

CREATE INDEX [IDX_PaymentsHistory_Course] ON Reference.RequiredPaymentsHistory ([Ukprn], [Uln], [StandardCode], [ProgrammeType], [FrameworkCode], [PathwayCode])
GO

