declare @ukprn int
declare @year varchar(4)='1617'
declare @return varchar(4)='R01'

select
	@ukprn=[UKPRN]
from
	[Valid].[LearningProvider]

-- =====================================================================================================
-- AEC_global
-- =====================================================================================================
if object_id('$(FromILRDatabase).dbo.AEC_global','u') is not null begin

	delete from 
		[$(FromILRDatabase)].[dbo].[AEC_global]
	where
		[UKPRN]=@ukprn
		and [Year]=@year

end else begin

	create table [$(FromILRDatabase)].[dbo].[AEC_global]
		(
			[UKPRN] int,
			[Year] varchar(4),
			[LARSVersion] varchar(100),
			[RulebaseVersion] varchar(10)
			primary key clustered
			(
				[UKPRN],
				[Year]
			)
		)
end

insert into
	[$(FromILRDatabase)].[dbo].[AEC_global]
select
	@ukprn,
	@year,
	[LARSVersion],
	[RulebaseVersion]
from
	[Intrajob].[Rulebase].[AEC_global]

-- =====================================================================================================
-- AEC_LearningDelivery
-- =====================================================================================================
if object_id('$(FromILRDatabase).dbo.AEC_LearningDelivery','u') is not null begin

	delete from 
		[$(FromILRDatabase)].[dbo].[AEC_LearningDelivery]
	where
		[UKPRN]=@ukprn
		and [Year]=@year

end else begin

	create table [$(FromILRDatabase)].[dbo].[AEC_LearningDelivery]
		(
			[UKPRN] int,
			[Year] varchar(4),
			[LearnRefNumber] varchar(12),
			[AimSeqNumber] int,
			[ActualDaysIL] int,
			[ActualNumInstalm] int,
			[AdjStartDate] date,
			[AgeAtProgStart] int,
			[AppAdjLearnStartDate] date,
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
				[UKPRN],
				[Year],
				[LearnRefNumber] asc,
				[AimSeqNumber] asc
			)
		)
end

insert into
	[$(FromILRDatabase)].[dbo].[AEC_LearningDelivery]
select
	@ukprn,
	@year,
	[LearnRefNumber],
	[AimSeqNumber],
	[ActualDaysIL],
	[ActualNumInstalm],
	[AdjStartDate],
	[AgeAtProgStart],
	[AppAdjLearnStartDate],
	[ApplicCompDate],
	[CombinedAdjProp],
	[Completed],
	[DisUpFactAdj],
	[FirstIncentiveThresholdDate],
	[FundStart],
	[LDApplic1618FrameworkUpliftBalancingValue],
	[LDApplic1618FrameworkUpliftCompElement],
	[LDApplic1618FRameworkUpliftCompletionValue],
	[LDApplic1618FrameworkUpliftMonthInstalVal],
	[LDApplic1618FrameworkUpliftPrevEarnings],
	[LDApplic1618FrameworkUpliftRemainingAmount],
	[LDApplic1618FrameworkUpliftTotalActEarnings],
	[LearnAimRef],
	[LearnDel1618AtStart],
	[LearnDelAppAccDaysIL],
	[LearnDelApplicDisadvAmount],
	[LearnDelApplicEmp1618Incentive],
	[LearnDelApplicEmpDate],
	[LearnDelApplicProv1618FrameworkUplift],
	[LearnDelApplicProv1618Incentive],
	[LearnDelApplicTot1618Incentive],
	[LearnDelAppPrevAccDaysIL],
	[LearnDelDaysIL],
	[LearnDelDisadAmount],
	[LearnDelEligDisadvPayment],
	[LearnDelHistDaysThisApp],
	[LearnDelHistProgEarnings],
	[LearnDelInitialFundLineType],
	[LearnDelSEMContWaiver],
	[MathEngAimValue],
	[OutstandNumOnProgInstalm],
	[PlannedNumOnProgInstalm],
	[PlannedTotalDaysIL],
	[SecondIncentiveThresholdDate],
	[ThresholdDays]
from
	[Intrajob].[Rulebase].[AEC_LearningDelivery]

