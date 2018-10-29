-----------------------------------------------------------------------------------------------------------------------------------------------
-- RawEarnings
-----------------------------------------------------------------------------------------------------------------------------------------------
IF EXISTS(SELECT [object_id] FROM sys.views WHERE [name]='RawEarnings' AND [schema_id] = SCHEMA_ID('Staging'))
BEGIN
    DROP VIEW Staging.RawEarnings
END
GO

-- This is for our test framework... with one intra database rather than two
IF EXISTS(SELECT [object_id] FROM sys.tables WHERE [name]='RawEarnings' AND [schema_id] = SCHEMA_ID('Staging'))
BEGIN
    DROP TABLE Staging.RawEarnings
END
GO

CREATE VIEW Staging.RawEarnings
AS
SELECT
	[APEP].[LearnRefNumber]
	,[LP].[Ukprn]
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
    ,COALESCE([APEP].[PriceEpisodeLearnerAdditionalPayment], 0) [TransactionType16]
    ,CASE WHEN [APE].[PriceEpisodeContractType] = 'Levy Contract' THEN 1 ELSE 2 END [ApprenticeshipContractType]
	,[APE].[PriceEpisodeFirstAdditionalPaymentThresholdDate] [FirstIncentiveCensusDate]
	,[APE].[PriceEpisodeSecondAdditionalPaymentThresholdDate] [SecondIncentiveCensusDate]
	,[APE].[PriceEpisodeLearnerAdditionalPaymentThresholdDate] [LearnerAdditionalPaymentsDate]
	,[APE].[PriceEpisodeTotalTnpPrice] [AgreedPrice]
	,[APE].[PriceEpisodeActualEndDate] [EndDate]
FROM [Rulebase].[AEC_ApprenticeshipPriceEpisode_Period] [APEP]
INNER JOIN [Rulebase].[AEC_ApprenticeshipPriceEpisode] [APE]
    ON [APEP].[LearnRefNumber] = [APE].[LearnRefNumber]
    AND [APEP].[PriceEpisodeIdentifier] = [APE].[PriceEpisodeIdentifier]
JOIN [Valid].[Learner] L
	ON [L].[LearnRefNumber] = [APEP].[LearnRefNumber]
JOIN [Valid].[LearningDelivery] LD
	ON [LD].[LearnRefNumber] = [APEP].[LearnRefNumber]
	AND [LD].[AimSeqNumber] = [APE].[PriceEpisodeAimSeqNumber]
CROSS JOIN Valid.LearningProvider [LP]
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
GO
