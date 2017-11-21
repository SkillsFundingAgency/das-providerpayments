IF NOT EXISTS(SELECT [schema_id] FROM sys.schemas WHERE [name]='DataLock')
BEGIN
    EXEC('CREATE SCHEMA DataLock')
END

-----------------------------------------------------------------------------------------------------------------------------------------------
-- vw_PriceEpisode
-----------------------------------------------------------------------------------------------------------------------------------------------
IF EXISTS(SELECT [object_id] FROM sys.views WHERE [name]='vw_PriceEpisode' AND [schema_id] = SCHEMA_ID('DataLock'))
BEGIN
    DROP VIEW DataLock.vw_PriceEpisode
END
GO

CREATE VIEW DataLock.vw_PriceEpisode
AS 
    SELECT
        (SELECT [UKPRN] FROM [Valid].[LearningProvider]) AS [Ukprn],
        l.[LearnRefNumber] AS [LearnRefNumber],
        l.[ULN] AS [Uln],
        l.[NINumber] AS [NiNumber],
        ld.[AimSeqNumber] AS [AimSeqNumber],
        ld.[StdCode] AS [StandardCode],
        ld.[ProgType] AS [ProgrammeType],
        ld.[FworkCode] AS [FrameworkCode],
        ld.[PwayCode] AS [PathwayCode],
        ape.[EpisodeEffectiveTNPStartDate] AS [StartDate],
        ape.[PriceEpisodeTotalTNPPrice] AS [NegotiatedPrice],
        ape.[PriceEpisodeIdentifier],
        COALESCE(ape.[PriceEpisodeActualEndDate], ape.[PriceEpisodePlannedEndDate]) AS [EndDate],
		ape.[PriceEpisodeFirstAdditionalPaymentThresholdDate] AS FirstAdditionalPaymentThresholdDate,
		ape.[PriceEpisodeSecondAdditionalPaymentThresholdDate] as SecondAdditionalPaymentThresholdDate
    FROM [Rulebase].[AEC_ApprenticeshipPriceEpisode] ape
       JOIN [Valid].[Learner] l ON ape.[LearnRefNumber] = l.[LearnRefNumber]
       JOIN [Valid].[LearningDelivery] ld ON ape.[LearnRefNumber] = ld.[LearnRefNumber]
          AND ape.[PriceEpisodeAimSeqNumber] = ld.[AimSeqNumber]
    WHERE ape.PriceEpisodeContractType = 'Levy Contract'
GO

-----------------------------------------------------------------------------------------------------------------------------------------------
-- vw_Providers
-----------------------------------------------------------------------------------------------------------------------------------------------
IF EXISTS(SELECT [object_id] FROM sys.views WHERE [name]='vw_Providers' AND [schema_id] = SCHEMA_ID('DataLock'))
BEGIN
    DROP VIEW DataLock.vw_Providers
END
GO

CREATE VIEW DataLock.vw_Providers
AS
SELECT
    p.UKPRN
FROM Valid.LearningProvider p
GO

-----------------------------------------------------------------------------------------------------------------------------------------------
-- vw_ValidationError
-----------------------------------------------------------------------------------------------------------------------------------------------
IF EXISTS(SELECT [object_id] FROM sys.views WHERE [name]='vw_ValidationError' AND [schema_id] = SCHEMA_ID('DataLock'))
BEGIN
    DROP VIEW DataLock.vw_ValidationError
END
GO

CREATE VIEW DataLock.vw_ValidationError
AS
SELECT 
    ve.[Ukprn],
    ve.[LearnRefNumber],
    ve.[AimSeqNumber],
    ve.[RuleId],
    ve.[PriceEpisodeIdentifier],
    cp.[CollectionPeriodName],
    cp.[CollectionPeriodMonth],
    cp.[CollectionPeriodYear]
FROM DataLock.ValidationError ve
    CROSS JOIN (
       SELECT TOP 1
          '${YearOfCollection}-' + [Name] AS [CollectionPeriodName],
          [CalendarMonth] AS [CollectionPeriodMonth],
          [CalendarYear] AS [CollectionPeriodYear]
       FROM [Reference].[CollectionPeriods]
       WHERE [Open] = 1
    ) cp
GO

-----------------------------------------------------------------------------------------------------------------------------------------------
-- vw_PriceEpisodeMatch
-----------------------------------------------------------------------------------------------------------------------------------------------
IF EXISTS(SELECT [object_id] FROM sys.views WHERE [name]='vw_PriceEpisodeMatch' AND [schema_id] = SCHEMA_ID('DataLock'))
BEGIN
    DROP VIEW DataLock.vw_PriceEpisodeMatch
END
GO

CREATE VIEW DataLock.vw_PriceEpisodeMatch
AS
SELECT 
    pem.[Ukprn],
    pem.[PriceEpisodeIdentifier],
    pem.[LearnRefNumber],
    pem.[AimSeqNumber],
    pem.[CommitmentId],
    cp.[CollectionPeriodName],
    cp.[CollectionPeriodMonth],
    cp.[CollectionPeriodYear],
	pem.[IsSuccess] 