-- =====================================================================================================
-- AEC_LearningDelivery_Period
-- =====================================================================================================
if object_id('$(FromILRDatabase).dbo.AEC_LearningDelivery_Period','u') is not null begin

	delete from 
		[$(FromILRDatabase)].[dbo].[AEC_LearningDelivery_Period]
	where
		[UKPRN]=@ukprn
		and [Year]=@year

end else begin

	create table [$(FromILRDatabase)].[dbo].[AEC_LearningDelivery_Period]
		(
			[UKPRN] int,
			[Year] varchar(4),
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
				[UKPRN],
				[Year],
				[LearnRefNumber] asc,
				[AimSeqNumber] asc,
				[Period] asc
			)
		)

end

insert into
	[$(FromILRDatabase)].[dbo].[AEC_LearningDelivery_Period]
select
	@ukprn,
	@year,
	[LearnRefNumber],
	[AimSeqNumber],
	[Period],
	[DisadvFirstPayment],
	[DisadvSecondPayment],
	[FundLineType],
	[InstPerPeriod],
	[LDApplic1618FrameworkUpliftBalancingPayment],
	[LDApplic1618FrameworkUpliftCompletionPayment],
	[LDApplic1618FrameworkUpliftOnProgPayment],
	[LearnDelContType],
	[LearnDelFirstEmp1618Pay],
	[LearnDelFirstProv1618Pay],
	[LearnDelLevyNonPayInd],
	[LearnDelSecondEmp1618Pay],
	[LearnDelSecondProv1618Pay],
	[LearnDelSFAContribPct],
	[LearnSuppFund],
	[LearnSuppFundCash],
	[MathEngBalPayment],
	[MathEngBalPct],
	[MathEngOnProgPayment],
	[MathEngOnProgPct],
	[ProgrammeAimBalPayment],
	[ProgrammeAimCompletionPayment],
	[ProgrammeAimOnProgPayment]
from
	[Rulebase].[AEC_LearningDelivery_Period]

-- =====================================================================================================
-- AEC_LearningDelivery_PeriodisedValues
-- =====================================================================================================
if object_id('$(FromILRDatabase).dbo.AEC_LearningDelivery_PeriodisedValues','u') is not null begin

	delete from 
		[$(FromILRDatabase)].[dbo].[AEC_LearningDelivery_PeriodisedValues]
	where
		[UKPRN]=@ukprn
		and [Year]=@year

end else begin

	create table [$(FromILRDatabase)].[dbo].[AEC_LearningDelivery_PeriodisedValues]
		(
			[UKPRN] int,
			[Year] varchar(4),
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
				[UKPRN],
				[Year],
				[LearnRefNumber] asc,
				[AimSeqNumber] asc,
				[AttributeName] asc
			)
		)

end

insert into
	[$(FromILRDatabase)].[dbo].[AEC_LearningDelivery_PeriodisedValues]
select
	@ukprn,
	@year,
	[LearnRefNumber],
	[AimSeqNumber],
	[AttributeName],
	[Period_1],
	[Period_2],
	[Period_3],
	[Period_4],
	[Period_5],
	[Period_6],
	[Period_7],
	[Period_8],
	[Period_9],
	[Period_10],
	[Period_11],
	[Period_12]
from
	[Rulebase].[AEC_LearningDelivery_PeriodisedValues]

-- =====================================================================================================
-- AEC_LearningDelivery_PeriodisedTextValues
-- =====================================================================================================
if object_id('$(FromILRDatabase).dbo.AEC_LearningDelivery_PeriodisedTextValues','u') is not null begin

	delete from 
		[$(FromILRDatabase)].[dbo].[AEC_LearningDelivery_PeriodisedTextValues]
	where
		[UKPRN]=@ukprn
		and [Year]=@year

end else begin

	create table [$(FromILRDatabase)].[dbo].[AEC_LearningDelivery_PeriodisedTextValues]
		(
			[UKPRN] int,
			[Year] varchar(4),
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
				[UKPRN],
				[Year],
				[LearnRefNumber] asc,
				[AimSeqNumber] asc,
				[AttributeName] asc
			)
		)

end

