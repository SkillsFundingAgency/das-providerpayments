IF NOT EXISTS(SELECT [schema_id] FROM sys.schemas WHERE [name]='Valid')
BEGIN
	EXEC('CREATE SCHEMA Valid')
END
GO

-----------------------------------------------------------------------------------------------------------------------------------------------
-- LearningProvider
-----------------------------------------------------------------------------------------------------------------------------------------------
IF EXISTS(SELECT [object_id] FROM sys.tables WHERE [name]='LearningProvider' AND [schema_id] = SCHEMA_ID('Valid'))
BEGIN
	DROP TABLE Valid.LearningProvider
END
GO

create table [Valid].[LearningProvider] (
	[UKPRN] [int] NOT NULL,
	PRIMARY KEY CLUSTERED ([UKPRN] ASC)
)

if object_id('[Valid].[Learner]','u') is not null
	drop table [Valid].[Learner]
GO

create table [Valid].[Learner](
	[UKPRN] [int] NOT NULL,
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
	PRIMARY KEY CLUSTERED ([UKPRN] ASC,	[LearnRefNumber] ASC)
)
GO


if object_id('[Valid].[LearningDelivery]','u') is not null
	drop table [Valid].[LearningDelivery]
GO

create table [Valid].[LearningDelivery](
	[UKPRN] [int] NOT NULL,
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
	PRIMARY KEY CLUSTERED ([UKPRN] ASC, [LearnRefNumber] ASC, [AimSeqNumber] ASC)
)
GO

if object_id('[Valid].[LearningDeliveryFAM]','u') is not null
	drop table [Valid].[LearningDeliveryFAM]
GO

create table [Valid].[LearningDeliveryFAM](
	[UKPRN] [int] NOT NULL,
	[LearnRefNumber] [varchar](12) NOT NULL,
	[AimSeqNumber] [int] NOT NULL,
	[LearnDelFAMType] [varchar](3) NULL,
	[LearnDelFAMCode] [varchar](5) NULL,
	[LearnDelFAMDateFrom] [date] NULL,
	[LearnDelFAMDateTo] [date] NULL
)
GO

create index [IX_Valid_LearningDeliveryFAM] on [Valid].[LearningDeliveryFAM]
	(
		[UKPRN] asc,
		[LearnRefNumber] asc,
		[AimSeqNumber] asc,
		[LearnDelFAMType] asc
	)
GO



IF EXISTS(SELECT [object_id] FROM sys.tables WHERE [name]='FileDetails' AND [schema_id] = SCHEMA_ID('dbo'))
BEGIN
       DROP TABLE [dbo].[FileDetails]
END
GO

CREATE TABLE [dbo].[FileDetails] (
    [ID] [int] IDENTITY(1,1),
    [UKPRN] [int] NOT NULL,
    [Filename] [nvarchar](50) NULL,
    [FileSizeKb] [bigint] NULL,
    [TotalLearnersSubmitted] [int] NULL,
    [TotalValidLearnersSubmitted] [int] NULL,
    [TotalInvalidLearnersSubmitted] [int] NULL,
    [TotalErrorCount] [int] NULL,
    [TotalWarningCount] [int] NULL,
    [SubmittedTime] [datetime] NULL,
    [Success] [bit]
    CONSTRAINT [PK_dbo.FileDetails] UNIQUE ([UKPRN], [Filename], [Success] ASC)
)
GO



if object_id('[Valid].[AppFinRecord]','u') is not null
begin
	drop table [Valid].[AppFinRecord]
end
GO
 
create table [Valid].[AppFinRecord]
(
	[UKPRN] [int] NOT NULL,
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
