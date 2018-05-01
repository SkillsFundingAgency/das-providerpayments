IF NOT EXISTS(SELECT [schema_id] FROM sys.schemas WHERE [name]='PaymentsDue')
BEGIN
    EXEC('CREATE SCHEMA PaymentsDue')
END
GO

-----------------------------------------------------------------------------------------------------------------------------------------------
-- vw_Earnings
-----------------------------------------------------------------------------------------------------------------------------------------------
IF EXISTS(SELECT [object_id] FROM sys.views WHERE [name]='vw_Earnings' AND [schema_id] = SCHEMA_ID('PaymentsDue'))
BEGIN
    DROP VIEW PaymentsDue.vw_Earnings
END
GO

CREATE VIEW PaymentsDue.vw_Earnings
AS
select
	APEP.LearnRefNumber,
	APEP.Ukprn,
	APEP.PriceEpisodeIdentifier,
	APE.EpisodeStartDate,
	APEP.[Period],
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
where APEP.[Period] <= (
		select cast(replace(Return_Code, 'R', '') as int)
		from Collection_Period_Mapping
		where Collection_Open = 1
	)
GO

-----------------------------------------------------------------------------------------------------------------------------------------------
-- vw_EarningsMathsEnglish
-----------------------------------------------------------------------------------------------------------------------------------------------
IF EXISTS(SELECT [object_id] FROM sys.views WHERE [name]='vw_EarningsMathsEnglish' AND [schema_id] = SCHEMA_ID('PaymentsDue'))
BEGIN
    DROP VIEW PaymentsDue.vw_EarningsMathsEnglish
END
GO

CREATE VIEW PaymentsDue.vw_EarningsMathsEnglish
AS
select 
	LDP.LearnRefNumber,
	LDP.Ukprn,
	LDP.AimSeqNumber,
	LD.LearnStartDate,
	LDP.[Period],
	RLD.LearnAimRef,
	LD.FworkCode,
	LD.PwayCode,
	LD.StdCode,
    MathEngOnProgPayment [TransactionType13],
    MathEngBalPayment [TransactionType14],
    LearnSuppFundCash [TransactionType15],
    CASE WHEN LDP.LearnDelContType = 'Levy Contract' THEN 1 ELSE 2 END [ACT]
from Rulebase.AEC_LearningDelivery_Period LDP
inner join Rulebase.AEC_LearningDelivery RLD
    on LDP.UKPRN = RLD.UKPRN
    and LDP.LearnRefNumber = RLD.LearnRefNumber
    and LDP.AimSeqNumber = RLD.AimSeqNumber
inner join Valid.LearningDelivery LD
    on LD.UKPRN = LDP.UKPRN
    and LD.LearnRefNumber = LDP.LearnRefNumber
    and LD.AimSeqNumber = RLD.AimSeqNumber
where (
    MathEngOnProgPayment > 0
    or MathEngBalPayment > 0
    or LearnSuppFundCash > 0
    )
    and LDP.[Period] <= (
		select cast(replace(Return_Code, 'R', '') as int)
		from Collection_Period_Mapping
		where Collection_Open = 1
	)
    and LD.LearnAimRef != 'ZPROG001'
GO
