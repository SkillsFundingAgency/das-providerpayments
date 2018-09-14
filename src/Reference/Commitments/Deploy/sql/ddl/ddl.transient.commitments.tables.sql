-----------------------------------------------------------------------------------------------------------------------------------------------
-- TaskLog
-----------------------------------------------------------------------------------------------------------------------------------------------
IF EXISTS(SELECT [object_id] FROM sys.tables WHERE [name]='TaskLog' AND [schema_id] = SCHEMA_ID('dbo'))
BEGIN
	DROP TABLE [dbo].[TaskLog]
END
GO

CREATE TABLE [dbo].[TaskLog](
	[TaskLogId] [uniqueidentifier] NOT NULL DEFAULT(NEWID()),
	[DateTime] [datetime] NOT NULL DEFAULT(GETDATE()),
	[Level] [nvarchar](10) NOT NULL,
	[Logger] [nvarchar](512) NOT NULL,
	[Message] [nvarchar](1024) NOT NULL,
	[Exception] [nvarchar](max) NULL
)
GO

-----------------------------------------------------------------------------------------------------------------------------------------------
-- Commitments
-----------------------------------------------------------------------------------------------------------------------------------------------
IF EXISTS(SELECT [object_id] FROM sys.tables WHERE [name]='DasCommitments' AND [schema_id] = SCHEMA_ID('dbo'))
BEGIN
	DROP TABLE [dbo].[DasCommitments]
END
GO

CREATE TABLE [dbo].[DasCommitments](
	[CommitmentId] [bigint] NOT NULL,
	[VersionId] varchar(25) NOT NULL,
	[Uln] [bigint] NOT NULL,
	[Ukprn] [bigint] NOT NULL,
	[AccountId] bigint NOT NULL,
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
	[EffectiveFromDate] [date] NOT NULL,
	[EffectiveToDate] [date] NULL,
	[LegalEntityName] [nvarchar](100) NULL,
	[TransferSendingEmployerAccountId] bigint null,
	[TransferApprovalDate] datetime null,
	[PausedOnDate] datetime null,
	[WithdrawnOnDate] datetime null,
	[AccountLegalEntityPublicHashedId] char(6) null,
	PRIMARY KEY CLUSTERED 
	(
		[CommitmentId] ASC,
		[VersionId] ASC
	)
)

-----------------------------------------------------------------------------------------------------------------------------------------------
-- ProcessError
-----------------------------------------------------------------------------------------------------------------------------------------------
IF EXISTS(SELECT [object_id] FROM sys.tables WHERE [name]='ProcessError' AND [schema_id] = SCHEMA_ID('dbo'))
BEGIN
	DROP TABLE [dbo].[ProcessError]
END
GO

CREATE TABLE [dbo].[ProcessError](
	[EntryId] int NOT NULL IDENTITY(1,1),
	[ErrorDetails] nvarchar(max) NOT NULL,
	PRIMARY KEY CLUSTERED
	(
		[EntryId]
	)
)

-----------------------------------------------------------------------------------------------------------------------------------------------
-- EventStreamPointer
-----------------------------------------------------------------------------------------------------------------------------------------------
IF EXISTS(SELECT [object_id] FROM sys.tables WHERE [name]='EventStreamPointer' AND [schema_id] = SCHEMA_ID('dbo'))
BEGIN
	DROP TABLE [dbo].[EventStreamPointer]
END
GO

CREATE TABLE [dbo].[EventStreamPointer](
	[EventId] [bigint] NOT NULL,
	[ReadDate] [datetime] NOT NULL
)


-----------------------------------------------------------------------------------------------------------------------------------------------
-- CommitmentsHistory
-----------------------------------------------------------------------------------------------------------------------------------------------
IF EXISTS(SELECT [object_id] FROM sys.tables WHERE [name]='DasCommitmentsHistory' AND [schema_id] = SCHEMA_ID('dbo'))
BEGIN
	DROP TABLE [dbo].[DasCommitmentsHistory]
END
GO

CREATE TABLE [dbo].[DasCommitmentsHistory](
	[CommitmentId] [bigint] NOT NULL,
	[VersionId] varchar(25) NOT NULL,
	[Uln] [bigint] NOT NULL,
	[Ukprn] [bigint] NOT NULL,
	[AccountId] bigint NOT NULL,
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
	[EffectiveFromDate] [date] NOT NULL,
	[EffectiveToDate] [date] NULL,
	[LegalEntityName] [nvarchar](100) NULL,
	[EventDateTime] datetime NOT NULL default getDate(),
	[TransferSendingEmployerAccountId] bigint null,
	[TransferApprovalDate] datetime null,
	[PausedOnDate] datetime null,
	[WithdrawnOnDate] datetime null,
	[AccountLegalEntityPublicHashedId] char(6) null
)
