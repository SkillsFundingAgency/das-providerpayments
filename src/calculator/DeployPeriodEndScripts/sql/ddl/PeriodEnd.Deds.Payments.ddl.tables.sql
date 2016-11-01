IF NOT EXISTS (SELECT [schema_id] FROM sys.schemas WHERE name='Payments')
	BEGIN
		EXEC('CREATE SCHEMA Payments')
	END
GO

IF EXISTS (SELECT [object_id] FROM sys.tables WHERE [name] = 'Periods' AND [schema_id] = SCHEMA_ID('Payments'))
	BEGIN
		DROP TABLE Payments.Periods
	END
GO

CREATE TABLE [Payments].[Periods](
	[PeriodName] [char](8) NOT NULL,
	[CalendarMonth] [int] NOT NULL,
	[CalendarYear] [int] NOT NULL,
	[AccountDataValidAt] [datetime] NOT NULL,
	[CommitmentDataValidAt] [datetime] NOT NULL,
	[CompletionDateTime] [datetime] NOT NULL,
	PRIMARY KEY CLUSTERED 
	(
		[CalendarYear],
		[CalendarMonth]
	)
)
GO