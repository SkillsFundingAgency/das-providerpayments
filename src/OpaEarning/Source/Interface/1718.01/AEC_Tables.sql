-- =====================================================================================================
-- Generated by Data Dictionary Version 1.0.0.0
-- Date: 27 June 2017 13:32
-- Profile: DCSS Calculation
-- Rulebase Version: ILR Apprenticeship Earnings Calc 1718, Version 1718.01
-- =====================================================================================================
if not exists(select schema_id from sys.schemas where name='Rulebase')
	exec('create schema Rulebase')
go

if object_id('[Rulebase].[AEC_Cases]','u') is not null

	drop table [Rulebase].[AEC_Cases]

create table [Rulebase].[AEC_Cases]
	(
		[LearnRefNumber] varchar(12),
		[CaseData] [xml] not null
	)
go

if object_id('[Rulebase].[AEC_global]','u') is not null
	drop table [Rulebase].[AEC_global]

create table [Rulebase].[AEC_global]
	(
		[UKPRN] int,
		[LARSVersion] varchar(100),
		[RulebaseVersion] varchar(10),
		[Year] varchar(4)
	)
go

if object_id('[Rulebase].[AEC_LearningDelivery]','u') is not null

	drop table [Rulebase].[AEC_LearningDelivery]


create table [Rulebase].[AEC_LearningDelivery]
	(
		[LearnRefNumber] varchar(12),
		[AimSeqNumber] int,
		[ActualDaysIL] int,
		[ActualNumInstalm] int,
		[AdjStartDate] date,
		[AgeAtProgStart] int,
		[AppAdjLearnStartDate] date,
		[AppAdjLearnStartDateMatchPathway] date,
		[ApplicCompDate] date,
		[CombinedAdjProp] decimal(10,5),
		[Completed] bit,
		[FirstIncentiveThresholdDate] date,
		[FundStart] bit,
		[LDApplic1618FrameworkUpliftBalancingValue] decimal(10,5),
		[LDApplic1618FrameworkUpliftCompElement] decimal(10,5),
		[LDApplic1618FRameworkUpliftCompletionValue] decimal(10,5),
		[LDApplic1618FrameworkUpliftMonthInstalVal] decimal(10,5),
		[LDApplic1618FrameworkUpliftPrevEarnings] decimal(10,5),
		[LDApplic1618FrameworkUpliftPrevEarningsStage1] decimal(10,5),
		[LDApplic1618FrameworkUpliftRemainingAmount] decimal(10,5),
		[LDApplic1618FrameworkUpliftTotalActEarnings] decimal(10,5),
		[LearnAimRef] varchar(8),
		[LearnDel1618AtStart] bit,
		[LearnDelAppAccDaysIL] int,
		[LearnDelApplicDisadvAmount] decimal(10,5),
		[LearnDelApplicEmp1618Incentive] decimal(10,5),
		[LearnDelApplicEmpDate] date,
		[LearnDelApplicProv1618FrameworkUplift] decimal(10,5),
		[LearnDelApplicProv1618Incentive] decimal(10,5),
		[LearnDelAppPrevAccDaysIL] int,
		[LearnDelDaysIL] int,
		[LearnDelDisadAmount] decimal(10,5),
		[LearnDelEligDisadvPayment] bit,
		[LearnDelEmpIdFirstAdditionalPaymentThreshold] int,
		[LearnDelEmpIdSecondAdditionalPaymentThreshold] int,
		[LearnDelHistDaysThisApp] int,
		[LearnDelHistProgEarnings] decimal(10,5),
		[LearnDelInitialFundLineType] varchar(100),
		[LearnDelMathEng] bit,
		[MathEngAimValue] decimal(10,5),
		[OutstandNumOnProgInstalm] int,
		[PlannedNumOnProgInstalm] int,
		[PlannedTotalDaysIL] int,
		[SecondIncentiveThresholdDate] date,
		[ThresholdDays] int
		primary key clustered
		(
			[LearnRefNumber] asc,
			[AimSeqNumber] asc
		)
	)
go

if object_id('[Rulebase].[AEC_LearningDelivery_Period]','u') is not null

	drop table [Rulebase].[AEC_LearningDelivery_Period]


