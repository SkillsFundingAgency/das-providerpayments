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
	APE.PriceEpisodeAimSeqNumber,
	APEP.PriceEpisodeIdentifier,
	APE.EpisodeStartDate,
	APE.EpisodeEffectiveTNPStartDate,
	APEP.[Period],
	L.ULN,
	LD.ProgType,
	LD.FworkCode,
	LD.PwayCode,
	LD.StdCode,
	APEP.PriceEpisodeSFAContribPct,
	APE.PriceEpisodeFundLineType,
	LD.LearnAimRef,
	LD.LearnStartDate,
    APEP.PriceEpisodeOnProgPayment [TransactionType01],
    APEP.PriceEpisodeCompletionPayment [TransactionType02],
    APEP.PriceEpisodeBalancePayment [TransactionType03],
    APEP.PriceEpisodeFirstEmp1618Pay [TransactionType04],
    APEP.PriceEpisodeFirstProv1618Pay [TransactionType05],
    APEP.PriceEpisodeSecondEmp1618Pay [TransactionType06],
    APEP.PriceEpisodeSecondProv1618Pay [TransactionType07],
    APEP.PriceEpisodeApplic1618FrameworkUpliftOnProgPayment [TransactionType08],
    APEP.PriceEpisodeApplic1618FrameworkUpliftCompletionPayment [TransactionType09],
    APEP.PriceEpisodeApplic1618FrameworkUpliftBalancing [TransactionType10],
    APEP.PriceEpisodeFirstDisadvantagePayment [TransactionType11],
    APEP.PriceEpisodeSecondDisadvantagePayment [TransactionType12],
    APEP.PriceEpisodeLSFCash [TransactionType15],
    CASE WHEN APE.PriceEpisodeContractType = 'Levy Contract' THEN 1 ELSE 2 END [ACT]
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
	LD.LearnStartDate,
	LDP.[Period],
	L.ULN,
	LD.ProgType,
	LD.FworkCode,
	LD.PwayCode,
	LD.StdCode,
	LDP.[LearnDelSFAContribPct],
	LDP.FundLineType,
	LD.LearnAimRef,
    MathEngOnProgPayment [TransactionType13],
    MathEngBalPayment [TransactionType14],
    LearnSuppFundCash [TransactionType15],
    CASE WHEN LDP.LearnDelContType = 'Levy Contract' THEN 1 ELSE 2 END [ACT]
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
