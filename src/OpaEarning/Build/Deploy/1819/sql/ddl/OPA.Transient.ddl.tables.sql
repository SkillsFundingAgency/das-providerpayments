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


DECLARE @schema NVARCHAR(100) = 'Input'
DECLARE @SqlStatement NVARCHAR(MAX)
SELECT @SqlStatement = 
    COALESCE(@SqlStatement, N'') + N'DROP TABLE [' + @schema + '].' + QUOTENAME(TABLE_NAME) + N';' + CHAR(13)
FROM INFORMATION_SCHEMA.TABLES
WHERE TABLE_SCHEMA = @schema and TABLE_TYPE = 'BASE TABLE'

EXEC sp_executesql @SqlStatement
GO


DECLARE @schema NVARCHAR(100) = 'Reference'
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
CREATE TABLE [Input].[AppFinRecord](
	[AppFinRecord_Id] [int] NOT NULL,
	[LearningDelivery_Id] [int] NOT NULL,
	[LearnRefNumber] [varchar](100) NULL,
	[AimSeqNumber] [bigint] NULL,
	[AFinType] [varchar](100) NULL,
	[AFinCode] [bigint] NULL,
	[AFinDate] [date] NULL,
	[AFinAmount] [bigint] NULL,
PRIMARY KEY CLUSTERED 
(
	[AppFinRecord_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Input].[CollectionDetails](
	[CollectionDetails_Id] [int] NOT NULL,
	[Collection] [varchar](3) NOT NULL,
	[Year] [varchar](4) NOT NULL,
	[FilePreparationDate] [date] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[CollectionDetails_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Input].[ContactPreference](
	[ContactPreference_Id] [int] NOT NULL,
	[Learner_Id] [int] NOT NULL,
	[LearnRefNumber] [varchar](100) NULL,
	[ContPrefType] [varchar](100) NULL,
	[ContPrefCode] [bigint] NULL,
PRIMARY KEY CLUSTERED 
(
	[ContactPreference_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Input].[DPOutcome](
	[DPOutcome_Id] [int] NOT NULL,
	[LearnerDestinationandProgression_Id] [int] NOT NULL,
	[LearnRefNumber] [varchar](100) NULL,
	[OutType] [varchar](100) NULL,
	[OutCode] [bigint] NULL,
	[OutStartDate] [date] NULL,
	[OutEndDate] [date] NULL,
	[OutCollDate] [date] NULL,
PRIMARY KEY CLUSTERED 
(
	[DPOutcome_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Input].[EmploymentStatusMonitoring](
	[EmploymentStatusMonitoring_Id] [int] NOT NULL,
	[LearnerEmploymentStatus_Id] [int] NOT NULL,
	[LearnRefNumber] [varchar](100) NULL,
	[DateEmpStatApp] [date] NULL,
	[ESMType] [varchar](100) NULL,
	[ESMCode] [bigint] NULL,
PRIMARY KEY CLUSTERED 
(
	[EmploymentStatusMonitoring_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Input].[Learner](
	[Learner_Id] [int] NOT NULL,
	[LearnRefNumber] [varchar](100) NULL,
	[PrevLearnRefNumber] [varchar](1000) NULL,
	[PrevUKPRN] [bigint] NULL,
	[PMUKPRN] [bigint] NULL,
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
	[CampId] [varchar](1000) NULL,
	[OTJHours] [bigint] NULL,
PRIMARY KEY CLUSTERED 
(
	[Learner_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Input].[LearnerDestinationandProgression](
	[LearnerDestinationandProgression_Id] [int] NOT NULL,
	[LearnRefNumber] [varchar](100) NULL,
	[ULN] [bigint] NULL,
PRIMARY KEY CLUSTERED 
(
	[LearnerDestinationandProgression_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Input].[LearnerEmploymentStatus](
	[LearnerEmploymentStatus_Id] [int] NOT NULL,
	[Learner_Id] [int] NOT NULL,
	[LearnRefNumber] [varchar](100) NULL,
	[EmpStat] [bigint] NULL,
	[DateEmpStatApp] [date] NULL,
	[EmpId] [bigint] NULL,
	[AgreeId] [varchar](1000) NULL,
PRIMARY KEY CLUSTERED 
(
	[LearnerEmploymentStatus_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Input].[LearnerFAM](
	[LearnerFAM_Id] [int] NOT NULL,
	[Learner_Id] [int] NOT NULL,
	[LearnRefNumber] [varchar](100) NULL,
	[LearnFAMType] [varchar](1000) NULL,
	[LearnFAMCode] [bigint] NULL,
PRIMARY KEY CLUSTERED 
(
	[LearnerFAM_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Input].[LearnerHE](
	[LearnerHE_Id] [int] NOT NULL,
	[Learner_Id] [int] NOT NULL,
	[LearnRefNumber] [varchar](100) NULL,
	[UCASPERID] [varchar](1000) NULL,
	[TTACCOM] [bigint] NULL,
PRIMARY KEY CLUSTERED 
(
	[LearnerHE_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Input].[LearnerHEFinancialSupport](
	[LearnerHEFinancialSupport_Id] [int] NOT NULL,
	[LearnerHE_Id] [int] NOT NULL,
	[LearnRefNumber] [varchar](100) NULL,
	[FINTYPE] [bigint] NULL,
	[FINAMOUNT] [bigint] NULL,
PRIMARY KEY CLUSTERED 
(
	[LearnerHEFinancialSupport_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Input].[LearningDelivery](
	[LearningDelivery_Id] [int] NOT NULL,
	[Learner_Id] [int] NOT NULL,
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
	[SWSupAimId] [varchar](1000) NULL,
PRIMARY KEY CLUSTERED 
(
	[LearningDelivery_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Input].[LearningDeliveryFAM](
	[LearningDeliveryFAM_Id] [int] NOT NULL,
	[LearningDelivery_Id] [int] NOT NULL,
	[LearnRefNumber] [varchar](100) NULL,
	[AimSeqNumber] [bigint] NULL,
	[LearnDelFAMType] [varchar](100) NULL,
	[LearnDelFAMCode] [varchar](1000) NULL,
	[LearnDelFAMDateFrom] [date] NULL,
	[LearnDelFAMDateTo] [date] NULL,
PRIMARY KEY CLUSTERED 
(
	[LearningDeliveryFAM_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Input].[LearningDeliveryHE](
	[LearningDeliveryHE_Id] [int] NOT NULL,
	[LearningDelivery_Id] [int] NOT NULL,
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
	[HEPostCode] [varchar](1000) NULL,
PRIMARY KEY CLUSTERED 
(
	[LearningDeliveryHE_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Input].[LearningDeliveryWorkPlacement](
	[LearningDeliveryWorkPlacement_Id] [int] NOT NULL,
	[LearningDelivery_Id] [int] NOT NULL,
	[LearnRefNumber] [varchar](100) NULL,
	[AimSeqNumber] [bigint] NULL,
	[WorkPlaceStartDate] [date] NULL,
	[WorkPlaceEndDate] [date] NULL,
	[WorkPlaceHours] [bigint] NULL,
	[WorkPlaceMode] [bigint] NULL,
	[WorkPlaceEmpId] [bigint] NULL,
PRIMARY KEY CLUSTERED 
(
	[LearningDeliveryWorkPlacement_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Input].[LearningProvider](
	[LearningProvider_Id] [int] NOT NULL,
	[UKPRN] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[LearningProvider_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Input].[LLDDandHealthProblem](
	[LLDDandHealthProblem_Id] [int] NOT NULL,
	[Learner_Id] [int] NOT NULL,
	[LearnRefNumber] [varchar](100) NULL,
	[LLDDCat] [bigint] NULL,
	[PrimaryLLDD] [bigint] NULL,
PRIMARY KEY CLUSTERED 
(
	[LLDDandHealthProblem_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Input].[ProviderSpecDeliveryMonitoring](
	[ProviderSpecDeliveryMonitoring_Id] [int] NOT NULL,
	[LearningDelivery_Id] [int] NOT NULL,
	[LearnRefNumber] [varchar](100) NULL,
	[AimSeqNumber] [bigint] NULL,
	[ProvSpecDelMonOccur] [varchar](100) NULL,
	[ProvSpecDelMon] [varchar](1000) NULL,
PRIMARY KEY CLUSTERED 
(
	[ProviderSpecDeliveryMonitoring_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Input].[ProviderSpecLearnerMonitoring](
	[ProviderSpecLearnerMonitoring_Id] [int] NOT NULL,
	[Learner_Id] [int] NOT NULL,
	[LearnRefNumber] [varchar](100) NULL,
	[ProvSpecLearnMonOccur] [varchar](100) NULL,
	[ProvSpecLearnMon] [varchar](1000) NULL,
PRIMARY KEY CLUSTERED 
(
	[ProviderSpecLearnerMonitoring_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Input].[Source](
	[Source_Id] [int] NOT NULL,
	[ProtectiveMarking] [varchar](30) NOT NULL,
	[UKPRN] [int] NOT NULL,
	[SoftwareSupplier] [varchar](40) NULL,
	[SoftwarePackage] [varchar](30) NULL,
	[Release] [varchar](20) NULL,
	[SerialNo] [varchar](2) NOT NULL,
	[DateTime] [datetime] NOT NULL,
	[ReferenceData] [varchar](100) NULL,
	[ComponentSetVersion] [varchar](20) NULL,
PRIMARY KEY CLUSTERED 
(
	[Source_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Input].[SourceFile](
	[SourceFile_Id] [int] NOT NULL,
	[SourceFileName] [varchar](50) NOT NULL,
	[FilePreparationDate] [date] NOT NULL,
	[SoftwareSupplier] [varchar](40) NULL,
	[SoftwarePackage] [varchar](30) NULL,
	[Release] [varchar](20) NULL,
	[SerialNo] [varchar](2) NOT NULL,
	[DateTime] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[SourceFile_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Invalid].[AppFinRecord](
	[AppFinRecord_Id] [int] NOT NULL,
	[LearningDelivery_Id] [int] NOT NULL,
	[LearnRefNumber] [varchar](100) NULL,
	[AimSeqNumber] [bigint] NULL,
	[AFinType] [varchar](100) NULL,
	[AFinCode] [bigint] NULL,
	[AFinDate] [date] NULL,
	[AFinAmount] [bigint] NULL,
PRIMARY KEY CLUSTERED 
(
	[AppFinRecord_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Invalid].[CollectionDetails](
	[CollectionDetails_Id] [int] NOT NULL,
	[Collection] [varchar](3) NOT NULL,
	[Year] [varchar](4) NOT NULL,
	[FilePreparationDate] [date] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[CollectionDetails_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Invalid].[ContactPreference](
	[ContactPreference_Id] [int] NOT NULL,
	[Learner_Id] [int] NOT NULL,
	[LearnRefNumber] [varchar](100) NULL,
	[ContPrefType] [varchar](100) NULL,
	[ContPrefCode] [bigint] NULL,
PRIMARY KEY CLUSTERED 
(
	[ContactPreference_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Invalid].[DPOutcome](
	[DPOutcome_Id] [int] NOT NULL,
	[LearnerDestinationandProgression_Id] [int] NOT NULL,
	[LearnRefNumber] [varchar](100) NULL,
	[OutType] [varchar](100) NULL,
	[OutCode] [bigint] NULL,
	[OutStartDate] [date] NULL,
	[OutEndDate] [date] NULL,
	[OutCollDate] [date] NULL,
PRIMARY KEY CLUSTERED 
(
	[DPOutcome_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Invalid].[EmploymentStatusMonitoring](
	[EmploymentStatusMonitoring_Id] [int] NOT NULL,
	[LearnerEmploymentStatus_Id] [int] NOT NULL,
	[LearnRefNumber] [varchar](100) NULL,
	[DateEmpStatApp] [date] NULL,
	[ESMType] [varchar](100) NULL,
	[ESMCode] [bigint] NULL,
PRIMARY KEY CLUSTERED 
(
	[EmploymentStatusMonitoring_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Invalid].[Learner](
	[Learner_Id] [int] NOT NULL,
	[LearnRefNumber] [varchar](100) NULL,
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
	[OTJHours] [bigint] NULL,
PRIMARY KEY CLUSTERED 
(
	[Learner_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Invalid].[LearnerDestinationandProgression](
	[LearnerDestinationandProgression_Id] [int] NOT NULL,
	[LearnRefNumber] [varchar](100) NULL,
	[ULN] [bigint] NULL,
PRIMARY KEY CLUSTERED 
(
	[LearnerDestinationandProgression_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Invalid].[LearnerEmploymentStatus](
	[LearnerEmploymentStatus_Id] [int] NOT NULL,
	[Learner_Id] [int] NOT NULL,
	[LearnRefNumber] [varchar](100) NULL,
	[EmpStat] [bigint] NULL,
	[DateEmpStatApp] [date] NULL,
	[EmpId] [bigint] NULL,
	[AgreeId] [varchar](1000) NULL,
PRIMARY KEY CLUSTERED 
(
	[LearnerEmploymentStatus_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Invalid].[LearnerFAM](
	[LearnerFAM_Id] [int] NOT NULL,
	[Learner_Id] [int] NOT NULL,
	[LearnRefNumber] [varchar](100) NULL,
	[LearnFAMType] [varchar](1000) NULL,
	[LearnFAMCode] [bigint] NULL,
PRIMARY KEY CLUSTERED 
(
	[LearnerFAM_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Invalid].[LearnerHE](
	[LearnerHE_Id] [int] NOT NULL,
	[Learner_Id] [int] NOT NULL,
	[LearnRefNumber] [varchar](100) NULL,
	[UCASPERID] [varchar](1000) NULL,
	[TTACCOM] [bigint] NULL,
PRIMARY KEY CLUSTERED 
(
	[LearnerHE_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Invalid].[LearnerHEFinancialSupport](
	[LearnerHEFinancialSupport_Id] [int] NOT NULL,
	[LearnerHE_Id] [int] NOT NULL,
	[LearnRefNumber] [varchar](100) NULL,
	[FINTYPE] [bigint] NULL,
	[FINAMOUNT] [bigint] NULL,
PRIMARY KEY CLUSTERED 
(
	[LearnerHEFinancialSupport_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Invalid].[LearningDelivery](
	[LearningDelivery_Id] [int] NOT NULL,
	[Learner_Id] [int] NOT NULL,
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
	[SWSupAimId] [varchar](1000) NULL,
PRIMARY KEY CLUSTERED 
(
	[LearningDelivery_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Invalid].[LearningDeliveryFAM](
	[LearningDeliveryFAM_Id] [int] NOT NULL,
	[LearningDelivery_Id] [int] NOT NULL,
	[LearnRefNumber] [varchar](100) NULL,
	[AimSeqNumber] [bigint] NULL,
	[LearnDelFAMType] [varchar](100) NULL,
	[LearnDelFAMCode] [varchar](1000) NULL,
	[LearnDelFAMDateFrom] [date] NULL,
	[LearnDelFAMDateTo] [date] NULL,
PRIMARY KEY CLUSTERED 
(
	[LearningDeliveryFAM_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Invalid].[LearningDeliveryHE](
	[LearningDeliveryHE_Id] [int] NOT NULL,
	[LearningDelivery_Id] [int] NOT NULL,
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
	[HEPostCode] [varchar](1000) NULL,
PRIMARY KEY CLUSTERED 
(
	[LearningDeliveryHE_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Invalid].[LearningDeliveryWorkPlacement](
	[LearningDeliveryWorkPlacement_Id] [int] NOT NULL,
	[LearningDelivery_Id] [int] NOT NULL,
	[LearnRefNumber] [varchar](100) NULL,
	[AimSeqNumber] [bigint] NULL,
	[WorkPlaceStartDate] [date] NULL,
	[WorkPlaceEndDate] [date] NULL,
	[WorkPlaceHours] [bigint] NULL,
	[WorkPlaceMode] [bigint] NULL,
	[WorkPlaceEmpId] [bigint] NULL,
PRIMARY KEY CLUSTERED 
(
	[LearningDeliveryWorkPlacement_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Invalid].[LearningProvider](
	[LearningProvider_Id] [int] NOT NULL,
	[UKPRN] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[LearningProvider_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Invalid].[LLDDandHealthProblem](
	[LLDDandHealthProblem_Id] [int] NOT NULL,
	[Learner_Id] [int] NOT NULL,
	[LearnRefNumber] [varchar](100) NULL,
	[LLDDCat] [bigint] NULL,
	[PrimaryLLDD] [bigint] NULL,
PRIMARY KEY CLUSTERED 
(
	[LLDDandHealthProblem_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Invalid].[ProviderSpecDeliveryMonitoring](
	[ProviderSpecDeliveryMonitoring_Id] [int] NOT NULL,
	[LearningDelivery_Id] [int] NOT NULL,
	[LearnRefNumber] [varchar](100) NULL,
	[AimSeqNumber] [bigint] NULL,
	[ProvSpecDelMonOccur] [varchar](100) NULL,
	[ProvSpecDelMon] [varchar](1000) NULL,
PRIMARY KEY CLUSTERED 
(
	[ProviderSpecDeliveryMonitoring_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Invalid].[ProviderSpecLearnerMonitoring](
	[ProviderSpecLearnerMonitoring_Id] [int] NOT NULL,
	[Learner_Id] [int] NOT NULL,
	[LearnRefNumber] [varchar](100) NULL,
	[ProvSpecLearnMonOccur] [varchar](100) NULL,
	[ProvSpecLearnMon] [varchar](1000) NULL,
PRIMARY KEY CLUSTERED 
(
	[ProviderSpecLearnerMonitoring_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Invalid].[Source](
	[Source_Id] [int] NOT NULL,
	[ProtectiveMarking] [varchar](30) NOT NULL,
	[UKPRN] [int] NOT NULL,
	[SoftwareSupplier] [varchar](40) NULL,
	[SoftwarePackage] [varchar](30) NULL,
	[Release] [varchar](20) NULL,
	[SerialNo] [varchar](2) NOT NULL,
	[DateTime] [datetime] NOT NULL,
	[ReferenceData] [varchar](100) NULL,
	[ComponentSetVersion] [varchar](20) NULL,
PRIMARY KEY CLUSTERED 
(
	[Source_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Invalid].[SourceFile](
	[SourceFile_Id] [int] NOT NULL,
	[SourceFileName] [varchar](50) NOT NULL,
	[FilePreparationDate] [date] NOT NULL,
	[SoftwareSupplier] [varchar](40) NULL,
	[SoftwarePackage] [varchar](30) NULL,
	[Release] [varchar](20) NULL,
	[SerialNo] [varchar](2) NOT NULL,
	[DateTime] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[SourceFile_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Reference].[AccountLegalEntity](
	[Id] [bigint] NOT NULL,
	[PublicHashedId] [char](6) NOT NULL,
	[AccountId] [bigint] NOT NULL,
	[LegalEntityId] [bigint] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Reference].[AEC_LatestInYearEarningHistory](
	[AppIdentifier] [varchar](50) NOT NULL,
	[AppProgCompletedInTheYearInput] [bit] NULL,
	[CollectionYear] [varchar](4) NOT NULL,
	[CollectionReturnCode] [varchar](3) NOT NULL,
	[DaysInYear] [int] NULL,
	[FworkCode] [int] NULL,
	[HistoricEffectiveTNPStartDateInput] [date] NULL,
	[HistoricEmpIdEndWithinYear] [bigint] NULL,
	[HistoricEmpIdStartWithinYear] [bigint] NULL,
	[HistoricLearner1618StartInput] [bit] NULL,
	[HistoricPMRAmount] [decimal](12, 5) NULL,
	[HistoricTNP1Input] [decimal](12, 5) NULL,
	[HistoricTNP2Input] [decimal](12, 5) NULL,
	[HistoricTNP3Input] [decimal](12, 5) NULL,
	[HistoricTNP4Input] [decimal](12, 5) NULL,
	[HistoricTotal1618UpliftPaymentsInTheYearInput] [decimal](12, 5) NULL,
	[HistoricVirtualTNP3EndOfTheYearInput] [decimal](12, 5) NULL,
	[HistoricVirtualTNP4EndOfTheYearInput] [decimal](12, 5) NULL,
	[HistoricLearnDelProgEarliestACT2DateInput] [date] NULL,
	[LatestInYear] [bit] NOT NULL,
	[LearnRefNumber] [varchar](12) NOT NULL,
	[ProgrammeStartDateIgnorePathway] [date] NULL,
	[ProgrammeStartDateMatchPathway] [date] NULL,
	[ProgType] [int] NULL,
	[PwayCode] [int] NULL,
	[STDCode] [int] NULL,
	[TotalProgAimPaymentsInTheYear] [decimal](12, 5) NULL,
	[UptoEndDate] [date] NULL,
	[UKPRN] [int] NOT NULL,
	[ULN] [bigint] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[LatestInYear] DESC,
	[LearnRefNumber] ASC,
	[UKPRN] ASC,
	[CollectionYear] ASC,
	[CollectionReturnCode] ASC,
	[AppIdentifier] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Reference].[CampusIdentifier](
	[CampusIdentifier] [varchar](8) NOT NULL,
	[MasterUKPRN] [bigint] NOT NULL,
	[OriginalUKPRN] [bigint] NULL,
	[EffectiveFrom] [date] NOT NULL,
	[EffectiveTo] [date] NULL
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Reference].[CareerLearningPilot_Postcode](
	[Postcode] [varchar](10) NOT NULL,
	[AreaCode] [varchar](50) NOT NULL,
	[EffectiveFrom] [date] NOT NULL,
	[EffectiveTo] [date] NULL
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Reference].[CollectionCalendar](
	[CollectionType] [varchar](20) NOT NULL,
	[CollectionReturnCode] [varchar](10) NOT NULL,
	[ProposedOpenDate] [datetime] NULL,
	[ProposedClosedDate] [datetime] NULL,
	[ActualClosedDate] [datetime] NULL,
 CONSTRAINT [PK_CollectionCalendar] PRIMARY KEY CLUSTERED 
(
	[CollectionType] ASC,
	[CollectionReturnCode] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Reference].[CollectionPeriods](
	[Id] [int] NOT NULL,
	[Name] [varchar](3) NOT NULL,
	[CalendarMonth] [int] NOT NULL,
	[CalendarYear] [int] NOT NULL,
	[Open] [bit] NOT NULL,
	[AcademicYear] [varchar](4) NULL,
 CONSTRAINT [PK_CollectionPeriods] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Reference].[ContractAllocation](
	[ContractAllocationNumber] [varchar](20) NOT NULL,
	[LotReference] [varchar](100) NOT NULL,
	[TenderSpecReference] [varchar](100) NOT NULL
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Reference].[DasAccounts](
	[AccountId] [bigint] NOT NULL,
	[AccountHashId] [varchar](50) NOT NULL,
	[AccountName] [varchar](125) NOT NULL,
	[Balance] [decimal](18, 4) NULL,
	[VersionId] [varchar](50) NOT NULL,
	[IsLevyPayer] [bit] NOT NULL,
	[TransferAllowance] [decimal](15, 5) NULL,
PRIMARY KEY CLUSTERED 
(
	[AccountId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Reference].[DasCommitments](
	[CommitmentId] [bigint] NOT NULL,
	[VersionId] [varchar](25) NOT NULL,
	[Uln] [bigint] NOT NULL,
	[Ukprn] [bigint] NOT NULL,
	[AccountId] [bigint] NOT NULL,
	[StartDate] [date] NOT NULL,
	[EndDate] [date] NOT NULL,
	[AgreedCost] [decimal](15, 2) NOT NULL,
	[StandardCode] [bigint] NULL,
	[ProgrammeType] [int] NULL,
	[FrameworkCode] [int] NULL,
	[PathwayCode] [int] NULL,
	[PaymentStatus] [int] NOT NULL,
	[PaymentStatusDescription] [varchar](50) NOT NULL,
	[Priority] [int] NOT NULL,
	[EffectiveFrom] [date] NOT NULL,
	[EffectiveTo] [date] NULL,
	[LegalEntityName] [nvarchar](100) NULL,
	[TransferSendingEmployerAccountId] [bigint] NULL,
	[TransferApprovalDate] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[CommitmentId] ASC,
	[VersionId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Reference].[DasCommitmentsHistory](
	[CommitmentId] [bigint] NOT NULL,
	[VersionId] [varchar](25) NOT NULL,
	[Uln] [bigint] NOT NULL,
	[Ukprn] [bigint] NOT NULL,
	[AccountId] [bigint] NOT NULL,
	[StartDate] [date] NOT NULL,
	[EndDate] [date] NOT NULL,
	[AgreedCost] [decimal](15, 2) NOT NULL,
	[StandardCode] [bigint] NULL,
	[ProgrammeType] [int] NULL,
	[FrameworkCode] [int] NULL,
	[PathwayCode] [int] NULL,
	[PaymentStatus] [int] NOT NULL,
	[PaymentStatusDescription] [varchar](50) NOT NULL,
	[Priority] [int] NOT NULL,
	[EffectiveFrom] [date] NOT NULL,
	[EffectiveTo] [date] NULL,
	[LegalEntityName] [nvarchar](100) NULL,
	[TransferSendingEmployerAccountId] [bigint] NULL,
	[TransferApprovalDate] [datetime] NULL
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Reference].[DataLockEventCommitmentVersions](
	[DataLockEventId] [uniqueidentifier] NOT NULL,
	[CommitmentVersion] [varchar](25) NOT NULL,
	[CommitmentStartDate] [date] NOT NULL,
	[CommitmentStandardCode] [bigint] NULL,
	[CommitmentProgrammeType] [int] NULL,
	[CommitmentFrameworkCode] [int] NULL,
	[CommitmentPathwayCode] [int] NULL,
	[CommitmentNegotiatedPrice] [decimal](12, 5) NOT NULL,
	[CommitmentEffectiveDate] [date] NOT NULL
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Reference].[DataLockEventErrors](
	[DataLockEventId] [uniqueidentifier] NOT NULL,
	[ErrorCode] [varchar](15) NOT NULL,
	[SystemDescription] [nvarchar](255) NOT NULL,
PRIMARY KEY NONCLUSTERED 
(
	[DataLockEventId] ASC,
	[ErrorCode] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Reference].[DataLockEventPeriods](
	[DataLockEventId] [uniqueidentifier] NOT NULL,
	[CollectionPeriodName] [varchar](8) NOT NULL,
	[CollectionPeriodMonth] [int] NOT NULL,
	[CollectionPeriodYear] [int] NOT NULL,
	[CommitmentVersion] [varchar](25) NOT NULL,
	[IsPayable] [bit] NOT NULL,
	[TransactionType] [int] NOT NULL,
	[TransactionTypesFlag] [int] NULL
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Reference].[DataLockEvents](
	[Id] [bigint] NOT NULL,
	[ProcessDateTime] [datetime] NOT NULL,
	[Status] [int] NOT NULL,
	[IlrFileName] [nvarchar](50) NOT NULL,
	[SubmittedDateTime] [datetime] NOT NULL,
	[AcademicYear] [varchar](4) NOT NULL,
	[UKPRN] [bigint] NOT NULL,
	[ULN] [bigint] NOT NULL,
	[LearnRefNumber] [varchar](12) NOT NULL,
	[AimSeqNumber] [int] NOT NULL,
	[PriceEpisodeIdentifier] [varchar](25) NOT NULL,
	[CommitmentId] [bigint] NOT NULL,
	[EmployerAccountId] [bigint] NOT NULL,
	[EventSource] [int] NOT NULL,
	[HasErrors] [bit] NOT NULL,
	[IlrStartDate] [date] NULL,
	[IlrStandardCode] [bigint] NULL,
	[IlrProgrammeType] [int] NULL,
	[IlrFrameworkCode] [int] NULL,
	[IlrPathwayCode] [int] NULL,
	[IlrTrainingPrice] [decimal](12, 5) NULL,
	[IlrEndpointAssessorPrice] [decimal](12, 5) NULL,
	[IlrPriceEffectiveFromDate] [date] NULL,
	[IlrPriceEffectiveToDate] [date] NULL,
	[DataLockEventId] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_Reference_DataLockEvents] PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Reference].[DeliverableCodeMappings](
	[ExternalDeliverableCode] [nvarchar](5) NULL,
	[FundingStreamPeriodCode] [nvarchar](20) NULL,
	[DeliverableName] [nvarchar](120) NULL,
	[FCSDeliverableCode] [bigint] NULL
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Reference].[EligibilityRule](
	[Benefits] [bit] NULL,
	[LotReference] [varchar](100) NOT NULL,
	[MaxAge] [int] NULL,
	[MaxLengthOfUnemployment] [int] NULL,
	[MaxPriorAttainment] [varchar](2) NULL,
	[MinAge] [int] NULL,
	[MinLengthOfUnemployment] [int] NULL,
	[MinPriorAttainment] [varchar](2) NULL,
	[TenderSpecificationReference] [varchar](100) NOT NULL
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Reference].[EligibilityRuleEmploymentStatus](
	[LotReference] [varchar](100) NOT NULL,
	[TenderSpecificationReference] [varchar](100) NOT NULL,
	[EmploymentStatusCode] [int] NOT NULL
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Reference].[EligibilityRuleLocalAuthority](
	[LotReference] [varchar](100) NOT NULL,
	[TenderSpecificationReference] [varchar](100) NOT NULL,
	[LocalAuthorityCode] [varchar](255) NOT NULL
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Reference].[EligibilityRuleLocalEnterprisePartnership](
	[LotReference] [varchar](100) NOT NULL,
	[TenderSpecificationReference] [varchar](100) NOT NULL,
	[LocalEnterprisePartnershipCode] [varchar](255) NOT NULL
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Reference].[EligibilityRuleSectorSubjectAreaLevel](
	[LotReference] [varchar](100) NOT NULL,
	[TenderSpecificationReference] [varchar](100) NOT NULL,
	[MaxLevelCode] [varchar](1) NULL,
	[MinLevelCode] [varchar](1) NULL,
	[SectorSubjectAreaCode] [decimal](5, 2) NULL
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Reference].[Employers](
	[URN] [int] NOT NULL
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Reference].[Employers_Current_Version](
	[CurrentVersion] [varchar](100) NULL
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Reference].[EPAOrganisation](
	[EPAOrgId] [nvarchar](100) NOT NULL,
	[StandardCode] [varchar](10) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[StandardCode] ASC,
	[EPAOrgId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Reference].[ESF_FundingData](
	[AcademicYear] [varchar](10) NOT NULL,
	[UKPRN] [int] NOT NULL,
	[ConRefNumber] [varchar](20) NOT NULL,
	[LearnRefNumber] [varchar](12) NOT NULL,
	[AimSeqNumber] [int] NOT NULL,
	[DeliverableCode] [varchar](5) NOT NULL,
	[AttributeName] [varchar](100) NOT NULL,
	[Period_1] [decimal](15, 5) NULL,
	[Period_2] [decimal](15, 5) NULL,
	[Period_3] [decimal](15, 5) NULL,
	[Period_4] [decimal](15, 5) NULL,
	[Period_5] [decimal](15, 5) NULL,
	[Period_6] [decimal](15, 5) NULL,
	[Period_7] [decimal](15, 5) NULL,
	[Period_8] [decimal](15, 5) NULL,
	[Period_9] [decimal](15, 5) NULL,
	[Period_10] [decimal](15, 5) NULL,
	[Period_11] [decimal](15, 5) NULL,
	[Period_12] [decimal](15, 5) NULL,
	[CollectionType] [varchar](20) NOT NULL,
	[CollectionReturnCode] [varchar](10) NOT NULL,
 CONSTRAINT [PK_ESFFundingData] PRIMARY KEY CLUSTERED 
(
	[AcademicYear] ASC,
	[UKPRN] ASC,
	[ConRefNumber] ASC,
	[LearnRefNumber] ASC,
	[AimSeqNumber] ASC,
	[DeliverableCode] ASC,
	[AttributeName] ASC,
	[CollectionType] ASC,
	[CollectionReturnCode] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Reference].[ESF_Processed_Providers](
	[UKPRN] [int] NOT NULL,
	[OrganisationId] [varchar](10) NOT NULL,
	[ProcessedTime] [datetime] NOT NULL,
	[SourceFileName] [nvarchar](50) NULL,
	[FilePreparationDate] [date] NULL,
	[FileUploadDate] [datetime] NULL,
	[AcademicYear] [varchar](10) NULL
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Reference].[ESF_SupplementaryData](
	[ConRefNumber] [varchar](20) NOT NULL,
	[DeliverableCode] [varchar](10) NOT NULL,
	[CalendarYear] [int] NOT NULL,
	[CalendarMonth] [int] NOT NULL,
	[CostType] [varchar](20) NOT NULL,
	[ReferenceType] [varchar](20) NOT NULL,
	[Reference] [varchar](100) NOT NULL,
	[Value] [decimal](8, 2) NULL
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Reference].[FCS_Deliverable_Description](
	[ContractAllocationNumber] [nvarchar](20) NOT NULL,
	[DeliverableCode] [varchar](10) NOT NULL,
	[DeliverableDescription] [nvarchar](255) NOT NULL
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Reference].[FCSContract](
	[UKPRN] [nvarchar](200) NULL,
	[OrganisationIdentifier] [nvarchar](10) NULL,
	[FundingStreamPeriodCode] [nvarchar](20) NULL,
	[Period] [nvarchar](100) NULL,
	[contractNumber] [nvarchar](100) NULL,
	[contractAllocationNumber] [nvarchar](100) NULL
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Reference].[FM25_PostcodeDisadvantage](
	[Postcode] [varchar](10) NOT NULL,
	[Uplift] [decimal](10, 5) NOT NULL,
	[EffectiveFrom] [date] NULL,
	[EffectiveTo] [date] NULL
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Reference].[LargeEmployers](
	[EffectiveFrom] [date] NOT NULL,
	[EffectiveTo] [date] NULL,
	[ERN] [int] NOT NULL
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Reference].[LARS_AnnualValue](
	[BasicSkills] [int] NULL,
	[BasicSkillsParticipation] [int] NULL,
	[BasicSkillsType] [int] NULL,
	[EffectiveFrom] [date] NOT NULL,
	[EffectiveTo] [date] NULL,
	[FullLevel2EntitlementCategory] [int] NULL,
	[FullLevel2Percent] [decimal](5, 2) NULL,
	[FullLevel3EntitlementCategory] [int] NULL,
	[FullLevel3Percent] [decimal](5, 2) NULL,
	[LearnAimRef] [varchar](8) NOT NULL,
	[SfaApprovalStatus] [int] NULL
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Reference].[LARS_ApprenticeshipFunding](
	[ApprenticeshipCode] [int] NOT NULL,
	[ApprenticeshipType] [varchar](50) NOT NULL,
	[ProgType] [int] NOT NULL,
	[PwayCode] [int] NULL,
	[FundingCategory] [varchar](15) NOT NULL,
	[EffectiveFrom] [date] NOT NULL,
	[EffectiveTo] [date] NULL,
	[BandNumber] [int] NULL,
	[CoreGovContributuionCap] [decimal](10, 5) NULL,
	[1618Incentive] [decimal](10, 5) NULL,
	[1618ProviderAdditionalPayment] [decimal](10, 5) NULL,
	[1618EmployerAdditionalPayment] [decimal](10, 5) NULL,
	[1618FrameworkUplift] [decimal](10, 5) NULL,
	[CareLeaverAdditionalPayment] [decimal](10, 5) NULL,
	[Duration] [decimal](10, 5) NULL,
	[ReservedValue2] [decimal](10, 5) NULL,
	[ReservedValue3] [decimal](10, 5) NULL,
	[MaxEmployerLevyCap] [decimal](10, 5) NULL,
	[FundableWithoutEmployer] [char](1) NULL
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Reference].[LARS_CareerLearningPilot](
	[LearnAimRef] [varchar](8) NOT NULL,
	[AreaCode] [varchar](50) NOT NULL,
	[EffectiveFrom] [date] NOT NULL,
	[EffectiveTo] [date] NULL,
	[MaxLoanAmount] [decimal](10, 5) NULL,
	[SubsidyPercent] [decimal](10, 5) NULL,
	[SubsidyRate] [decimal](10, 5) NULL
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Reference].[LARS_Current_Version](
	[CurrentVersion] [varchar](100) NULL
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Reference].[LARS_Framework](
	[EffectiveTo] [date] NULL,
	[FworkCode] [int] NOT NULL,
	[ProgType] [int] NOT NULL,
	[PwayCode] [int] NOT NULL
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Reference].[LARS_FrameworkAims](
	[EffectiveTo] [date] NULL,
	[FrameworkComponentType] [int] NULL,
	[FworkCode] [int] NOT NULL,
	[LearnAimRef] [varchar](8) NOT NULL,
	[ProgType] [int] NOT NULL,
	[PwayCode] [int] NOT NULL
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Reference].[LARS_FrameworkCmnComp](
	[CommonComponent] [int] NOT NULL,
	[FworkCode] [int] NOT NULL,
	[ProgType] [int] NOT NULL,
	[PwayCode] [int] NOT NULL,
	[EffectiveFrom] [date] NOT NULL,
	[EffectiveTo] [date] NULL
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Reference].[LARS_Funding](
	[EffectiveFrom] [date] NOT NULL,
	[EffectiveTo] [date] NULL,
	[FundingCategory] [varchar](15) NOT NULL,
	[LearnAimRef] [varchar](8) NOT NULL,
	[RateUnWeighted] [decimal](10, 5) NULL,
	[RateWeighted] [decimal](10, 5) NULL,
	[WeightingFactor] [varchar](1) NOT NULL
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Reference].[LARS_LearningDelivery](
	[AwardOrgAimRef] [varchar](50) NULL,
	[AwardOrgCode] [varchar](20) NULL,
	[CreditBasedFwkType] [int] NULL,
	[EFACOFType] [int] NULL,
	[EnglandFEHEStatus] [char](1) NULL,
	[EnglPrscID] [int] NULL,
	[FrameworkCommonComponent] [int] NULL,
	[LearnAimRef] [varchar](8) NOT NULL,
	[LearnAimRefTitle] [varchar](254) NULL,
	[LearnAimRefType] [varchar](4) NULL,
	[LearnDirectClassSystemCode1] [varchar](12) NULL,
	[LearnDirectClassSystemCode2] [varchar](12) NULL,
	[LearnDirectClassSystemCode3] [varchar](12) NULL,
	[LearningDeliveryGenre] [varchar](3) NULL,
	[NotionalNVQLevel] [char](1) NULL,
	[NotionalNVQLevelv2] [varchar](5) NULL,
	[RegulatedCreditValue] [int] NULL,
	[SectorSubjectAreaTier1] [decimal](5, 2) NULL,
	[SectorSubjectAreaTier2] [decimal](5, 2) NULL,
	[UnemployedOnly] [int] NULL,
	[UnitType] [varchar](50) NULL,
	[EffectiveFrom] [date] NOT NULL,
	[EffectiveTo] [date] NULL
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Reference].[LARS_LearningDeliveryCategory](
	[CategoryRef] [int] NOT NULL,
	[EffectiveFrom] [date] NOT NULL,
	[EffectiveTo] [date] NULL,
	[LearnAimRef] [varchar](8) NOT NULL
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Reference].[LARS_LearningDeliveryCategory_Children](
	[CategoryRef] [int] NULL,
	[ParentCategoryRef] [int] NULL,
	[RootCategoryRef] [int] NULL
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Reference].[LARS_LearningDeliveryCategory_TopMostCategory](
	[CategoryRef] [int] NULL,
	[LearnAimRef] [varchar](8) NULL
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Reference].[LARS_Section96](
	[LearnAimRef] [varchar](8) NOT NULL,
	[Section96ApprovalStatus] [int] NULL,
	[Section96ReviewDate] [date] NULL,
	[Section96Valid16to18] [int] NULL
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Reference].[LARS_Standard](
	[EffectiveFrom] [date] NULL,
	[EffectiveTo] [date] NULL,
	[StandardCode] [int] NOT NULL,
	[NotionalEndLevel] [varchar](5) NULL
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Reference].[LARS_StandardCommonComponent](
	[CommonComponent] [int] NOT NULL,
	[EffectiveFrom] [date] NOT NULL,
	[EffectiveTo] [date] NULL,
	[StandardCode] [int] NOT NULL
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Reference].[LARS_StandardFunding](
	[1618Incentive] [decimal](10, 5) NULL,
	[AchievementIncentive] [decimal](10, 5) NULL,
	[CoreGovContributionCap] [decimal](10, 5) NULL,
	[EffectiveFrom] [date] NOT NULL,
	[EffectiveTo] [date] NULL,
	[FundableWithoutEmployer] [varchar](50) NULL,
	[FundingCategory] [varchar](15) NOT NULL,
	[SmallBusinessIncentive] [decimal](10, 5) NULL,
	[StandardCode] [int] NOT NULL
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Reference].[LARS_Validity](
	[EndDate] [date] NULL,
	[LastNewStartDate] [date] NULL,
	[LearnAimRef] [varchar](8) NOT NULL,
	[StartDate] [date] NOT NULL,
	[ValidityCategory] [varchar](50) NOT NULL
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Reference].[LARSLARS](
	[LearnAimRef] [varchar](8) NULL,
	[BasicSkills] [int] NULL,
	[AwardOrgAimRef] [varchar](50) NULL,
	[UnemployedOnly] [int] NULL,
	[LearnDirectClassSystemCode1] [varchar](12) NULL,
	[LearnDirectClassSystemCode2] [varchar](12) NULL,
	[LearnDirectClassSystemCode3] [varchar](12) NULL,
	[LearnAimRefType] [varchar](4) NULL,
	[FullLevel2EntitlementCategory] [int] NULL,
	[FullLevel3EntitlementCategory] [int] NULL,
	[NotionalNVQLevelv2] [varchar](1) NULL,
	[RegulatedCreditValue] [int] NULL,
	[UnitType] [varchar](50) NULL,
	[Section96ReviewDate] [date] NULL,
	[Section96ApprovalStatus] [int] NULL,
	[FrameworkCommonComponent] [int] NULL,
	[BasicSkillsParticipation] [int] NULL,
	[BasicSkillsType] [int] NULL,
	[FullLevel2Percent] [decimal](5, 2) NULL,
	[FullLevel3Percent] [decimal](5, 2) NULL,
	[MI_FullLevel3Percent] [decimal](5, 2) NULL,
	[NotionalNVQLevel] [varchar](1) NULL,
	[CreditBasedFwkType] [int] NULL,
	[EnglandFEHEStatus] [varchar](1) NULL,
	[LearnAimRefTitle] [varchar](254) NULL,
	[SectorSubjectAreaTier2] [decimal](5, 2) NULL,
	[EnglPrscID] [int] NULL,
	[EffectiveFrom] [date] NULL,
	[EffectiveTo] [date] NULL,
	[SfaApprovalStatus] [int] NULL
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Reference].[Lot](
	[CalcMethod] [int] NULL,
	[LotReference] [varchar](100) NOT NULL,
	[TenderSpecificationReference] [varchar](100) NOT NULL
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Reference].[OLASSEASResult](
	[PaymentType] [nvarchar](250) NULL,
	[PaymentValue] [decimal](12, 2) NULL
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Reference].[ONS_Postcode](
	[doterm] [varchar](6) NULL,
	[EffectiveFrom] [date] NOT NULL,
	[EffectiveTo] [date] NULL,
	[lep1] [varchar](9) NULL,
	[lep2] [varchar](9) NULL,
	[oslaua] [varchar](9) NULL,
	[pcds] [varchar](8) NOT NULL
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Reference].[Org_Current_Version](
	[CurrentVersion] [varchar](100) NULL
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Reference].[Org_Details](
	[LegalOrgType] [varchar](50) NULL,
	[ThirdSector] [int] NULL,
	[UKPRN] [bigint] NOT NULL
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Reference].[Org_Funding](
	[EffectiveFrom] [date] NOT NULL,
	[EffectiveTo] [date] NULL,
	[FundingFactor] [varchar](250) NOT NULL,
	[FundingFactorType] [varchar](100) NOT NULL,
	[FundingFactorValue] [varchar](250) NOT NULL,
	[UKPRN] [bigint] NOT NULL
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Reference].[Org_HMPP_PostCode](
	[EffectiveFrom] [date] NOT NULL,
	[HMPPPostCode] [varchar](15) NOT NULL,
	[UKPRN] [bigint] NOT NULL
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Reference].[Org_PartnerUKPRN](
	[UKPRN] [bigint] NOT NULL
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Reference].[Org_PMUKPRN](
	[UKPRN] [bigint] NOT NULL
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Reference].[PFR_EAS](
	[Ukprn] [nvarchar](10) NULL,
	[PaymentName] [nvarchar](250) NULL,
	[PaymentValue] [decimal](12, 2) NULL,
	[CollectionPeriod] [int] NULL
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Reference].[PFR_OLASS](
	[Ukprn] [nvarchar](10) NULL,
	[PaymentName] [nvarchar](250) NULL,
	[PaymentValue] [decimal](12, 2) NULL,
	[CollectionPeriod] [int] NULL
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Reference].[Postcodes](
	[Postcode] [nvarchar](8) NOT NULL
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Reference].[SFA_PostcodeAreaCost](
	[AreaCostFactor] [decimal](10, 5) NOT NULL,
	[EffectiveFrom] [date] NOT NULL,
	[EffectiveTo] [date] NULL,
	[Postcode] [varchar](10) NOT NULL
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Reference].[SFA_PostcodeAreaCost_Current_Version](
	[CurrentVersion] [varchar](100) NULL
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Reference].[SFA_PostcodeDisadvantage](
	[EffectiveFrom] [date] NOT NULL,
	[EffectiveTo] [date] NULL,
	[Postcode] [varchar](10) NOT NULL,
	[Uplift] [decimal](10, 5) NOT NULL,
	[Apprenticeship_Uplift] [decimal](7, 2) NULL
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Reference].[UniqueLearnerNumbers](
	[ULN] [bigint] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ULN] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Reference].[Version](
	[LARSVersion] [varchar](100) NOT NULL,
	[CCMVersion] [varchar](100) NOT NULL,
	[OrgVersion] [varchar](100) NOT NULL,
	[PostcodeAreaCostVersion] [varchar](100) NOT NULL,
	[EmployerVersion] [varchar](100) NOT NULL
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Reference].[vw_ContractAllocation](
	[contractAllocationNumber] [nvarchar](100) NULL,
	[startDate] [nvarchar](100) NULL
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Reference].[vw_ContractDescription](
	[contractAllocationNumber] [nvarchar](100) NULL,
	[contractEndDate] [nvarchar](100) NULL,
	[contractStartDate] [nvarchar](100) NULL,
	[deliverableCode] [int] NULL,
	[fundingStreamPeriodCode] [nvarchar](100) NULL,
	[learningRatePremiumFactor] [decimal](13, 2) NULL,
	[unitCost] [decimal](13, 2) NULL
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Reference].[vw_ContractValidation](
	[contractAllocationNumber] [nvarchar](100) NULL,
	[fundingStreamPeriodCode] [nvarchar](100) NULL,
	[startDate] [nvarchar](100) NULL,
	[UKPRN] [int] NULL
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Valid].[AppFinRecord](
	[LearnRefNumber] [varchar](12) NOT NULL,
	[AimSeqNumber] [int] NOT NULL,
	[AFinType] [varchar](3) NOT NULL,
	[AFinCode] [int] NOT NULL,
	[AFinDate] [date] NOT NULL,
	[AFinAmount] [int] NOT NULL
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Valid].[CollectionDetails](
	[Collection] [varchar](3) NOT NULL,
	[Year] [varchar](4) NOT NULL,
	[FilePreparationDate] [date] NULL
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Valid].[ContactPreference](
	[LearnRefNumber] [varchar](12) NOT NULL,
	[ContPrefType] [varchar](3) NOT NULL,
	[ContPrefCode] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[LearnRefNumber] ASC,
	[ContPrefType] ASC,
	[ContPrefCode] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Valid].[DPOutcome](
	[LearnRefNumber] [varchar](12) NOT NULL,
	[OutType] [varchar](3) NOT NULL,
	[OutCode] [int] NOT NULL,
	[OutStartDate] [date] NOT NULL,
	[OutEndDate] [date] NULL,
	[OutCollDate] [date] NOT NULL
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Valid].[EmploymentStatusMonitoring](
	[LearnRefNumber] [varchar](12) NOT NULL,
	[DateEmpStatApp] [date] NOT NULL,
	[ESMType] [varchar](3) NOT NULL,
	[ESMCode] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[LearnRefNumber] ASC,
	[DateEmpStatApp] ASC,
	[ESMType] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Valid].[Learner](
	[LearnRefNumber] [varchar](12) NOT NULL,
	[PrevLearnRefNumber] [varchar](12) NULL,
	[PrevUKPRN] [int] NULL,
	[PMUKPRN] [int] NULL,
	[CampId] [varchar](8) NULL,
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
	[Email] [varchar](1000) NULL,
	[OTJHours] [bigint] NULL,
PRIMARY KEY CLUSTERED 
(
	[LearnRefNumber] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Valid].[LearnerDestinationandProgression](
	[LearnRefNumber] [varchar](12) NOT NULL,
	[ULN] [bigint] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[LearnRefNumber] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Valid].[LearnerEmploymentStatus](
	[LearnRefNumber] [varchar](12) NOT NULL,
	[EmpStat] [int] NULL,
	[DateEmpStatApp] [date] NOT NULL,
	[EmpId] [bigint] NULL,
	[AgreeId] [varchar](6) NULL,
PRIMARY KEY CLUSTERED 
(
	[LearnRefNumber] ASC,
	[DateEmpStatApp] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Valid].[LearnerEmploymentStatusDenormTbl](
	[LearnRefNumber] [varchar](12) NOT NULL,
	[EmpStat] [int] NULL,
	[EmpId] [int] NULL,
	[DateEmpStatApp] [date] NOT NULL,
	[ESMCode_BSI] [int] NULL,
	[ESMCode_EII] [int] NULL,
	[ESMCode_LOE] [int] NULL,
	[ESMCode_LOU] [int] NULL,
	[ESMCode_PEI] [int] NULL,
	[ESMCode_SEI] [int] NULL,
	[ESMCode_SEM] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[LearnRefNumber] ASC,
	[DateEmpStatApp] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Valid].[LearnerFAM](
	[LearnRefNumber] [varchar](12) NOT NULL,
	[LearnFAMType] [varchar](3) NULL,
	[LearnFAMCode] [int] NOT NULL
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Valid].[LearnerHE](
	[LearnRefNumber] [varchar](12) NOT NULL,
	[UCASPERID] [varchar](10) NULL,
	[TTACCOM] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[LearnRefNumber] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Valid].[LearnerHEFinancialSupport](
	[LearnRefNumber] [varchar](12) NOT NULL,
	[FINTYPE] [int] NOT NULL,
	[FINAMOUNT] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[LearnRefNumber] ASC,
	[FINTYPE] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Valid].[LearningDelivery](
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
	[LearnRefNumber] ASC,
	[AimSeqNumber] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Valid].[LearningDeliveryDenormTbl](
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
	[HEM1] [varchar](5) NULL,
	[HEM2] [varchar](5) NULL,
	[HEM3] [varchar](5) NULL,
	[HHS1] [varchar](5) NULL,
	[HHS2] [varchar](5) NULL,
	[LDFAM_SOF] [varchar](5) NULL,
	[LDFAM_EEF] [varchar](5) NULL,
	[LDFAM_RES] [varchar](5) NULL,
	[LDFAM_ADL] [varchar](5) NULL,
	[LDFAM_FFI] [varchar](5) NULL,
	[LDFAM_WPP] [varchar](5) NULL,
	[LDFAM_POD] [varchar](5) NULL,
	[LDFAM_ASL] [varchar](5) NULL,
	[LDFAM_FLN] [varchar](5) NULL,
	[LDFAM_NSA] [varchar](5) NULL,
	[ProvSpecDelMon_A] [varchar](20) NULL,
	[ProvSpecDelMon_B] [varchar](20) NULL,
	[ProvSpecDelMon_C] [varchar](20) NULL,
	[ProvSpecDelMon_D] [varchar](20) NULL,
	[LDM1] [varchar](5) NULL,
	[LDM2] [varchar](5) NULL,
	[LDM3] [varchar](5) NULL,
	[LDM4] [varchar](5) NULL,
PRIMARY KEY CLUSTERED 
(
	[LearnRefNumber] ASC,
	[AimSeqNumber] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Valid].[LearningDeliveryFAM](
	[LearnRefNumber] [varchar](12) NOT NULL,
	[AimSeqNumber] [int] NOT NULL,
	[LearnDelFAMType] [varchar](3) NOT NULL,
	[LearnDelFAMCode] [varchar](5) NOT NULL,
	[LearnDelFAMDateFrom] [date] NULL,
	[LearnDelFAMDateTo] [date] NULL
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Valid].[LearningDeliveryHE](
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
	[LearnRefNumber] ASC,
	[AimSeqNumber] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Valid].[LearningDeliveryWorkPlacement](
	[LearnRefNumber] [varchar](12) NOT NULL,
	[AimSeqNumber] [int] NOT NULL,
	[WorkPlaceStartDate] [date] NOT NULL,
	[WorkPlaceEndDate] [date] NULL,
	[WorkPlaceHours] [int] NULL,
	[WorkPlaceMode] [int] NOT NULL,
	[WorkPlaceEmpId] [int] NULL
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Valid].[LearningProvider](
	[UKPRN] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[UKPRN] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Valid].[LLDDandHealthProblem](
	[LearnRefNumber] [varchar](12) NOT NULL,
	[LLDDCat] [int] NOT NULL,
	[PrimaryLLDD] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[LearnRefNumber] ASC,
	[LLDDCat] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Valid].[ProviderSpecDeliveryMonitoring](
	[LearnRefNumber] [varchar](12) NOT NULL,
	[AimSeqNumber] [int] NOT NULL,
	[ProvSpecDelMonOccur] [varchar](1) NOT NULL,
	[ProvSpecDelMon] [varchar](20) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[LearnRefNumber] ASC,
	[AimSeqNumber] ASC,
	[ProvSpecDelMonOccur] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Valid].[ProviderSpecLearnerMonitoring](
	[LearnRefNumber] [varchar](12) NOT NULL,
	[ProvSpecLearnMonOccur] [varchar](1) NOT NULL,
	[ProvSpecLearnMon] [varchar](20) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[LearnRefNumber] ASC,
	[ProvSpecLearnMonOccur] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
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
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Valid].[SourceFile](
	[SourceFileName] [varchar](50) NOT NULL,
	[FilePreparationDate] [date] NULL,
	[SoftwareSupplier] [varchar](40) NULL,
	[SoftwarePackage] [varchar](30) NULL,
	[Release] [varchar](20) NULL,
	[SerialNo] [varchar](2) NULL,
	[DateTime] [datetime] NULL
) ON [PRIMARY]
GO
