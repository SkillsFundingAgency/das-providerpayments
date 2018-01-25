--Rulebase schema

if not exists(select schema_id from sys.schemas where name='Rulebase')
begin
	exec('create schema Rulebase')
end
GO

if object_id('[Rulebase].[EFA_SFA_Cases]','u') is not null
begin
	drop table [Rulebase].[EFA_SFA_Cases]
end
GO

create table [Rulebase].[EFA_SFA_Cases]
(
	[LearnRefNumber] varchar(12),
	[CaseData] [xml] not null
)
GO

if object_id('[Rulebase].[EFA_SFA_global]','u') is not null
begin
	drop table [Rulebase].[EFA_SFA_global]
end
GO

create table [Rulebase].[EFA_SFA_global]
(
	[UKPRN] int,
	[RulebaseVersion] varchar(10),
)
GO

if object_id('[Rulebase].[EFA_SFA_Learner_Period]','u') is not null
begin
	drop table [Rulebase].[EFA_SFA_Learner_Period]
end
GO

create table [Rulebase].[EFA_SFA_Learner_Period]
(
	[LearnRefNumber] varchar(12),
	[Period] int,
	[LnrOnProgPay] decimal(10,5),
	primary key clustered
	(
		[LearnRefNumber] asc,
		[Period] asc
	)
)
GO

if object_id('[Rulebase].[EFA_SFA_Learner_PeriodisedValues]','u') is not null
begin
	drop table [Rulebase].[EFA_SFA_Learner_PeriodisedValues]
end
GO

create table [Rulebase].[EFA_SFA_Learner_PeriodisedValues]
(
	[LearnRefNumber] varchar(12),
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
		[AttributeName] asc
	)
)
GO

if object_id('[Rulebase].[EFA_Cases]','u') is not null
begin	
	drop table [Rulebase].[EFA_Cases]
end
GO

create table [Rulebase].[EFA_Cases]
(
	[LearnRefNumber] varchar(12),
	[CaseData] [xml] not null
)
GO

if object_id('[Rulebase].[EFA_global]','u') is not null
begin
	drop table [Rulebase].[EFA_global]
end
GO

create table [Rulebase].[EFA_global]
(
	[UKPRN] int,
	[LARSVersion] varchar(50),
	[OrgVersion] varchar(50),
	[PostcodeDisadvantageVersion] varchar(50),
	[RulebaseVersion] varchar(10),
)
GO

if object_id('[Rulebase].[EFA_Learner]','u') is not null
begin
	drop table [Rulebase].[EFA_Learner]
end
GO

create table [Rulebase].[EFA_Learner]
(
	[LearnRefNumber] varchar(12),
	[AcadMonthPayment] int,
	[AcadProg] bit,
	[ActualDaysILCurrYear] int,
	[AreaCostFact1618Hist] decimal(10,5),
	[Block1DisadvUpliftNew] decimal(10,5),
	[Block2DisadvElementsNew] decimal(10,5),
	[ConditionOfFundingEnglish] varchar(100),
	[ConditionOfFundingMaths] varchar(100),
	[CoreAimSeqNumber] int,
	[FullTimeEquiv] decimal(10,5),
	[FundLine] varchar(100),
	[LearnerActEndDate] date,
	[LearnerPlanEndDate] date,
	[LearnerStartDate] date,
	[NatRate] decimal(10,5),
	[OnProgPayment] decimal(10,5),
	[PlannedDaysILCurrYear] int,
	[ProgWeightHist] decimal(10,5),
	[ProgWeightNew] decimal(10,5),
	[PrvDisadvPropnHist] decimal(10,5),
	[PrvHistLrgProgPropn] decimal(10,5),
	[PrvRetentFactHist] decimal(10,5),
	[RateBand] varchar(50),
	[RetentNew] decimal(10,5),
	[StartFund] bit,
	[ThresholdDays] int
	primary key clustered
	(
		[LearnRefNumber] asc
	)
)
GO

if not exists(select schema_id from sys.schemas where name='Rulebase')
begin
	exec('create schema Rulebase')
end
GO

if object_id('[Rulebase].[VAL_Cases]','u') is not null
begin
	drop table [Rulebase].[VAL_Cases]
end
GO

create table [Rulebase].[VAL_Cases]
(
	[Learner_Id] [int] primary key,
	[CaseData] [xml] not null
)
GO

if object_id('[Rulebase].[VAL_global]','u') is not null
begin
	drop table [Rulebase].[VAL_global]
end
GO

create table [Rulebase].[VAL_global]
(
	[UKPRN] int,
	[EmployerVersion] varchar(50),
	[LARSVersion] varchar(50),
	[OrgVersion] varchar(50),
	[PostcodeVersion] varchar(50),
	[RulebaseVersion] varchar(10),
)
GO

if object_id('Rulebase.VAL_Learner', 'u') is not null
begin
	drop table [Rulebase].[VAL_Learner]
end
GO

create table [Rulebase].VAL_Learner
(
	[LearnRefNumber] varchar(12)
)
GO

if object_id('Rulebase.VAL_LearningDelivery', 'u') is not null
begin
	drop table Rulebase.VAL_LearningDelivery
end
GO

create table Rulebase.VAL_LearningDelivery
(
	[AimSeqNumber] int
)
GO

if object_id('[Rulebase].[VAL_ValidationError]','u') is not null
begin
	drop table [Rulebase].[VAL_ValidationError]
end
GO

create table [Rulebase].[VAL_ValidationError]
(
	[AimSeqNumber] bigint,
	[ErrorString] varchar(2000),
	[FieldValues] varchar(2000),
	[LearnRefNumber] varchar(100),
	[RuleId] varchar(50)
)
GO


