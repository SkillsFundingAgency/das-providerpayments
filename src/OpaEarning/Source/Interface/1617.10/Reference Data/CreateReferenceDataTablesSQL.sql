-- =====================================================================================================
-- Generated by Data Dictionary
-- Date: 13 January 2017 10:07
-- Rulebase Versions:
--          ILR Apprenticeship Earnings Calc 1617, Version 1617.10
-- =====================================================================================================
if not exists(select schema_id from sys.schemas where name='Reference')
	exec('create schema [Reference]')
go

if object_id('[Reference].[AEC_LatestInYearEarningHistory]','u') is not null
	drop table [Reference].[AEC_LatestInYearEarningHistory]
go
create table [Reference].[AEC_LatestInYearEarningHistory]
	(
		[AppIdentifier] varchar(50) not null,
		[CollectionReturnCode] varchar(3) not null,
		[CollectionYear] varchar(4) not null,
		[DaysInYear] int,
		[FworkCode] int,
		[LearnRefNumber] varchar(12),
		[ProgType] int,
		[PwayCode] int,
		[STDCode] int,
		[UKPRN] int not null,
		[ULN] int not null,
		[UptoEndDate] date
	)

create clustered index IX_AEC_LatestInYearEarningHistory on [Reference].[AEC_LatestInYearEarningHistory]
	(
		[CollectionYear],
		[ULN]
	)
if object_id('[Reference].[LARS_ApprenticeshipFunding]','u') is not null
	drop table [Reference].[LARS_ApprenticeshipFunding]
go
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
go
create table [Reference].[LARS_Current_Version]
	(
		[CurrentVersion] varchar(100)
	)

if object_id('[Reference].[LARS_FrameworkCmnComp]','u') is not null
	drop table [Reference].[LARS_FrameworkCmnComp]
go
create table [Reference].[LARS_FrameworkCmnComp]
	(
		[CommonComponent] int not null,
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
if object_id('[Reference].[LARS_LearningDelivery]','u') is not null
	drop table [Reference].[LARS_LearningDelivery]
go
create table [Reference].[LARS_LearningDelivery]
	(
		[EffectiveFrom] date not null,
		[EffectiveTo] date,
		[FrameworkCommonComponent] int,
		[LearnAimRef] varchar(8) not null
	)

create clustered index IX_LARS_LearningDelivery on [Reference].[LARS_LearningDelivery]
	(
		[LearnAimRef]
	)
if object_id('[Reference].[LARS_StandardCommonComponent]','u') is not null
	drop table [Reference].[LARS_StandardCommonComponent]
go
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
go
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
