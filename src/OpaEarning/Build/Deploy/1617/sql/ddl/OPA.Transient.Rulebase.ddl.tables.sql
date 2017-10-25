if not exists(select schema_id from sys.schemas where name='Rulebase')
	exec('create schema Rulebase')
GO
if object_id('[Rulebase].[AEC_Cases]','u') is not null
	drop table [Rulebase].[AEC_Cases]
create table [Rulebase].[AEC_Cases]
	(
		[LearnRefNumber] varchar(12),
		[CaseData] [xml] not null
	)
GO
if object_id('[Rulebase].[AEC_global]','u') is not null
	drop table [Rulebase].[AEC_global]
create table [Rulebase].[AEC_global]
	(
		[UKPRN] int,
		[LARSVersion] varchar(100),
		[RulebaseVersion] varchar(10),
		[Year] varchar(4)
	)
GO
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
		[DisUpFactAdj] decimal(10,4),
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
		[LearnDelApplicTot1618Incentive] decimal(10,5),
		[LearnDelAppPrevAccDaysIL] int,
		[LearnDelDaysIL] int,
		[LearnDelDisadAmount] decimal(10,5),
		[LearnDelEligDisadvPayment] bit,
		[LearnDelEmpIdFirstAdditionalPaymentThreshold] int,
		[LearnDelEmpIdSecondAdditionalPaymentThreshold] int,
		[LearnDelHistDaysThisApp] int,
		[LearnDelHistProgEarnings] decimal(10,5),
		[LearnDelInitialFundLineType] varchar(60),
		[LearnDelSEMContWaiver] bit,
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
GO
if object_id('[Rulebase].[AEC_LearningDelivery_Period]','u') is not null
	drop table [Rulebase].[AEC_LearningDelivery_Period]
create table [Rulebase].[AEC_LearningDelivery_Period]
	(
		[LearnRefNumber] varchar(12),
		[AimSeqNumber] int,
		[Period] int,
		[DisadvFirstPayment] decimal(10,5),
		[DisadvSecondPayment] decimal(10,5),
		[FundLineType] varchar(60),
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
		primary key clustered
		(
			[LearnRefNumber] asc,
			[AimSeqNumber] asc,
			[Period] asc
		)
	)
GO
if object_id('[Rulebase].[AEC_LearningDelivery_PeriodisedValues]','u') is not null
	drop table [Rulebase].[AEC_LearningDelivery_PeriodisedValues]