if object_id('[Rulebase].[ALB_Cases]','u') is not null
BEGIN
	drop table [Rulebase].[ALB_Cases]
END
GO

create table [Rulebase].[ALB_Cases]
(
	[LearnRefNumber] varchar(12),
	[CaseData] [xml] not null
)
GO

if object_id('[Rulebase].[ALB_global]','u') is not null
begin
	drop table [Rulebase].[ALB_global]
END
GO

create table [Rulebase].[ALB_global]
(
	[UKPRN] int,
	[LARSVersion] varchar(100),
	[PostcodeAreaCostVersion] varchar(20),
	[RulebaseVersion] varchar(10),
)
GO

if object_id('[Rulebase].[ALB_Learner_Period]','u') is not null
BEGIN
	drop table [Rulebase].[ALB_Learner_Period]
END
GO

create table [Rulebase].[ALB_Learner_Period]
(
	[LearnRefNumber] varchar(12),
	[Period] int,
	[ALBSeqNum] int,
	primary key clustered
	(
		[LearnRefNumber] asc,
		[Period] asc
	)
)
GO

if object_id('[Rulebase].[ALB_Learner_PeriodisedValues]','u') is not null
BEGIN
	drop table [Rulebase].[ALB_Learner_PeriodisedValues]
END
GO

create table [Rulebase].[ALB_Learner_PeriodisedValues]
(
	[LearnRefNumber] varchar(12),
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
		[AttributeName] asc
	)
)
GO

if object_id('[Rulebase].[ALB_LearningDelivery]','u') is not null
begin
	drop table [Rulebase].[ALB_LearningDelivery]
END
GO

create table [Rulebase].[ALB_LearningDelivery]
(
	[LearnRefNumber] varchar(12),
	[AimSeqNumber] int,
	[Achieved] bit,
	[ActualNumInstalm] int,
	[AdvLoan] bit,
	[ApplicFactDate] date,
	[ApplicProgWeightFact] varchar(1),
	[AreaCostFactAdj] decimal(10,5),
	[AreaCostInstalment] decimal(10,5),
	[FundLine] varchar(50),
	[FundStart] bit,
	[LiabilityDate] date,
	[LoanBursAreaUplift] bit,
	[LoanBursSupp] bit,
	[OutstndNumOnProgInstalm] int,
	[PlannedNumOnProgInstalm] int,
	[WeightedRate] decimal(10,4)
	primary key clustered
	(
		[LearnRefNumber] asc,
		[AimSeqNumber] asc
	)
)
GO

if object_id('[Rulebase].[ALB_LearningDelivery_Period]','u') is not null
BEGIN
	drop table [Rulebase].[ALB_LearningDelivery_Period]
end
GO

create table [Rulebase].[ALB_LearningDelivery_Period]
( 
	[LearnRefNumber] varchar(12),
	[AimSeqNumber] int,
	[Period] int,
	[ALBCode] int,
	[ALBSupportPayment] decimal(10,5),
	[AreaUpliftBalPayment] decimal(10,5),
	[AreaUpliftOnProgPayment] decimal(10,5),
	primary key clustered
	(
		[LearnRefNumber] asc,
		[AimSeqNumber] asc,
		[Period] asc
	)
)
GO

if object_id('[Rulebase].[ALB_LearningDelivery_PeriodisedValues]','u') is not null
BEGIN
	drop table [Rulebase].[ALB_LearningDelivery_PeriodisedValues]
END
GO

create table [Rulebase].[ALB_LearningDelivery_PeriodisedValues]
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


if object_id('[Rulebase].[VALDP_Cases]','u') is not null
BEGIN
	drop table [Rulebase].[VALDP_Cases]
END
GO

create table [Rulebase].[VALDP_Cases]
(
	[LearnerDestinationAndProgression_Id] [int] primary key,
	[CaseData] [xml] not null
)
GO

if object_id('[Rulebase].[VALDP_global]','u') is not null
BEGIN
	drop table [Rulebase].[VALDP_global]
END
GO

create table [Rulebase].[VALDP_global]
(
	[UKPRN] int,
	[OrgVersion] varchar(50),
	[RulebaseVersion] varchar(10),
	[ULNVersion] varchar(50)
)
GO

if object_id('[Rulebase].[VALDP_ValidationError]','u') is not null
BEGIN
	drop table [Rulebase].[VALDP_ValidationError]
END
GO

create table [Rulebase].[VALDP_ValidationError]
(
	[ErrorString] varchar(2000),
	[FieldValues] varchar(2000),
	[LearnRefNumber] varchar(100),
	[RuleId] varchar(50)
)
GO


if object_id('[Rulebase].[VAL_Cases]','u') is not null
begin
	drop table [Rulebase].[VAL_Cases]
end
GO

create table [Rulebase].[VAL_Cases]
(
	[Learner_Id] [int] primary key,
	[CaseData] [xml] not null
)
GO

if object_id('[Rulebase].[VAL_global]','u') is not null
begin
	drop table [Rulebase].[VAL_global]
end
GO

create table [Rulebase].[VAL_global]
(
	[UKPRN] int,
	[EmployerVersion] varchar(50),
	[LARSVersion] varchar(50),
	[OrgVersion] varchar(50),
	[PostcodeVersion] varchar(50),
	[RulebaseVersion] varchar(10),
)
GO

if object_id('[Rulebase].[VAL_ValidationError]','u') is not null
begin
	drop table [Rulebase].[VAL_ValidationError]
end
GO

create table [Rulebase].[VAL_ValidationError]
(
	[AimSeqNumber] bigint,
	[ErrorString] varchar(2000),
	[FieldValues] varchar(2000),
	[LearnRefNumber] varchar(100),
	[RuleId] varchar(50)
)
GO

