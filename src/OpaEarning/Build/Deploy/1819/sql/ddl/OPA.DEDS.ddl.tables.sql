
if not exists(select schema_id from sys.schemas where name='Valid')
begin
	exec('create schema Valid')
end
GO


if not exists(select schema_id from sys.schemas where name='Invalid')
begin
	exec('create schema Invalid')
end
GO


DECLARE @schema NVARCHAR(100) = 'Valid'
DECLARE @SqlStatement NVARCHAR(MAX)
SELECT @SqlStatement = 
    COALESCE(@SqlStatement, N'') + N'DROP TABLE [' + @schema + '].' + QUOTENAME(TABLE_NAME) + N';' + CHAR(13)
FROM INFORMATION_SCHEMA.TABLES
WHERE TABLE_SCHEMA = @schema and TABLE_TYPE = 'BASE TABLE'

EXEC sp_executesql @SqlStatement
GO


DECLARE @schema NVARCHAR(100) = 'Invalid'
DECLARE @SqlStatement NVARCHAR(MAX)
SELECT @SqlStatement = 
    COALESCE(@SqlStatement, N'') + N'DROP TABLE [' + @schema + '].' + QUOTENAME(TABLE_NAME) + N';' + CHAR(13)
FROM INFORMATION_SCHEMA.TABLES
WHERE TABLE_SCHEMA = @schema and TABLE_TYPE = 'BASE TABLE'

EXEC sp_executesql @SqlStatement
GO



SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Invalid].[AppFinRecord]') AND type in (N'U'))
BEGIN
CREATE TABLE [Invalid].[AppFinRecord](
	[AppFinRecord_Id] [int] NULL,
	[UKPRN] [int] NULL,
	[LearningDelivery_Id] [int] NULL,
	[LearnRefNumber] [varchar](100) NULL,
	[AimSeqNumber] [bigint] NULL,
	[AFinType] [varchar](100) NULL,
	[AFinCode] [bigint] NULL,
	[AFinDate] [date] NULL,
	[AFinAmount] [bigint] NULL
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Invalid].[CollectionDetails]') AND type in (N'U'))
BEGIN
CREATE TABLE [Invalid].[CollectionDetails](
	[CollectionDetails_Id] [int] NULL,
	[UKPRN] [int] NULL,
	[Collection] [varchar](3) NULL,
	[Year] [varchar](4) NULL,
	[FilePreparationDate] [date] NULL
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Invalid].[ContactPreference]') AND type in (N'U'))
BEGIN
CREATE TABLE [Invalid].[ContactPreference](
	[ContactPreference_Id] [int] NULL,
	[Learner_Id] [int] NULL,
	[UKPRN] [int] NULL,
	[LearnRefNumber] [varchar](100) NULL,
	[ContPrefType] [varchar](100) NULL,
	[ContPrefCode] [bigint] NULL
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Invalid].[DPOutcome]') AND type in (N'U'))
BEGIN
CREATE TABLE [Invalid].[DPOutcome](
	[DPOutcome_Id] [int] NULL,
	[UKPRN] [int] NULL,
	[LearnerDestinationandProgression_Id] [int] NULL,
	[LearnRefNumber] [varchar](100) NULL,
	[OutType] [varchar](100) NULL,
	[OutCode] [bigint] NULL,
	[OutStartDate] [date] NULL,
	[OutEndDate] [date] NULL,
	[OutCollDate] [date] NULL
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Invalid].[EmploymentStatusMonitoring]') AND type in (N'U'))
BEGIN
CREATE TABLE [Invalid].[EmploymentStatusMonitoring](
	[EmploymentStatusMonitoring_Id] [int] NULL,
	[LearnerEmploymentStatus_Id] [int] NULL,
	[UKPRN] [int] NULL,
	[LearnRefNumber] [varchar](100) NULL,
	[DateEmpStatApp] [date] NULL,
	[ESMType] [varchar](100) NULL,
	[ESMCode] [bigint] NULL
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Invalid].[Learner]') AND type in (N'U'))
BEGIN
CREATE TABLE [Invalid].[Learner](
	[Learner_Id] [int] NULL,
	[LearnRefNumber] [varchar](100) NULL,
	[UKPRN] [int] NULL,
	[PrevLearnRefNumber] [varchar](1000) NULL,
	[PrevUKPRN] [bigint] NULL,
	[PMUKPRN] [bigint] NULL,
	[CampId] [varchar](1000) NULL,
	[ULN] [bigint] NULL,
	[FamilyName] [varchar](1000) NULL,
	[GivenNames] [varchar](1000) NULL,
	[DateOfBirth] [date] NULL,
	[Ethnicity] [bigint] NULL,
	[Sex] [varchar](1000) NULL,
	[LLDDHealthProb] [bigint] NULL,
	[NINumber] [varchar](1000) NULL,
	[PriorAttain] [bigint] NULL,
	[Accom] [bigint] NULL,
	[ALSCost] [bigint] NULL,
	[PlanLearnHours] [bigint] NULL,
	[PlanEEPHours] [bigint] NULL,
	[MathGrade] [varchar](1000) NULL,
	[EngGrade] [varchar](1000) NULL,
	[PostcodePrior] [varchar](1000) NULL,
	[Postcode] [varchar](1000) NULL,
	[AddLine1] [varchar](1000) NULL,
	[AddLine2] [varchar](1000) NULL,
	[AddLine3] [varchar](1000) NULL,
	[AddLine4] [varchar](1000) NULL,
	[TelNo] [varchar](1000) NULL,
	[Email] [varchar](1000) NULL,
	[OTJHours] [bigint] NULL
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Invalid].[LearnerDestinationandProgression]') AND type in (N'U'))
BEGIN
CREATE TABLE [Invalid].[LearnerDestinationandProgression](
	[LearnerDestinationandProgression_Id] [int] NULL,
	[UKPRN] [int] NULL,
	[LearnRefNumber] [varchar](100) NULL,
	[ULN] [bigint] NULL
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Invalid].[LearnerEmploymentStatus]') AND type in (N'U'))
BEGIN
CREATE TABLE [Invalid].[LearnerEmploymentStatus](
	[LearnerEmploymentStatus_Id] [int] NULL,
	[Learner_Id] [int] NULL,
	[UKPRN] [int] NULL,
	[LearnRefNumber] [varchar](100) NULL,
	[EmpStat] [bigint] NULL,
	[DateEmpStatApp] [date] NULL,
	[EmpId] [bigint] NULL,
	[AgreeId] [varchar](1000) NULL
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Invalid].[LearnerFAM]') AND type in (N'U'))
BEGIN
CREATE TABLE [Invalid].[LearnerFAM](
	[LearnerFAM_Id] [int] NULL,
	[Learner_Id] [int] NULL,
	[UKPRN] [int] NULL,
	[LearnRefNumber] [varchar](100) NULL,
	[LearnFAMType] [varchar](1000) NULL,
	[LearnFAMCode] [bigint] NULL
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Invalid].[LearnerHE]') AND type in (N'U'))
BEGIN
CREATE TABLE [Invalid].[LearnerHE](
	[LearnerHE_Id] [int] NULL,
	[Learner_Id] [int] NULL,
	[UKPRN] [int] NULL,
	[LearnRefNumber] [varchar](100) NULL,
	[UCASPERID] [varchar](1000) NULL,
	[TTACCOM] [bigint] NULL
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Invalid].[LearnerHEFinancialSupport]') AND type in (N'U'))
BEGIN
CREATE TABLE [Invalid].[LearnerHEFinancialSupport](
	[LearnerHEFinancialSupport_Id] [int] NULL,
	[LearnerHE_Id] [int] NULL,
	[UKPRN] [int] NULL,
	[LearnRefNumber] [varchar](100) NULL,
	[FINTYPE] [bigint] NULL,
	[FINAMOUNT] [bigint] NULL
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Invalid].[LearningDelivery]') AND type in (N'U'))
BEGIN
CREATE TABLE [Invalid].[LearningDelivery](
	[LearningDelivery_Id] [int] NULL,
	[Learner_Id] [int] NULL,
	[UKPRN] [int] NULL,
	[LearnRefNumber] [varchar](100) NULL,
	[LearnAimRef] [varchar](1000) NULL,
	[AimType] [bigint] NULL,
	[AimSeqNumber] [bigint] NULL,
	[LearnStartDate] [date] NULL,
	[OrigLearnStartDate] [date] NULL,
	[LearnPlanEndDate] [date] NULL,
	[FundModel] [bigint] NULL,
	[ProgType] [bigint] NULL,
	[FworkCode] [bigint] NULL,
	[PwayCode] [bigint] NULL,
	[StdCode] [bigint] NULL,
	[PartnerUKPRN] [bigint] NULL,
	[DelLocPostCode] [varchar](1000) NULL,
	[AddHours] [bigint] NULL,
	[PriorLearnFundAdj] [bigint] NULL,
	[OtherFundAdj] [bigint] NULL,
	[ConRefNumber] [varchar](1000) NULL,
	[EPAOrgID] [varchar](1000) NULL,
	[EmpOutcome] [bigint] NULL,
	[CompStatus] [bigint] NULL,
	[LearnActEndDate] [date] NULL,
	[WithdrawReason] [bigint] NULL,
	[Outcome] [bigint] NULL,
	[AchDate] [date] NULL,
	[OutGrade] [varchar](1000) NULL,
	[SWSupAimId] [varchar](1000) NULL
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Invalid].[LearningDeliveryFAM]') AND type in (N'U'))
BEGIN
CREATE TABLE [Invalid].[LearningDeliveryFAM](
	[LearningDeliveryFAM_Id] [int] NULL,
	[LearningDelivery_Id] [int] NULL,
	[UKPRN] [int] NULL,
	[LearnRefNumber] [varchar](100) NULL,
	[AimSeqNumber] [bigint] NULL,
	[LearnDelFAMType] [varchar](100) NULL,
	[LearnDelFAMCode] [varchar](1000) NULL,
	[LearnDelFAMDateFrom] [date] NULL,
	[LearnDelFAMDateTo] [date] NULL
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Invalid].[LearningDeliveryHE]') AND type in (N'U'))
BEGIN
CREATE TABLE [Invalid].[LearningDeliveryHE](
	[LearningDeliveryHE_Id] [int] NULL,
	[UKPRN] [int] NULL,
	[LearningDelivery_Id] [int] NULL,
	[LearnRefNumber] [varchar](100) NULL,
	[AimSeqNumber] [bigint] NULL,
	[NUMHUS] [varchar](1000) NULL,
	[SSN] [varchar](1000) NULL,
	[QUALENT3] [varchar](1000) NULL,
	[SOC2000] [bigint] NULL,
	[SEC] [bigint] NULL,
	[UCASAPPID] [varchar](1000) NULL,
	[TYPEYR] [bigint] NULL,
	[MODESTUD] [bigint] NULL,
	[FUNDLEV] [bigint] NULL,
	[FUNDCOMP] [bigint] NULL,
	[STULOAD] [float] NULL,
	[YEARSTU] [bigint] NULL,
	[MSTUFEE] [bigint] NULL,
	[PCOLAB] [float] NULL,
	[PCFLDCS] [float] NULL,
	[PCSLDCS] [float] NULL,
	[PCTLDCS] [float] NULL,
	[SPECFEE] [bigint] NULL,
	[NETFEE] [bigint] NULL,
	[GROSSFEE] [bigint] NULL,
	[DOMICILE] [varchar](1000) NULL,
	[ELQ] [bigint] NULL,
	[HEPostCode] [varchar](1000) NULL
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Invalid].[LearningDeliveryWorkPlacement]') AND type in (N'U'))
BEGIN
CREATE TABLE [Invalid].[LearningDeliveryWorkPlacement](
	[LearningDeliveryWorkPlacement_Id] [int] NULL,
	[LearningDelivery_Id] [int] NULL,
	[UKPRN] [int] NULL,
	[LearnRefNumber] [varchar](100) NULL,
	[AimSeqNumber] [bigint] NULL,
	[WorkPlaceStartDate] [date] NULL,
	[WorkPlaceEndDate] [date] NULL,
	[WorkPlaceHours] [bigint] NULL,
	[WorkPlaceMode] [bigint] NULL,
	[WorkPlaceEmpId] [bigint] NULL
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Invalid].[LearningProvider]') AND type in (N'U'))
BEGIN
CREATE TABLE [Invalid].[LearningProvider](
	[LearningProvider_Id] [int] NULL,
	[UKPRN] [int] NULL
) ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Invalid].[LLDDandHealthProblem]') AND type in (N'U'))
BEGIN
CREATE TABLE [Invalid].[LLDDandHealthProblem](
	[LLDDandHealthProblem_Id] [int] NULL,
	[Learner_Id] [int] NULL,
	[UKPRN] [int] NULL,
	[LearnRefNumber] [varchar](100) NULL,
	[LLDDCat] [bigint] NULL,
	[PrimaryLLDD] [bigint] NULL
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Invalid].[ProviderSpecDeliveryMonitoring]') AND type in (N'U'))
BEGIN
CREATE TABLE [Invalid].[ProviderSpecDeliveryMonitoring](
	[ProviderSpecDeliveryMonitoring_Id] [int] NULL,
	[UKPRN] [int] NULL,
	[LearningDelivery_Id] [int] NULL,
	[LearnRefNumber] [varchar](100) NULL,
	[AimSeqNumber] [bigint] NULL,
	[ProvSpecDelMonOccur] [varchar](100) NULL,
	[ProvSpecDelMon] [varchar](1000) NULL
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Invalid].[ProviderSpecLearnerMonitoring]') AND type in (N'U'))
BEGIN
CREATE TABLE [Invalid].[ProviderSpecLearnerMonitoring](
	[ProviderSpecLearnerMonitoring_Id] [int] NULL,
	[Learner_Id] [int] NULL,
	[UKPRN] [int] NULL,
	[LearnRefNumber] [varchar](100) NULL,
	[ProvSpecLearnMonOccur] [varchar](100) NULL,
	[ProvSpecLearnMon] [varchar](1000) NULL
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Invalid].[Source]') AND type in (N'U'))
BEGIN
CREATE TABLE [Invalid].[Source](
	[Source_Id] [int] NULL,
	[ProtectiveMarking] [varchar](30) NULL,
	[UKPRN] [int] NULL,
	[SoftwareSupplier] [varchar](40) NULL,
	[SoftwarePackage] [varchar](30) NULL,
	[Release] [varchar](20) NULL,
	[SerialNo] [varchar](2) NULL,
	[DateTime] [datetime] NULL,
	[ReferenceData] [varchar](100) NULL,
	[ComponentSetVersion] [varchar](20) NULL
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Invalid].[SourceFile]') AND type in (N'U'))
BEGIN
CREATE TABLE [Invalid].[SourceFile](
	[SourceFile_Id] [int] NULL,
	[UKPRN] [int] NULL,
	[SourceFileName] [varchar](50) NULL,
	[FilePreparationDate] [date] NULL,
	[SoftwareSupplier] [varchar](40) NULL,
	[SoftwarePackage] [varchar](30) NULL,
	[Release] [varchar](20) NULL,
	[SerialNo] [varchar](2) NULL,
	[DateTime] [datetime] NULL
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Valid].[AppFinRecord]') AND type in (N'U'))
BEGIN
CREATE TABLE [Valid].[AppFinRecord](
	[UKPRN] [int] NOT NULL,
	[LearnRefNumber] [varchar](12) NOT NULL,
	[AimSeqNumber] [int] NOT NULL,
	[AFinType] [varchar](3) NOT NULL,
	[AFinCode] [int] NOT NULL,
	[AFinDate] [date] NOT NULL,
	[AFinAmount] [int] NOT NULL
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Valid].[CollectionDetails]') AND type in (N'U'))
BEGIN
CREATE TABLE [Valid].[CollectionDetails](
	[UKPRN] [int] NOT NULL,
	[Collection] [varchar](3) NOT NULL,
	[Year] [varchar](4) NOT NULL,
	[FilePreparationDate] [date] NULL
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Valid].[ContactPreference]') AND type in (N'U'))
BEGIN
CREATE TABLE [Valid].[ContactPreference](
	[UKPRN] [int] NOT NULL,
	[LearnRefNumber] [varchar](12) NOT NULL,
	[ContPrefType] [varchar](3) NOT NULL,
	[ContPrefCode] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[UKPRN] ASC,
	[LearnRefNumber] ASC,
	[ContPrefType] ASC,
	[ContPrefCode] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Valid].[DPOutcome]') AND type in (N'U'))
