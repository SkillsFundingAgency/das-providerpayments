/****** Object:  Schema [Input]    Script Date: 19/07/2016 14:55:32 ******/
IF NOT EXISTS (SELECT * FROM sys.schemas WHERE name = N'Input')
EXEC sys.sp_executesql N'CREATE SCHEMA [Input]'
GO
/****** Object:  Schema [Invalid]    Script Date: 19/07/2016 14:55:32 ******/
IF NOT EXISTS (SELECT * FROM sys.schemas WHERE name = N'Invalid')
EXEC sys.sp_executesql N'CREATE SCHEMA [Invalid]'
GO
/****** Object:  Schema [Valid]    Script Date: 19/07/2016 14:55:32 ******/
IF NOT EXISTS (SELECT * FROM sys.schemas WHERE name = N'Valid')
EXEC sys.sp_executesql N'CREATE SCHEMA [Valid]'
GO
/****** Object:  Table [Input].[CollectionDetails]    Script Date: 19/07/2016 14:53:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Input].[CollectionDetails]') AND type in (N'U'))
BEGIN
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
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [Input].[ContactPreference]    Script Date: 19/07/2016 14:53:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Input].[ContactPreference]') AND type in (N'U'))
BEGIN
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
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [Input].[DPOutcome]    Script Date: 19/07/2016 14:53:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Input].[DPOutcome]') AND type in (N'U'))
BEGIN
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
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [Input].[EmploymentStatusMonitoring]    Script Date: 19/07/2016 14:53:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Input].[EmploymentStatusMonitoring]') AND type in (N'U'))
BEGIN
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
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [Input].[Learner]    Script Date: 19/07/2016 14:53:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Input].[Learner]') AND type in (N'U'))
BEGIN
CREATE TABLE [Input].[Learner](
	[Learner_Id] [int] NOT NULL,
	[LearnRefNumber] [varchar](100) NULL,
	[PrevLearnRefNumber] [varchar](1000) NULL,
	[PrevUKPRN] [bigint] NULL,
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
PRIMARY KEY CLUSTERED 
(
	[Learner_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [Input].[LearnerContact]    Script Date: 19/07/2016 14:53:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Input].[LearnerContact]') AND type in (N'U'))
BEGIN
CREATE TABLE [Input].[LearnerContact](
	[LearnerContact_Id] [int] NOT NULL,
	[Learner_Id] [int] NOT NULL,
	[LearnRefNumber] [varchar](100) NULL,
	[LocType] [bigint] NULL,
	[ContType] [bigint] NULL,
	[PostCode] [varchar](1000) NULL,
	[TelNumber] [varchar](1000) NULL,
	[Email] [varchar](1000) NULL,
PRIMARY KEY CLUSTERED 
(
	[LearnerContact_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [Input].[LearnerDestinationandProgression]    Script Date: 19/07/2016 14:53:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Input].[LearnerDestinationandProgression]') AND type in (N'U'))
BEGIN
CREATE TABLE [Input].[LearnerDestinationandProgression](
	[LearnerDestinationandProgression_Id] [int] NOT NULL,
	[LearnRefNumber] [varchar](100) NULL,
	[ULN] [bigint] NULL,
PRIMARY KEY CLUSTERED 
(
	[LearnerDestinationandProgression_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [Input].[LearnerEmploymentStatus]    Script Date: 19/07/2016 14:53:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Input].[LearnerEmploymentStatus]') AND type in (N'U'))
BEGIN
CREATE TABLE [Input].[LearnerEmploymentStatus](
	[LearnerEmploymentStatus_Id] [int] NOT NULL,
	[Learner_Id] [int] NOT NULL,
	[LearnRefNumber] [varchar](100) NULL,
	[EmpStat] [bigint] NULL,
	[DateEmpStatApp] [date] NULL,
	[EmpId] [bigint] NULL,
PRIMARY KEY CLUSTERED 
(
	[LearnerEmploymentStatus_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [Input].[LearnerFAM]    Script Date: 19/07/2016 14:53:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Input].[LearnerFAM]') AND type in (N'U'))
BEGIN
CREATE TABLE [Input].[LearnerFAM](
	[LearnerFAM_Id] [int] NOT NULL,
	[Learner_Id] [int] NOT NULL,
	[LearnRefNumber] [varchar](100) NULL,
	[LearnFAMType] [varchar](100) NULL,
	[LearnFAMCode] [bigint] NULL,
PRIMARY KEY CLUSTERED 
(
	[LearnerFAM_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [Input].[LearnerHE]    Script Date: 19/07/2016 14:53:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Input].[LearnerHE]') AND type in (N'U'))
BEGIN
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
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [Input].[LearnerHEFinancialSupport]    Script Date: 19/07/2016 14:53:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Input].[LearnerHEFinancialSupport]') AND type in (N'U'))
BEGIN
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
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [Input].[LearningDelivery]    Script Date: 19/07/2016 14:53:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Input].[LearningDelivery]') AND type in (N'U'))
BEGIN
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
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [Input].[LearningDeliveryFAM]    Script Date: 19/07/2016 14:53:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Input].[LearningDeliveryFAM]') AND type in (N'U'))
BEGIN
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
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [Input].[LearningDeliveryHE]    Script Date: 19/07/2016 14:53:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Input].[LearningDeliveryHE]') AND type in (N'U'))
BEGIN
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
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [Input].[LearningDeliveryWorkPlacement]    Script Date: 19/07/2016 14:53:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Input].[LearningDeliveryWorkPlacement]') AND type in (N'U'))
BEGIN
CREATE TABLE [Input].[LearningDeliveryWorkPlacement](
	[LearningDeliveryWorkPlacement_Id] [int] NOT NULL,
	[LearningDelivery_Id] [int] NOT NULL,
	[LearnRefNumber] [varchar](100) NULL,
	[AimSeqNumber] [bigint] NULL,
	[WorkPlaceStartDate] [date] NULL,
	[WorkPlaceEndDate] [date] NULL,
	[WorkPlaceMode] [bigint] NULL,
	[WorkPlaceEmpId] [bigint] NULL,
PRIMARY KEY CLUSTERED 
(
	[LearningDeliveryWorkPlacement_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [Input].[LearningProvider]    Script Date: 19/07/2016 14:53:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Input].[LearningProvider]') AND type in (N'U'))
BEGIN
CREATE TABLE [Input].[LearningProvider](
	[LearningProvider_Id] [int] NOT NULL,
	[UKPRN] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[LearningProvider_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [Input].[LLDDandHealthProblem]    Script Date: 19/07/2016 14:53:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Input].[LLDDandHealthProblem]') AND type in (N'U'))
BEGIN
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
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [Input].[PostAdd]    Script Date: 19/07/2016 14:53:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Input].[PostAdd]') AND type in (N'U'))
BEGIN
CREATE TABLE [Input].[PostAdd](
	[PostAdd_Id] [int] NOT NULL,
	[LearnerContact_Id] [int] NOT NULL,
	[LearnRefNumber] [varchar](100) NULL,
	[ContType] [bigint] NULL,
	[LocType] [bigint] NULL,
	[AddLine1] [varchar](1000) NULL,
	[AddLine2] [varchar](1000) NULL,
	[AddLine3] [varchar](1000) NULL,
	[AddLine4] [varchar](1000) NULL,
PRIMARY KEY CLUSTERED 
(
	[PostAdd_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [Input].[ProviderSpecDeliveryMonitoring]    Script Date: 19/07/2016 14:53:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Input].[ProviderSpecDeliveryMonitoring]') AND type in (N'U'))
BEGIN
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
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [Input].[ProviderSpecLearnerMonitoring]    Script Date: 19/07/2016 14:53:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Input].[ProviderSpecLearnerMonitoring]') AND type in (N'U'))
BEGIN
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
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [Input].[Source]    Script Date: 19/07/2016 14:53:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Input].[Source]') AND type in (N'U'))
BEGIN
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
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [Input].[SourceFile]    Script Date: 19/07/2016 14:53:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Input].[SourceFile]') AND type in (N'U'))
BEGIN
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
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [Input].[TrailblazerApprenticeshipFinancialRecord]    Script Date: 19/07/2016 14:53:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Input].[TrailblazerApprenticeshipFinancialRecord]') AND type in (N'U'))
BEGIN
CREATE TABLE [Input].[TrailblazerApprenticeshipFinancialRecord](
	[TrailblazerApprenticeshipFinancialRecord_Id] [int] NOT NULL,
	[LearningDelivery_Id] [int] NOT NULL,
	[LearnRefNumber] [varchar](100) NULL,
	[AimSeqNumber] [bigint] NULL,
	[TBFinType] [varchar](100) NULL,
	[TBFinCode] [bigint] NULL,
	[TBFinDate] [date] NULL,
	[TBFinAmount] [bigint] NULL,
PRIMARY KEY CLUSTERED 
(
	[TrailblazerApprenticeshipFinancialRecord_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [Invalid].[CollectionDetails]    Script Date: 19/07/2016 14:53:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Invalid].[CollectionDetails]') AND type in (N'U'))
BEGIN
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
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [Invalid].[ContactPreference]    Script Date: 19/07/2016 14:53:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Invalid].[ContactPreference]') AND type in (N'U'))
BEGIN
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
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [Invalid].[DPOutcome]    Script Date: 19/07/2016 14:53:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Invalid].[DPOutcome]') AND type in (N'U'))
BEGIN
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
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [Invalid].[EmploymentStatusMonitoring]    Script Date: 19/07/2016 14:53:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Invalid].[EmploymentStatusMonitoring]') AND type in (N'U'))
BEGIN
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
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [Invalid].[Learner]    Script Date: 19/07/2016 14:53:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Invalid].[Learner]') AND type in (N'U'))
BEGIN
CREATE TABLE [Invalid].[Learner](
	[Learner_Id] [int] NOT NULL,
	[LearnRefNumber] [varchar](100) NULL,
	[PrevLearnRefNumber] [varchar](1000) NULL,
	[PrevUKPRN] [bigint] NULL,
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
PRIMARY KEY CLUSTERED 
(
	[Learner_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [Invalid].[LearnerContact]    Script Date: 19/07/2016 14:53:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Invalid].[LearnerContact]') AND type in (N'U'))
BEGIN
CREATE TABLE [Invalid].[LearnerContact](
	[LearnerContact_Id] [int] NOT NULL,
	[Learner_Id] [int] NOT NULL,
	[LearnRefNumber] [varchar](100) NULL,
	[LocType] [bigint] NULL,
	[ContType] [bigint] NULL,
	[PostCode] [varchar](1000) NULL,
	[TelNumber] [varchar](1000) NULL,
	[Email] [varchar](1000) NULL,
PRIMARY KEY CLUSTERED 
(
	[LearnerContact_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [Invalid].[LearnerDestinationandProgression]    Script Date: 19/07/2016 14:53:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Invalid].[LearnerDestinationandProgression]') AND type in (N'U'))
BEGIN
CREATE TABLE [Invalid].[LearnerDestinationandProgression](
	[LearnerDestinationandProgression_Id] [int] NOT NULL,
	[LearnRefNumber] [varchar](100) NULL,
	[ULN] [bigint] NULL,
PRIMARY KEY CLUSTERED 
(
	[LearnerDestinationandProgression_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [Invalid].[LearnerEmploymentStatus]    Script Date: 19/07/2016 14:53:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Invalid].[LearnerEmploymentStatus]') AND type in (N'U'))
BEGIN
CREATE TABLE [Invalid].[LearnerEmploymentStatus](
	[LearnerEmploymentStatus_Id] [int] NOT NULL,
	[Learner_Id] [int] NOT NULL,
	[LearnRefNumber] [varchar](100) NULL,
	[EmpStat] [bigint] NULL,
	[DateEmpStatApp] [date] NULL,
	[EmpId] [bigint] NULL,
PRIMARY KEY CLUSTERED 
(
	[LearnerEmploymentStatus_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [Invalid].[LearnerFAM]    Script Date: 19/07/2016 14:53:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Invalid].[LearnerFAM]') AND type in (N'U'))
BEGIN
CREATE TABLE [Invalid].[LearnerFAM](
	[LearnerFAM_Id] [int] NOT NULL,
	[Learner_Id] [int] NOT NULL,
	[LearnRefNumber] [varchar](100) NULL,
	[LearnFAMType] [varchar](100) NULL,
	[LearnFAMCode] [bigint] NULL,
PRIMARY KEY CLUSTERED 
(
	[LearnerFAM_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [Invalid].[LearnerHE]    Script Date: 19/07/2016 14:53:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Invalid].[LearnerHE]') AND type in (N'U'))
BEGIN
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
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [Invalid].[LearnerHEFinancialSupport]    Script Date: 19/07/2016 14:53:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Invalid].[LearnerHEFinancialSupport]') AND type in (N'U'))
BEGIN
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
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [Invalid].[LearningDelivery]    Script Date: 19/07/2016 14:53:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Invalid].[LearningDelivery]') AND type in (N'U'))
BEGIN
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
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [Invalid].[LearningDeliveryFAM]    Script Date: 19/07/2016 14:53:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Invalid].[LearningDeliveryFAM]') AND type in (N'U'))
BEGIN
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
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [Invalid].[LearningDeliveryHE]    Script Date: 19/07/2016 14:53:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Invalid].[LearningDeliveryHE]') AND type in (N'U'))
BEGIN
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
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [Invalid].[LearningDeliveryWorkPlacement]    Script Date: 19/07/2016 14:53:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Invalid].[LearningDeliveryWorkPlacement]') AND type in (N'U'))
BEGIN
CREATE TABLE [Invalid].[LearningDeliveryWorkPlacement](
	[LearningDeliveryWorkPlacement_Id] [int] NOT NULL,
	[LearningDelivery_Id] [int] NOT NULL,
	[LearnRefNumber] [varchar](100) NULL,
	[AimSeqNumber] [bigint] NULL,
	[WorkPlaceStartDate] [date] NULL,
	[WorkPlaceEndDate] [date] NULL,
	[WorkPlaceMode] [bigint] NULL,
	[WorkPlaceEmpId] [bigint] NULL,
PRIMARY KEY CLUSTERED 
(
	[LearningDeliveryWorkPlacement_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [Invalid].[LearningProvider]    Script Date: 19/07/2016 14:53:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Invalid].[LearningProvider]') AND type in (N'U'))
BEGIN
CREATE TABLE [Invalid].[LearningProvider](
	[LearningProvider_Id] [int] NOT NULL,
	[UKPRN] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[LearningProvider_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [Invalid].[LLDDandHealthProblem]    Script Date: 19/07/2016 14:53:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Invalid].[LLDDandHealthProblem]') AND type in (N'U'))
BEGIN
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
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [Invalid].[PostAdd]    Script Date: 19/07/2016 14:53:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Invalid].[PostAdd]') AND type in (N'U'))
BEGIN
CREATE TABLE [Invalid].[PostAdd](
	[PostAdd_Id] [int] NOT NULL,
	[LearnerContact_Id] [int] NOT NULL,
	[LearnRefNumber] [varchar](100) NULL,
	[ContType] [bigint] NULL,
	[LocType] [bigint] NULL,
	[AddLine1] [varchar](1000) NULL,
	[AddLine2] [varchar](1000) NULL,
	[AddLine3] [varchar](1000) NULL,
	[AddLine4] [varchar](1000) NULL,
PRIMARY KEY CLUSTERED 
(
	[PostAdd_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [Invalid].[ProviderSpecDeliveryMonitoring]    Script Date: 19/07/2016 14:53:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Invalid].[ProviderSpecDeliveryMonitoring]') AND type in (N'U'))
BEGIN
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
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [Invalid].[ProviderSpecLearnerMonitoring]    Script Date: 19/07/2016 14:53:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Invalid].[ProviderSpecLearnerMonitoring]') AND type in (N'U'))
BEGIN
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
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [Invalid].[Source]    Script Date: 19/07/2016 14:53:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Invalid].[Source]') AND type in (N'U'))
BEGIN
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
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [Invalid].[SourceFile]    Script Date: 19/07/2016 14:53:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Invalid].[SourceFile]') AND type in (N'U'))
BEGIN
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
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [Invalid].[TrailblazerApprenticeshipFinancialRecord]    Script Date: 19/07/2016 14:53:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Invalid].[TrailblazerApprenticeshipFinancialRecord]') AND type in (N'U'))
BEGIN
CREATE TABLE [Invalid].[TrailblazerApprenticeshipFinancialRecord](
	[TrailblazerApprenticeshipFinancialRecord_Id] [int] NOT NULL,
	[LearningDelivery_Id] [int] NOT NULL,
	[LearnRefNumber] [varchar](100) NULL,
	[AimSeqNumber] [bigint] NULL,
	[TBFinType] [varchar](100) NULL,
	[TBFinCode] [bigint] NULL,
	[TBFinDate] [date] NULL,
	[TBFinAmount] [bigint] NULL,
PRIMARY KEY CLUSTERED 
(
	[TrailblazerApprenticeshipFinancialRecord_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [Valid].[CollectionDetails]    Script Date: 19/07/2016 14:53:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Valid].[CollectionDetails]') AND type in (N'U'))
BEGIN
CREATE TABLE [Valid].[CollectionDetails](
	[Collection] [varchar](3) NOT NULL,
	[Year] [varchar](4) NOT NULL,
	[FilePreparationDate] [date] NOT NULL
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [Valid].[ContactPreference]    Script Date: 19/07/2016 14:53:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Valid].[ContactPreference]') AND type in (N'U'))
BEGIN
CREATE TABLE [Valid].[ContactPreference](
	[LearnRefNumber] [varchar](12) NOT NULL,
	[ContPrefType] [varchar](3) NOT NULL,
	[ContPrefCode] [int] NULL
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [Valid].[DPOutcome]    Script Date: 19/07/2016 14:53:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Valid].[DPOutcome]') AND type in (N'U'))
BEGIN
CREATE TABLE [Valid].[DPOutcome](
	[LearnRefNumber] [varchar](12) NOT NULL,
	[OutType] [varchar](3) NOT NULL,
	[OutCode] [int] NULL,
	[OutStartDate] [date] NOT NULL,
	[OutEndDate] [date] NULL,
	[OutCollDate] [date] NOT NULL
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [Valid].[Learner]    Script Date: 19/07/2016 14:53:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Valid].[Learner]') AND type in (N'U'))
BEGIN
CREATE TABLE [Valid].[Learner](
	[LearnRefNumber] [varchar](12) NOT NULL,
	[PrevLearnRefNumber] [varchar](12) NULL,
	[PrevUKPRN] [int] NULL,
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
	[HomePostcode] [varchar](8) NULL,
	[CurrentPostcode] [varchar](8) NULL,
	[LrnFAM_DLA] [int] NULL,
	[LrnFAM_ECF] [int] NULL,
	[LrnFAM_EDF1] [int] NULL,
	[LrnFAM_EDF2] [int] NULL,
	[LrnFAM_EHC] [int] NULL,
	[LrnFAM_FME] [int] NULL,
	[LrnFAM_HNS] [int] NULL,
	[LrnFAM_LDA] [int] NULL,
	[LrnFAM_LSR1] [int] NULL,
	[LrnFAM_LSR2] [int] NULL,
	[LrnFAM_LSR3] [int] NULL,
	[LrnFAM_LSR4] [int] NULL,
	[LrnFAM_MCF] [int] NULL,
	[LrnFAM_NLM1] [int] NULL,
	[LrnFAM_NLM2] [int] NULL,
	[LrnFAM_PPE1] [int] NULL,
	[LrnFAM_PPE2] [int] NULL,
	[LrnFAM_SEN] [int] NULL,
	[ProvSpecMon_A] [varchar](20) NULL,
	[ProvSpecMon_B] [varchar](20) NULL,
PRIMARY KEY CLUSTERED 
(
	[LearnRefNumber] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [Valid].[LearnerContact]    Script Date: 19/07/2016 14:53:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Valid].[LearnerContact]') AND type in (N'U'))
BEGIN
CREATE TABLE [Valid].[LearnerContact](
	[LearnRefNumber] [varchar](12) NOT NULL,
	[HomePostcode] [varchar](8) NULL,
	[CurrentPostcode] [varchar](8) NULL,
	[TelNumber] [varchar](18) NULL,
	[Email] [varchar](100) NULL,
	[AddLine1] [varchar](50) NULL,
	[AddLine2] [varchar](50) NULL,
	[AddLine3] [varchar](50) NULL,
	[AddLine4] [varchar](50) NULL,
PRIMARY KEY CLUSTERED 
(
	[LearnRefNumber] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [Valid].[LearnerDestinationandProgression]    Script Date: 19/07/2016 14:53:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Valid].[LearnerDestinationandProgression]') AND type in (N'U'))
BEGIN
CREATE TABLE [Valid].[LearnerDestinationandProgression](
	[LearnRefNumber] [varchar](12) NOT NULL,
	[ULN] [bigint] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[LearnRefNumber] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [Valid].[LearnerEmploymentStatus]    Script Date: 19/07/2016 14:53:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Valid].[LearnerEmploymentStatus]') AND type in (N'U'))
BEGIN
CREATE TABLE [Valid].[LearnerEmploymentStatus](
	[LearnRefNumber] [varchar](12) NOT NULL,
	[EmpStat] [int] NULL,
	[DateEmpStatApp] [date] NOT NULL,
	[EmpId] [int] NULL,
	[EmpStatMon_BSI] [int] NULL,
	[EmpStatMon_EII] [int] NULL,
	[EmpStatMon_LOE] [int] NULL,
	[EmpStatMon_LOU] [int] NULL,
	[EmpStatMon_PEI] [int] NULL,
	[EmpStatMon_SEI] [int] NULL,
	[EmpStatMon_SEM] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[LearnRefNumber] ASC,
	[DateEmpStatApp] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [Valid].[LearnerHE]    Script Date: 19/07/2016 14:53:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Valid].[LearnerHE]') AND type in (N'U'))
BEGIN
CREATE TABLE [Valid].[LearnerHE](
	[LearnRefNumber] [varchar](12) NOT NULL,
	[UCASPERID] [varchar](1000) NULL,
	[TTACCOM] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[LearnRefNumber] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [Valid].[LearnerHEFinancialSupport]    Script Date: 19/07/2016 14:53:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Valid].[LearnerHEFinancialSupport]') AND type in (N'U'))
BEGIN
CREATE TABLE [Valid].[LearnerHEFinancialSupport](
	[LearnRefNumber] [varchar](12) NOT NULL,
	[FINTYPE] [int] NOT NULL,
	[FINAMOUNT] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[LearnRefNumber] ASC,
	[FINTYPE] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [Valid].[LearningDelivery]    Script Date: 19/07/2016 14:53:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Valid].[LearningDelivery]') AND type in (N'U'))
BEGIN
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
	[StdCode] [bigint] NULL,
	[PartnerUKPRN] [int] NULL,
	[DelLocPostCode] [varchar](8) NULL,
	[AddHours] [int] NULL,
	[PriorLearnFundAdj] [int] NULL,
	[OtherFundAdj] [int] NULL,
	[ConRefNumber] [varchar](20) NULL,
	[EmpOutcome] [int] NULL,
	[CompStatus] [int] NULL,
	[LearnActEndDate] [date] NULL,
	[WithdrawReason] [int] NULL,
	[Outcome] [int] NULL,
	[AchDate] [date] NULL,
	[OutGrade] [varchar](6) NULL,
	[SWSupAimId] [varchar](36) NULL,
	[LrnDelFAM_ADL] [varchar](5) NULL,
	[LrnDelFAM_ASL] [varchar](5) NULL,
	[LrnDelFAM_EEF] [varchar](5) NULL,
	[LrnDelFAM_FFI] [varchar](5) NULL,
	[LrnDelFAM_FLN] [varchar](5) NULL,
	[LrnDelFAM_HEM1] [varchar](5) NULL,
	[LrnDelFAM_HEM2] [varchar](5) NULL,
	[LrnDelFAM_HEM3] [varchar](5) NULL,
	[LrnDelFAM_HHS1] [varchar](5) NULL,
	[LrnDelFAM_HHS2] [varchar](5) NULL,
	[LrnDelFAM_LDM1] [varchar](5) NULL,
	[LrnDelFAM_LDM2] [varchar](5) NULL,
	[LrnDelFAM_LDM3] [varchar](5) NULL,
	[LrnDelFAM_LDM4] [varchar](5) NULL,
	[LrnDelFAM_NSA] [varchar](5) NULL,
	[LrnDelFAM_POD] [varchar](5) NULL,
	[LrnDelFAM_RES] [varchar](5) NULL,
	[LrnDelFAM_SOF] [varchar](5) NULL,
	[LrnDelFAM_SPP] [varchar](5) NULL,
	[LrnDelFAM_WPP] [varchar](5) NULL,
	[ProvSpecMon_A] [varchar](20) NULL,
	[ProvSpecMon_B] [varchar](20) NULL,
	[ProvSpecMon_C] [varchar](20) NULL,
	[ProvSpecMon_D] [varchar](20) NULL,
PRIMARY KEY CLUSTERED 
(
	[LearnRefNumber] ASC,
	[AimSeqNumber] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [Valid].[LearningDeliveryFAM]    Script Date: 19/07/2016 14:53:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Valid].[LearningDeliveryFAM]') AND type in (N'U'))
BEGIN
CREATE TABLE [Valid].[LearningDeliveryFAM](
	[LearnRefNumber] [varchar](12) NOT NULL,
	[AimSeqNumber] [int] NOT NULL,
	[LearnDelFAMType] [varchar](3) NULL,
	[LearnDelFAMCode] [varchar](5) NULL,
	[LearnDelFAMDateFrom] [date] NULL,
	[LearnDelFAMDateTo] [date] NULL
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [Valid].[LearningDeliveryHE]    Script Date: 19/07/2016 14:53:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Valid].[LearningDeliveryHE]') AND type in (N'U'))
BEGIN
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
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [Valid].[LearningDeliveryWorkPlacement]    Script Date: 19/07/2016 14:53:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Valid].[LearningDeliveryWorkPlacement]') AND type in (N'U'))
BEGIN
CREATE TABLE [Valid].[LearningDeliveryWorkPlacement](
	[LearnRefNumber] [varchar](12) NOT NULL,
	[AimSeqNumber] [int] NOT NULL,
	[WorkPlaceStartDate] [date] NOT NULL,
	[WorkPlaceEndDate] [date] NULL,
	[WorkPlaceMode] [int] NOT NULL,
	[WorkPlaceEmpId] [int] NULL
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [Valid].[LearningProvider]    Script Date: 19/07/2016 14:53:27 ******/
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
/****** Object:  Table [Valid].[LLDDandHealthProblem]    Script Date: 19/07/2016 14:53:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Valid].[LLDDandHealthProblem]') AND type in (N'U'))
BEGIN
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
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [Valid].[Source]    Script Date: 19/07/2016 14:53:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Valid].[Source]') AND type in (N'U'))
BEGIN
CREATE TABLE [Valid].[Source](
	[ProtectiveMarking] [varchar](30) NOT NULL,
	[UKPRN] [int] NOT NULL,
	[SoftwareSupplier] [varchar](40) NULL,
	[SoftwarePackage] [varchar](30) NULL,
	[Release] [varchar](20) NULL,
	[SerialNo] [varchar](2) NOT NULL,
	[DateTime] [datetime] NOT NULL,
	[ReferenceData] [varchar](100) NULL,
	[ComponentSetVersion] [varchar](20) NULL
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [Valid].[SourceFile]    Script Date: 19/07/2016 14:53:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Valid].[SourceFile]') AND type in (N'U'))
BEGIN
CREATE TABLE [Valid].[SourceFile](
	[SourceFileName] [varchar](50) NOT NULL,
	[FilePreparationDate] [date] NULL,
	[SoftwareSupplier] [varchar](40) NULL,
	[SoftwarePackage] [varchar](30) NULL,
	[Release] [varchar](20) NULL,
	[SerialNo] [varchar](2) NOT NULL,
	[DateTime] [datetime] NULL
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [Valid].[TrailblazerApprenticeshipFinancialRecord]    Script Date: 19/07/2016 14:53:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Valid].[TrailblazerApprenticeshipFinancialRecord]') AND type in (N'U'))
BEGIN
CREATE TABLE [Valid].[TrailblazerApprenticeshipFinancialRecord](
	[LearnRefNumber] [varchar](12) NOT NULL,
	[AimSeqNumber] [int] NOT NULL,
	[TBFinType] [varchar](3) NOT NULL,
	[TBFinCode] [int] NULL,
	[TBFinDate] [date] NULL,
	[TBFinAmount] [int] NOT NULL
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
