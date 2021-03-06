

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
-- vw_ValidationErrorByPeriod
-----------------------------------------------------------------------------------------------------------------------------------------------
IF EXISTS(SELECT [object_id] FROM sys.views WHERE [name]='vw_ValidationErrorByPeriod' AND [schema_id] = SCHEMA_ID('DataLock'))
BEGIN
    DROP VIEW DataLock.vw_ValidationErrorByPeriod
END
GO

CREATE VIEW DataLock.vw_ValidationErrorByPeriod
AS
SELECT 
    ve.[Ukprn],
    ve.[LearnRefNumber],
    ve.[AimSeqNumber],
    ve.[RuleId],
    ve.[PriceEpisodeIdentifier],
	ve.[Period],
    cp.[CollectionPeriodName],
    cp.[CollectionPeriodMonth],
    cp.[CollectionPeriodYear]
FROM DataLock.ValidationErrorByPeriod ve
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
SELECT c.[CommitmentId]
      ,c.[VersionId]
      ,c.[Uln]
      ,c.[Ukprn] as UKPRN
	  ,l.UKPRN as ProviderUKPRN
      ,c.[AccountId]
      ,c.[StartDate]
      ,c.[EndDate]
      ,c.[AgreedCost]
      ,c.[StandardCode]
      ,c.[ProgrammeType]
      ,c.[FrameworkCode]
      ,c.[PathwayCode]
      ,c.[PaymentStatus]
      ,c.[PaymentStatusDescription]
      ,c.[Priority]
	  ,c.[EffectiveFrom]
	  ,c.[EffectiveTo]
      ,c.[WithdrawnOnDate]
	  ,c.[PausedOnDate]

  FROM Reference.[DasCommitments] c
  CROSS JOIN Valid.LearningProvider l 
  
GO


