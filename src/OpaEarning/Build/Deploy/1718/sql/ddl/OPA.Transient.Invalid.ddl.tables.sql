if not exists(select schema_id from sys.schemas where name='Invalid')
	exec('create schema Invalid')
GO
 
if object_id('[Invalid].[CollectionDetails]','u') is not null
	drop table [Invalid].[CollectionDetails]
GO
 
create table [Invalid].[CollectionDetails]
(		[CollectionDetails_Id] int  primary key,
		[Collection] varchar(3) not null,
		[Year] varchar(4) not null,
		[FilePreparationDate] date not null
	)

if object_id('[Invalid].[Source]','u') is not null
	drop table [Invalid].[Source]
 
create table [Invalid].[Source]
(		[Source_Id] int  primary key,
		[ProtectiveMarking] varchar(30) not null,
		[UKPRN] int not null,
		[SoftwareSupplier] varchar(40),
		[SoftwarePackage] varchar(30),
		[Release] varchar(20),
		[SerialNo] varchar(2) not null,
		[DateTime] datetime not null,
		[ReferenceData] varchar(100),
		[ComponentSetVersion] varchar(20)
	)

if object_id('[Invalid].[SourceFile]','u') is not null
	drop table [Invalid].[SourceFile]
 
create table [Invalid].[SourceFile]
(		[SourceFile_Id] int  primary key,
		[SourceFileName] varchar(50) not null,
		[FilePreparationDate] date not null,
		[SoftwareSupplier] varchar(40),
		[SoftwarePackage] varchar(30),
		[Release] varchar(20),
		[SerialNo] varchar(2) not null,
		[DateTime] datetime
	)
create index [IX_Invalid_SourceFile] on [Invalid].[SourceFile]
	(
		[SourceFileName] asc
)

if object_id('[Invalid].[LearningProvider]','u') is not null
	drop table [Invalid].[LearningProvider]
 
create table [Invalid].[LearningProvider]
(		[LearningProvider_Id] int  primary key,
		[UKPRN] int not null
	)
create index [IX_Invalid_LearningProvider] on [Invalid].[LearningProvider]
	(
		[UKPRN] asc
)

if object_id('[Invalid].[Learner]','u') is not null
	drop table [Invalid].[Learner]
 
create table [Invalid].[Learner]
(		[Learner_Id] int  primary key,
		[LearnRefNumber] varchar(12),
		[PrevLearnRefNumber] varchar(1000),
		[PrevUKPRN] bigint,
		[PMUKPRN] bigint,
		[ULN] bigint,
		[FamilyName] varchar(1000),
		[GivenNames] varchar(1000),
		[DateOfBirth] date,
		[Ethnicity] bigint,
		[Sex] varchar(1000),
		[LLDDHealthProb] bigint,
		[NINumber] varchar(1000),
		[PriorAttain] bigint,
		[Accom] bigint,
		[ALSCost] bigint,
		[PlanLearnHours] bigint,
		[PlanEEPHours] bigint,
		[MathGrade] varchar(1000),
		[EngGrade] varchar(1000),
		[PostcodePrior] varchar(1000),
		[Postcode] varchar(1000),
		[AddLine1] varchar(1000),
		[AddLine2] varchar(1000),
		[AddLine3] varchar(1000),
		[AddLine4] varchar(1000),
		[TelNo] varchar(1000),
		[Email] varchar(1000)
	)
create index [IX_Invalid_Learner] on [Invalid].[Learner]
	(
		[LearnRefNumber] asc
)

if object_id('[Invalid].[ContactPreference]','u') is not null
	drop table [Invalid].[ContactPreference]
 
create table [Invalid].[ContactPreference]
(		[ContactPreference_Id] int  primary key,
		[Learner_Id] int  not null,
		[LearnRefNumber] varchar(12),
		[ContPrefType] varchar(100),
		[ContPrefCode] bigint
	)
create index [IX_Parent_Invalid_ContactPreference] on [Invalid].[ContactPreference]
	(
		[Learner_Id] asc
	)
create index [IX_Invalid_ContactPreference] on [Invalid].[ContactPreference]
	(
		[LearnRefNumber] asc,
		[ContPrefType] asc
)

if object_id('[Invalid].[LLDDandHealthProblem]','u') is not null
	drop table [Invalid].[LLDDandHealthProblem]
 
create table [Invalid].[LLDDandHealthProblem]
(		[LLDDandHealthProblem_Id] int  primary key,
		[Learner_Id] int  not null,
		[LearnRefNumber] varchar(12),
		[LLDDCat] bigint,
		[PrimaryLLDD] bigint
	)