create table [Rulebase].[AEC_LearningDelivery_Period]
	(
		[LearnRefNumber] varchar(12),
		[AimSeqNumber] int,
		[Period] int,
		[DisadvFirstPayment] decimal(10,5),
		[DisadvSecondPayment] decimal(10,5),
		[FundLineType] varchar(100),
		[InstPerPeriod] int,
		[LDApplic1618FrameworkUpliftBalancingPayment] decimal(10,5),
		[LDApplic1618FrameworkUpliftCompletionPayment] decimal(10,5),
		[LDApplic1618FrameworkUpliftOnProgPayment] decimal(10,5),
		[LearnDelContType] varchar(50),
		[LearnDelFirstEmp1618Pay] decimal(10,5),
		[LearnDelFirstProv1618Pay] decimal(10,5),
		[LearnDelLevyNonPayInd] int,
		[LearnDelSecondEmp1618Pay] decimal(10,5),
		[LearnDelSecondProv1618Pay] decimal(10,5),
		[LearnDelSEMContWaiver] bit,
		[LearnDelSFAContribPct] decimal(10,5),
		[LearnSuppFund] bit,
		[LearnSuppFundCash] decimal(10,5),
		[MathEngBalPayment] decimal(10,5),
		[MathEngBalPct] decimal(8,5),
		[MathEngOnProgPayment] decimal(10,5),
		[MathEngOnProgPct] decimal(8,5),
		[ProgrammeAimBalPayment] decimal(10,5),
		[ProgrammeAimCompletionPayment] decimal(10,5),
		[ProgrammeAimOnProgPayment] decimal(10,5),
		[ProgrammeAimProgFundIndMaxEmpCont] decimal(12,5),
		[ProgrammeAimProgFundIndMinCoInvest] decimal(12,5),
		[ProgrammeAimTotProgFund] decimal(12,5),
		primary key clustered
		(
			[LearnRefNumber] asc,
			[AimSeqNumber] asc,
			[Period] asc
		)
	)
go

if object_id('[Rulebase].[AEC_LearningDelivery_PeriodisedValues]','u') is not null

	drop table [Rulebase].[AEC_LearningDelivery_PeriodisedValues]


create table [Rulebase].[AEC_LearningDelivery_PeriodisedValues]
	(
		[LearnRefNumber] varchar(12),
		[AimSeqNumber] int,
		[AttributeName] varchar(100) not null,
		[Period_1] [decimal](15,5),
		[Period_2] [decimal](15,5),
		[Period_3] [decimal](15,5),
		[Period_4] [decimal](15,5),
		[Period_5] [decimal](15,5),
		[Period_6] [decimal](15,5),
		[Period_7] [decimal](15,5),
		[Period_8] [decimal](15,5),
		[Period_9] [decimal](15,5),
		[Period_10] [decimal](15,5),
		[Period_11] [decimal](15,5),
		[Period_12] [decimal](15,5),
		primary key clustered
		(
			[LearnRefNumber] asc,
			[AimSeqNumber] asc,
			[AttributeName] asc
		)
	)
go

if object_id('[Rulebase].[AEC_LearningDelivery_PeriodisedTextValues]','u') is not null

	drop table [Rulebase].[AEC_LearningDelivery_PeriodisedTextValues]


create table [Rulebase].[AEC_LearningDelivery_PeriodisedTextValues]
	(
		[LearnRefNumber] varchar(12),
		[AimSeqNumber] int,
		[AttributeName] [varchar](100) not null,
		[Period_1] [varchar](255),
		[Period_2] [varchar](255),
		[Period_3] [varchar](255),
		[Period_4] [varchar](255),
		[Period_5] [varchar](255),
		[Period_6] [varchar](255),
		[Period_7] [varchar](255),
		[Period_8] [varchar](255),
		[Period_9] [varchar](255),
		[Period_10] [varchar](255),
		[Period_11] [varchar](255),
		[Period_12] [varchar](255),
		primary key clustered
		(
			[LearnRefNumber] asc,
			[AimSeqNumber] asc,
			[AttributeName] asc
		)
	)
go

if object_id('[Rulebase].[AEC_HistoricEarningOutput]','u') is not null

	drop table [Rulebase].[AEC_HistoricEarningOutput]