if object_id('[Rulebase].[SFA_Cases]','u') is not null
BEGIN
	drop table [Rulebase].[SFA_Cases]
END
GO

create table [Rulebase].[SFA_Cases]
(
	[LearnRefNumber] varchar(12),
	[CaseData] [xml] not null
)
GO

if object_id('[Rulebase].[SFA_global]','u') is not null
BEGIN
	drop table [Rulebase].[SFA_global]
END
GO

create table [Rulebase].[SFA_global]
(
	[UKPRN] varchar(8),
	[CurFundYr] varchar(9),
	[LARSVersion] varchar(100),
	[OrgVersion] varchar(100),
	[PostcodeDisadvantageVersion] varchar(50),
	[RulebaseVersion] varchar(10),
)
GO

if object_id('[Rulebase].[SFA_LearningDelivery]','u') is not null
BEGIN
	drop table [Rulebase].[SFA_LearningDelivery]
END
GO

create table [Rulebase].[SFA_LearningDelivery]
(
	[LearnRefNumber] varchar(12),
	[AimSeqNumber] int,
	[AchApplicDate] date,
	[Achieved] bit,
	[AchieveElement] decimal(10,5),
	[AchievePayElig] bit,
	[AchievePayPctPreTrans] decimal(10,5),
	[AchPayTransHeldBack] decimal(10,5),
	[ActualDaysIL] int,
	[ActualNumInstalm] int,
	[ActualNumInstalmPreTrans] int,
	[ActualNumInstalmTrans] int,
	[AdjLearnStartDate] date,
	[AdltLearnResp] bit,
	[AgeAimStart] int,
	[AimValue] decimal(10,5),
	[AppAdjLearnStartDate] date,
	[AppAgeFact] decimal(10,5),
	[AppATAGTA] bit,
	[AppCompetency] bit,
	[AppFuncSkill] bit,
	[AppFuncSkill1618AdjFact] decimal(10,5),
	[AppKnowl] bit,
	[AppLearnStartDate] date,
	[ApplicEmpFactDate] date,
	[ApplicFactDate] date,
	[ApplicFundRateDate] date,
	[ApplicProgWeightFact] varchar(1),
	[ApplicUnweightFundRate] decimal(10,5),
	[ApplicWeightFundRate] decimal(10,5),
	[AppNonFund] bit,
	[AreaCostFactAdj] decimal(10,5),
	[BalInstalmPreTrans] int,
	[BaseValueUnweight] decimal(10,5),
	[CapFactor] decimal(10,5),
	[DisUpFactAdj] decimal(10,4),
	[EmpOutcomePayElig] bit,
	[EmpOutcomePctHeldBackTrans] decimal(10,5),
	[EmpOutcomePctPreTrans] decimal(10,5),
	[EmpRespOth] bit,
	[ESOL] bit,
	[FullyFund] bit,
	[FundLine] varchar(100),
	[FundStart] bit,
	[LargeEmployerID] int,
	[LargeEmployerSFAFctr] decimal(10,2),
	[LargeEmployerStatusDate] date,
	[LTRCUpliftFctr] decimal(10,5),
	[NonGovCont] decimal(10,5),
	[OLASSCustody] bit,
	[OnProgPayPctPreTrans] decimal(10,5),
	[OutstndNumOnProgInstalm] int,
	[OutstndNumOnProgInstalmTrans] int,
	[PlannedNumOnProgInstalm] int,
	[PlannedNumOnProgInstalmTrans] int,
	[PlannedTotalDaysIL] int,
	[PlannedTotalDaysILPreTrans] int,
	[PropFundRemain] decimal(10,2),
	[PropFundRemainAch] decimal(10,2),
	[PrscHEAim] bit,
	[Residential] bit,
	[Restart] bit,
	[SpecResUplift] decimal(10,5),
	[StartPropTrans] decimal(10,5),
	[ThresholdDays] int,
	[Traineeship] bit,
	[Trans] bit,
	[TrnAdjLearnStartDate] date,
	[TrnWorkPlaceAim] bit,
	[TrnWorkPrepAim] bit,
	[UnWeightedRateFromESOL] decimal(10,5),
	[UnweightedRateFromLARS] decimal(10,5),
	[WeightedRateFromESOL] decimal(10,5),
	[WeightedRateFromLARS] decimal(10,5)
	primary key clustered
	(
		[LearnRefNumber] asc,
		[AimSeqNumber] asc
	)
)
GO

if object_id('[Rulebase].[SFA_LearningDelivery_Period]','u') is not null
BEGIN
	drop table [Rulebase].[SFA_LearningDelivery_Period]
END
GO

create table [Rulebase].[SFA_LearningDelivery_Period]
(
	[LearnRefNumber] varchar(12),
	[AimSeqNumber] int,
	[Period] int,
	[AchievePayment] decimal(10,5),
	[AchievePayPct] decimal(10,5),
	[AchievePayPctTrans] decimal(10,5),
	[BalancePayment] decimal(10,5),
	[BalancePaymentUncapped] decimal(10,5),
	[BalancePct] decimal(10,5),
	[BalancePctTrans] decimal(10,5),
	[EmpOutcomePay] decimal(10,5),
	[EmpOutcomePct] decimal(10,5),
	[EmpOutcomePctTrans] decimal(10,5),
	[InstPerPeriod] int,
	[LearnSuppFund] bit,
	[LearnSuppFundCash] decimal(10,5),
	[OnProgPayment] decimal(10,5),
	[OnProgPaymentUncapped] decimal(10,5),
	[OnProgPayPct] decimal(10,5),
	[OnProgPayPctTrans] decimal(10,5),
	[TransInstPerPeriod] int,
	primary key clustered
	(
		[LearnRefNumber] asc,
		[AimSeqNumber] asc,
		[Period] asc
	)
)
GO

