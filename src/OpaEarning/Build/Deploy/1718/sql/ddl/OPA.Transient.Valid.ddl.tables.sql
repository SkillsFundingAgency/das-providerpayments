if not exists(select schema_id from sys.schemas where name='Valid')
	exec('create schema Valid')
GO
 
if object_id('[Valid].[CollectionDetails]','u') is not null
begin
	drop table [Valid].[CollectionDetails]
end
GO
 
create table [Valid].[CollectionDetails]
(
	[Collection] varchar(3) not null,
	[Year] varchar(4) not null,
	[FilePreparationDate] date null
)
GO

if object_id('[Valid].[Source]','u') is not null
begin
	drop table [Valid].[Source]
end
GO
 
create table [Valid].[Source]
(
	[ProtectiveMarking] varchar(30) null,
	[UKPRN] int not null,
	[SoftwareSupplier] varchar(40) null,
	[SoftwarePackage] varchar(30) null,
	[Release] varchar(20) null,
	[SerialNo] varchar(2) null,
	[DateTime] datetime null,
	[ReferenceData] varchar(100) null,
	[ComponentSetVersion] varchar(20) null
)
GO

if object_id('[Valid].[SourceFile]','u') is not null
begin
	drop table [Valid].[SourceFile]
end
GO
 
create table [Valid].[SourceFile]
(	
	[SourceFileName] varchar(50) not null,
	[FilePreparationDate] date null,
	[SoftwareSupplier] varchar(40) null,
	[SoftwarePackage] varchar(30) null,
	[Release] varchar(20) null,
	[SerialNo] varchar(2) null,
	[DateTime] datetime null
)
create clustered index [IX_Valid_SourceFile] on [Valid].[SourceFile]
(
	[SourceFileName] asc
)
GO

if object_id('[Valid].[LearningProvider]','u') is not null
begin
	drop table [Valid].[LearningProvider]
end
GO
 
create table [Valid].[LearningProvider]
(		
	[UKPRN] int not null
	,primary key clustered
	(
		[UKPRN] asc
	)
)
GO

if object_id('[Valid].[Learner]','u') is not null
begin
	drop table [Valid].[Learner]
end
GO
 
create table [Valid].[Learner]
(
	[LearnRefNumber] varchar(12) not null,
	[PrevLearnRefNumber] varchar(12) null,
	[PrevUKPRN] int null,
	[PMUKPRN] int null,
	[ULN] bigint not null,
	[FamilyName] varchar(100) null,
	[GivenNames] varchar(100) null,
	[DateOfBirth] date null,
	[Ethnicity] int not null,
	[Sex] varchar(1) not null,
	[LLDDHealthProb] int not null,
	[NINumber] varchar(9) null,
	[PriorAttain] int null,
	[Accom] int null,
	[ALSCost] int null,
	[PlanLearnHours] int null,
	[PlanEEPHours] int null,
	[MathGrade] varchar(4) null,
	[EngGrade] varchar(4) null,
	[PostcodePrior] varchar(8) null,
	[Postcode] varchar(8) null,
	[AddLine1] varchar(50) null,
	[AddLine2] varchar(50) null,
	[AddLine3] varchar(50) null,
	[AddLine4] varchar(50) null,
	[TelNo] varchar(18) null,
	[Email] varchar(100) null
	,primary key clustered
	(
		[LearnRefNumber] asc
	)
)
GO

if object_id('[Valid].[ContactPreference]','u') is not null
begin
	drop table [Valid].[ContactPreference]
end
GO
 
create table [Valid].[ContactPreference]
(
	[LearnRefNumber] varchar(12) not null,
	[ContPrefType] varchar(3) not null,
	[ContPrefCode] int not null,
	primary key
	(
		LearnRefNumber
		,ContPrefType
		,ContPrefCode
	)
)
GO

if object_id('[Valid].[LLDDandHealthProblem]','u') is not null
begin
	drop table [Valid].[LLDDandHealthProblem]
end
GO
 
create table [Valid].[LLDDandHealthProblem]
(
	[LearnRefNumber] varchar(12) not null,
	[LLDDCat] int not null,
	[PrimaryLLDD] int null
	,primary key clustered
	(
		[LearnRefNumber] asc,
		[LLDDCat] asc
	)
)
GO

if object_id('[Valid].[LearnerFAM]','u') is not null
begin
	drop table [Valid].[LearnerFAM]
end
GO
 
create table [Valid].[LearnerFAM]
(
	[LearnRefNumber] varchar(12) not null,
	[LearnFAMType] varchar(3) null,
	[LearnFAMCode] int not null
)
create clustered index [IX_Valid_LearnerFAM] on [Valid].[LearnerFAM]
(
	[LearnRefNumber] asc
)
GO

