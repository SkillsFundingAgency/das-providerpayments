
IF(EXISTS(SELECT [object_id] FROM sys.tables WHERE [name] = 'FileDetails'))
BEGIN
	DROP TABLE dbo.FileDetails
END


CREATE TABLE [dbo].[FileDetails](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[UKPRN] [int] NOT NULL,
	[Filename] [nvarchar](50) NULL,
	[FileSizeKb] [bigint] NULL,
	[TotalLearnersSubmitted] [int] NULL,
	[TotalValidLearnersSubmitted] [int] NULL,
	[TotalInvalidLearnersSubmitted] [int] NULL,
	[TotalErrorCount] [int] NULL,
	[TotalWarningCount] [int] NULL,
	[SubmittedTime] [datetime] NULL
) ON [PRIMARY]
GO

