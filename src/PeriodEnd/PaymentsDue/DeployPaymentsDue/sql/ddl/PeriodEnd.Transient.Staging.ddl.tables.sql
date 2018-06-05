IF NOT EXISTS(SELECT [schema_id] FROM sys.schemas WHERE [name]='Staging')
BEGIN
	EXEC('CREATE SCHEMA Staging')
END
GO

-----------------------------------------------------------------------------------------------------------------------------------------------
-- CollectionPeriods
-----------------------------------------------------------------------------------------------------------------------------------------------
IF EXISTS(SELECT [object_id] FROM sys.tables WHERE [name]='CollectionPeriods' AND [schema_id] = SCHEMA_ID('Staging'))
BEGIN
	DROP TABLE Staging.CollectionPeriods
END
GO

CREATE TABLE Staging.CollectionPeriods
(
	Id int PRIMARY KEY,
	Name varchar(3) NOT NULL,
	CalendarMonth int NOT NULL,
	CalendarYear int NOT NULL,
	[Open] bit NOT NULL,
	PeriodNumber int NOT NULL,
	FirstDayOfAcademicYear date NOT NULL
)
GO

-----------------------------------------------------------------------------------------------------------------------------------------------
-- NonDasTransactionTypes
-----------------------------------------------------------------------------------------------------------------------------------------------
IF EXISTS(SELECT [object_id] FROM sys.tables WHERE [name]='NonDasTransactionTypes' AND [schema_id] = SCHEMA_ID('Staging'))
BEGIN
	DROP TABLE Staging.NonDasTransactionTypes
END
GO

CREATE TABLE Staging.NonDasTransactionTypes
(
	ApprenticeshipContractType int,
	TransactionType int,
)
GO

IF EXISTS(SELECT [object_id] FROM sys.indexes WHERE [name]='IX_ApprenticeshipContractType')
BEGIN
       DROP INDEX [IX_ApprenticeshipContractType] ON [Staging].[NonDasTransactionTypes]
END
GO
 
CREATE INDEX [IX_ApprenticeshipContractType] ON [Staging].[NonDasTransactionTypes] (ApprenticeshipContractType)
GO
 

-----------------------------------------------------------------------------------------------------------------------------------------------
-- LearnerPriceEpisodePerPeriod
-----------------------------------------------------------------------------------------------------------------------------------------------
IF EXISTS(SELECT [object_id] FROM sys.tables WHERE [name]='LearnerPriceEpisodePerPeriod' AND [schema_id] = SCHEMA_ID('Staging'))
BEGIN
	DROP TABLE Staging.LearnerPriceEpisodePerPeriod
END
GO

CREATE TABLE Staging.LearnerPriceEpisodePerPeriod
(
	Ukprn bigint,
	LearnRefNumber varchar(12),
	AimSeqNumber int,
	Period int,
	MaxEpisodeStartDate date
)
GO
CREATE NONCLUSTERED INDEX [IX_DAS_UkPrn_LearnRefNumber_AimSeqNumber_Period_MaxEpisodeStartDate]
ON [Staging].[LearnerPriceEpisodePerPeriod] ([Ukprn],[LearnRefNumber],[AimSeqNumber],[Period],[MaxEpisodeStartDate])
GO

-----------------------------------------------------------------------------------------------------------------------------------------------
-- ApprenticeshipEarningsRequiringPayments
-----------------------------------------------------------------------------------------------------------------------------------------------
IF EXISTS(SELECT [object_id] FROM sys.tables WHERE [name]='ApprenticeshipEarningsRequiringPayments' AND [schema_id] = SCHEMA_ID('Staging'))
BEGIN
	DROP TABLE Staging.ApprenticeshipEarningsRequiringPayments
END
GO

