IF NOT EXISTS(SELECT [schema_id] FROM sys.schemas WHERE [name]='PaymentsDue')
BEGIN
	EXEC('CREATE SCHEMA PaymentsDue')
END
GO

-----------------------------------------------------------------------------------------------------------------------------------------------
-- TaskLog
-----------------------------------------------------------------------------------------------------------------------------------------------
IF EXISTS(SELECT [object_id] FROM sys.tables WHERE [name]='TaskLog' AND [schema_id] = SCHEMA_ID('PaymentsDue'))
BEGIN
	DROP TABLE PaymentsDue.TaskLog
END
GO

CREATE TABLE PaymentsDue.TaskLog
(
	[TaskLogId] uniqueidentifier NOT NULL DEFAULT(NEWID()),
	[DateTime] datetime NOT NULL DEFAULT(GETDATE()),
	[Level] nvarchar(10) NOT NULL,
	[Logger] nvarchar(512) NOT NULL,
	[Message] nvarchar(1024) NOT NULL,
	[Exception] nvarchar(max) NULL
)
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
	AccountId bigint,
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
	ApprenticeshipContractType int,
	DeliveryMonth int,
	DeliveryYear int,
	CollectionPeriodName varchar(8) NOT NULL,
	CollectionPeriodMonth int NOT NULL,
	CollectionPeriodYear int NOT NULL,
	TransactionType int,
	AmountDue decimal(15,5),
	SfaContributionPercentage decimal(15,5),
	FundingLineType varchar(120),
	UseLevyBalance bit,
	IsSmallEmployer bit
)
GO

CREATE NONCLUSTERED INDEX [IX_PaymentsDue_TransactionType_UseLevy_Commitment_Query]
ON [PaymentsDue].[RequiredPayments] ([CommitmentId],[UseLevyBalance],[TransactionType])
GO