if object_id('[Valid].[ProviderSpecLearnerMonitoring]','u') is not null
begin
	drop table [Valid].[ProviderSpecLearnerMonitoring]
end
GO
 
create table [Valid].[ProviderSpecLearnerMonitoring]
(
	[LearnRefNumber] varchar(12) not null,
	[ProvSpecLearnMonOccur] varchar(1) not null,
	[ProvSpecLearnMon] varchar(20) not null
	,primary key clustered
	(
		[LearnRefNumber] asc,
		[ProvSpecLearnMonOccur] asc
	)
)
GO

if object_id('[Valid].[LearnerEmploymentStatus]','u') is not null
begin
	drop table [Valid].[LearnerEmploymentStatus]
end
GO
 
create table [Valid].[LearnerEmploymentStatus]
(
	[LearnRefNumber] varchar(12) not null,
	[EmpStat] int null,
	[DateEmpStatApp] date not null,
	[EmpId] int null
	,primary key clustered
	(
		[LearnRefNumber] asc,
		[DateEmpStatApp] asc
	)
)
GO

if object_id('[Valid].[LearnerEmploymentStatusDenormTbl]','u') is not null
begin
	drop table [Valid].[LearnerEmploymentStatusDenormTbl]
end
GO
 
CREATE TABLE [Valid].[LearnerEmploymentStatusDenormTbl]
(
	 [LearnRefNumber] [varchar](12) NOT NULL
	,[EmpStat] [int] NULL
	,[EmpId] [int] NULL
	,[DateEmpStatApp] [date] NOT NULL
	,[ESMCode_BSI] [int] NULL
	,[ESMCode_EII] [int] NULL
	,[ESMCode_LOE] [int] NULL
	,[ESMCode_LOU] [int] NULL
	,[ESMCode_PEI] [int] NULL
	,[ESMCode_SEI] [int] NULL
	,[ESMCode_SEM] [int] NULL
	,PRIMARY KEY CLUSTERED
	(
		 [LearnRefNumber] asc
		,[DateEmpStatApp] asc
	)
)
GO



if object_id('[Valid].[EmploymentStatusMonitoring]','u') is not null
begin
	drop table [Valid].[EmploymentStatusMonitoring]
end
GO
 
create table [Valid].[EmploymentStatusMonitoring]
(	
	[LearnRefNumber] varchar(12) not null,
	[DateEmpStatApp] date not null,
	[ESMType] varchar(3) not null,
	[ESMCode] int null
	,primary key clustered
	(
		[LearnRefNumber] asc,
		[DateEmpStatApp] asc,
		[ESMType] asc
	)
)
GO

if object_id('[Valid].[LearnerHE]','u') is not null
begin
	drop table [Valid].[LearnerHE]
end
GO
 
create table [Valid].[LearnerHE]
(
	[LearnRefNumber] varchar(12) not null,
	[UCASPERID] varchar(10) null,
	[TTACCOM] int null
	,primary key clustered
	(
		[LearnRefNumber] asc
	)
)
GO

if object_id('[Valid].[LearnerHEFinancialSupport]','u') is not null
begin
	drop table [Valid].[LearnerHEFinancialSupport]
end
GO
 
create table [Valid].[LearnerHEFinancialSupport]
(
	[LearnRefNumber] varchar(12) not null,
	[FINTYPE] int not null,
	[FINAMOUNT] int not null
	,primary key clustered
	(
		[LearnRefNumber] asc,
		[FINTYPE] asc
	)
)
GO

if object_id('[Valid].[LearningDelivery]','u') is not null
begin
	drop table [Valid].[LearningDelivery]
end
GO
 
create table [Valid].[LearningDelivery]
(
	[LearnRefNumber] varchar(12) not null,
	[LearnAimRef] varchar(8) not null,
	[AimType] int not null,
	[AimSeqNumber] int not null,
	[LearnStartDate] date not null,
	[OrigLearnStartDate] date null,
	[LearnPlanEndDate] date not null,
	[FundModel] int not null,
	[ProgType] int null,
	[FworkCode] int null,
	[PwayCode] int null,
	[StdCode] int null,
	[PartnerUKPRN] int null,
	[DelLocPostCode] varchar(8) null,
	[AddHours] int null,
	[PriorLearnFundAdj] int null,
	[OtherFundAdj] int null,
	[ConRefNumber] varchar(20) null,
	[EPAOrgID] varchar(7) null,
	[EmpOutcome] int null,
	[CompStatus] int not null,
	[LearnActEndDate] date null,
	[WithdrawReason] int null,
	[Outcome] int null,
	[AchDate] date null,
	[OutGrade] varchar(6) null,
	[SWSupAimId] varchar(36) null
	,primary key clustered
	(
		[LearnRefNumber] asc,
		[AimSeqNumber] asc
	)
)
GO

