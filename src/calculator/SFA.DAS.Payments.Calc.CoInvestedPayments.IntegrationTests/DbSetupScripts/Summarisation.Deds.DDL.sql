-----------------------------------------------------------------------------------------------------------------------------------------------
-- Collection_Period_Mapping
-----------------------------------------------------------------------------------------------------------------------------------------------
IF EXISTS(SELECT [object_id] FROM sys.tables WHERE [name]='Collection_Period_Mapping' AND [schema_id] = SCHEMA_ID('dbo'))
BEGIN
	DROP TABLE dbo.Collection_Period_Mapping
END
GO

CREATE TABLE [dbo].[Collection_Period_Mapping] (
	[Period_ID] [int] NOT NULL,
	[Collection_Period] [varchar](3) NOT NULL,
	[Period] [int] NOT NULL,
	[Calendar_Year] [int] NOT NULL,
	[Collection_Open] [bit] NOT NULL,
	[ActualsSchemaPeriod] [int] NOT NULL,
	CONSTRAINT [PK_Collection_Period_Mapping] PRIMARY KEY CLUSTERED ([Period_ID] ASC)
)
