 -----------------------------------------------------------------------------------------------------------------------------------------------
-- TABLES
-----------------------------------------------------------------------------------------------------------------------------------------------


-----------------------------------------------------------------------------------------------------------------------------------------------
-- NonPayableEarnings
-----------------------------------------------------------------------------------------------------------------------------------------------

IF NOT EXISTS(SELECT NULL FROM 
	sys.tables t INNER JOIN sys.schemas s ON t.schema_id = s.schema_id
	WHERE t.name='NonPayableEarnings' AND s.name='PaymentsDue'
)
BEGIN
	CREATE TABLE PaymentsDue.NonPayableEarnings
	(
		Id uniqueidentifier DEFAULT(NEWID()),
		CommitmentId bigint,
		CommitmentVersionId varchar(25),
		AccountId bigint,
		AccountVersionId varchar(50),
		Uln bigint,
		LearnRefNumber varchar(12) NOT NULL,
		AimSeqNumber int,
		Ukprn bigint,
		IlrSubmissionDateTime datetime,
		PriceEpisodeIdentifier varchar(25) NULL,
		StandardCode bigint,
		ProgrammeType int,
		FrameworkCode int,
		PathwayCode int,
		ApprenticeshipContractType int,
		DeliveryMonth int,
		DeliveryYear int,
		CollectionPeriodName varchar(8) NOT NULL,
		CollectionPeriodMonth int,
		CollectionPeriodYear int,
		TransactionType int,
		AmountDue decimal(15,5),
		SfaContributionPercentage decimal(15,5),
		FundingLineType varchar(100),
		UseLevyBalance bit,
		LearnAimRef varchar(8) NOT NULL,
		LearningStartDate datetime,
		PaymentFailureMessage varchar(1000) NOT NULL,
		PaymentFailureReason int NOT NULL,
	)

	CREATE INDEX IX_PaymentsDue_NonPayableEarnings_CollectionPeriodName ON PaymentsDue.NonPayableEarnings (CollectionPeriodName)
	CREATE INDEX IX_PaymentsDue_NonPayableEarnings_UkprnLearnRefNumber ON PaymentsDue.NonPayableEarnings (Ukprn, LearnRefNumber)
	CREATE INDEX IX_PaymentsDue_NonPayableEarnings_Uln ON PaymentsDue.NonPayableEarnings (Uln)
	CREATE INDEX IX_PaymentsDue_NonPayableEarnings_CommitmentId ON PaymentsDue.NonPayableEarnings (CommitmentId)
	CREATE INDEX IX_PaymentsDue_NonPayableEarnings_PaymentFailureReason ON PaymentsDue.NonPayableEarnings (PaymentFailureReason)
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
	SELECT Period_Id
	FROM ${DAS_PeriodEnd.FQ}.dbo.Collection_Period_Mapping
	WHERE Collection_Open = 1
)
SELECT
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
	,[APE].[PriceEpisodeFirstAdditionalPaymentThresholdDate] [FirstIncentiveCensusDate]
	,[APE].[PriceEpisodeSecondAdditionalPaymentThresholdDate] [SecondIncentiveCensusDate]
	,[APE].[PriceEpisodeTotalTnpPrice] [AgreedPrice]
	,[APE].[PriceEpisodeActualEndDate] [EndDate]
	,[APE].[PriceEpisodeCumulativePMRs] [CumulativePmrs]
	,[APE].[PriceEpisodeCompExemCode] [CompExemCode]
FROM [Period],
	${ILR_Deds.FQ}.Rulebase.AEC_ApprenticeshipPriceEpisode_Period APEP
INNER JOIN ${ILR_Deds.FQ}.Rulebase.AEC_ApprenticeshipPriceEpisode APE
    on APEP.UKPRN = APE.UKPRN
    and APEP.LearnRefNumber = APE.LearnRefNumber
    and APEP.PriceEpisodeIdentifier = APE.PriceEpisodeIdentifier
JOIN ${ILR_Deds.FQ}.Valid.Learner L
	on L.UKPRN = APEP.Ukprn
	and L.LearnRefNumber = APEP.LearnRefNumber
JOIN ${ILR_Deds.FQ}.Valid.LearningDelivery LD
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
	OR APEP.[Period] = 1
    )
	and APEP.[Period] <= Period_Id
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
	SELECT Period_Id
	FROM ${DAS_PeriodEnd.FQ}.dbo.Collection_Period_Mapping
	WHERE Collection_Open = 1
)
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
FROM [Period],
	${ILR_Deds.FQ}.Rulebase.AEC_LearningDelivery_Period LDP