BEGIN
CREATE TABLE [Valid].[DPOutcome](
	[UKPRN] [int] NOT NULL,
	[LearnRefNumber] [varchar](12) NOT NULL,
	[OutType] [varchar](3) NOT NULL,
	[OutCode] [int] NOT NULL,
	[OutStartDate] [date] NOT NULL,
	[OutEndDate] [date] NULL,
	[OutCollDate] [date] NOT NULL
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Valid].[EmploymentStatusMonitoring]') AND type in (N'U'))
BEGIN
CREATE TABLE [Valid].[EmploymentStatusMonitoring](
	[UKPRN] [int] NOT NULL,
	[LearnRefNumber] [varchar](12) NOT NULL,
	[DateEmpStatApp] [date] NOT NULL,
	[ESMType] [varchar](3) NOT NULL,
	[ESMCode] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[UKPRN] ASC,
	[LearnRefNumber] ASC,
	[DateEmpStatApp] ASC,
	[ESMType] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Valid].[Learner]') AND type in (N'U'))
BEGIN
CREATE TABLE [Valid].[Learner](
	[UKPRN] [int] NOT NULL,
	[LearnRefNumber] [varchar](12) NOT NULL,
	[PrevLearnRefNumber] [varchar](12) NULL,
	[PrevUKPRN] [int] NULL,
	[CampId] [varchar](8) NULL,
	[PMUKPRN] [int] NULL,
	[ULN] [bigint] NOT NULL,
	[FamilyName] [varchar](100) NULL,
	[GivenNames] [varchar](100) NULL,
	[DateOfBirth] [date] NULL,
	[Ethnicity] [int] NOT NULL,
	[Sex] [varchar](1) NOT NULL,
	[LLDDHealthProb] [int] NOT NULL,
	[NINumber] [varchar](9) NULL,
	[PriorAttain] [int] NULL,
	[Accom] [int] NULL,
	[ALSCost] [int] NULL,
	[PlanLearnHours] [int] NULL,
	[PlanEEPHours] [int] NULL,
	[MathGrade] [varchar](4) NULL,
	[EngGrade] [varchar](4) NULL,
	[PostcodePrior] [varchar](8) NULL,
	[Postcode] [varchar](8) NULL,
	[AddLine1] [varchar](50) NULL,
	[AddLine2] [varchar](50) NULL,
	[AddLine3] [varchar](50) NULL,
	[AddLine4] [varchar](50) NULL,
	[TelNo] [varchar](18) NULL,
	[Email] [varchar](100) NULL,
	[OTJHours] [bigint] NULL,
PRIMARY KEY CLUSTERED 
(
	[UKPRN] ASC,
	[LearnRefNumber] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Valid].[LearnerDestinationandProgression]') AND type in (N'U'))
BEGIN
CREATE TABLE [Valid].[LearnerDestinationandProgression](
	[UKPRN] [int] NOT NULL,
	[LearnRefNumber] [varchar](12) NOT NULL,
	[ULN] [bigint] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[UKPRN] ASC,
	[LearnRefNumber] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Valid].[LearnerEmploymentStatus]') AND type in (N'U'))
BEGIN
CREATE TABLE [Valid].[LearnerEmploymentStatus](
	[UKPRN] [int] NOT NULL,
	[LearnRefNumber] [varchar](12) NOT NULL,
	[EmpStat] [int] NULL,
	[DateEmpStatApp] [date] NOT NULL,
	[EmpId] [int] NULL,
	[AgreeId] [varchar](6) NULL,
PRIMARY KEY CLUSTERED 
(
	[UKPRN] ASC,
	[LearnRefNumber] ASC,
	[DateEmpStatApp] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Valid].[LearnerFAM]') AND type in (N'U'))
BEGIN
CREATE TABLE [Valid].[LearnerFAM](
	[UKPRN] [int] NOT NULL,
	[LearnRefNumber] [varchar](12) NOT NULL,
	[LearnFAMType] [varchar](3) NULL,
	[LearnFAMCode] [int] NOT NULL
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Valid].[LearnerHE]') AND type in (N'U'))
BEGIN
CREATE TABLE [Valid].[LearnerHE](
	[UKPRN] [int] NOT NULL,
	[LearnRefNumber] [varchar](12) NOT NULL,
	[UCASPERID] [varchar](10) NULL,
	[TTACCOM] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[UKPRN] ASC,
	[LearnRefNumber] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Valid].[LearnerHEFinancialSupport]') AND type in (N'U'))
BEGIN
CREATE TABLE [Valid].[LearnerHEFinancialSupport](
	[UKPRN] [int] NOT NULL,
	[LearnRefNumber] [varchar](12) NOT NULL,
	[FINTYPE] [int] NOT NULL,
	[FINAMOUNT] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[UKPRN] ASC,
	[LearnRefNumber] ASC,
	[FINTYPE] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Valid].[LearningDelivery]') AND type in (N'U'))
BEGIN
CREATE TABLE [Valid].[LearningDelivery](
	[UKPRN] [int] NOT NULL,
	[LearnRefNumber] [varchar](12) NOT NULL,
	[LearnAimRef] [varchar](8) NOT NULL,
	[AimType] [int] NOT NULL,
	[AimSeqNumber] [int] NOT NULL,
	[LearnStartDate] [date] NOT NULL,
	[OrigLearnStartDate] [date] NULL,
	[LearnPlanEndDate] [date] NOT NULL,
	[FundModel] [int] NOT NULL,
	[ProgType] [int] NULL,
	[FworkCode] [int] NULL,
	[PwayCode] [int] NULL,
	[StdCode] [int] NULL,
	[PartnerUKPRN] [int] NULL,
	[DelLocPostCode] [varchar](8) NULL,
	[AddHours] [int] NULL,
	[PriorLearnFundAdj] [int] NULL,
	[OtherFundAdj] [int] NULL,
	[ConRefNumber] [varchar](20) NULL,
	[EPAOrgID] [varchar](7) NULL,
	[EmpOutcome] [int] NULL,
	[CompStatus] [int] NOT NULL,
	[LearnActEndDate] [date] NULL,
	[WithdrawReason] [int] NULL,
	[Outcome] [int] NULL,
	[AchDate] [date] NULL,
	[OutGrade] [varchar](6) NULL,
	[SWSupAimId] [varchar](36) NULL,
PRIMARY KEY CLUSTERED 
(
	[UKPRN] ASC,
	[LearnRefNumber] ASC,
	[AimSeqNumber] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Valid].[LearningDeliveryFAM]') AND type in (N'U'))
BEGIN
CREATE TABLE [Valid].[LearningDeliveryFAM](
	[UKPRN] [int] NOT NULL,
	[LearnRefNumber] [varchar](12) NOT NULL,
	[AimSeqNumber] [int] NOT NULL,
	[LearnDelFAMType] [varchar](3) NOT NULL,
	[LearnDelFAMCode] [varchar](5) NOT NULL,
	[LearnDelFAMDateFrom] [date] NULL,
	[LearnDelFAMDateTo] [date] NULL
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Valid].[LearningDeliveryHE]') AND type in (N'U'))
BEGIN
CREATE TABLE [Valid].[LearningDeliveryHE](
	[UKPRN] [int] NOT NULL,
	[LearnRefNumber] [varchar](12) NOT NULL,
	[AimSeqNumber] [int] NOT NULL,
	[NUMHUS] [varchar](20) NULL,
	[SSN] [varchar](13) NULL,
	[QUALENT3] [varchar](3) NULL,
	[SOC2000] [int] NULL,
	[SEC] [int] NULL,
	[UCASAPPID] [varchar](9) NULL,
	[TYPEYR] [int] NOT NULL,
	[MODESTUD] [int] NOT NULL,
	[FUNDLEV] [int] NOT NULL,
	[FUNDCOMP] [int] NOT NULL,
	[STULOAD] [decimal](4, 1) NULL,
	[YEARSTU] [int] NOT NULL,
	[MSTUFEE] [int] NOT NULL,
	[PCOLAB] [decimal](4, 1) NULL,
	[PCFLDCS] [decimal](4, 1) NULL,
	[PCSLDCS] [decimal](4, 1) NULL,
	[PCTLDCS] [decimal](4, 1) NULL,
	[SPECFEE] [int] NOT NULL,
	[NETFEE] [int] NULL,
	[GROSSFEE] [int] NULL,
	[DOMICILE] [varchar](2) NULL,
	[ELQ] [int] NULL,
	[HEPostCode] [varchar](8) NULL,
PRIMARY KEY CLUSTERED 
(
	[UKPRN] ASC,
	[LearnRefNumber] ASC,
	[AimSeqNumber] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Valid].[LearningDeliveryWorkPlacement]') AND type in (N'U'))
BEGIN
CREATE TABLE [Valid].[LearningDeliveryWorkPlacement](
	[UKPRN] [int] NOT NULL,
	[LearnRefNumber] [varchar](12) NOT NULL,
	[AimSeqNumber] [int] NOT NULL,
	[WorkPlaceStartDate] [date] NOT NULL,
	[WorkPlaceEndDate] [date] NULL,
	[WorkPlaceHours] [int] NULL,
	[WorkPlaceMode] [int] NOT NULL,
	[WorkPlaceEmpId] [int] NULL
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Valid].[LearningProvider]') AND type in (N'U'))
BEGIN
CREATE TABLE [Valid].[LearningProvider](
	[UKPRN] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[UKPRN] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Valid].[LLDDandHealthProblem]') AND type in (N'U'))
BEGIN
CREATE TABLE [Valid].[LLDDandHealthProblem](
	[UKPRN] [int] NOT NULL,
	[LearnRefNumber] [varchar](12) NOT NULL,
	[LLDDCat] [int] NOT NULL,
	[PrimaryLLDD] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[UKPRN] ASC,
	[LearnRefNumber] ASC,
	[LLDDCat] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Valid].[ProviderSpecDeliveryMonitoring]') AND type in (N'U'))
BEGIN
CREATE TABLE [Valid].[ProviderSpecDeliveryMonitoring](
	[UKPRN] [int] NOT NULL,
	[LearnRefNumber] [varchar](12) NOT NULL,
	[AimSeqNumber] [int] NOT NULL,
	[ProvSpecDelMonOccur] [varchar](1) NOT NULL,
	[ProvSpecDelMon] [varchar](20) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[UKPRN] ASC,
	[LearnRefNumber] ASC,
	[AimSeqNumber] ASC,
	[ProvSpecDelMonOccur] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Valid].[ProviderSpecLearnerMonitoring]') AND type in (N'U'))
BEGIN
CREATE TABLE [Valid].[ProviderSpecLearnerMonitoring](
	[UKPRN] [int] NOT NULL,
	[LearnRefNumber] [varchar](12) NOT NULL,
	[ProvSpecLearnMonOccur] [varchar](1) NOT NULL,
	[ProvSpecLearnMon] [varchar](20) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[UKPRN] ASC,
	[LearnRefNumber] ASC,
	[ProvSpecLearnMonOccur] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Valid].[Source]') AND type in (N'U'))
BEGIN
CREATE TABLE [Valid].[Source](
	[ProtectiveMarking] [varchar](30) NULL,
	[UKPRN] [int] NOT NULL,
	[SoftwareSupplier] [varchar](40) NULL,
	[SoftwarePackage] [varchar](30) NULL,
	[Release] [varchar](20) NULL,
	[SerialNo] [varchar](2) NULL,
	[DateTime] [datetime] NULL,
	[ReferenceData] [varchar](100) NULL,
	[ComponentSetVersion] [varchar](20) NULL
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Valid].[SourceFile]') AND type in (N'U'))
BEGIN
CREATE TABLE [Valid].[SourceFile](
	[UKPRN] [int] NOT NULL,
	[SourceFileName] [varchar](50) NOT NULL,
	[FilePreparationDate] [date] NULL,
	[SoftwareSupplier] [varchar](40) NULL,
	[SoftwarePackage] [varchar](30) NULL,
	[Release] [varchar](20) NULL,
	[SerialNo] [varchar](2) NULL,
	[DateTime] [datetime] NULL
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