create index [IX_Parent_Invalid_LLDDandHealthProblem] on [Invalid].[LLDDandHealthProblem]
	(
		[Learner_Id] asc
	)
create index [IX_Invalid_LLDDandHealthProblem] on [Invalid].[LLDDandHealthProblem]
	(
		[LearnRefNumber] asc,
		[LLDDCat] asc
)

if object_id('[Invalid].[LearnerFAM]','u') is not null
	drop table [Invalid].[LearnerFAM]
 
create table [Invalid].[LearnerFAM]
(		[LearnerFAM_Id] int  primary key,
		[Learner_Id] int  not null,
		[LearnRefNumber] varchar(12),
		[LearnFAMType] varchar(1000),
		[LearnFAMCode] bigint
	)
create index [IX_Parent_Invalid_LearnerFAM] on [Invalid].[LearnerFAM]
	(
		[Learner_Id] asc
	)
create index [IX_Invalid_LearnerFAM] on [Invalid].[LearnerFAM]
	(
		[LearnRefNumber] asc
)

if object_id('[Invalid].[ProviderSpecLearnerMonitoring]','u') is not null
	drop table [Invalid].[ProviderSpecLearnerMonitoring]
 
create table [Invalid].[ProviderSpecLearnerMonitoring]
(		[ProviderSpecLearnerMonitoring_Id] int  primary key,
		[Learner_Id] int  not null,
		[LearnRefNumber] varchar(12),
		[ProvSpecLearnMonOccur] varchar(100),
		[ProvSpecLearnMon] varchar(1000)
	)
create index [IX_Parent_Invalid_ProviderSpecLearnerMonitoring] on [Invalid].[ProviderSpecLearnerMonitoring]
	(
		[Learner_Id] asc
	)
create index [IX_Invalid_ProviderSpecLearnerMonitoring] on [Invalid].[ProviderSpecLearnerMonitoring]
	(
		[LearnRefNumber] asc,
		[ProvSpecLearnMonOccur] asc
)

if object_id('[Invalid].[LearnerEmploymentStatus]','u') is not null
	drop table [Invalid].[LearnerEmploymentStatus]
 
create table [Invalid].[LearnerEmploymentStatus]
(		[LearnerEmploymentStatus_Id] int  primary key,
		[Learner_Id] int  not null,
		[LearnRefNumber] varchar(12),
		[EmpStat] bigint,
		[DateEmpStatApp] date,
		[EmpId] bigint
	)
create index [IX_Parent_Invalid_LearnerEmploymentStatus] on [Invalid].[LearnerEmploymentStatus]
	(
		[Learner_Id] asc
	)
create index [IX_Invalid_LearnerEmploymentStatus] on [Invalid].[LearnerEmploymentStatus]
	(
		[LearnRefNumber] asc,
		[DateEmpStatApp] asc
)

if object_id('[Invalid].[EmploymentStatusMonitoring]','u') is not null
	drop table [Invalid].[EmploymentStatusMonitoring]
 
create table [Invalid].[EmploymentStatusMonitoring]
(		[EmploymentStatusMonitoring_Id] int  primary key,
		[LearnerEmploymentStatus_Id] int  not null,
		[LearnRefNumber] varchar(12),
		[DateEmpStatApp] date,
		[ESMType] varchar(100),
		[ESMCode] bigint
	)
create index [IX_Parent_Invalid_EmploymentStatusMonitoring] on [Invalid].[EmploymentStatusMonitoring]
	(
		[LearnerEmploymentStatus_Id] asc
	)
create index [IX_Invalid_EmploymentStatusMonitoring] on [Invalid].[EmploymentStatusMonitoring]
	(
		[LearnRefNumber] asc,
		[DateEmpStatApp] asc,
		[ESMType] asc
)

if object_id('[Invalid].[LearnerHE]','u') is not null
	drop table [Invalid].[LearnerHE]
 
create table [Invalid].[LearnerHE]
(		[LearnerHE_Id] int  primary key,
		[Learner_Id] int  not null,
		[LearnRefNumber] varchar(12),
		[UCASPERID] varchar(1000),
		[TTACCOM] bigint
	)
create index [IX_Parent_Invalid_LearnerHE] on [Invalid].[LearnerHE]
	(
		[Learner_Id] asc
	)
create index [IX_Invalid_LearnerHE] on [Invalid].[LearnerHE]
	(
		[LearnRefNumber] asc
)

if object_id('[Invalid].[LearnerHEFinancialSupport]','u') is not null
	drop table [Invalid].[LearnerHEFinancialSupport]
 