FROM DataLock.PriceEpisodeMatch pem
    CROSS JOIN (
       SELECT TOP 1
          '${YearOfCollection}-' + [Name] AS [CollectionPeriodName],
          [CalendarMonth] AS [CollectionPeriodMonth],
          [CalendarYear] AS [CollectionPeriodYear]
       FROM [Reference].[CollectionPeriods]
       WHERE [Open] = 1
    ) cp
GO

-----------------------------------------------------------------------------------------------------------------------------------------------
-- vw_PriceEpisodePeriodMatch
-----------------------------------------------------------------------------------------------------------------------------------------------
IF EXISTS(SELECT [object_id] FROM sys.views WHERE [name]='vw_PriceEpisodePeriodMatch' AND [schema_id] = SCHEMA_ID('DataLock'))
BEGIN
    DROP VIEW DataLock.vw_PriceEpisodePeriodMatch
END
GO

CREATE VIEW DataLock.vw_PriceEpisodePeriodMatch
AS
SELECT 
    pepm.[Ukprn],
    pepm.[PriceEpisodeIdentifier],
    pepm.[LearnRefNumber],
    pepm.[AimSeqNumber],
    pepm.[CommitmentId],
    pepm.[VersionId],
    pepm.[Period],
    pepm.[Payable],
	pepm.[TransactionType],
    cp.[CollectionPeriodName],
    cp.[CollectionPeriodMonth],
    cp.[CollectionPeriodYear],
	pepm.TransactionTypesFlag
FROM DataLock.PriceEpisodePeriodMatch pepm
    CROSS JOIN (
       SELECT TOP 1
          '${YearOfCollection}-' + [Name] AS [CollectionPeriodName],
          [CalendarMonth] AS [CollectionPeriodMonth],
          [CalendarYear] AS [CollectionPeriodYear]
       FROM [Reference].[CollectionPeriods]
       WHERE [Open] = 1
    ) cp
GO


-----------------------------------------------------------------------------------------------------------------------------------------------
-- vw_Commitments
-----------------------------------------------------------------------------------------------------------------------------------------------
IF EXISTS(SELECT [object_id] FROM sys.views WHERE [name]='vw_Commitments' AND [schema_id] = SCHEMA_ID('DataLock'))
BEGIN
    DROP VIEW DataLock.vw_Commitments
END
GO

CREATE VIEW DataLock.vw_Commitments
AS
SELECT [CommitmentId]
      ,[VersionId]
      ,c.[Uln]
      ,c.[Ukprn] as UKPRN
	  ,x.[Ukprn] as ProviderUKPRN
      ,[AccountId]
      ,[StartDate]
      ,[EndDate]
      ,[AgreedCost]
      ,[StandardCode]
      ,[ProgrammeType]
      ,[FrameworkCode]
      ,[PathwayCode]
      ,[PaymentStatus]
      ,[PaymentStatusDescription]
      ,[Priority]
	  ,[EffectiveFrom]
	  ,[EffectiveTo]
      
  FROM Reference.[DasCommitments] c
  JOIN
  (SELECT (SELECT UKPRN FROM VALID.LEARNINGPROVIDER) AS UKPRN,
        L.ULN FROM VALID.LEARNER L) X
	ON X.ULN  = C.ULN
 
GO



-----------------------------------------------------------------------------------------------------------------------------------------------
-- vw_PriceEpisode
-----------------------------------------------------------------------------------------------------------------------------------------------
IF EXISTS(SELECT [object_id] FROM sys.views WHERE [name]='vw_16To18IncentiveEarnings' AND [schema_id] = SCHEMA_ID('DataLock'))
BEGIN
    DROP VIEW DataLock.vw_16To18IncentiveEarnings
END
GO

CREATE VIEW DataLock.vw_16To18IncentiveEarnings
AS 
   
    SELECT
        (SELECT [UKPRN] FROM [Valid].[LearningProvider]) AS [Ukprn],
        ape.[LearnRefNumber] AS [LearnRefNumber],
		p.Period,
		p.PriceEpisodeFirstEmp1618Pay,
		p.PriceEpisodeSecondEmp1618Pay,
		ape.PriceEpisodeIdentifier
    FROM [Rulebase].[AEC_ApprenticeshipPriceEpisode] ape
	JOIN [Rulebase].[AEC_ApprenticeshipPriceEpisode_Period] p
		On p.LearnRefNumber = ape.LearnRefNumber 
		And p.PriceEpisodeIdentifier = ape.PriceEpisodeIdentifier
    WHERE ape.PriceEpisodeContractType = 'Levy Contract'
	And (IsNull(p.PriceEpisodeFirstEmp1618Pay,0) <> 0 OR IsNull(p.PriceEpisodeSecondEmp1618Pay,0) <> 0)
	
GO