CREATE INDEX IDX_FundModel ON Valid.LearningDelivery (FundModel)


IF OBJECT_ID('[Valid].[LearningDeliveryDenormTbl]','U') is not null
BEGIN
	DROP TABLE [Valid].[LearningDeliveryDenormTbl]
END
GO

CREATE TABLE [Valid].[LearningDeliveryDenormTbl]
(
	 [LearnRefNumber]		[varchar](12)	NOT NULL
	,[LearnAimRef]			[varchar](8)	NOT NULL
	,[AimType]				[int]			NOT NULL
	,[AimSeqNumber]			[int]			NOT NULL
	,[LearnStartDate]		[date]			NOT NULL
	,[OrigLearnStartDate]	[date]			NULL
	,[LearnPlanEndDate]		[date]			NOT NULL
	,[FundModel]			[int]			NOT NULL
	,[ProgType]				[int]			NULL
	,[FworkCode]			[int]			NULL
	,[PwayCode]				[int]			NULL
	,[StdCode]				[int]			NULL
	,[PartnerUKPRN]			[int]			NULL
	,[DelLocPostCode]		[varchar](8)	NULL
	,[AddHours]				[int]			NULL
	,[PriorLearnFundAdj]	[int]			NULL
	,[OtherFundAdj]			[int]			NULL
	,[ConRefNumber]			[varchar](20)	NULL
	,[EPAOrgID]				[varchar](7)	NULL
	,[EmpOutcome]			[int]			NULL
	,[CompStatus]			[int]			NOT NULL
	,[LearnActEndDate]		[date]			NULL
	,[WithdrawReason]		[int]			NULL
	,[Outcome]				[int]			NULL
	,[AchDate]				[date]			NULL
	,[OutGrade]				[varchar](6)	NULL
	,[SWSupAimId]			[varchar](36)	NULL
	,[HEM1]					[varchar](5)	NULL
	,[HEM2]					[varchar](5)	NULL
	,[HEM3]					[varchar](5)	NULL
	,[HHS1]					[varchar](5)	NULL
	,[HHS2]					[varchar](5)	NULL
	,[LDFAM_SOF]			[varchar](5)	NULL
	,[LDFAM_EEF]			[varchar](5)	NULL
	,[LDFAM_RES]			[varchar](5)	NULL
	,[LDFAM_ADL]			[varchar](5)	NULL
	,[LDFAM_FFI]			[varchar](5)	NULL
	,[LDFAM_WPP]			[varchar](5)	NULL
	,[LDFAM_POD]			[varchar](5)	NULL
	,[LDFAM_ASL]			[varchar](5)	NULL
	,[LDFAM_FLN]			[varchar](5)	NULL
	,[LDFAM_NSA]			[varchar](5)	NULL
	,[ProvSpecDelMon_A]		[varchar](20)	NULL
	,[ProvSpecDelMon_B]		[varchar](20)	NULL
	,[ProvSpecDelMon_C]		[varchar](20)	NULL
	,[ProvSpecDelMon_D]		[varchar](20)	NULL
	,[LDM1]					[varchar](5)	NULL
	,[LDM2]					[varchar](5)	NULL
	,[LDM3]					[varchar](5)	NULL
	,[LDM4]					[varchar](5)	NULL
	,primary key clustered
	(
		[LearnRefNumber] asc,
		[AimSeqNumber] asc
	)
) ON [PRIMARY]

if object_id('[Valid].[LearningDeliveryFAM]','u') is not null
begin
	drop table [Valid].[LearningDeliveryFAM]
end
GO
 
create table [Valid].[LearningDeliveryFAM]
(
	[LearnRefNumber] varchar(12) not null,
	[AimSeqNumber] int not null,
	[LearnDelFAMType] varchar(3) not null,
	[LearnDelFAMCode] varchar(5) not null,
	[LearnDelFAMDateFrom] date null,
	[LearnDelFAMDateTo] date null
)
create clustered index [IX_Valid_LearningDeliveryFAM] on [Valid].[LearningDeliveryFAM]
(
	[LearnRefNumber] asc,
	[AimSeqNumber] asc,
	[LearnDelFAMType] asc,
	[LearnDelFAMDateFrom] asc
)
GO