create table [Invalid].[LearnerHEFinancialSupport]
(		[LearnerHEFinancialSupport_Id] int  primary key,
		[LearnerHE_Id] int  not null,
		[LearnRefNumber] varchar(12),
		[FINTYPE] bigint,
		[FINAMOUNT] bigint
	)
create index [IX_Parent_Invalid_LearnerHEFinancialSupport] on [Invalid].[LearnerHEFinancialSupport]
	(
		[LearnerHE_Id] asc
	)
create index [IX_Invalid_LearnerHEFinancialSupport] on [Invalid].[LearnerHEFinancialSupport]
	(
		[LearnRefNumber] asc,
		[FINTYPE] asc
)

if object_id('[Invalid].[LearningDelivery]','u') is not null
	drop table [Invalid].[LearningDelivery]
 
create table [Invalid].[LearningDelivery]
(		[LearningDelivery_Id] int  primary key,
		[Learner_Id] int  not null,
		[LearnRefNumber] varchar(12),
		[LearnAimRef] varchar(1000),
		[AimType] bigint,
		[AimSeqNumber] int,
		[LearnStartDate] date,
		[OrigLearnStartDate] date,
		[LearnPlanEndDate] date,
		[FundModel] bigint,
		[ProgType] bigint,
		[FworkCode] bigint,
		[PwayCode] bigint,
		[StdCode] bigint,
		[PartnerUKPRN] bigint,
		[DelLocPostCode] varchar(1000),
		[AddHours] bigint,
		[PriorLearnFundAdj] bigint,
		[OtherFundAdj] bigint,
		[ConRefNumber] varchar(1000),
		[EPAOrgID] varchar(1000),
		[EmpOutcome] bigint,
		[CompStatus] bigint,
		[LearnActEndDate] date,
		[WithdrawReason] bigint,
		[Outcome] bigint,
		[AchDate] date,
		[OutGrade] varchar(1000),
		[SWSupAimId] varchar(1000)
	)
create index [IX_Parent_Invalid_LearningDelivery] on [Invalid].[LearningDelivery]
	(
		[Learner_Id] asc
	)
create index [IX_Invalid_LearningDelivery] on [Invalid].[LearningDelivery]
	(
		[LearnRefNumber] asc,
		[AimSeqNumber] asc
)

if object_id('[Invalid].[LearningDeliveryFAM]','u') is not null
	drop table [Invalid].[LearningDeliveryFAM]
 
create table [Invalid].[LearningDeliveryFAM]
(		[LearningDeliveryFAM_Id] int  primary key,
		[LearningDelivery_Id] int  not null,
		[LearnRefNumber] varchar(12),
		[AimSeqNumber] int,
		[LearnDelFAMType] varchar(100),
		[LearnDelFAMCode] varchar(1000),
		[LearnDelFAMDateFrom] date,
		[LearnDelFAMDateTo] date
	)
create index [IX_Parent_Invalid_LearningDeliveryFAM] on [Invalid].[LearningDeliveryFAM]
	(
		[LearningDelivery_Id] asc
	)
create index [IX_Invalid_LearningDeliveryFAM] on [Invalid].[LearningDeliveryFAM]
	(
		[LearnRefNumber] asc,
		[AimSeqNumber] asc,
		[LearnDelFAMType] asc,
		[LearnDelFAMDateFrom] asc
)

if object_id('[Invalid].[LearningDeliveryWorkPlacement]','u') is not null
	drop table [Invalid].[LearningDeliveryWorkPlacement]
 
create table [Invalid].[LearningDeliveryWorkPlacement]
(		[LearningDeliveryWorkPlacement_Id] int  primary key,
		[LearningDelivery_Id] int  not null,
		[LearnRefNumber] varchar(12),
		[AimSeqNumber] int,
		[WorkPlaceStartDate] date,
		[WorkPlaceEndDate] date,
		[WorkPlaceHours] bigint,
		[WorkPlaceMode] bigint,
		[WorkPlaceEmpId] bigint
	)
create index [IX_Parent_Invalid_LearningDeliveryWorkPlacement] on [Invalid].[LearningDeliveryWorkPlacement]
	(
		[LearningDelivery_Id] asc
	)
create index [IX_Invalid_LearningDeliveryWorkPlacement] on [Invalid].[LearningDeliveryWorkPlacement]
	(
		[LearnRefNumber] asc,
		[AimSeqNumber] asc,
		[WorkPlaceStartDate] asc,
		[WorkPlaceMode] asc,
		[WorkPlaceEmpId] asc
)

if object_id('[Invalid].[AppFinRecord]','u') is not null
	drop table [Invalid].[AppFinRecord]
 