insert into
	[$(FromILRDatabase)].[dbo].[AEC_LearningDelivery_PeriodisedTextValues]
select
	@ukprn,
	@year,
	[LearnRefNumber],
	[AimSeqNumber],
	[AttributeName],
	[Period_1],
	[Period_2],
	[Period_3],
	[Period_4],
	[Period_5],
	[Period_6],
	[Period_7],
	[Period_8],
	[Period_9],
	[Period_10],
	[Period_11],
	[Period_12]
from
	[Rulebase].[AEC_LearningDelivery_PeriodisedTextValues]

/* Not persisting to source database

if object_id('$(FromILRDatabase).dbo.AEC_HistoricEarningOutput','u') is not null begin

	delete from 
		[$(FromILRDatabase)].[dbo].[AEC_HistoricEarningOutput]
	where
		[UKPRN]=@ukprn
		and [Year]=@year

end else begin

	create table [$(FromILRDatabase)].[dbo].[AEC_HistoricEarningOutput]
		(
			[UKPRN] int,
			[Year] varchar(4),
			[LearnRefNumber] varchar(12),
			[AppIdentifierOutput] varchar(10),
			[HistoricDaysInYearOutput] int,
			[HistoricFworkCodeOutput] int,
			[HistoricProgTypeOutput] int,
			[HistoricPwayCodeOutput] int,
			[HistoricSTDCodeOutput] int,
			[HistoricULNOutput] bigint,
			[HistoricUptoEndDateOutput] date
			primary key clustered
			(
				[UKPRN],
				[Year],
				[LearnRefNumber],
				[AppIdentifierOutput]
			)
		)

end

insert into
	[$(FromILRDatabase)].[dbo].[AEC_HistoricEarningOutput]
select
	@ukprn,
	@year,
	[LearnRefNumber],
	[AppIdentifierOutput],
	[HistoricDaysInYearOutput],
	[HistoricFworkCodeOutput],
	[HistoricProgTypeOutput],
	[HistoricPwayCodeOutput],
	[HistoricSTDCodeOutput],
	[HistoricULNOutput],
	[HistoricUptoEndDateOutput]
from
	[Rulebase].[AEC_HistoricEarningOutput]

*/

-- =====================================================================================================
-- AEC_ApprenticeshipPriceEpisode
-- =====================================================================================================
if object_id('$(FromILRDatabase).dbo.AEC_ApprenticeshipPriceEpisode','u') is not null begin

	delete from 
		[$(FromILRDatabase)].[dbo].[AEC_ApprenticeshipPriceEpisode]
	where
		[UKPRN]=@ukprn
		and [Year]=@year

end else begin

	create table [$(FromILRDatabase)].[dbo].[AEC_ApprenticeshipPriceEpisode]
		(
			[UKPRN] int,
			[Year] varchar(4),
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
				[UKPRN],
				[Year],
				[LearnRefNumber],
				[EpisodeStartDate],
				[PriceEpisodeIdentifier]
			)
		)
end

insert into
	[$(FromILRDatabase)].[dbo].[AEC_ApprenticeshipPriceEpisode]
select
	@ukprn,
	@year,
	[LearnRefNumber],
	[PriceEpisodeIdentifier],
	[EpisodeEffectiveTNPStartDate],
	[EpisodeStartDate],
	[PriceEpisodeActualEndDate],
	[PriceEpisodeActualInstalments],
	[PriceEpisodeAimSeqNumber],
	[PriceEpisodeCappedRemainingTNPAmount],
	[PriceEpisodeCompleted],
	[PriceEpisodeCompletionElement],
	[PriceEpisodeContractType],
	[PriceEpisodeExpectedTotalMonthlyValue],
	[PriceEpisodeFirstAdditionalPaymentThresholdDate],
	[PriceEpisodeFundLineType],
	[PriceEpisodeInstalmentValue],
	[PriceEpisodePlannedEndDate],
	[PriceEpisodePlannedInstalments],
	[PriceEpisodePreviousEarnings],
	[PriceEpisodeRemainingAmountWithinUpperLimit],
	[PriceEpisodeRemainingTNPAmount],
	[PriceEpisodeSecondAdditionalPaymentThresholdDate],
	[PriceEpisodeTotalEarnings],
	[PriceEpisodeTotalTNPPrice],
	[PriceEpisodeUpperBandLimit],
	[PriceEpisodeUpperLimitAdjustment],
	[TNP1],
	[TNP2],
	[TNP3],
	[TNP4]