create table [Rulebase].[AEC_HistoricEarningOutput]
	(
		[LearnRefNumber] varchar(12),
		[AppIdentifierOutput] varchar(10),
		[AppProgCompletedInTheYearOutput] bit,
		[HistoricDaysInYearOutput] int,
		[HistoricEffectiveTNPStartDateOutput] date,
		[HistoricEmpIdEndWithinYearOutput] int,
		[HistoricEmpIdStartWithinYearOutput] int,
		[HistoricFworkCodeOutput] int,
		[HistoricLearner1618AtStartOutput] bit,
		[HistoricPMRAmountOutput] decimal(12,5),
		[HistoricProgrammeStartDateIgnorePathwayOutput] date,
		[HistoricProgrammeStartDateMatchPathwayOutput] date,
		[HistoricProgTypeOutput] int,
		[HistoricPwayCodeOutput] int,
		[HistoricSTDCodeOutput] int,
		[HistoricTNP1Output] decimal(12,5),
		[HistoricTNP2Output] decimal(12,5),
		[HistoricTNP3Output] decimal(12,5),
		[HistoricTNP4Output] decimal(12,5),
		[HistoricTotal1618UpliftPaymentsInTheYear] decimal(11,5),
		[HistoricTotalProgAimPaymentsInTheYear] decimal(11,5),
		[HistoricULNOutput] bigint,
		[HistoricUptoEndDateOutput] date,
		[HistoricVirtualTNP3EndofThisYearOutput] decimal(12,5),
		[HistoricVirtualTNP4EndofThisYearOutput] decimal(12,5)
		primary key clustered
		(
			[LearnRefNumber] asc,
			[AppIdentifierOutput] asc
		)
	)
go

if object_id('[Rulebase].[AEC_ApprenticeshipPriceEpisode]','u') is not null

	drop table [Rulebase].[AEC_ApprenticeshipPriceEpisode]

CREATE TABLE [Rulebase].[AEC_ApprenticeshipPriceEpisode]
(
	[LearnRefNumber] varchar(12),
	[PriceEpisodeIdentifier] varchar(25),
	[TNP4] decimal(12,5) ,
	[TNP1] decimal(12,5) ,
	[EpisodeStartDate] date ,
	[TNP2] decimal(12,5) ,
	[TNP3] decimal(12,5) ,
	[PriceEpisodeUpperBandLimit] decimal(12,5) ,
	[PriceEpisodePlannedEndDate] date ,
	[PriceEpisodeActualEndDate] date ,
	[PriceEpisodeTotalTNPPrice] decimal(12,5) ,
	[PriceEpisodeUpperLimitAdjustment] decimal(12,5) ,
	[PriceEpisodePlannedInstalments] int ,
	[PriceEpisodeActualInstalments] int ,
	[PriceEpisodeInstalmentsThisPeriod] int ,
	[PriceEpisodeCompletionElement] decimal(12,5) ,
	[PriceEpisodePreviousEarnings] decimal(12,5) ,
	[PriceEpisodeInstalmentValue] decimal(12,5) ,
	[PriceEpisodeOnProgPayment] decimal(12,5) ,
	[PriceEpisodeTotalEarnings] decimal(12,5) ,
	[PriceEpisodeBalanceValue] decimal(12,5) ,
	[PriceEpisodeBalancePayment] decimal(12,5) ,
	[PriceEpisodeCompleted] bit ,
	[PriceEpisodeCompletionPayment] decimal(12,5) ,
	[PriceEpisodeRemainingTNPAmount] decimal(12,5) ,
	[PriceEpisodeRemainingAmountWithinUpperLimit] decimal(12,5) ,
	[PriceEpisodeCappedRemainingTNPAmount] decimal(12,5) ,
	[PriceEpisodeExpectedTotalMonthlyValue] decimal(12,5) ,
	[PriceEpisodeAimSeqNumber] bigint ,
	[PriceEpisodeFirstDisadvantagePayment] decimal(12,5) ,
	[PriceEpisodeSecondDisadvantagePayment] decimal(12,5) ,
	[PriceEpisodeApplic1618FrameworkUpliftBalancing] decimal(12,5) ,
	[PriceEpisodeApplic1618FrameworkUpliftCompletionPayment] decimal(12,5) ,
	[PriceEpisodeApplic1618FrameworkUpliftOnProgPayment] decimal(12,5) ,
	[PriceEpisodeSecondProv1618Pay] decimal(12,5) ,
	[PriceEpisodeFirstEmp1618Pay] decimal(12,5) ,
	[PriceEpisodeSecondEmp1618Pay] decimal(12,5) ,
	[PriceEpisodeFirstProv1618Pay] decimal(12,5) ,
	[PriceEpisodeLSFCash] decimal(12,5) ,
	[PriceEpisodeFundLineType] varchar(100) ,
	[PriceEpisodeSFAContribPct] decimal(10, 5) ,
	[PriceEpisodeLevyNonPayInd] int ,
	[EpisodeEffectiveTNPStartDate] date ,
	[PriceEpisodeFirstAdditionalPaymentThresholdDate] date ,
	[PriceEpisodeSecondAdditionalPaymentThresholdDate] date ,
	[PriceEpisodeContractType] varchar(50) ,
	[PriceEpisodePreviousEarningsSameProvider] decimal(12,5) ,
	[PriceEpisodeTotProgFunding] decimal(12,5) ,
	[PriceEpisodeProgFundIndMinCoInvest] decimal(12,5) ,
	[PriceEpisodeProgFundIndMaxEmpCont] decimal(12,5) ,
	[PriceEpisodeTotalPMRs] decimal(12,5) ,
	[PriceEpisodeCumulativePMRs] decimal(12,5) ,
	[PriceEpisodeCompExemCode]int ,
	primary key 
	(
		LearnRefNumber,
		PriceEpisodeIdentifier
	)
)
go

