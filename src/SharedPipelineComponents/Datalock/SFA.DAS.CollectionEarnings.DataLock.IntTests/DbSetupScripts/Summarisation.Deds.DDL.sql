-----------------------------------------------------------------------------------------------------------------------------------------------
-- Collection_Period_Mapping
-----------------------------------------------------------------------------------------------------------------------------------------------
IF EXISTS(SELECT [object_id] FROM sys.tables WHERE [name]='Collection_Period_Mapping' AND [schema_id] = SCHEMA_ID('dbo'))
BEGIN
	DROP TABLE dbo.Collection_Period_Mapping
END
GO
CREATE TABLE [dbo].[Collection_Period_Mapping](
       [Collection_Year] [int] NOT NULL,
       [Period_ID] [int] NOT NULL,
       [Return_Code] [varchar](10) NOT NULL,
       [Collection_Period_Name] [nvarchar](20) NOT NULL,
       [Collection_ReturnCode] [varchar](10) NOT NULL,
       [Calendar_Month] [int] NOT NULL,
       [Calendar_Year] [int] NOT NULL,
       [Collection_Open] [bit] NOT NULL,
       [ActualsSchemaPeriod] [int] NOT NULL
       
CONSTRAINT [PK_Collection_Period_Mapping] PRIMARY KEY CLUSTERED 
(
       [Collection_Year],[Period_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