from
	[Intrajob].[Rulebase].[AEC_ApprenticeshipPriceEpisode]

-- =====================================================================================================
-- AEC_ApprenticeshipPriceEpisode_Period
-- =====================================================================================================
if object_id('$(FromILRDatabase).dbo.AEC_ApprenticeshipPriceEpisode_Period','u') is not null begin

	delete from 
		[$(FromILRDatabase)].[dbo].[AEC_ApprenticeshipPriceEpisode_Period]
	where
		[UKPRN]=@ukprn
		and [Year]=@year

end else begin

	create table [$(FromILRDatabase)].[dbo].[AEC_ApprenticeshipPriceEpisode_Period]
		(
			[UKPRN] int,
			[Year] varchar(4),
			[LearnRefNumber] varchar(12),
--			[EpisodeStartDate] date,
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
			[PriceEpisodeSFAContribPct] decimal(10,5)
			primary key clustered
			(
				[UKPRN],
				[Year],
				[LearnRefNumber],
--				[EpisodeStartDate],
				[PriceEpisodeIdentifier],
				[Period]
			)
		)

end

insert into
	[$(FromILRDatabase)].[dbo].[AEC_ApprenticeshipPriceEpisode_Period]
select
	@ukprn,
	@year,
	[LearnRefNumber],
--	[EpisodeStartDate],
	[PriceEpisodeIdentifier],
	[Period],
	[PriceEpisodeApplic1618FrameworkUpliftBalancing],
	[PriceEpisodeApplic1618FrameworkUpliftCompletionPayment],
	[PriceEpisodeApplic1618FrameworkUpliftOnProgPayment],
	[PriceEpisodeBalancePayment],
	[PriceEpisodeBalanceValue],
	[PriceEpisodeCompletionPayment],
	[PriceEpisodeFirstDisadvantagePayment],
	[PriceEpisodeFirstEmp1618Pay],
	[PriceEpisodeFirstProv1618Pay],
	[PriceEpisodeInstalmentsThisPeriod],
	[PriceEpisodeLevyNonPayInd],
	[PriceEpisodeLSFCash],
	[PriceEpisodeOnProgPayment],
	[PriceEpisodeSecondDisadvantagePayment],
	[PriceEpisodeSecondEmp1618Pay],
	[PriceEpisodeSecondProv1618Pay],
	[PriceEpisodeSFAContribPct]
from
	[Rulebase].[AEC_ApprenticeshipPriceEpisode_Period]

-- =====================================================================================================
-- AEC_ApprenticeshipPriceEpisode_PeriodisedValues
-- =====================================================================================================
if object_id('$(FromILRDatabase).dbo.AEC_ApprenticeshipPriceEpisode_PeriodisedValues','u') is not null begin

	delete from 
		[$(FromILRDatabase)].[dbo].[AEC_ApprenticeshipPriceEpisode_PeriodisedValues]
	where
		[UKPRN]=@ukprn
		and [Year]=@year

end else begin

	create table [$(FromILRDatabase)].[dbo].[AEC_ApprenticeshipPriceEpisode_PeriodisedValues]
		(
			[UKPRN] int,
			[Year] varchar(4),
			[LearnRefNumber] varchar(12),
--			[EpisodeStartDate] date,
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
				[UKPRN],
				[Year],
				[LearnRefNumber],
--				[EpisodeStartDate],
				[PriceEpisodeIdentifier],
				[AttributeName]
			)
		)

end

insert into
	[$(FromILRDatabase)].[dbo].[AEC_ApprenticeshipPriceEpisode_PeriodisedValues]
select
	@ukprn,
	@year,
	[LearnRefNumber],
--	[EpisodeStartDate],
	[PriceEpisodeIdentifier],
	[AttributeName],
	[Period_1],
	[Period_2],
	[Period_3],
	[Period_4],
	[Period_5],
	[Period_6],
	[Period_7],
	[Period_8],
	[Period_9],
	[Period_10],
	[Period_11],
	[Period_12]
