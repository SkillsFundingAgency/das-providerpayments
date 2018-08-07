-----------------------------------------------------------------------------------------------------------------------------------------------
-- Temporary Views created on the ILR database for bulk copy purposes
-----------------------------------------------------------------------------------------------------------------------------------------------
IF NOT EXISTS(SELECT [schema_id] FROM sys.schemas WHERE [name]='PaymentsDue')
BEGIN
	EXEC('CREATE SCHEMA PaymentsDue')
END
GO

-----------------------------------------------------------------------------------------------------------------------------------------------
-- VIEWS
-----------------------------------------------------------------------------------------------------------------------------------------------


-----------------------------------------------------------------------------------------------------------------------------------------------
-- vw_RawEarnings
-----------------------------------------------------------------------------------------------------------------------------------------------
IF EXISTS(SELECT [object_id] FROM sys.views WHERE [name]='vw_RawEarnings' AND [schema_id] = SCHEMA_ID('PaymentsDue'))
BEGIN
    DROP VIEW PaymentsDue.vw_RawEarnings
END
GO

CREATE VIEW PaymentsDue.vw_RawEarnings
AS
WITH [Period] AS (
	SELECT ${Period_Id} AS Period_Id
)
SELECT
	[APEP].[LearnRefNumber]
	,[APEP].[Ukprn]
	,[APE].[PriceEpisodeAimSeqNumber] [AimSeqNumber]
	,[APEP].[PriceEpisodeIdentifier]
	,[APE].[EpisodeStartDate]
	,[APE].[EpisodeEffectiveTNPStartDate]
	,[APEP].[Period]
	,[L].[ULN]
	,COALESCE([LD].[ProgType], 0) [ProgrammeType]
	,COALESCE([LD].[FworkCode], 0) [FrameworkCode]
	,COALESCE([LD].[PwayCode], 0) [PathwayCode]
	,COALESCE([LD].[StdCode], 0) [StandardCode]
	,COALESCE([APEP].[PriceEpisodeSFAContribPct], 0) [SfaContributionPercentage]
	,[APE].[PriceEpisodeFundLineType] [FundingLineType]
	,[LD].[LearnAimRef]
	,[LD].[LearnStartDate] [LearningStartDate]
    ,COALESCE([APEP].[PriceEpisodeOnProgPayment], 0) [TransactionType01]
    ,COALESCE([APEP].[PriceEpisodeCompletionPayment], 0) [TransactionType02]
    ,COALESCE([APEP].[PriceEpisodeBalancePayment], 0) [TransactionType03]
    ,COALESCE([APEP].[PriceEpisodeFirstEmp1618Pay], 0) [TransactionType04]
    ,COALESCE([APEP].[PriceEpisodeFirstProv1618Pay], 0) [TransactionType05]
    ,COALESCE([APEP].[PriceEpisodeSecondEmp1618Pay], 0) [TransactionType06]
    ,COALESCE([APEP].[PriceEpisodeSecondProv1618Pay], 0) [TransactionType07]
    ,COALESCE([APEP].[PriceEpisodeApplic1618FrameworkUpliftOnProgPayment], 0) [TransactionType08]
    ,COALESCE([APEP].[PriceEpisodeApplic1618FrameworkUpliftCompletionPayment], 0) [TransactionType09]
    ,COALESCE([APEP].[PriceEpisodeApplic1618FrameworkUpliftBalancing], 0) [TransactionType10]
    ,COALESCE([APEP].[PriceEpisodeFirstDisadvantagePayment], 0) [TransactionType11]
    ,COALESCE([APEP].[PriceEpisodeSecondDisadvantagePayment], 0) [TransactionType12]
	,0 [TransactionType13]
	,0 [TransactionType14]
    ,COALESCE([APEP].[PriceEpisodeLSFCash], 0) [TransactionType15]
    ,CASE WHEN [APE].[PriceEpisodeContractType] = 'Levy Contract' THEN 1 ELSE 2 END [ApprenticeshipContractType]
FROM [Period],
	[Rulebase].[AEC_ApprenticeshipPriceEpisode_Period] [APEP]
INNER JOIN [Rulebase].[AEC_ApprenticeshipPriceEpisode] [APE]
    ON [APEP].[UKPRN] = [APE].[UKPRN]
    AND [APEP].[LearnRefNumber] = [APE].[LearnRefNumber]
    AND [APEP].[PriceEpisodeIdentifier] = [APE].[PriceEpisodeIdentifier]
JOIN [Valid].[Learner] L
	ON [L].[UKPRN] = [APEP].[Ukprn]
	AND [L].[LearnRefNumber] = [APEP].[LearnRefNumber]
JOIN [Valid].[LearningDelivery] LD
	ON [LD].[UKPRN] = [APEP].[Ukprn]
	AND [LD].[LearnRefNumber] = [APEP].[LearnRefNumber]
	AND [LD].[AimSeqNumber] = [APE].[PriceEpisodeAimSeqNumber]
WHERE (
    [APEP].[PriceEpisodeOnProgPayment] != 0
    OR [APEP].[PriceEpisodeCompletionPayment] != 0
    OR [APEP].[PriceEpisodeBalancePayment] != 0
	OR [APEP].[PriceEpisodeFirstEmp1618Pay] != 0
	OR [APEP].[PriceEpisodeFirstProv1618Pay] != 0
	OR [APEP].[PriceEpisodeSecondEmp1618Pay] != 0
	OR [APEP].[PriceEpisodeSecondProv1618Pay] != 0
	OR [APEP].[PriceEpisodeApplic1618FrameworkUpliftOnProgPayment] != 0
	OR [APEP].[PriceEpisodeApplic1618FrameworkUpliftCompletionPayment] != 0
	OR [APEP].[PriceEpisodeApplic1618FrameworkUpliftBalancing] != 0
	OR [APEP].[PriceEpisodeFirstDisadvantagePayment] != 0
	OR [APEP].[PriceEpisodeSecondDisadvantagePayment] != 0
	OR [APEP].[PriceEpisodeLSFCash] != 0
	OR [APEP].[Period] = 1
    )
	AND [APEP].[Period] <= Period_Id
