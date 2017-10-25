if schema_id('Version_001') is null 
	exec sp_executesql N'create schema [Version_001]'
GO
if object_id('[dbo].[AEC_EarningHistory]','u') is not null
	drop table [dbo].[AEC_EarningHistory]
create table [dbo].[AEC_EarningHistory]
	(
		[AppIdentifier] varchar(50) not null,
		[AppProgCompletedInTheYearInput] bit null,
		[BalancingProgAimPaymentsInTheYear] decimal(10,5),
		[CollectionYear] varchar(4) not null,
		[CollectionReturnCode] varchar(3) not null,
		[CompletionProgAimPaymentsInTheYear] decimal(10,5),
		[DaysInYear] int null,
		[FworkCode] int null,
		[HistoricEffectiveTNPStartDateInput] date null,
		[HistoricLearner1618StartInput] bit null,
		[HistoricTotal1618UpliftPaymentsInTheYearInput] decimal(10,5) null,
		[HistoricTNP1Input] decimal(10,5) null,
		[HistoricTNP2Input] decimal(10,5) null,
		[HistoricTNP3Input] decimal(10,5) null,
		[HistoricTNP4Input] decimal(10,5) null,
		[HistoricVirtualTNP3EndOfTheYearInput] decimal(10,5) null,
		[HistoricVirtualTNP4EndOfTheYearInput] decimal(10,5) null,
		[LatestInYear] bit not null,
		[LearnRefNumber] varchar(12) not null,
		[OnProgProgAimPaymentsInTheYear] decimal(10,5),
		[ProgrammeStartDateIgnorePathway] date null,
		[ProgrammeStartDateMatchPathway] date null,
		[ProgType] int null,
		[PwayCode] int null,
		[STDCode] int null,
		[TotalProgAimPaymentsInTheYear] decimal(10,5),
		[UptoEndDate] date null,
		[UKPRN] int not null,
		[ULN] bigint not null,
		primary key clustered
		(
			[LatestInYear] desc,
			[LearnRefNumber] asc,
			[UKPRN] asc,
			[CollectionYear] asc,
			[CollectionReturnCode] asc,
			[AppIdentifier] asc
		)

	)
GO
create index [IDX_AEC_EarningHistory] on [dbo].[AEC_EarningHistory]
	(
		[LatestInYear],
		[CollectionYear],
		[UKPRN]
	)
GO
if object_id('[Version_001].[AEC_EarningHistory]','v') is not null
	drop view [Version_001].[AEC_EarningHistory]
GO
create view [Version_001].[AEC_EarningHistory] as

	select 
		[AEC_EarningHistory].[AppIdentifier],
		[AEC_EarningHistory].[AppProgCompletedInTheYearInput],
		[AEC_EarningHistory].[BalancingProgAimPaymentsInTheYear],
		[AEC_EarningHistory].[CollectionYear],
		[AEC_EarningHistory].[CollectionReturnCode],
		[AEC_EarningHistory].[CompletionProgAimPaymentsInTheYear],
		[AEC_EarningHistory].[DaysInYear],
		[AEC_EarningHistory].[FworkCode],
		[AEC_EarningHistory].[HistoricEffectiveTNPStartDateInput],
		[AEC_EarningHistory].[HistoricLearner1618StartInput],
		[AEC_EarningHistory].[HistoricTNP1Input],
		[AEC_EarningHistory].[HistoricTNP2Input],
		[AEC_EarningHistory].[HistoricTNP3Input],
		[AEC_EarningHistory].[HistoricTNP4Input],
		[AEC_EarningHistory].[HistoricTotal1618UpliftPaymentsInTheYearInput],
		[AEC_EarningHistory].[HistoricVirtualTNP3EndOfTheYearInput],
		[AEC_EarningHistory].[HistoricVirtualTNP4EndOfTheYearInput],
		[AEC_EarningHistory].[LatestInYear],
		[AEC_EarningHistory].[LearnRefNumber],
		[AEC_EarningHistory].[OnProgProgAimPaymentsInTheYear],
		[AEC_EarningHistory].[ProgrammeStartDateIgnorePathway],
		[AEC_EarningHistory].[ProgrammeStartDateMatchPathway],
		[AEC_EarningHistory].[ProgType],
		[AEC_EarningHistory].[PwayCode],
		[AEC_EarningHistory].[STDCode],
		[AEC_EarningHistory].[TotalProgAimPaymentsInTheYear],
		[AEC_EarningHistory].[UptoEndDate],
		[AEC_EarningHistory].[UKPRN],
		[AEC_EarningHistory].[ULN]
	from
		[dbo].[AEC_EarningHistory]