from
	[Rulebase].[AEC_ApprenticeshipPriceEpisode_PeriodisedValues]

---- =====================================================================================================
---- AEC_ApprenticeshipPriceEpisode_PeriodisedTextValues
---- =====================================================================================================
--if object_id('$(FromILRDatabase).dbo.AEC_ApprenticeshipPriceEpisode_PeriodisedTextValues','u') is not null begin

--	delete from 
--		[$(FromILRDatabase)].[dbo].[AEC_ApprenticeshipPriceEpisode_PeriodisedTextValues]
--	where
--		[UKPRN]=@ukprn
--		and [Year]=@year

--end else begin

--	create table [$(FromILRDatabase)].[dbo].[AEC_ApprenticeshipPriceEpisode_PeriodisedTextValues]
--		(
--			[UKPRN] int,
--			[Year] varchar(4),
--			[LearnRefNumber] varchar(12),
--			[PriceEpisodeIdentifier] varchar(25),
--			[AttributeName] [varchar](100) not null,
--			[Period_1] [varchar](255),
--			[Period_2] [varchar](255),
--			[Period_3] [varchar](255),
--			[Period_4] [varchar](255),
--			[Period_5] [varchar](255),
--			[Period_6] [varchar](255),
--			[Period_7] [varchar](255),
--			[Period_8] [varchar](255),
--			[Period_9] [varchar](255),
--			[Period_10] [varchar](255),
--			[Period_11] [varchar](255),
--			[Period_12] [varchar](255),
--			primary key clustered
--			(
--				[UKPRN],
--				[Year],
--				[LearnRefNumber],
--				[PriceEpisodeIdentifier],
--				[AttributeName]
--			)
--		)

--end

--insert into
--	[$(FromILRDatabase)].[dbo].[AEC_ApprenticeshipPriceEpisode_PeriodisedTextValues]
--select
--	@ukprn,
--	@year,
--	[LearnRefNumber],
--	[PriceEpisodeIdentifier],
--	[AttributeName],
--	[Period_1],
--	[Period_2],
--	[Period_3],
--	[Period_4],
--	[Period_5],
--	[Period_6],
--	[Period_7],
--	[Period_8],
--	[Period_9],
--	[Period_10],
--	[Period_11],
--	[Period_12]
--from
--	[Rulebase].[AEC_ApprenticeshipPriceEpisode_PeriodisedTextValues]


-- =====================================================================================================
-- AEC_HistoricEarningOutput
-- =====================================================================================================
exec [$(AECHistory)].[dbo].[PrepareForNewData] @year, @return, @ukprn

insert into
	[$(AECHistory)].[dbo].[AEC_EarningHistory]
	(
		[AppIdentifier],
		[BalancingProgAimPaymentsInTheYear],
		[CollectionReturnCode],
		[CollectionYear],
		[CompletionProgAimPaymentsInTheYear],
		[DaysInYear],
		[FworkCode],
		[LatestInYear],
		[LearnRefNumber],
		[OnProgProgAimPaymentsInTheYear],
		[ProgType],
		[PwayCode],
		[STDCode],
		[TotalProgAimPaymentsInTheYear],
		[UKPRN],
		[ULN],
		[UptoEndDate]
	)
select
	[AppIdentifierOutput],
	[HistoricBalancingProgAimPaymentsInTheYear],
	@return,
	@year,
	[HistoricCompletionProgAimPaymentsInTheYear],
	[HistoricDaysInYearOutput],
	[HistoricFworkCodeOutput],
	1 [LatestInYear],
	[LearnRefNumber],
	[HistoricOnProgProgAimPaymentsInTheYear],
	[HistoricProgTypeOutput],
	[HistoricPwayCodeOutput],
	[HistoricSTDCodeOutput],
	[HistoricTotalProgAimPaymentsInTheYear],
	@ukprn,
	[HistoricULNOutput],
	[HistoricUptoEndDateOutput]
from
	[Rulebase].[AEC_HistoricEarningOutput]