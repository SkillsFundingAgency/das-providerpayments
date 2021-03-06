IF NOT EXISTS(SELECT [schema_id] FROM sys.schemas WHERE [name]='Rulebase')
BEGIN
	EXEC('CREATE SCHEMA Rulebase')
END
GO

-----------------------------------------------------------------------------------------------------------------------------------------------
-- AEC_ApprenticeshipPriceEpisode
-----------------------------------------------------------------------------------------------------------------------------------------------
IF EXISTS(SELECT [object_id] FROM sys.tables WHERE [name]='AEC_ApprenticeshipPriceEpisode' AND [schema_id] = SCHEMA_ID('Rulebase'))
BEGIN
	DROP TABLE Rulebase.AEC_ApprenticeshipPriceEpisode
END
GO

CREATE TABLE [Rulebase].[AEC_ApprenticeshipPriceEpisode]
(
	[Ukprn] bigint NOT NULL,
	[LearnRefNumber] varchar(12),
	[PriceEpisodeIdentifier] varchar(25),
	[EpisodeEffectiveTNPStartDate] date,
	[EpisodeStartDate] date,
	[PriceEpisodeActualEndDate] date,
	[PriceEpisodeActualInstalments] int,
	[PriceEpisodeAimSeqNumber] int,
	[PriceEpisodeCappedRemainingTNPAmount] decimal(10,5),
	[PriceEpisodeCompleted] bit,
	[PriceEpisodeCompletionElement] decimal(10,5),
	[PriceEpisodeExpectedTotalMonthlyValue] decimal(10,5),
	[PriceEpisodeInstalmentValue] decimal(10,5),
	[PriceEpisodePlannedEndDate] date,
	[PriceEpisodePlannedInstalments] int,
	[PriceEpisodePreviousEarnings] decimal(10,5),
	[PriceEpisodeRemainingAmountWithinUpperLimit] decimal(10,5),
	[PriceEpisodeRemainingTNPAmount] decimal(10,5),
	[PriceEpisodeTotalEarnings] decimal(10,5),
	[PriceEpisodeTotalTNPPrice] decimal(10,5),
	[PriceEpisodeUpperBandLimit] decimal(10,5),
	[PriceEpisodeUpperLimitAdjustment] decimal(10,5),
	[TNP1] decimal(10,5),
	[TNP2] decimal(10,5),
	[TNP3] decimal(10,5),
	[TNP4] decimal(10,5),
	PriceEpisodeFirstAdditionalPaymentThresholdDate date NULL,
	PriceEpisodeSecondAdditionalPaymentThresholdDate date NULL,
	PriceEpisodeContractType varchar(50) NULL
	primary key clustered
	(
		[Ukprn] asc,
		[LearnRefNumber] asc,
		[PriceEpisodeIdentifier] asc
	)
)
GO

-----------------------------------------------------------------------------------------------------------------------------------------------
-- AEC_ApprenticeshipPriceEpisode_Period
-----------------------------------------------------------------------------------------------------------------------------------------------
IF EXISTS(SELECT [object_id] FROM sys.tables WHERE [name]='AEC_ApprenticeshipPriceEpisode_Period' AND [schema_id] = SCHEMA_ID('Rulebase'))
BEGIN
	DROP TABLE Rulebase.AEC_ApprenticeshipPriceEpisode_Period
END
GO