create table [Invalid].[AppFinRecord]
(		[AppFinRecord_Id] int  primary key,
		[LearningDelivery_Id] int  not null,
		[LearnRefNumber] varchar(12),
		[AimSeqNumber] int,
		[AFinType] varchar(100),
		[AFinCode] bigint,
		[AFinDate] date,
		[AFinAmount] bigint
	)
create index [IX_Parent_Invalid_AppFinRecord] on [Invalid].[AppFinRecord]
	(
		[LearningDelivery_Id] asc
	)
create index [IX_Invalid_AppFinRecord] on [Invalid].[AppFinRecord]
	(
		[LearnRefNumber] asc,
		[AimSeqNumber] asc,
		[AFinType] asc
)

if object_id('[Invalid].[ProviderSpecDeliveryMonitoring]','u') is not null
	drop table [Invalid].[ProviderSpecDeliveryMonitoring]
 
create table [Invalid].[ProviderSpecDeliveryMonitoring]
(		[ProviderSpecDeliveryMonitoring_Id] int  primary key,
		[LearningDelivery_Id] int  not null,
		[LearnRefNumber] varchar(12),
		[AimSeqNumber] int,
		[ProvSpecDelMonOccur] varchar(100),
		[ProvSpecDelMon] varchar(1000)
	)
create index [IX_Parent_Invalid_ProviderSpecDeliveryMonitoring] on [Invalid].[ProviderSpecDeliveryMonitoring]
	(
		[LearningDelivery_Id] asc
	)
create index [IX_Invalid_ProviderSpecDeliveryMonitoring] on [Invalid].[ProviderSpecDeliveryMonitoring]
	(
		[LearnRefNumber] asc,
		[AimSeqNumber] asc,
		[ProvSpecDelMonOccur] asc
)

if object_id('[Invalid].[LearningDeliveryHE]','u') is not null
	drop table [Invalid].[LearningDeliveryHE]
 
create table [Invalid].[LearningDeliveryHE]
(		[LearningDeliveryHE_Id] int  primary key,
		[LearningDelivery_Id] int  not null,
		[LearnRefNumber] varchar(12),
		[AimSeqNumber] int,
		[NUMHUS] varchar(1000),
		[SSN] varchar(1000),
		[QUALENT3] varchar(1000),
		[SOC2000] bigint,
		[SEC] bigint,
		[UCASAPPID] varchar(1000),
		[TYPEYR] bigint,
		[MODESTUD] bigint,
		[FUNDLEV] bigint,
		[FUNDCOMP] bigint,
		[STULOAD] float,
		[YEARSTU] bigint,
		[MSTUFEE] bigint,
		[PCOLAB] float,
		[PCFLDCS] float,
		[PCSLDCS] float,
		[PCTLDCS] float,
		[SPECFEE] bigint,
		[NETFEE] bigint,
		[GROSSFEE] bigint,
		[DOMICILE] varchar(1000),
		[ELQ] bigint,
		[HEPostCode] varchar(1000)
	)
create index [IX_Parent_Invalid_LearningDeliveryHE] on [Invalid].[LearningDeliveryHE]
	(
		[LearningDelivery_Id] asc
	)
create index [IX_Invalid_LearningDeliveryHE] on [Invalid].[LearningDeliveryHE]
	(
		[LearnRefNumber] asc,
		[AimSeqNumber] asc
)

if object_id('[Invalid].[LearnerDestinationandProgression]','u') is not null
	drop table [Invalid].[LearnerDestinationandProgression]
 
create table [Invalid].[LearnerDestinationandProgression]
(		[LearnerDestinationandProgression_Id] int  primary key,
		[LearnRefNumber] varchar(12),
		[ULN] bigint
	)
create index [IX_Invalid_LearnerDestinationandProgression] on [Invalid].[LearnerDestinationandProgression]
	(
		[LearnRefNumber] asc
)

if object_id('[Invalid].[DPOutcome]','u') is not null
	drop table [Invalid].[DPOutcome]
 
create table [Invalid].[DPOutcome]
(		[DPOutcome_Id] int  primary key,
		[LearnerDestinationandProgression_Id] int  not null,
		[LearnRefNumber] varchar(12),
		[OutType] varchar(100),
		[OutCode] bigint,
		[OutStartDate] date,
		[OutEndDate] date,
		[OutCollDate] date
	)
create index [IX_Parent_Invalid_DPOutcome] on [Invalid].[DPOutcome]
	(
		[LearnerDestinationandProgression_Id] asc
	)
create index [IX_Invalid_DPOutcome] on [Invalid].[DPOutcome]
	(
		[LearnRefNumber] asc,
		[OutType] asc,
		[OutCode] asc,
		[OutStartDate] asc
)
