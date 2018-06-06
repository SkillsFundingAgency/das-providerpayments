IF NOT EXISTS(SELECT [schema_id] FROM sys.schemas WHERE [name]='PaymentsDue')
BEGIN
    EXEC('CREATE SCHEMA PaymentsDue')
END
GO

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
select
	APEP.LearnRefNumber,
	APEP.Ukprn,
	APE.PriceEpisodeAimSeqNumber [AimSeqNumber],
	APEP.PriceEpisodeIdentifier,
	APE.EpisodeStartDate,
	APE.EpisodeEffectiveTNPStartDate,
	APEP.[Period],
	L.ULN,
	COALESCE(LD.ProgType, 0) [ProgrammeType],
	COALESCE(LD.FworkCode, 0) [FrameworkCode],
	COALESCE(LD.PwayCode, 0) [PathwayCode],
	COALESCE(LD.StdCode, 0) [StandardCode],
	COALESCE(APEP.PriceEpisodeSFAContribPct, 0) [SfaContributionPercentage],
	APE.PriceEpisodeFundLineType [FundingLineType],
	LD.LearnAimRef,
	LD.LearnStartDate [LearningStartDate],
    COALESCE(APEP.PriceEpisodeOnProgPayment, 0) [TransactionType01],
    COALESCE(APEP.PriceEpisodeCompletionPayment, 0) [TransactionType02],
    COALESCE(APEP.PriceEpisodeBalancePayment, 0) [TransactionType03],
    COALESCE(APEP.PriceEpisodeFirstEmp1618Pay, 0) [TransactionType04],
    COALESCE(APEP.PriceEpisodeFirstProv1618Pay, 0) [TransactionType05],
    COALESCE(APEP.PriceEpisodeSecondEmp1618Pay, 0) [TransactionType06],
    COALESCE(APEP.PriceEpisodeSecondProv1618Pay, 0) [TransactionType07],
    COALESCE(APEP.PriceEpisodeApplic1618FrameworkUpliftOnProgPayment, 0) [TransactionType08],
    COALESCE(APEP.PriceEpisodeApplic1618FrameworkUpliftCompletionPayment, 0) [TransactionType09],
    COALESCE(APEP.PriceEpisodeApplic1618FrameworkUpliftBalancing, 0) [TransactionType10],
    COALESCE(APEP.PriceEpisodeFirstDisadvantagePayment, 0) [TransactionType11],
    COALESCE(APEP.PriceEpisodeSecondDisadvantagePayment, 0) [TransactionType12],
	0 [TransactionType13],
	0 [TransactionType14],
    COALESCE(APEP.PriceEpisodeLSFCash, 0) [TransactionType15],
    CASE WHEN APE.PriceEpisodeContractType = 'Levy Contract' THEN 1 ELSE 2 END [ApprenticeshipContractType]
from Rulebase.AEC_ApprenticeshipPriceEpisode_Period APEP
inner join Rulebase.AEC_ApprenticeshipPriceEpisode APE
    on APEP.UKPRN = APE.UKPRN
    and APEP.LearnRefNumber = APE.LearnRefNumber
    and APEP.PriceEpisodeIdentifier = APE.PriceEpisodeIdentifier
join Valid.Learner L
	on L.UKPRN = APEP.Ukprn
	and L.LearnRefNumber = APEP.LearnRefNumber
join Valid.LearningDelivery LD
	on LD.UKPRN = APEP.Ukprn
	and LD.LearnRefNumber = APEP.LearnRefNumber
	and LD.AimSeqNumber = APE.PriceEpisodeAimSeqNumber
where (
    APEP.PriceEpisodeOnProgPayment != 0
    or APEP.PriceEpisodeCompletionPayment != 0
    or APEP.PriceEpisodeBalancePayment != 0
	or APEP.PriceEpisodeFirstEmp1618Pay != 0
	or APEP.PriceEpisodeFirstProv1618Pay != 0
	or APEP.PriceEpisodeSecondEmp1618Pay != 0
	or APEP.PriceEpisodeSecondProv1618Pay != 0
	or APEP.PriceEpisodeApplic1618FrameworkUpliftOnProgPayment != 0
	or APEP.PriceEpisodeApplic1618FrameworkUpliftCompletionPayment != 0
	or APEP.PriceEpisodeApplic1618FrameworkUpliftBalancing != 0
	or APEP.PriceEpisodeFirstDisadvantagePayment != 0
	or APEP.PriceEpisodeSecondDisadvantagePayment != 0
	or APEP.PriceEpisodeLSFCash != 0
    )
	and APEP.[Period] <= (
		select cast(replace(Return_Code, 'R', '') as int)
		from Collection_Period_Mapping
		where Collection_Open = 1
	)
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
select 
	LDP.LearnRefNumber,
	LDP.Ukprn,
	LDP.AimSeqNumber,
	NULL [PriceEpisodeIdentifier],
	NULL [EpisodeStartDate],
	NULL [EpisodeEffectiveTNPStartDate],
	LDP.[Period],
	L.ULN,
	COALESCE(LD.ProgType, 0) [ProgrammeType],
	COALESCE(LD.FworkCode, 0) [FrameworkCode],
	COALESCE(LD.PwayCode, 0) [PathwayCode],
	COALESCE(LD.StdCode, 0) [StandardCode],
	COALESCE(LDP.[LearnDelSFAContribPct], 0) [SfaContributionPercentage],
	LDP.FundLineType [FundingLineType],
	LD.LearnAimRef,
	LD.LearnStartDate [LearningStartDate],
	0 [TransactionType01],
	0 [TransactionType02],
	0 [TransactionType03],
	0 [TransactionType04],
	0 [TransactionType05],
	0 [TransactionType06],
	0 [TransactionType07],
	0 [TransactionType08],
	0 [TransactionType09],
	0 [TransactionType10],
	0 [TransactionType11],
	0 [TransactionType12],
    COALESCE(MathEngOnProgPayment, 0) [TransactionType13],
    COALESCE(MathEngBalPayment, 0) [TransactionType14],
    COALESCE(LearnSuppFundCash, 0) [TransactionType15],
    CASE WHEN LDP.LearnDelContType = 'Levy Contract' THEN 1 ELSE 2 END [ApprenticeshipContractType]
from Rulebase.AEC_LearningDelivery_Period LDP
inner join Valid.LearningDelivery LD
    on LD.UKPRN = LDP.UKPRN
    and LD.LearnRefNumber = LDP.LearnRefNumber
    and LD.AimSeqNumber = LDP.AimSeqNumber
join Valid.Learner L
	on L.UKPRN = LD.Ukprn
	and L.LearnRefNumber = LD.LearnRefNumber
where (
    MathEngOnProgPayment != 0
    or MathEngBalPayment != 0
    or LearnSuppFundCash != 0
    )
    and LDP.[Period] <= (
		select cast(replace(Return_Code, 'R', '') as int)
		from Collection_Period_Mapping
		where Collection_Open = 1
	)
    and LD.LearnAimRef != 'ZPROG001'
GO