CREATE TABLE [Rulebase].[AEC_ApprenticeshipPriceEpisode_Period]
(
	[Ukprn] bigint NOT NULL,
	[LearnRefNumber] varchar(12),
	[PriceEpisodeIdentifier] varchar(25),
	[Period] int,
	[PriceEpisodeApplic1618FrameworkUpliftBalancing] decimal(10,5),
	[PriceEpisodeApplic1618FrameworkUpliftCompletionPayment] decimal(10,5),
	[PriceEpisodeApplic1618FrameworkUpliftOnProgPayment] decimal(10,5),
	[PriceEpisodeBalancePayment] decimal(10,5),
	[PriceEpisodeBalanceValue] decimal(10,5),
	[PriceEpisodeCompletionPayment] decimal(10,5),
	[PriceEpisodeFirstDisadvantagePayment] decimal(10,5),
	[PriceEpisodeFirstEmp1618Pay] decimal(10,5),
	[PriceEpisodeFirstProv1618Pay] decimal(10,5),
	[PriceEpisodeFundLineType] varchar(120),
	[PriceEpisodeInstalmentsThisPeriod] int,
	[PriceEpisodeLSFCash] decimal(10,5),
	[PriceEpisodeOnProgPayment] decimal(10,5),
	[PriceEpisodeSecondDisadvantagePayment] decimal(10,5),
	[PriceEpisodeSecondEmp1618Pay] decimal(10,5),
	[PriceEpisodeSecondProv1618Pay] decimal(10,5),
	primary key clustered
	(
		[Ukprn] asc,
		[LearnRefNumber] asc,
		[PriceEpisodeIdentifier] asc,
		[Period] asc
	)
)
GO

-----------------------------------------------------------------------------------------------------------------------------------------------
-- AEC_ApprenticeshipPriceEpisode_PeriodisedValues
-----------------------------------------------------------------------------------------------------------------------------------------------
IF EXISTS(SELECT [object_id] FROM sys.tables WHERE [name]='AEC_ApprenticeshipPriceEpisode_PeriodisedValues' AND [schema_id] = SCHEMA_ID('Rulebase'))
BEGIN
	DROP TABLE Rulebase.AEC_ApprenticeshipPriceEpisode_PeriodisedValues
END
GO

CREATE TABLE [Rulebase].[AEC_ApprenticeshipPriceEpisode_PeriodisedValues]
(
	[Ukprn] bigint NOT NULL,
	[LearnRefNumber] varchar(12),
	[PriceEpisodeIdentifier] varchar(25),
	[AttributeName] varchar(100) not null,
	[Period_1] decimal(15,5),
	[Period_2] decimal(15,5),
	[Period_3] decimal(15,5),
	[Period_4] decimal(15,5),
	[Period_5] decimal(15,5),
	[Period_6] decimal(15,5),
	[Period_7] decimal(15,5),
	[Period_8] decimal(15,5),
	[Period_9] decimal(15,5),
	[Period_10] decimal(15,5),
	[Period_11] decimal(15,5),
	[Period_12] decimal(15,5),
	primary key clustered
	(
		[Ukprn] asc,
		[LearnRefNumber] asc,
		[PriceEpisodeIdentifier] asc,
		[AttributeName] asc
	)
)
GO

-----------------------------------------------------------------------------------------------------------------------------------------------
-- AEC_ApprenticeshipPriceEpisode_PeriodisedTextValues
-----------------------------------------------------------------------------------------------------------------------------------------------
IF EXISTS(SELECT [object_id] FROM sys.tables WHERE [name]='AEC_ApprenticeshipPriceEpisode_PeriodisedTextValues' AND [schema_id] = SCHEMA_ID('Rulebase'))
BEGIN
	DROP TABLE Rulebase.AEC_ApprenticeshipPriceEpisode_PeriodisedTextValues
END
GO

CREATE TABLE [Rulebase].[AEC_ApprenticeshipPriceEpisode_PeriodisedTextValues]
(
	[Ukprn] bigint NOT NULL,
	[LearnRefNumber] varchar(12),
	[PriceEpisodeIdentifier] varchar(25),
	[AttributeName] varchar(100) not null,
	[Period_1] varchar(255),
	[Period_2] varchar(255),
	[Period_3] varchar(255),
	[Period_4] varchar(255),
	[Period_5] varchar(255),
	[Period_6] varchar(255),
	[Period_7] varchar(255),
	[Period_8] varchar(255),
	[Period_9] varchar(255),
	[Period_10] varchar(255),
	[Period_11] varchar(255),
	[Period_12] varchar(255),
	primary key clustered
	(
		[Ukprn] asc,
		[LearnRefNumber] asc,
		[PriceEpisodeIdentifier] asc,
		[AttributeName] asc
	)
)
GO