if object_id('[Rulebase].[SFA_LearningDelivery_PeriodisedValues]','u') is not null
BEGIN
	drop table [Rulebase].[SFA_LearningDelivery_PeriodisedValues]
END
GO

create table [Rulebase].[SFA_LearningDelivery_PeriodisedValues]
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

if object_id('[Rulebase].[TBL_Cases]','u') is not null
BEGIN
	drop table [Rulebase].[TBL_Cases]
END
GO

create table [Rulebase].[TBL_Cases]
(
	[LearnRefNumber] varchar(12),
	[CaseData] [xml] not null
)
GO

if object_id('[Rulebase].[TBL_global]','u') is not null
BEGIN
	drop table [Rulebase].[TBL_global]
END
GO

create table [Rulebase].[TBL_global]
(
	[UKPRN] int,
	[CurFundYr] varchar(10),
	[LARSVersion] varchar(100),
	[RulebaseVersion] varchar(10),
)
GO

if object_id('[Rulebase].[TBL_LearningDelivery]','u') is not null
BEGIN
	drop table [Rulebase].[TBL_LearningDelivery]
END
GO

create table [Rulebase].[TBL_LearningDelivery]
(	
	LearnRefNumber varchar(12)
	,AimSeqNumber int
	,ProgStandardStartDate date
	,FundLine varchar(50)
	,MathEngAimValue	decimal(10,5)
	,PlannedNumOnProgInstalm int
	,LearnSuppFundCash decimal(10,5)
	,AdjProgStartDate date
	,LearnSuppFund bit
	,MathEnGOnProgPayment decimal(10,5)
	,InstPerPeriod int
	,SmallBusPayment decimal(10,5)
	,YoungAppSecondPayment decimal(10,5)
	,CoreGOvContPayment decimal(10,5)
	,YoungAppEligible bit
	,SmallBusEligible bit
	,MathEnGOnProgPct int
	,AgeStandardStart int
	,YoungAppSecondThresholdDate date
	,EmpIdFirstDayStandard int
	,EmpIdFirstYoungAppDate	int
	,EmpIdSecondYoungAppDate int
	,EmpIdSmallBusDate int 
	,YoungAppFirstThresholdDate date 
	,AchApplicDate date
	,AchEligible bit
	,Achieved bit
	,AchievementApplicVal decimal(10,5)
	,AchPayment decimal(10,5)
	,ActualNumInstalm int
	,CombinedAdjProp bigint
	,EmpIdAchDate int
	,LearnDelDaysIL int
	,LearnDelStandardAccDaysIL int
	,LearnDelStandardPrevAccDaysIL int
	,LearnDelStandardTotalDaysIL int
	,ActualDaysIL int
	,MathEngBalPayment decimal(10,5)
	,MathEngBalPct bigint
	,MathEngLSFFundStart bit
	,PlannedTotalDaysIL int
	,MathEngLSFThresholdDays int
	,OutstandNumOnProgInstalm int
	,SmallBusApplicVal decimal(10,5)
	,SmallBusStatusFirstDayStandard int
	,SmallBusStatusThreshold int
	,YoungAppApplicVal decimal(10,5)
	,CoreGOvContCapApplicVal bigint
	,CoreGOvContUncapped decimal(10,5)
	,ApplicFundValDate date
	,YoungAppFirstPayment decimal(10,5)
	,YoungAppPayment decimal(10,5)	
	primary key clustered
	(
		[LearnRefNumber] asc,
		[AimSeqNumber] asc
	)
)
GO

if object_id('[Rulebase].[TBL_LearningDelivery_Period]','u') is not null
BEGIN
	drop table [Rulebase].[TBL_LearningDelivery_Period]
END
GO

create table [Rulebase].[TBL_LearningDelivery_Period]
(
	[LearnRefNumber] varchar(12),
	[AimSeqNumber] int,
	[Period] int,
	[AchPayment] decimal(10,5),
	[CoreGOvContPayment] decimal(10,5),
	[CoreGOvContUncapped] decimal(10,5),
	[InstPerPeriod] int,
	[LearnSuppFund] bit,
	[LearnSuppFundCash] decimal(10,5),
	[MathEngBalPayment] decimal(10,5),
	[MathEngBalPct] decimal(8,5),
	[MathEnGOnProgPayment] decimal(10,5),
	[MathEnGOnProgPct] decimal(8,5),
	[SmallBusPayment] decimal(10,5),
	[YoungAppFirstPayment] decimal(10,5),
	[YoungAppPayment] decimal(10,5),
	[YoungAppSecondPayment] decimal(10,5),
	primary key clustered
	(
		[LearnRefNumber] asc,
		[AimSeqNumber] asc,
		[Period] asc
	)
)
GO

if object_id('[Rulebase].[TBL_LearningDelivery_PeriodisedValues]','u') is not null
BEGIN
	drop table [Rulebase].[TBL_LearningDelivery_PeriodisedValues]
END
GO

create table [Rulebase].[TBL_LearningDelivery_PeriodisedValues]
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
if not exists(select schema_id from sys.schemas where name='Rulebase')
	exec('create schema Rulebase')
GO

if object_id('[Rulebase].[AEC_Cases]','u') is not null
begin
	drop table [Rulebase].[AEC_Cases]
end
GO

create table [Rulebase].[AEC_Cases]
	(
		[LearnRefNumber] varchar(12),
		[CaseData] [xml] not null
	)
GO

if object_id('[Rulebase].[AEC_global]','u') is not null
begin
	drop table [Rulebase].[AEC_global]
end
GO

create table [Rulebase].[AEC_global]
	(
		[UKPRN] int,
		[LARSVersion] varchar(100),
		[RulebaseVersion] varchar(10),
		[Year] varchar(4)
	)