if object_id('[Valid].[LearningDeliveryWorkPlacement]','u') is not null
begin
	drop table [Valid].[LearningDeliveryWorkPlacement]
end
GO
 
create table [Valid].[LearningDeliveryWorkPlacement]
(
	[LearnRefNumber] varchar(12) not null,
	[AimSeqNumber] int not null,
	[WorkPlaceStartDate] date not null,
	[WorkPlaceEndDate] date null,
	[WorkPlaceHours] int null,
	[WorkPlaceMode] int not null,
	[WorkPlaceEmpId] int null
)
create clustered index [IX_Valid_LearningDeliveryWorkPlacement] on [Valid].[LearningDeliveryWorkPlacement]
(
	[LearnRefNumber] asc,
	[AimSeqNumber] asc,
	[WorkPlaceStartDate] asc,
	[WorkPlaceMode] asc,
	[WorkPlaceEmpId] asc
)
GO

if object_id('[Valid].[AppFinRecord]','u') is not null
begin
	drop table [Valid].[AppFinRecord]
end
GO
 
create table [Valid].[AppFinRecord]
(
	[LearnRefNumber] varchar(12) not null,
	[AimSeqNumber] int not null,
	[AFinType] varchar(3) not null,
	[AFinCode] int not null,
	[AFinDate] date not null,
	[AFinAmount] int not null
)
create clustered index [IX_Valid_AppFinRecord] on [Valid].[AppFinRecord]
(
	[LearnRefNumber] asc,
	[AimSeqNumber] asc,
	[AFinType] asc
)
GO

if object_id('[Valid].[ProviderSpecDeliveryMonitoring]','u') is not null
begin
	drop table [Valid].[ProviderSpecDeliveryMonitoring]
end
GO
 
create table [Valid].[ProviderSpecDeliveryMonitoring]
(
	[LearnRefNumber] varchar(12) not null,
	[AimSeqNumber] int not null,
	[ProvSpecDelMonOccur] varchar(1) not null,
	[ProvSpecDelMon] varchar(20) not null
	,primary key clustered
	(
		[LearnRefNumber] asc,
		[AimSeqNumber] asc,
		[ProvSpecDelMonOccur] asc
	)
)
GO

if object_id('[Valid].[LearningDeliveryHE]','u') is not null
begin
	drop table [Valid].[LearningDeliveryHE]
end
GO
 
create table [Valid].[LearningDeliveryHE]
(
	[LearnRefNumber] varchar(12) not null,
	[AimSeqNumber] int not null,
	[NUMHUS] varchar(20) null,
	[SSN] varchar(13) null,
	[QUALENT3] varchar(3) null,
	[SOC2000] int null,
	[SEC] int null,
	[UCASAPPID] varchar(9) null,
	[TYPEYR] int not null,
	[MODESTUD] int not null,
	[FUNDLEV] int not null,
	[FUNDCOMP] int not null,
	[STULOAD] decimal(4,1) null,
	[YEARSTU] int not null,
	[MSTUFEE] int not null,
	[PCOLAB] decimal(4,1) null,
	[PCFLDCS] decimal(4,1) null,
	[PCSLDCS] decimal(4,1) null,
	[PCTLDCS] decimal(4,1) null,
	[SPECFEE] int not null,
	[NETFEE] int null,
	[GROSSFEE] int null,
	[DOMICILE] varchar(2) null,
	[ELQ] int null,
	[HEPostCode] varchar(8) null
	,primary key clustered
	(
		[LearnRefNumber] asc,
		[AimSeqNumber] asc
	)
)
GO

if object_id('[Valid].[LearnerDestinationandProgression]','u') is not null
begin
	drop table [Valid].[LearnerDestinationandProgression]
end
GO
 
create table [Valid].[LearnerDestinationandProgression]
(
	[LearnRefNumber] varchar(12) not null,
	[ULN] bigint not null
	,primary key clustered
	(
		[LearnRefNumber] asc
	)
)
GO

if object_id('[Valid].[DPOutcome]','u') is not null
begin
	drop table [Valid].[DPOutcome]
end
GO
 
create table [Valid].[DPOutcome]
(
	[LearnRefNumber] varchar(12) not null,
	[OutType] varchar(3) not null,
	[OutCode] int not null,
	[OutStartDate] date not null,
	[OutEndDate] date null,
	[OutCollDate] date not null
)
create clustered index [IX_Valid_DPOutcome] on [Valid].[DPOutcome]
(
	[LearnRefNumber] asc,
	[OutType] asc,
	[OutCode] asc,
	[OutStartDate] asc
)
GO

