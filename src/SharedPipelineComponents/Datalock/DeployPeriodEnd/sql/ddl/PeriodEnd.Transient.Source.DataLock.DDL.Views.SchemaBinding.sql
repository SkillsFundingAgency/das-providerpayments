IF NOT EXISTS(SELECT [schema_id] FROM sys.schemas WHERE [name]='DataLock')
BEGIN
    EXEC('CREATE SCHEMA DataLock')
END
GO

-----------------------------------------------------------------------------------------------------------------------------------------------
-- vw_DEDS_PriceEpisodePeriodMatch
-----------------------------------------------------------------------------------------------------------------------------------------------
IF EXISTS(SELECT [object_id] FROM sys.views WHERE [name]='vw_DEDS_PriceEpisodePeriodMatch' AND [schema_id] = SCHEMA_ID('DataLock'))
BEGIN
    DROP VIEW DataLock.vw_DEDS_PriceEpisodePeriodMatch
END
GO

CREATE VIEW DataLock.vw_DEDS_PriceEpisodePeriodMatch
AS
	SELECT pepm.*
	FROM DataLock.PriceEpisodePeriodMatch pepm
	INNER MERGE JOIN [Reference].[ApprenticeshipPriceEpisode_Period] pe 
	  ON 
		pepm.Ukprn = pe.Ukprn AND
		pepm.[LearnRefNumber] = pe.[LearnRefNumber] AND
		pe.[Period] = pepm.[Period] AND
		pe.[PriceEpisodeIdentifier] = pepm.[PriceEpisodeIdentifier]  
	INNER JOIN [Reference].[Providers] p
		ON p.UKPRN = pepm.UKPRN
	WHERE 
	((pepm.TransactionType = 1 and IsNull(PriceEpisodeOnProgPayment,0) <> 0 )
	OR (pepm.TransactionType = 2 and IsNull(PriceEpisodeCompletionPayment,0) <> 0 )
	OR (pepm.TransactionType = 3 and IsNull(PriceEpisodeBalancePayment,0) <> 0 )
	OR (pepm.TransactionType = 4 and IsNull(PriceEpisodeFirstEmp1618Pay,0) <> 0 )
	OR (pepm.TransactionType = 5 and IsNull(PriceEpisodeFirstProv1618Pay,0) <> 0 )
	OR (pepm.TransactionType = 6 and IsNull(PriceEpisodeSecondEmp1618Pay,0) <> 0 )
	OR (pepm.TransactionType = 7 and IsNull(PriceEpisodeSecondProv1618Pay,0) <> 0 )
	OR (pepm.TransactionType = 8 and IsNull(PriceEpisodeApplic1618FrameworkUpliftOnProgPayment,0) <> 0 )
	OR (pepm.TransactionType = 9 and IsNull(PriceEpisodeApplic1618FrameworkUpliftCompletionPayment,0) <> 0 )
	OR (pepm.TransactionType = 10 and IsNull(PriceEpisodeApplic1618FrameworkUpliftBalancing,0) <> 0 )
	OR (pepm.TransactionType = 11 and IsNull(PriceEpisodeFirstDisadvantagePayment,0) <> 0 )
	OR (pepm.TransactionType = 12 and IsNull(PriceEpisodeSecondDisadvantagePayment,0) <> 0 )
	OR pepm.TransactionType > 12)
GO

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
        [Ukprn],
        [LearnRefNumber],
        [Uln],
        [NiNumber],
        [AimSeqNumber],
        [StandardCode],
        [ProgrammeType],
        [FrameworkCode],
        [PathwayCode],
        [StartDate],
        [NegotiatedPrice],
        [PriceEpisodeIdentifier],
        [EndDate],
		[PriceEpisodeFirstAdditionalPaymentThresholdDate] AS FirstAdditionalPaymentThresholdDate,
		[PriceEpisodeSecondAdditionalPaymentThresholdDate] as SecondAdditionalPaymentThresholdDate
    FROM Reference.DataLockPriceEpisode
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
    p.Ukprn
FROM Reference.Providers p
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
    cp.[CollectionPeriodYear]
FROM DataLock.vw_DEDS_PriceEpisodePeriodMatch pepm
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
IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.VIEWS WHERE TABLE_NAME = 'vw_commitments' AND TABLE_SCHEMA = 'DataLock')
BEGIN
	DROP VIEW DataLock.vw_Commitments
END
GO
    
CREATE VIEW DataLock.vw_Commitments
WITH SCHEMABINDING
AS
SELECT c.[CommitmentId]
      ,c.[VersionId]
      ,c.[Uln]
      ,c.[Ukprn] AS UKPRN
	  ,x.[Ukprn] AS ProviderUKPRN
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
      
FROM Reference.[DasCommitments] c
  INNER JOIN [Reference].[DataLockPriceEpisode] x
  ON x.ULN = c.Uln

GO