GO

if object_id('[Rulebase].[AEC_LearningDelivery]','u') is not null
begin
	drop table [Rulebase].[AEC_LearningDelivery]
end
GO

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
		[CombinedAdjProp] decimal(12,5),
		[Completed] bit,
		[FirstIncentiveThresholdDate] date,
		[FundStart] bit,
		[LDApplic1618FrameworkUpliftBalancingValue] decimal(12,5),
		[LDApplic1618FrameworkUpliftCompElement] decimal(12,5),
		[LDApplic1618FRameworkUpliftCompletionValue] decimal(12,5),
		[LDApplic1618FrameworkUpliftMonthInstalVal] decimal(12,5),
		[LDApplic1618FrameworkUpliftPrevEarnings] decimal(12,5),
		[LDApplic1618FrameworkUpliftPrevEarningsStage1] decimal(12,5),
		[LDApplic1618FrameworkUpliftRemainingAmount] decimal(12,5),
		[LDApplic1618FrameworkUpliftTotalActEarnings] decimal(12,5),
		[LearnAimRef] varchar(8),
		[LearnDel1618AtStart] bit,
		[LearnDelAppAccDaysIL] int,
		[LearnDelApplicDisadvAmount] decimal(12,5),
		[LearnDelApplicEmp1618Incentive] decimal(12,5),
		[LearnDelApplicEmpDate] date,
		[LearnDelApplicProv1618FrameworkUplift] decimal(12,5),
		[LearnDelApplicProv1618Incentive] decimal(12,5),
		[LearnDelAppPrevAccDaysIL] int,
		[LearnDelDaysIL] int,
		[LearnDelDisadAmount] decimal(12,5),
		[LearnDelEligDisadvPayment] bit,
		[LearnDelEmpIdFirstAdditionalPaymentThreshold] int,
		[LearnDelEmpIdSecondAdditionalPaymentThreshold] int,
		[LearnDelHistDaysThisApp] int,
		[LearnDelHistProgEarnings] decimal(12,5),
		[LearnDelInitialFundLineType] varchar(100),
		[LearnDelMathEng] bit,
		[LearnDelProgEarliestACT2Date] date,
		[LearnDelNonLevyProcured] bit,
		[MathEngAimValue] decimal(12,5),
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
begin
	drop table [Rulebase].[AEC_LearningDelivery_Period]
end
GO

create table [Rulebase].[AEC_LearningDelivery_Period]
	(
		[LearnRefNumber] varchar(12),
		[AimSeqNumber] int,
		[Period] int,
		[DisadvFirstPayment] decimal(12,5),
		[DisadvSecondPayment] decimal(12,5),
		[FundLineType] varchar(100),
		[InstPerPeriod] int,
		[LDApplic1618FrameworkUpliftBalancingPayment] decimal(12,5),
		[LDApplic1618FrameworkUpliftCompletionPayment] decimal(12,5),
		[LDApplic1618FrameworkUpliftOnProgPayment] decimal(12,5),
		[LearnDelContType] varchar(50),
		[LearnDelFirstEmp1618Pay] decimal(12,5),
		[LearnDelFirstProv1618Pay] decimal(12,5),
		[LearnDelLevyNonPayInd] int,
		[LearnDelSecondEmp1618Pay] decimal(12,5),
		[LearnDelSecondProv1618Pay] decimal(12,5),
		[LearnDelSEMContWaiver] bit,
		[LearnDelSFAContribPct] decimal(12,5),
		[LearnSuppFund] bit,
		[LearnSuppFundCash] decimal(12,5),
		[MathEngBalPayment] decimal(12,5),
		[MathEngBalPct] decimal(12,5),
		[MathEnGOnProgPayment] decimal(12,5),
		[MathEnGOnProgPct] decimal(12,5),
		[ProgrammeAimBalPayment] decimal(12,5),
		[ProgrammeAimCompletionPayment] decimal(12,5),
		[ProgrammeAimOnProgPayment] decimal(12,5),
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
GO

if object_id('[Rulebase].[AEC_LearningDelivery_PeriodisedValues]','u') is not null
begin
	drop table [Rulebase].[AEC_LearningDelivery_PeriodisedValues]
end
GO

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
begin
	drop table [Rulebase].[AEC_LearningDelivery_PeriodisedTextValues]
end
GO

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

if object_id('[Rulebase].[AEC_HistoricEarninGOutput]','u') is not null
begin
	drop table [Rulebase].[AEC_HistoricEarninGOutput]
end
GO

create table [Rulebase].[AEC_HistoricEarninGOutput]
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
		[HistoricTotal1618UpliftPaymentsInTheYear] decimal(12,5),
		[HistoricTotalProgAimPaymentsInTheYear] decimal(12,5),
		[HistoricULNOutput] bigint,
		[HistoricUptoEndDateOutput] date,
		[HistoricVirtualTNP3EndofThisYearOutput] decimal(12,5),
		[HistoricVirtualTNP4EndofThisYearOutput] decimal(12,5),
		[HistoricLearnDelProgEarliestACT2DateOutput] date
		primary key clustered
		(
			[LearnRefNumber] asc,
			[AppIdentifierOutput] asc
		)
	)
GO

if object_id('[Rulebase].[AEC_ApprenticeshipPriceEpisode]','u') is not null
begin
	drop table [Rulebase].[AEC_ApprenticeshipPriceEpisode]
end
GO

