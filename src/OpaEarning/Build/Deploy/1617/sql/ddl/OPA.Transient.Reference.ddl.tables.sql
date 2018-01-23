if not exists(select schema_id from sys.schemas where name='Reference')
	exec('create schema [Reference]')
GO

if object_id('[Reference].[AEC_LatestInYearEarningHistory]','u') is not null
	drop table [Reference].[AEC_LatestInYearEarningHistory]
GO
create table [Reference].[AEC_LatestInYearEarningHistory]
	(
		[AppIdentifier] varchar(50) not null,
		[AppProgCompletedInTheYearInput] bit,
		[CollectionReturnCode] varchar(3) not null,
		[CollectionYear] varchar(4) not null,
		[DaysInYear] int,
		[FworkCode] int,
		[HistoricEffectiveTNPStartDateInput] date,
		[HistoricLearner1618StartInput] bit,
		[HistoricTNP1Input] decimal(10,5),
		[HistoricTNP2Input] decimal(10,5),
		[HistoricTNP3Input] decimal(10,5),
		[HistoricTNP4Input] decimal(10,5),
		[HistoricTotal1618UpliftPaymentsInTheYearInput] decimal(10,5),
		[HistoricVirtualTNP3EndOfTheYearInput] decimal(10,5),
		[HistoricVirtualTNP4EndOfTheYearInput] decimal(10,5),
		[LearnRefNumber] varchar(12) not null,
		[ProgrammeStartDateIgnorePathway] date,
		[ProgrammeStartDateMatchPathway] date,
		[ProgType] int,
		[PwayCode] int,
		[STDCode] int,
		[TotalProgAimPaymentsInTheYear] decimal(10,5),
		[UKPRN] int not null,
		[ULN] bigint not null,
		[UptoEndDate] date
	)

if object_id('[Reference].[LARS_ApprenticeshipFunding]','u') is not null
	drop table [Reference].[LARS_ApprenticeshipFunding]
GO
create table [Reference].[LARS_ApprenticeshipFunding]
	(
		[1618Incentive] decimal(10,5),
		[ApprenticeshipCode] int not null,
		[ApprenticeshipType] varchar(50) not null,
		[EffectiveFrom] date not null,
		[EffectiveTo] date,
		[FundingCategory] varchar(15) not null,
		[MaxEmployerLevyCap] decimal(10,5),
		[ProgType] int not null,
		[PwayCode] int not null,
		[ReservedValue1] decimal(10,5),
		[ReservedValue2] decimal(10,5)
	)

create clustered index IX_LARS_ApprenticeshipFunding on [Reference].[LARS_ApprenticeshipFunding]
	(
		[ApprenticeshipCode],
		[ApprenticeshipType],
		[EffectiveFrom],
		[FundingCategory],
		[ProgType],
		[PwayCode]
	)
if object_id('[Reference].[LARS_Current_Version]','u') is not null
	drop table [Reference].[LARS_Current_Version]
GO
create table [Reference].[LARS_Current_Version]
	(
		[CurrentVersion] varchar(100)
	)

if object_id('[Reference].[LARS_FrameworkCmnComp]','u') is not null
	drop table [Reference].[LARS_FrameworkCmnComp]
GO
create table [Reference].[LARS_FrameworkCmnComp]
	(
		[CommonComponent] int not null,
		[EffectiveFrom] date not null,
		[EffectiveTo] date,
		[FworkCode] int not null,
		[ProgType] int not null,
		[PwayCode] int not null
	)

create clustered index IX_LARS_FrameworkCmnComp on [Reference].[LARS_FrameworkCmnComp]
	(
		[CommonComponent],
		[FworkCode],
		[ProgType],
		[PwayCode]
	)
if object_id('[Reference].[LARS_Funding]','u') is not null
	drop table [Reference].[LARS_Funding]
GO
create table [Reference].[LARS_Funding]
	(
		[EffectiveFrom] date not null,
		[EffectiveTo] date,
		[FundingCategory] varchar(15) not null,
		[RateWeighted] decimal(10,5)
	)

if object_id('[Reference].[LARS_LearningDelivery]','u') is not null
	drop table [Reference].[LARS_LearningDelivery]
GO
create table [Reference].[LARS_LearningDelivery]
	(
		[FrameworkCommonComponent] int,
		[LearnAimRef] varchar(8) not null
	)

create clustered index IX_LARS_LearningDelivery on [Reference].[LARS_LearningDelivery]
	(
		[LearnAimRef]
	)
if object_id('[Reference].[LARS_StandardCommonComponent]','u') is not null
	drop table [Reference].[LARS_StandardCommonComponent]
GO
create table [Reference].[LARS_StandardCommonComponent]
	(
		[CommonComponent] int not null,
		[EffectiveFrom] date not null,
		[EffectiveTo] date,
		[StandardCode] int not null
	)

create clustered index IX_LARS_StandardCommonComponent on [Reference].[LARS_StandardCommonComponent]
	(
		[StandardCode]
	)
if object_id('[Reference].[SFA_PostcodeDisadvantage]','u') is not null
	drop table [Reference].[SFA_PostcodeDisadvantage]
GO
create table [Reference].[SFA_PostcodeDisadvantage]
	(
		[EffectiveFrom] date not null,
		[EffectiveTo] date,
		[Postcode] varchar(10) not null,
		[Uplift] decimal(10,5) not null
	)

create clustered index IX_SFA_PostcodeDisadvantage on [Reference].[SFA_PostcodeDisadvantage]
	(
		[EffectiveFrom],
		[Postcode]
	)
