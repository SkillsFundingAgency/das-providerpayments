IF NOT EXISTS(SELECT [schema_id] FROM sys.schemas WHERE [name]='Reference')
BEGIN
	EXEC('CREATE SCHEMA Reference')
END
GO

-----------------------------------------------------------------------------------------------------------------------------------------------
-- DataLockPriceEpisode
-----------------------------------------------------------------------------------------------------------------------------------------------
IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.VIEWS WHERE TABLE_NAME = 'vw_commitments' AND TABLE_SCHEMA = 'DataLock')
BEGIN
	DROP VIEW DataLock.vw_Commitments
END
GO

IF EXISTS(SELECT [object_id] FROM sys.tables WHERE [name]='DataLockPriceEpisode' AND [schema_id] = SCHEMA_ID('Reference'))
BEGIN
	DROP TABLE Reference.DataLockPriceEpisode
END
GO

CREATE TABLE Reference.DataLockPriceEpisode (
	[Ukprn] bigint NOT NULL,
	[LearnRefNumber] varchar(12) NOT NULL,
	[Uln] bigint NOT NULL,
	[NiNumber] varchar(9) NULL,
	[AimSeqNumber] int NOT NULL,
	[StandardCode] bigint NULL,
	[ProgrammeType] int NULL,
	[FrameworkCode] int NULL,
	[PathwayCode] int NULL,
	[StartDate] date NOT NULL,
	[NegotiatedPrice] int NOT NULL,
	[PriceEpisodeIdentifier] varchar(25) NOT NULL,
	[EndDate] date NOT NULL,
	[PriceEpisodeFirstAdditionalPaymentThresholdDate] date NULL,
	[PriceEpisodeSecondAdditionalPaymentThresholdDate] date NULL,
	[Tnp1] decimal(15,5) NULL,
	[Tnp2] decimal(15,5) NULL,
	[Tnp3] decimal(15,5) NULL,
	[Tnp4] decimal(15,5) NULL,
	[LearningStartDate] date NULL,
	[EffectiveToDate] date NULL
)
GO

CREATE CLUSTERED INDEX [IDX_DataLockPriceEpisode_Ukprn] ON Reference.DataLockPriceEpisode ([Ukprn])
GO




---------------------------------------------------------------------------------------------------------------------------------------------
-- ApprenticeshipPriceEpisode_Period
-----------------------------------------------------------------------------------------------------------------------------------------------
IF EXISTS(SELECT [object_id] FROM sys.tables WHERE [name]='ApprenticeshipPriceEpisode_Period' AND [schema_id] = SCHEMA_ID('Reference'))
BEGIN
	DROP TABLE Reference.ApprenticeshipPriceEpisode_Period
END
GO

CREATE TABLE Reference.ApprenticeshipPriceEpisode_Period
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
	[PriceEpisodeFundLineType] varchar(60),
	[PriceEpisodeInstalmentsThisPeriod] int,
	[PriceEpisodeLSFCash] decimal(10,5),
	[PriceEpisodeOnProgPayment] decimal(10,5),
	[PriceEpisodeSecondDisadvantagePayment] decimal(10,5),
	[PriceEpisodeSecondEmp1618Pay] decimal(10,5),
	[PriceEpisodeSecondProv1618Pay] decimal(10,5)
	
)
GO

CREATE CLUSTERED INDEX [IDX_ApprenticeshipPriceEpisode_Period_ukprn] ON 
	Reference.ApprenticeshipPriceEpisode_Period ([Ukprn],[LearnRefNumber],[PriceEpisodeIdentifier],[Period])
GO