create table Rulebase.AEC_ApprenticeshipPriceEpisode
(
	LearnRefNumber varchar(12) not null
    ,PriceEpisodeIdentifier varchar(25) not null
    ,TNP4 decimal(12,5)
    ,TNP1 decimal(12,5)
    ,EpisodeStartDate date
    ,TNP2 decimal(12, 5)
    ,TNP3 decimal(12, 5)
    ,PriceEpisodeUpperBandLimit decimal(12, 5)
    ,PriceEpisodePlannedEndDate date
    ,PriceEpisodeActualEndDate date
    ,PriceEpisodeTotalTNPPrice decimal(12, 5)
    ,PriceEpisodeUpperLimitAdjustment decimal(12, 5)
    ,PriceEpisodePlannedInstalments INT
    ,PriceEpisodeActualInstalments INT
    ,PriceEpisodeInstalmentsThisPeriod INT
    ,PriceEpisodeCompletionElement decimal(12, 5)
    ,PriceEpisodePreviousEarnings decimal(12, 5)
    ,PriceEpisodeInstalmentValue decimal(12, 5)
    ,PriceEpisodeOnProgPayment decimal(12, 5)
    ,PriceEpisodeTotalEarnings decimal(12, 5)
    ,PriceEpisodeBalanceValue decimal(12, 5)
    ,PriceEpisodeBalancePayment decimal(12, 5)
    ,PriceEpisodeCompleted BIT
    ,PriceEpisodeCompletionPayment decimal(12, 5)
    ,PriceEpisodeRemainingTNPAmount decimal(12, 5)
    ,PriceEpisodeRemainingAmountWithinUpperLimit decimal(12, 5)
    ,PriceEpisodeCappedRemainingTNPAmount decimal(12, 5)
    ,PriceEpisodeExpectedTotalMonthlyValue decimal(12, 5)
    ,PriceEpisodeAimSeqNumber bigint
    ,PriceEpisodeFirstDisadvantagePayment decimal(12, 5)
    ,PriceEpisodeSecondDisadvantagePayment decimal(12, 5)
    ,PriceEpisodeApplic1618FrameworkUpliftBalancing decimal(12, 5)
    ,PriceEpisodeApplic1618FrameworkUpliftCompletionPayment decimal(12, 5)
    ,PriceEpisodeApplic1618FrameworkUpliftOnProgPayment decimal(12, 5)
    ,PriceEpisodeSecondProv1618Pay decimal(12, 5)
    ,PriceEpisodeFirstEmp1618Pay decimal(12, 5)
    ,PriceEpisodeSecondEmp1618Pay decimal(12, 5)
    ,PriceEpisodeFirstProv1618Pay decimal(12, 5)
    ,PriceEpisodeLSFCash decimal(12, 5)
    ,PriceEpisodeFundLineType varchar(100)
    ,PriceEpisodeSFAContribPct decimal(12, 5)
    ,PriceEpisodeLevyNonPayInd INT
    ,EpisodeEffectiveTNPStartDate DATE
    ,PriceEpisodeFirstAdditionalPaymentThresholdDate date
    ,PriceEpisodeSecondAdditionalPaymentThresholdDate DATE
    ,PriceEpisodeContractType varchar(50)
    ,PriceEpisodePreviousEarningsSameProvider decimal(12, 5)
    ,PriceEpisodeTotProgFunding decimal(12, 5)
    ,PriceEpisodeProgFundIndMinCoInvest decimal(12, 5)
    ,PriceEpisodeProgFundIndMaxEmpCont decimal(12, 5)
    ,PriceEpisodeTotalPMRs decimal(12, 5)
    ,PriceEpisodeCumulativePMRs decimal(12, 5)
    ,PriceEpisodeCompExemCode int
	,primary key
	(
		LearnRefNumber
		,PriceEpisodeIdentifier
	)
)
GO

if object_id('[Rulebase].[AEC_ApprenticeshipPriceEpisode_Period]','u') is not null
begin
	drop table [Rulebase].[AEC_ApprenticeshipPriceEpisode_Period]
end
GO

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
		[PriceEpisodeSFAContribPct] decimal(12,5),
		[PriceEpisodeTotProgFunding] decimal(12,5),
		primary key clustered
		(
			[LearnRefNumber] asc,
			[PriceEpisodeIdentifier] asc,
			[Period] asc
		)
	)
GO

if object_id('[Rulebase].[AEC_ApprenticeshipPriceEpisode_PeriodisedValues]','u') is not null
begin
	drop table [Rulebase].[AEC_ApprenticeshipPriceEpisode_PeriodisedValues]
end
GO

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

if not exists(select schema_id from sys.schemas where name='Rulebase')
	exec('create schema Rulebase')
GO
if object_id('[Rulebase].[ESFVAL_Cases]','u') is not null
	drop table [Rulebase].[ESFVAL_Cases]
create table [Rulebase].[ESFVAL_Cases]
	(
		[Learner_Id] [int] primary key,
		[CaseData] [xml] not null
	)
GO
if object_id('[Rulebase].[ESFVAL_global]','u') is not null
	drop table [Rulebase].[ESFVAL_global]
create table [Rulebase].[ESFVAL_global]
	(
		[UKPRN] int,
		[RulebaseVersion] varchar(10),
	)
GO
if object_id('[Rulebase].[ESFVAL_ValidationError]','u') is not null
	drop table [Rulebase].[ESFVAL_ValidationError]
create table [Rulebase].[ESFVAL_ValidationError]
	(
		[AimSeqNumber] bigint,
		[ErrorString] varchar(2000),
		[FieldValues] varchar(2000),
		[LearnRefNumber] varchar(100),
		[RuleId] varchar(50)
	)
GO

if object_id('Rulebase.VALFD_ValidationError','u') is not null
	drop table [Rulebase].[VALFD_ValidationError]
GO