if object_id('[Rulebase].[AEC_ApprenticeshipPriceEpisode_Period]','u') is not null

	drop table [Rulebase].[AEC_ApprenticeshipPriceEpisode_Period]


create table [Rulebase].[AEC_ApprenticeshipPriceEpisode_Period]
	(
		[LearnRefNumber] varchar(12),
		[PriceEpisodeIdentifier] varchar(25),
		[Period] int,
		[PriceEpisodeApplic1618FrameworkUpliftBalancing] decimal(12,5),
		[PriceEpisodeApplic1618FrameworkUpliftCompletionPayment] decimal(12,5),
		[PriceEpisodeApplic1618FrameworkUpliftOnProgPayment] decimal(12,5),
		[PriceEpisodeBalancePayment] decimal(12,5),
		[PriceEpisodeBalanceValue] decimal(12,5),
		[PriceEpisodeCompletionPayment] decimal(12,5),
		[PriceEpisodeFirstDisadvantagePayment] decimal(12,5),
		[PriceEpisodeFirstEmp1618Pay] decimal(12,5),
		[PriceEpisodeFirstProv1618Pay] decimal(12,5),
		[PriceEpisodeInstalmentsThisPeriod] int,
		[PriceEpisodeLevyNonPayInd] int,
		[PriceEpisodeLSFCash] decimal(12,5),
		[PriceEpisodeOnProgPayment] decimal(12,5),
		[PriceEpisodeProgFundIndMaxEmpCont] decimal(12,5),
		[PriceEpisodeProgFundIndMinCoInvest] decimal(12,5),
		[PriceEpisodeSecondDisadvantagePayment] decimal(12,5),
		[PriceEpisodeSecondEmp1618Pay] decimal(12,5),
		[PriceEpisodeSecondProv1618Pay] decimal(12,5),
		[PriceEpisodeSFAContribPct] decimal(10,5),
		[PriceEpisodeTotProgFunding] decimal(12,5),
		primary key clustered
		(
			[LearnRefNumber] asc,
			[PriceEpisodeIdentifier] asc,
			[Period] asc
		)
	)
go

if object_id('[Rulebase].[AEC_ApprenticeshipPriceEpisode_PeriodisedValues]','u') is not null

	drop table [Rulebase].[AEC_ApprenticeshipPriceEpisode_PeriodisedValues]

create table [Rulebase].[AEC_ApprenticeshipPriceEpisode_PeriodisedValues]
	(
		[LearnRefNumber] varchar(12),
		[PriceEpisodeIdentifier] varchar(25),
		[AttributeName] varchar(100) not null,
		[Period_1] [decimal](15,5),
		[Period_2] [decimal](15,5),
		[Period_3] [decimal](15,5),
		[Period_4] [decimal](15,5),
		[Period_5] [decimal](15,5),
		[Period_6] [decimal](15,5),
		[Period_7] [decimal](15,5),
		[Period_8] [decimal](15,5),
		[Period_9] [decimal](15,5),
		[Period_10] [decimal](15,5),
		[Period_11] [decimal](15,5),
		[Period_12] [decimal](15,5),
		primary key clustered
		(
			[LearnRefNumber] asc,
			[PriceEpisodeIdentifier] asc,
			[AttributeName] asc
		)
	)
go
