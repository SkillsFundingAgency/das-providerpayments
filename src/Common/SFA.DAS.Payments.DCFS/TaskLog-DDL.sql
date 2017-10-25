CREATE TABLE [MyTransientSchemaNameForLogging].[TaskLog](
	[TaskLogId] [uniqueidentifier] NOT NULL DEFAULT(NEWID()),
	[DateTime] [datetime] NOT NULL DEFAULT(GETDATE()),
	[Level] [nvarchar](10) NOT NULL,
	[Logger] [nvarchar](512) NOT NULL,
	[Message] [nvarchar](1024) NOT NULL,
	[Exception] [nvarchar](max) NULL
)