CREATE TABLE Staging.ApprenticeshipEarningsRequiringPayments
(
	Ukprn bigint,
    Uln bigint,
    LearnRefNumber varchar(12),
    AimSeqNumber int,
    Period int,
    PriceEpisodeEndDate date,
    StandardCode bigint,
    ProgrammeType int,
    FrameworkCode int,
    PathwayCode int,
    ApprenticeshipContractType int,
    PriceEpisodeIdentifier varchar(25),
    PriceEpisodeFundLineType varchar(120),
    PriceEpisodeSfaContribPct decimal(15,5),
    PriceEpisodeLevyNonPayInd int,
	EpisodeStartDate date,
	PriceEpisodeOnProgPayment decimal(15,5),
	PriceEpisodeCompletionPayment decimal(15,5),
	PriceEpisodeBalancePayment decimal(15,5),
	PriceEpisodeFirstEmp1618Pay decimal(15,5),
	PriceEpisodeFirstProv1618Pay decimal(15,5),
	PriceEpisodeSecondEmp1618Pay decimal(15,5),
	PriceEpisodeSecondProv1618Pay decimal(15,5),
	PriceEpisodeApplic1618FrameworkUpliftOnProgPayment decimal(15,5),
	PriceEpisodeApplic1618FrameworkUpliftCompletionPayment decimal(15,5),
	PriceEpisodeApplic1618FrameworkUpliftBalancing decimal(15,5),
	PriceEpisodeFirstDisadvantagePayment decimal(15,5),
	PriceEpisodeSecondDisadvantagePayment decimal(15,5),
	LearningSupportPayment decimal(15,5),
	LearnAimRef varchar(8),
	LearningStartDate datetime,
	LearningPlannedEndDate datetime NOT NULL,
	LearningActualEndDate datetime,
	CompletionStatus int,
	CompletionAmount decimal(15,5),
	TotalInstallments int NOT NULL	,
	MonthlyInstallment decimal(15,5) NOT NULL,
	EndpointAssessorId varchar(7)
)

-----------------------------------------------------------------------------------------------------------------------------------------------
-- ApprenticeshipEarnings
-----------------------------------------------------------------------------------------------------------------------------------------------
IF EXISTS(SELECT [object_id] FROM sys.tables WHERE [name]='ApprenticeshipEarnings' AND [schema_id] = SCHEMA_ID('Staging'))
BEGIN
	DROP TABLE Staging.ApprenticeshipEarnings
END
GO

CREATE TABLE Staging.ApprenticeshipEarnings
(
	CommitmentId int,
	CommitmentVersionId varchar(25),
	AccountId int,
	AccountVersionId varchar(25),
	Ukprn bigint,
	Uln bigint,
	LearnRefNumber varchar(12),
	AimSeqNumber int,
	Period int,
	PriceEpisodeEndDate date,
	StandardCode bigint,
	ProgrammeType int,
	FrameworkCode int,
	PathwayCode int,
	ApprenticeshipContractType int,
    PriceEpisodeIdentifier varchar(25),
    PriceEpisodeFundLineType varchar(120),
    PriceEpisodeSfaContribPct decimal(15,5),
    PriceEpisodeLevyNonPayInd int,
	TransactionType int,
	EpisodeStartDate date,
	IsSuccess bit,
	Payable bit,
	PriceEpisodeOnProgPayment decimal(15,5),
	PriceEpisodeCompletionPayment decimal(15,5),
	PriceEpisodeBalancePayment decimal(15,5),
	PriceEpisodeFirstEmp1618Pay decimal(15,5),
	PriceEpisodeFirstProv1618Pay decimal(15,5),
	PriceEpisodeSecondEmp1618Pay decimal(15,5),
	PriceEpisodeSecondProv1618Pay decimal(15,5),
	PriceEpisodeApplic1618FrameworkUpliftOnProgPayment decimal(15,5),
	PriceEpisodeApplic1618FrameworkUpliftCompletionPayment decimal(15,5),
	PriceEpisodeApplic1618FrameworkUpliftBalancing decimal(15,5),
	PriceEpisodeFirstDisadvantagePayment decimal(15,5),
	PriceEpisodeSecondDisadvantagePayment decimal(15,5),
	LearningSupportPayment decimal(15,5),
	LearnAimRef varchar(8),
	LearningStartDate datetime,
	LearningPlannedEndDate datetime NOT NULL,
	LearningActualEndDate datetime,
	CompletionStatus int,
	CompletionAmount decimal(15,5),
	TotalInstallments int NOT NULL	,
	MonthlyInstallment decimal(15,5) NOT NULL,
	EndpointAssessorId varchar(7)
)
GO


