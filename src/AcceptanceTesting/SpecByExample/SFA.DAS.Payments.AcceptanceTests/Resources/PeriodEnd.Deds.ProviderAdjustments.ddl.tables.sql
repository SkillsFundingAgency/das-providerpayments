-----------------------------------------------------------------------------------------------------------------------------------------------
-- Payment Types
-----------------------------------------------------------------------------------------------------------------------------------------------

IF EXISTS(SELECT [object_id] FROM sys.tables WHERE [name]='Payment_Types' AND [schema_id] = SCHEMA_ID('dbo'))
BEGIN
    DROP TABLE dbo.Payment_Types
END
GO

CREATE TABLE [dbo].[Payment_Types](
	[Payment_Id] [int] NOT NULL,
	[PaymentName] [nvarchar](250) NOT NULL,
	[FM36] [bit] NOT NULL,
 CONSTRAINT [PK_Payment_Types] PRIMARY KEY CLUSTERED 
(
	[Payment_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Payment_Types] ADD  CONSTRAINT [DF_Payment_Types_FM36]  DEFAULT ((0)) FOR [FM36]
GO


/****** Object:  Table [dbo].[EAS_Submission]    Script Date: 23/11/2017 15:08:57 ******/
IF EXISTS(SELECT [object_id] FROM sys.tables WHERE [name]='EAS_Submission' AND [schema_id] = SCHEMA_ID('dbo'))
BEGIN
    DROP TABLE dbo.EAS_Submission
END
GO

CREATE TABLE [dbo].[EAS_Submission](
	[Submission_Id] [uniqueidentifier] NOT NULL,
	[UKPRN] [nvarchar](10) NOT NULL,
	[CollectionPeriod] [int] NOT NULL,
	[ProviderName] [nvarchar](250) NOT NULL,
	[UpdatedOn] [datetime] NOT NULL,
	[DeclarationChecked] [bit] NOT NULL,
	[NilReturn] [bit] NOT NULL,
	[UpdatedBy] [nvarchar](250) NULL,
 CONSTRAINT [PK_EAS_Submission] PRIMARY KEY CLUSTERED 
(
	[Submission_Id] ASC,
	[CollectionPeriod] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO


/****** Object:  Table [dbo].[EAS_Submission_Values]    Script Date: 23/11/2017 15:09:20 ******/
IF EXISTS(SELECT [object_id] FROM sys.tables WHERE [name]='EAS_Submission_Values' AND [schema_id] = SCHEMA_ID('dbo'))
BEGIN
	DROP TABLE [dbo].[EAS_Submission_Values]
END
GO

CREATE TABLE [dbo].[EAS_Submission_Values](
	[Submission_Id] [uniqueidentifier] NOT NULL,
	[CollectionPeriod] [int] NOT NULL,
	[Payment_Id] [int] NOT NULL,
	[PaymentValue] [decimal](10, 2) NOT NULL,
 CONSTRAINT [PK_EAS_Submission_Values] PRIMARY KEY CLUSTERED 
(
	[Submission_Id] ASC,
	[CollectionPeriod] ASC,
	[Payment_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