create table [Rulebase].[AEC_LearningDelivery_PeriodisedValues]
	(
		[LearnRefNumber] varchar(12),
		[AimSeqNumber] int,
		[AttributeName] [varchar](100) not null,
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
GO
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
GO
if object_id('[Rulebase].[AEC_HistoricEarningOutput]','u') is not null
	drop table [Rulebase].[AEC_HistoricEarningOutput]
create table [Rulebase].[AEC_HistoricEarningOutput]
	(
		[LearnRefNumber] varchar(12),
		[AppIdentifierOutput] varchar(10),
		[AppProgCompletedInTheYearOutput] bit,
		[HistoricDaysInYearOutput] int,
		[HistoricEffectiveTNPStartDateOutput] date,
		[HistoricFworkCodeOutput] int,
		[HistoricLearner1618AtStartOutput] bit,
		[HistoricProgrammeStartDateIgnorePathwayOutput] date,
		[HistoricProgrammeStartDateMatchPathwayOutput] date,
		[HistoricProgTypeOutput] int,
		[HistoricPwayCodeOutput] int,
		[HistoricSTDCodeOutput] int,
		[HistoricTNP1Output] decimal(10,5),
		[HistoricTNP2Output] decimal(10,5),
		[HistoricTNP3Output] decimal(10,5),
		[HistoricTNP4Output] decimal(10,5),
		[HistoricTotal1618UpliftPaymentsInTheYear] decimal(10,5),
		[HistoricTotalProgAimPaymentsInTheYear] decimal(10,5),
		[HistoricULNOutput] bigint,
		[HistoricUptoEndDateOutput] date,
		[HistoricVirtualTNP3EndofThisYearOutput] decimal(10,5),
		[HistoricVirtualTNP4EndofThisYearOutput] decimal(10,5)
		primary key clustered
		(
			[LearnRefNumber] asc,
			[AppIdentifierOutput] asc
		)
	)
GO
if object_id('[Rulebase].[AEC_ApprenticeshipPriceEpisode]','u') is not null
	drop table [Rulebase].[AEC_ApprenticeshipPriceEpisode]
create table [Rulebase].[AEC_ApprenticeshipPriceEpisode]
	(
		[LearnRefNumber] varchar(12),
		[PriceEpisodeIdentifier] varchar(25),
		[EpisodeEffectiveTNPStartDate] date,
		[EpisodeStartDate] date,
		[PriceEpisodeActualEndDate] date,
		[PriceEpisodeActualInstalments] int,
		[PriceEpisodeAimSeqNumber] int,
		[PriceEpisodeCappedRemainingTNPAmount] decimal(10,5),
		[PriceEpisodeCompleted] bit,
		[PriceEpisodeCompletionElement] decimal(10,5),
		[PriceEpisodeContractType] varchar(50),
		[PriceEpisodeExpectedTotalMonthlyValue] decimal(10,5),
		[PriceEpisodeFirstAdditionalPaymentThresholdDate] date,
		[PriceEpisodeFundLineType] varchar(60),
		[PriceEpisodeInstalmentValue] decimal(10,5),
		[PriceEpisodePlannedEndDate] date,
		[PriceEpisodePlannedInstalments] int,
		[PriceEpisodePreviousEarnings] decimal(10,5),
		[PriceEpisodePreviousEarningsSameProvider] decimal(10,5),
		[PriceEpisodeRemainingAmountWithinUpperLimit] decimal(10,5),
		[PriceEpisodeRemainingTNPAmount] decimal(10,5),
		[PriceEpisodeSecondAdditionalPaymentThresholdDate] date,
		[PriceEpisodeTotalEarnings] decimal(10,5),
		[PriceEpisodeTotalTNPPrice] decimal(10,5),
		[PriceEpisodeUpperBandLimit] decimal(10,5),
		[PriceEpisodeUpperLimitAdjustment] decimal(10,5),
		[TNP1] decimal(10,5),
		[TNP2] decimal(10,5),
		[TNP3] decimal(10,5),
		[TNP4] decimal(10,5)
		primary key clustered
		(
			[LearnRefNumber] asc,
			[PriceEpisodeIdentifier] asc
		)
	)
GO
if object_id('[Rulebase].[AEC_ApprenticeshipPriceEpisode_Period]','u') is not null
	drop table [Rulebase].[AEC_ApprenticeshipPriceEpisode_Period]
create table [Rulebase].[AEC_ApprenticeshipPriceEpisode_Period]
	(
		[LearnRefNumber] varchar(12),
		[PriceEpisodeIdentifier] varchar(25),
		[Period] int,
		[PriceEpisodeApplic1618FrameworkUpliftBalancing] decimal(10,5),
		[PriceEpisodeApplic1618FrameworkUpliftCompletionPayment] decimal(10,5),
		[PriceEpisodeApplic1618FrameworkUpliftOnProgPayment] decimal(10,5),
		[PriceEpisodeBalancePayment] decimal(10,5),
		[PriceEpisodeBalanceValue] decimal(10,5),
		[PriceEpisodeCompletionPayment] decimal(10,5),
		[PriceEpisodeFirstDisadvantagePayment] decimal(10,5),
		[PriceEpisodeFirstEmp1618Pay] decimal(10,5),
		[PriceEpisodeFirstProv1618Pay] decimal(10,5),
		[PriceEpisodeInstalmentsThisPeriod] int,
		[PriceEpisodeLevyNonPayInd] int,
		[PriceEpisodeLSFCash] decimal(10,5),
		[PriceEpisodeOnProgPayment] decimal(10,5),
		[PriceEpisodeSecondDisadvantagePayment] decimal(10,5),
		[PriceEpisodeSecondEmp1618Pay] decimal(10,5),
		[PriceEpisodeSecondProv1618Pay] decimal(10,5),
		[PriceEpisodeSFAContribPct] decimal(10,5),
		primary key clustered
		(
			[LearnRefNumber] asc,
			[PriceEpisodeIdentifier] asc,
			[Period] asc
		)
	)
GO
if object_id('[Rulebase].[AEC_ApprenticeshipPriceEpisode_PeriodisedValues]','u') is not null
	drop table [Rulebase].[AEC_ApprenticeshipPriceEpisode_PeriodisedValues]
create table [Rulebase].[AEC_ApprenticeshipPriceEpisode_PeriodisedValues]
	(
		[LearnRefNumber] varchar(12),
		[PriceEpisodeIdentifier] varchar(25),
		[AttributeName] [varchar](100) not null,
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
GO