IF EXISTS(SELECT [object_id] FROM sys.indexes WHERE [name]='IX_TransType_StartDate')
BEGIN
		DROP INDEX [IX_TransType_StartDate] ON [Staging].[ApprenticeshipEarnings]
END
GO
 
CREATE INDEX [IX_TransType_StartDate] ON Staging.ApprenticeshipEarnings (TransactionType, EpisodeStartDate)
GO
-----------------------------------------------------------------------------------------------------------------------------------------------
-- ApprenticeshipEarnings1
-----------------------------------------------------------------------------------------------------------------------------------------------
IF EXISTS(SELECT [object_id] FROM sys.tables WHERE [name]='ApprenticeshipEarnings1' AND [schema_id] = SCHEMA_ID('Staging'))
BEGIN
	DROP TABLE Staging.ApprenticeshipEarnings1
END
GO

CREATE TABLE Staging.ApprenticeshipEarnings1
(
	CommitmentId bigint,
	CommitmentVersionId varchar(25),
	AccountId bigint,
	AccountVersionId varchar(50),
	Ukprn bigint,
	Uln bigint,
	LearnRefNumber varchar(12),
	AimSeqNumber int,
	Period int,
	PriceEpisodeEndDate date,
	StandardCode bigint,
	ProgrammeType int,
	FrameworkCode int,
	PathwayCode int,
	ApprenticeshipContractType int,
	PriceEpisodeIdentifier varchar(25),
	PriceEpisodeFundLineType varchar(120),
	PriceEpisodeSfaContribPct decimal(15,5),
	PriceEpisodeLevyNonPayInd int,
	TransactionType int,
	EarningAmount decimal(15,5),
	EpisodeStartDate date,
	IsSuccess bit,
	Payable bit,
	LearnAimRef varchar(8),
	LearningStartDate datetime,
	LearningPlannedEndDate datetime NOT NULL,
	LearningActualEndDate datetime,
	CompletionStatus int,
	CompletionAmount decimal(15,5),
	TotalInstallments int NOT NULL,
	MonthlyInstallment decimal(15,5) NOT NULL,
	EndpointAssessorId varchar(7)
)
GO

-----------------------------------------------------------------------------------------------------------------------------------------------
-- ApprenticeshipEarnings2
-----------------------------------------------------------------------------------------------------------------------------------------------
IF EXISTS(SELECT [object_id] FROM sys.tables WHERE [name]='ApprenticeshipEarnings2' AND [schema_id] = SCHEMA_ID('Staging'))
BEGIN
	DROP TABLE Staging.ApprenticeshipEarnings2
END
GO

CREATE TABLE Staging.ApprenticeshipEarnings2
(
	CommitmentId bigint,
	CommitmentVersionId varchar(25),
	AccountId bigint,
	AccountVersionId varchar(50),
	Ukprn bigint,
	Uln bigint,
	LearnRefNumber varchar(12),
	AimSeqNumber int,
	Period int,
	PriceEpisodeEndDate date,
	StandardCode bigint,
	ProgrammeType int,
	FrameworkCode int,
	PathwayCode int,
	ApprenticeshipContractType int,
	PriceEpisodeIdentifier varchar(25),
	PriceEpisodeFundLineType varchar(120),
	PriceEpisodeSfaContribPct decimal(15,5),
	PriceEpisodeLevyNonPayInd int,
	TransactionType int,
	EarningAmount decimal(15,5),
	EpisodeStartDate date,
	IsSuccess bit,
	Payable bit,
	LearnAimRef varchar(8),
	LearningStartDate datetime,
	LearningPlannedEndDate datetime NOT NULL,
	LearningActualEndDate datetime,
	CompletionStatus int,
	CompletionAmount decimal(15,5),
	TotalInstallments int NOT NULL		,
	MonthlyInstallment decimal(15,5) NOT NULL,
	EndpointAssessorId varchar(7)
)
GO

