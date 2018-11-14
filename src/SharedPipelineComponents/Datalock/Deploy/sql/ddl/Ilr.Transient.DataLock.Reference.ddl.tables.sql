IF NOT EXISTS(SELECT [schema_id] FROM sys.schemas WHERE [name]='Reference')
BEGIN
	EXEC('CREATE SCHEMA Reference')
END
GO

-----------------------------------------------------------------------------------------------------------------------------------------------
-- DataLockPriceEpisode
-----------------------------------------------------------------------------------------------------------------------------------------------
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

CREATE INDEX IX_DataLockPriceEpisode_Uln ON Reference.DataLockPriceEpisode (Uln)


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
	[PriceEpisodeFirstEmp1618Pay] decimal(10,5),
	[PriceEpisodeSecondEmp1618Pay] decimal(10,5),
)
GO

CREATE CLUSTERED INDEX [IDX_ApprenticeshipPriceEpisode_Period_ukprn] ON 
	Reference.ApprenticeshipPriceEpisode_Period ([Ukprn],[LearnRefNumber],[PriceEpisodeIdentifier],[Period])
GO
