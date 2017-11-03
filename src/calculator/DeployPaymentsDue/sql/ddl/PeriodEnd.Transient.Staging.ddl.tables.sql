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
    PriceEpisodeFundLineType varchar(60),
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
    PriceEpisodeFundLineType varchar(60),
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
	PriceEpisodeFundLineType varchar(60),
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
	PriceEpisodeFundLineType varchar(60),
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
	PriceEpisodeFundLineType varchar(60),
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
	MonthlyInstallment	decimal(15,5) not null
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
	FundingLineType varchar(60),
	UseLevyBalance bit
)
GO