-----------------------------------------------------------------------------------------------------------------------------------------------
-- ApprenticeshipEarnings3
-----------------------------------------------------------------------------------------------------------------------------------------------
IF EXISTS(SELECT [object_id] FROM sys.tables WHERE [name]='ApprenticeshipEarnings3' AND [schema_id] = SCHEMA_ID('Staging'))
BEGIN
	DROP TABLE Staging.ApprenticeshipEarnings3
END
GO

CREATE TABLE Staging.ApprenticeshipEarnings3
(
	CommitmentId bigint,
	CommitmentVersionId varchar(25),
	AccountId bigint,
	AccountVersionId varchar(50),
	Ukprn bigint,
	Uln bigint,
	LearnRefNumber varchar(12),
	AimSeqNumber int,
	Period int,
	PriceEpisodeEndDate date,
	StandardCode bigint,
	ProgrammeType int,
	FrameworkCode int,
	PathwayCode int,
	ApprenticeshipContractType int,
	PriceEpisodeIdentifier varchar(25),
	PriceEpisodeFundLineType varchar(120),
	PriceEpisodeSfaContribPct decimal(15,5),
	PriceEpisodeLevyNonPayInd int,
	TransactionType int,
	EarningAmount decimal(15,5),
	EpisodeStartDate date,
	IsSuccess bit,
	Payable bit,
	LearnAimRef varchar(8),
	LearningStartDate datetime	,
	LearningPlannedEndDate datetime NOT NULL,
	LearningActualEndDate datetime,
	CompletionStatus int,
	CompletionAmount decimal(15,5),
	TotalInstallments int NOT NULL,
	MonthlyInstallment	decimal(15,5) not null,
	EndpointAssessorId varchar(7)
)
GO



-----------------------------------------------------------------------------------------------------------------------------------------------
-- EarningsWithoutPayments
-----------------------------------------------------------------------------------------------------------------------------------------------
IF EXISTS(SELECT [object_id] FROM sys.tables WHERE [name]='EarningsWithoutPayments' AND [schema_id] = SCHEMA_ID('Staging'))
BEGIN
	DROP TABLE Staging.EarningsWithoutPayments
END
GO

CREATE TABLE Staging.EarningsWithoutPayments (
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
	FundingLineType varchar(120),
	UseLevyBalance bit
)
GO

-----------------------------------------------------------------------------------------------------------------------------------------------
-- RawEarnings
-----------------------------------------------------------------------------------------------------------------------------------------------
IF EXISTS(SELECT [object_id] FROM sys.tables WHERE [name]='RawEarnings' AND [schema_id] = SCHEMA_ID('Staging'))
BEGIN
	DROP TABLE Staging.RawEarnings
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
	SfaContributionPct decimal(15,5),
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
	TransactionType15 decimal(15,5),
	ACT tinyint
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
	LearningStartDate date NOT NULL,
	[Period] int NOT NULL,
	ULN bigint NOT NULL,
	ProgrammeType int,
	FrameworkCode int,
	PathwayCode int,
	StandardCode int,
	SfaContributionPct decimal(15,5),
	FundingLineType varchar(100),
	LearnAimRef varchar(8),
	TransactionType13 decimal(15,5),
	TransactionType14 decimal(15,5),
	TransactionType15 decimal(15,5),
	ACT tinyint
)
GO