create table [Rulebase].[VALFD_ValidationError]
	(
		[AimSeqNumber] bigint null,
		[ErrorString] varchar(2000) null,
		[FieldValues] varchar(2000) null,
		[LearnRefNumber] varchar(100) null,
		[RuleId] varchar(50) null
	)
GO

if object_id('[Rulebase].[DV_Cases]','u') is not null
begin
	drop table [Rulebase].[DV_Cases]
end
GO

create table [Rulebase].[DV_Cases]
	(
		[LearnRefNumber] varchar(12),
		[CaseData] [xml] not null
	)
GO

if object_id('[Rulebase].[DV_global]','u') is not null
begin
	drop table [Rulebase].[DV_global]
end
GO

create table [Rulebase].[DV_global]
	(
		[UKPRN] int,
		[RulebaseVersion] varchar(10),
	)
GO

if object_id('[Rulebase].[DV_Learner]','u') is not null
begin
	drop table [Rulebase].[DV_Learner]
end
GO

create table [Rulebase].[DV_Learner]
	(
		[LearnRefNumber] varchar(12),
		[Learn_3rdSector] int,
		[Learn_Active] int,
		[Learn_ActiveJan] int,
		[Learn_ActiveNov] int,
		[Learn_ActiveOct] int,
		[Learn_Age31Aug] int,
		[Learn_BasicSkill] int,
		[Learn_EmpStatFDL] int,
		[Learn_EmpStatPrior] int,
		[Learn_FirstFullLevel2] int,
		[Learn_FirstFullLevel2Ach] int,
		[Learn_FirstFullLevel3] int,
		[Learn_FirstFullLevel3Ach] int,
		[Learn_FullLevel2] int,
		[Learn_FullLevel2Ach] int,
		[Learn_FullLevel3] int,
		[Learn_FullLevel3Ach] int,
		[Learn_FundAgency] int,
		[Learn_FundingSource] int,
		[Learn_FundPrvYr] int,
		[Learn_ILAcMonth1] int,
		[Learn_ILAcMonth10] int,
		[Learn_ILAcMonth11] int,
		[Learn_ILAcMonth12] int,
		[Learn_ILAcMonth2] int,
		[Learn_ILAcMonth3] int,
		[Learn_ILAcMonth4] int,
		[Learn_ILAcMonth5] int,
		[Learn_ILAcMonth6] int,
		[Learn_ILAcMonth7] int,
		[Learn_ILAcMonth8] int,
		[Learn_ILAcMonth9] int,
		[Learn_ILCurrAcYr] int,
		[Learn_LargeEmployer] int,
		[Learn_LenEmp] int,
		[Learn_LenUnemp] int,
		[Learn_LrnAimRecords] int,
		[Learn_ModeAttPlanHrs] int,
		[Learn_NotionLev] int,
		[Learn_NotionLevV2] int,
		[Learn_OLASS] int,
		[Learn_PrefMethContact] int,
		[Learn_PrimaryLLDD] int,
		[Learn_PriorEducationStatus] int,
		[Learn_UnempBenFDL] int,
		[Learn_UnempBenPrior] int,
		[Learn_Uplift1516EFA] decimal(6,5),
		[Learn_Uplift1516SFA] decimal(6,5),
		primary key clustered
		(
			[LearnRefNumber] asc
		)
	)
GO

if object_id('[Rulebase].[DV_LearningDelivery]','u') is not null
begin
	drop table [Rulebase].[DV_LearningDelivery]
end
GO

