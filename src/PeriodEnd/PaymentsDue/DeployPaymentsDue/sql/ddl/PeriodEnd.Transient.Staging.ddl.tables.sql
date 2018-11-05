IF NOT EXISTS(SELECT [schema_id] FROM sys.schemas WHERE [name]='Staging')
BEGIN
	EXEC('CREATE SCHEMA Staging')
END
GO



-----------------------------------------------------------------------------------------------------------------------------------------------
-- RawEarnings
-----------------------------------------------------------------------------------------------------------------------------------------------
IF EXISTS(SELECT [object_id] FROM sys.tables WHERE [name]='RawEarnings' AND [schema_id] = SCHEMA_ID('Staging'))
BEGIN
	DROP TABLE Staging.RawEarnings
END
GO


-- This is for our test framework... with one intra database rather than two
IF EXISTS(SELECT [object_id] FROM sys.views WHERE [name]='RawEarnings' AND [schema_id] = SCHEMA_ID('Staging'))
BEGIN
    DROP VIEW Staging.RawEarnings
END
GO

CREATE TABLE Staging.RawEarnings (
	LearnRefNumber varchar(12) NOT NULL,
	Ukprn bigint NOT NULL,
	AimSeqNumber int not null,
	PriceEpisodeIdentifier varchar(25),
	EpisodeStartDate date,
	EpisodeEffectiveTNPStartDate date,
	[Period] int NOT NULL,
	ULN bigint NOT NULL,
	ProgrammeType int,
	FrameworkCode int,
	PathwayCode int,
	StandardCode int,
	SfaContributionPercentage decimal(15,5),
	FundingLineType varchar(100),
	LearnAimRef varchar(8),
	LearningStartDate date not null,
	TransactionType01 decimal(15,5),
	TransactionType02 decimal(15,5),
	TransactionType03 decimal(15,5),
	TransactionType04 decimal(15,5),
	TransactionType05 decimal(15,5),
	TransactionType06 decimal(15,5),
	TransactionType07 decimal(15,5),
	TransactionType08 decimal(15,5),
	TransactionType09 decimal(15,5),
	TransactionType10 decimal(15,5),
	TransactionType11 decimal(15,5),
	TransactionType12 decimal(15,5),
	TransactionType13 decimal(15,5),
	TransactionType14 decimal(15,5),
	TransactionType15 decimal(15,5),
	TransactionType16 decimal(15,5),
	ApprenticeshipContractType int,
	FirstIncentiveCensusDate date,
	SecondIncentiveCensusDate date,
	LearnerAdditionalPaymentsDate date,
	AgreedPrice decimal(15,5),
	EndDate date,
	CumulativePmrs decimal(12,5),
	ExemptionCodeForCompletionHoldback int,
)
GO

-----------------------------------------------------------------------------------------------------------------------------------------------
-- RawEarningsMathsEnglish
-----------------------------------------------------------------------------------------------------------------------------------------------
IF EXISTS(SELECT [object_id] FROM sys.tables WHERE [name]='RawEarningsMathsEnglish' AND [schema_id] = SCHEMA_ID('Staging'))
BEGIN
	DROP TABLE Staging.RawEarningsMathsEnglish
END
GO

CREATE TABLE Staging.RawEarningsMathsEnglish (
	LearnRefNumber varchar(12) NOT NULL,
	Ukprn bigint NOT NULL,
	AimSeqNumber int not null,
	PriceEpisodeIdentifier varchar(25),
	EpisodeStartDate date,
	EpisodeEffectiveTNPStartDate date,
	[Period] int NOT NULL,
	ULN bigint NOT NULL,
	ProgrammeType int,
	FrameworkCode int,
	PathwayCode int,
	StandardCode int,
	SfaContributionPercentage decimal(15,5),
	FundingLineType varchar(100),
	LearnAimRef varchar(8),
	LearningStartDate date NOT NULL,
	TransactionType01 decimal(15,5),
	TransactionType02 decimal(15,5),
	TransactionType03 decimal(15,5),
	TransactionType04 decimal(15,5),
	TransactionType05 decimal(15,5),
	TransactionType06 decimal(15,5),
	TransactionType07 decimal(15,5),
	TransactionType08 decimal(15,5),
	TransactionType09 decimal(15,5),
	TransactionType10 decimal(15,5),
	TransactionType11 decimal(15,5),
	TransactionType12 decimal(15,5),
	TransactionType13 decimal(15,5),
	TransactionType14 decimal(15,5),
	TransactionType15 decimal(15,5),
	TransactionType16 decimal(15,5),
	ApprenticeshipContractType tinyint
)
GO