GO

-----------------------------------------------------------------------------------------------------------------------------------------------
-- vw_RawEarningsMathsEnglish
-----------------------------------------------------------------------------------------------------------------------------------------------
IF EXISTS(SELECT [object_id] FROM sys.views WHERE [name]='vw_RawEarningsMathsEnglish' AND [schema_id] = SCHEMA_ID('PaymentsDue'))
BEGIN
    DROP VIEW PaymentsDue.vw_RawEarningsMathsEnglish
END
GO

CREATE VIEW PaymentsDue.vw_RawEarningsMathsEnglish
AS
WITH [Period] AS (
	SELECT ${Period_Id} AS Period_Id
)
select
	[LDP].[LearnRefNumber]
	,[LDP].[Ukprn]
	,[LDP].[AimSeqNumber]
	,NULL [PriceEpisodeIdentifier]
	,NULL [EpisodeStartDate]
	,NULL [EpisodeEffectiveTNPStartDate]
	,[LDP].[Period]
	,[L].[ULN]
	,COALESCE([LD].[ProgType], 0) [ProgrammeType]
	,COALESCE([LD].[FworkCode], 0) [FrameworkCode]
	,COALESCE([LD].[PwayCode], 0) [PathwayCode]
	,COALESCE([LD].[StdCode], 0) [StandardCode]
	,COALESCE([LDP].[LearnDelSFAContribPct], 0) [SfaContributionPercentage]
	,[LDP].[FundLineType] [FundingLineType]
	,[LD].[LearnAimRef]
	,[LD].[LearnStartDate] [LearningStartDate]
	,0 [TransactionType01]
	,0 [TransactionType02]
	,0 [TransactionType03]
	,0 [TransactionType04]
	,0 [TransactionType05]
	,0 [TransactionType06]
	,0 [TransactionType07]
	,0 [TransactionType08]
	,0 [TransactionType09]
	,0 [TransactionType10]
	,0 [TransactionType11]
	,0 [TransactionType12]
    ,COALESCE([MathEngOnProgPayment], 0) [TransactionType13]
    ,COALESCE([MathEngBalPayment], 0) [TransactionType14]
    ,COALESCE([LearnSuppFundCash], 0) [TransactionType15]
    ,CASE WHEN [LDP].[LearnDelContType] = 'Levy Contract' THEN 1 ELSE 2 END [ApprenticeshipContractType]
FROM [Period],
	[Rulebase].[AEC_LearningDelivery_Period] LDP
INNER JOIN [Valid].[LearningDelivery] LD
    ON [LD].[UKPRN] = [LDP].[UKPRN]
    AND [LD].[LearnRefNumber] = [LDP].[LearnRefNumber]
    AND [LD].[AimSeqNumber] = [LDP].[AimSeqNumber]
JOIN [Valid].[Learner] L
	ON [L].[UKPRN] = [LD].[Ukprn]
	AND [L].[LearnRefNumber] = [LD].[LearnRefNumber]
WHERE (
    MathEngOnProgPayment != 0
    OR MathEngBalPayment != 0
    OR LearnSuppFundCash != 0
    )
    AND LDP.[Period] <= Period_Id
    AND LD.LearnAimRef != 'ZPROG001'
GO



-----------------------------------------------------------------------------------------------------------------------------------------------
-- vw_IlrBreakdown 
-----------------------------------------------------------------------------------------------------------------------------------------------
IF EXISTS(SELECT [object_id] FROM sys.views WHERE [name]='vw_IlrBreakdown' AND [schema_id] = SCHEMA_ID('PaymentsDue'))
BEGIN
    DROP VIEW PaymentsDue.vw_IlrBreakdown
END
GO

CREATE VIEW PaymentsDue.vw_IlrBreakdown
AS
SELECT 
	[APE].[UKPRN]
	,[APE].[LearnRefNumber]
	,[PriceEpisodeIdentifier]
	,[LD].[LearnStartDate] [StartDate]
	,[LD].[LearnPlanEndDate] [PlannedEndDate]
	,[LD].[LearnActEndDate] [ActualEndDate]
	,[LD].[CompStatus] [CompletionStatus]
	,[RLD].[PlannedNumOnProgInstalm] [PlannedInstalments]
	,[PriceEpisodeCompletionElement] [CompletionPayment]
	,[PriceEpisodeInstalmentValue] [Instalment]
	,[PriceEpisodeCumulativePMRs] [EmployerPayments]
	,[EPAOrgID] [EndpointAssessorId]
FROM [Rulebase].[AEC_ApprenticeshipPriceEpisode] APE
INNER JOIN [Valid].[LearningDelivery] LD
	ON [APE].[UKPRN] = [LD].[UKPRN]
	AND [APE].[LearnRefNumber] = [LD].[LearnRefNumber]
	AND [APE].[PriceEpisodeAimSeqNumber] = [LD].[AimSeqNumber]
INNER JOIN [Rulebase].[AEC_LearningDelivery] RLD
	ON [APE].[UKPRN] = [RLD].[UKPRN]
	AND [APE].[LearnRefNumber] = [RLD].[LearnRefNumber]
	AND [APE].[PriceEpisodeAimSeqNumber] = [RLD].[AimSeqNumber]
GO