create table [Rulebase].[DV_LearningDelivery]
	(
		[LearnRefNumber] varchar(12),
		[AimSeqNumber] int,
		[LearnDel_AccToApp] int,
		[LearnDel_AccToAppEmpDate] date,
		[LearnDel_AccToAppEmpStat] int,
		[LearnDel_AchFullLevel2Pct] decimal(5,2),
		[LearnDel_AchFullLevel3Pct] decimal(5,2),
		[LearnDel_Achieved] int,
		[LearnDel_AchievedIY] int,
		[LearnDel_AcMonthYTD] varchar(2),
		[LearnDel_ActDaysILAfterCurrAcYr] int,
		[LearnDel_ActDaysILCurrAcYr] int,
		[LearnDel_ActEndDateOnAfterJan1] int,
		[LearnDel_ActEndDateOnAfterNov1] int,
		[LearnDel_ActEndDateOnAfterOct1] int,
		[LearnDel_ActiveIY] int,
		[LearnDel_ActiveJan] int,
		[LearnDel_ActiveNov] int,
		[LearnDel_ActiveOct] int,
		[LearnDel_ActTotalDaysIL] int,
		[LearnDel_AdvLoan] int,
		[LearnDel_AgeAimOrigStart] int,
		[LearnDel_AgeAtStart] int,
		[LearnDel_App] int,
		[LearnDel_App1618Fund] int,
		[LearnDel_App1925Fund] int,
		[LearnDel_AppAimType] int,
		[LearnDel_AppKnowl] int,
		[LearnDel_AppMainAim] int,
		[LearnDel_AppNonFund] int,
		[LearnDel_BasicSkills] int,
		[LearnDel_BasicSkillsParticipation] int,
		[LearnDel_BasicSkillsType] int,
		[LearnDel_CarryIn] int,
		[LearnDel_ClassRm] int,
		[LearnDel_CompAimApp] int,
		[LearnDel_CompAimProg] int,
		[LearnDel_Completed] int,
		[LearnDel_CompletedIY] int,
		[LearnDel_CompleteFullLevel2Pct] decimal(5,2),
		[LearnDel_CompleteFullLevel3Pct] decimal(5,2),
		[LearnDel_EFACoreAim] int,
		[LearnDel_Emp6MonthAimStart] int,
		[LearnDel_Emp6MonthProgStart] int,
		[LearnDel_EmpDateBeforeFDL] date,
		[LearnDel_EmpDatePriorFDL] date,
		[LearnDel_EmpID] int,
		[LearnDel_Employed] int,
		[LearnDel_EmpStatFDL] int,
		[LearnDel_EmpStatPrior] int,
		[LearnDel_EmpStatPriorFDL] int,
		[LearnDel_EnhanAppFund] int,
		[LearnDel_FullLevel2AchPct] decimal(5,2),
		[LearnDel_FullLevel2ContPct] decimal(5,2),
		[LearnDel_FullLevel3AchPct] decimal(5,2),
		[LearnDel_FullLevel3ContPct] decimal(5,2),
		[LearnDel_FuncSkills] int,
		[LearnDel_FundAgency] int,
		[LearnDel_FundingLineType] varchar(100),
		[LearnDel_FundingSource] int,
		[LearnDel_FundPrvYr] int,
		[LearnDel_FundStart] int,
		[LearnDel_GCE] int,
		[LearnDel_GCSE] int,
		[LearnDel_ILAcMonth1] int,
		[LearnDel_ILAcMonth10] int,
		[LearnDel_ILAcMonth11] int,
		[LearnDel_ILAcMonth12] int,
		[LearnDel_ILAcMonth2] int,
		[LearnDel_ILAcMonth3] int,
		[LearnDel_ILAcMonth4] int,
		[LearnDel_ILAcMonth5] int,
		[LearnDel_ILAcMonth6] int,
		[LearnDel_ILAcMonth7] int,
		[LearnDel_ILAcMonth8] int,
		[LearnDel_ILAcMonth9] int,
		[LearnDel_ILCurrAcYr] int,
		[LearnDel_IYActEndDate] date,
		[LearnDel_IYPlanEndDate] date,
		[LearnDel_IYStartDate] date,
		[LearnDel_KeySkills] int,
		[LearnDel_LargeEmpDiscountId] int,
		[LearnDel_LargeEmployer] int,
		[LearnDel_LastEmpDate] date,
		[LearnDel_LeaveMonth] int,
		[LearnDel_LenEmp] int,
		[LearnDel_LenUnemp] int,
		[LearnDel_LoanBursFund] int,
		[LearnDel_NotionLevel] int,
		[LearnDel_NotionLevelV2] int,
		[LearnDel_NumHEDatasets] int,
		[LearnDel_OccupAim] int,
		[LearnDel_OLASS] int,
		[LearnDel_OLASSCom] int,
		[LearnDel_OLASSCus] int,
		[LearnDel_OrigStartDate] date,
		[LearnDel_PlanDaysILAfterCurrAcYr] int,
		[LearnDel_PlanDaysILCurrAcYr] int,
		[LearnDel_PlanEndBeforeAug1] int,
		[LearnDel_PlanEndOnAfterJan1] int,
		[LearnDel_PlanEndOnAfterNov1] int,
		[LearnDel_PlanEndOnAfterOct1] int,
		[LearnDel_PlanTotalDaysIL] int,
		[LearnDel_PriorEducationStatus] int,
		[LearnDel_Prog] int,
		[LearnDel_ProgAimAch] int,
		[LearnDel_ProgAimApp] int,
		[LearnDel_ProgCompleted] int,
		[LearnDel_ProgCompletedIY] int,
		[LearnDel_ProgStartDate] date,
		[LearnDel_QCF] int,
		[LearnDel_QCFCert] int,
		[LearnDel_QCFDipl] int,
		[LearnDel_QCFType] int,
		[LearnDel_RegAim] int,
		[LearnDel_SecSubAreaTier1] varchar(3),
		[LearnDel_SecSubAreaTier2] varchar(5),
		[LearnDel_SFAApproved] int,
		[LearnDel_SourceFundEFA] int,
		[LearnDel_SourceFundSFA] int,
		[LearnDel_StartBeforeApr1] int,
		[LearnDel_StartBeforeAug1] int,
		[LearnDel_StartBeforeDec1] int,
		[LearnDel_StartBeforeFeb1] int,
		[LearnDel_StartBeforeJan1] int,
		[LearnDel_StartBeforeJun1] int,
		[LearnDel_StartBeforeMar1] int,
		[LearnDel_StartBeforeMay1] int,
		[LearnDel_StartBeforeNov1] int,
		[LearnDel_StartBeforeOct1] int,
		[LearnDel_StartBeforeSep1] int,
		[LearnDel_StartIY] int,
		[LearnDel_StartJan1] int,
		[LearnDel_StartMonth] int,
		[LearnDel_StartNov1] int,
		[LearnDel_StartOct1] int,
		[LearnDel_SuccRateStat] int,
		[LearnDel_TrainAimType] int,
		[LearnDel_TransferDiffProvider] int,
		[LearnDel_TransferDiffProviderGOvStrat] int,
		[LearnDel_TransferProvider] int,
		[LearnDel_UfIProv] int,
		[LearnDel_UnempBenFDL] int,
		[LearnDel_UnempBenPrior] int,
		[LearnDel_Withdrawn] int,
		[LearnDel_WorkplaceLocPostcode] varchar(8),
		[Prog_AccToApp] int,
		[Prog_Achieved] int,
		[Prog_AchievedIY] int,
		[Prog_ActEndDate] date,
		[Prog_ActiveIY] int,
		[Prog_AgeAtStart] int,
		[Prog_EarliestAim] int,
		[Prog_Employed] int,
		[Prog_FundPrvYr] int,
		[Prog_ILCurrAcYear] int,
		[Prog_LatestAim] int,
		[Prog_SourceFundEFA] int,
		[Prog_SourceFundSFA] int
		primary key clustered
		(
			[LearnRefNumber] asc,
			[AimSeqNumber] asc
		)
	)
GO