INNER JOIN ${ILR_Deds.FQ}.Valid.LearningDelivery LD
    on LD.UKPRN = LDP.UKPRN
    and LD.LearnRefNumber = LDP.LearnRefNumber
    and LD.AimSeqNumber = LDP.AimSeqNumber
JOIN ${ILR_Deds.FQ}.Valid.Learner L
	on L.UKPRN = LD.Ukprn
	and L.LearnRefNumber = LD.LearnRefNumber
where (
    MathEngOnProgPayment != 0
    or MathEngBalPayment != 0
    or LearnSuppFundCash != 0
    )
    and LDP.[Period] <= Period_Id
    and LD.LearnAimRef != 'ZPROG001'
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
	APE.UKPRN,
	APE.LearnRefNumber, 
	PriceEpisodeIdentifier,
	LD.LearnStartDate [StartDate],
	LD.LearnPlanEndDate [PlannedEndDate],
	LD.LearnActEndDate [ActualEndDate],
	LD.CompStatus [CompletionStatus],
	RLD.PlannedNumOnProgInstalm [PlannedInstalments],
	PriceEpisodeCompletionElement [CompletionPayment],
	PriceEpisodeInstalmentValue [Instalment],
	PriceEpisodeCumulativePMRs [EmployerPayments],
	EPAOrgID [EndpointAssessorId]

FROM ${ILR_Deds.FQ}.Rulebase.AEC_ApprenticeshipPriceEpisode APE
INNER JOIN ${ILR_Deds.FQ}.Valid.LearningDelivery LD
	ON APE.UKPRN = LD.UKPRN
	AND APE.LearnRefNumber = LD.LearnRefNumber
	AND APE.PriceEpisodeAimSeqNumber = LD.AimSeqNumber
INNER JOIN ${ILR_Deds.FQ}.Rulebase.AEC_LearningDelivery RLD
	ON APE.UKPRN = RLD.UKPRN
	AND APE.LearnRefNumber = RLD.LearnRefNumber
	AND APE.PriceEpisodeAimSeqNumber = RLD.AimSeqNumber
GO



-----------------------------------------------------------------------------------------------------------------------------------------------
-- vw_RequiredPaymentsForThisAcademicYear
-----------------------------------------------------------------------------------------------------------------------------------------------
IF EXISTS(SELECT [object_id] FROM sys.views WHERE [name]='vw_RequiredPaymentsForThisAcademicYear' AND [schema_id] = SCHEMA_ID('PaymentsDue'))
BEGIN
    DROP VIEW PaymentsDue.vw_RequiredPaymentsForThisAcademicYear
END
GO

CREATE VIEW PaymentsDue.vw_RequiredPaymentsForThisAcademicYear
AS
WITH [Period] AS (
	SELECT CONCAT(Collection_Year, '-R%') [CollectionYear]
	FROM ${DAS_PeriodEnd.FQ}.dbo.Collection_Period_Mapping
	WHERE Collection_Open = 1
)
SELECT 
	[Id]
	,[CommitmentId]
	,[CommitmentVersionId]
	,[AccountId]
	,[AccountVersionId]
	,[Uln]
	,[LearnRefNumber]
	,[AimSeqNumber]
	,[Ukprn]
	,[IlrSubmissionDateTime]
	,[PriceEpisodeIdentifier]
	,[StandardCode]
	,[ProgrammeType]
	,[FrameworkCode]
	,[PathwayCode]
	,[ApprenticeshipContractType]
	,[DeliveryMonth]
	,[DeliveryYear]
	,[CollectionPeriodName]
	,[CollectionPeriodMonth]
	,[CollectionPeriodYear]
	,[TransactionType]
	,[AmountDue]
	,[SfaContributionPercentage]
	,[FundingLineType]
	,[UseLevyBalance]
	,[LearnAimRef]
	,[LearningStartDate]
FROM PaymentsDue.RequiredPayments, [Period]
WHERE CollectionPeriodName LIKE CollectionYear
GO



-----------------------------------------------------------------------------------------------------------------------------------------------
-- INDEXES
-----------------------------------------------------------------------------------------------------------------------------------------------

IF IndexProperty(
	Object_Id('PaymentsDue.RequiredPayments'), 
	'IX_PaymentsDue_PaymentsDue_RequiredPayments_CollectionPeriodName', 
	'IndexId') IS NULL
BEGIN
	CREATE NONCLUSTERED INDEX [IX_PaymentsDue_PaymentsDue_RequiredPayments_CollectionPeriodName]
		ON [PaymentsDue].[RequiredPayments] (CollectionPeriodName)
END
GO