GO
if object_id('[Version_001].[AEC_LatestInYearEarningHistory]','v') is not null
	drop view [Version_001].[AEC_LatestInYearEarningHistory]
GO
create view [Version_001].[AEC_LatestInYearEarningHistory] as

	select 
		[AEC_EarningHistory].[AppIdentifier],
		[AEC_EarningHistory].[AppProgCompletedInTheYearInput],
		[AEC_EarningHistory].[BalancingProgAimPaymentsInTheYear],
		[AEC_EarningHistory].[CollectionYear],
		[AEC_EarningHistory].[CollectionReturnCode],
		[AEC_EarningHistory].[CompletionProgAimPaymentsInTheYear],
		[AEC_EarningHistory].[DaysInYear],
		[AEC_EarningHistory].[FworkCode],
		[AEC_EarningHistory].[HistoricEffectiveTNPStartDateInput],
		[AEC_EarningHistory].[HistoricLearner1618StartInput],
		[AEC_EarningHistory].[HistoricTNP1Input],
		[AEC_EarningHistory].[HistoricTNP2Input],
		[AEC_EarningHistory].[HistoricTNP3Input],
		[AEC_EarningHistory].[HistoricTNP4Input],
		[AEC_EarningHistory].[HistoricTotal1618UpliftPaymentsInTheYearInput],
		[AEC_EarningHistory].[HistoricVirtualTNP3EndOfTheYearInput],
		[AEC_EarningHistory].[HistoricVirtualTNP4EndOfTheYearInput],
		[AEC_EarningHistory].[LatestInYear],
		[AEC_EarningHistory].[LearnRefNumber],
		[AEC_EarningHistory].[OnProgProgAimPaymentsInTheYear],
		[AEC_EarningHistory].[ProgrammeStartDateIgnorePathway],
		[AEC_EarningHistory].[ProgrammeStartDateMatchPathway],
		[AEC_EarningHistory].[ProgType],
		[AEC_EarningHistory].[PwayCode],
		[AEC_EarningHistory].[STDCode],
		[AEC_EarningHistory].[TotalProgAimPaymentsInTheYear],
		[AEC_EarningHistory].[UptoEndDate],
		[AEC_EarningHistory].[UKPRN],
		[AEC_EarningHistory].[ULN]
	from
		[dbo].[AEC_EarningHistory]
	where
		[AEC_EarningHistory].[LatestInYear]=1
GO
if object_id('[dbo].[PrepareForNewData]','p') is not null
	drop procedure [dbo].[PrepareForNewData]
GO
create procedure [dbo].[PrepareForNewData]
	(
		@pCollectionYear int,
		@pCollectionReturnCode varchar(3),
		@pUKPRN bigint
	)
as
	begin

		delete from
			[dbo].[AEC_EarningHistory]
		where
			[CollectionYear]=@pCollectionYear
			and [CollectionReturnCode]=@pCollectionReturnCode
			and [UKPRN]=@pUKPRN

		update
			[dbo].[AEC_EarningHistory]
		set
			[LatestInYear]=0
		where
			[LatestInYear]=1
			and [CollectionYear]=@pCollectionYear
			and [UKPRN]=@pUKPRN

	end
GO