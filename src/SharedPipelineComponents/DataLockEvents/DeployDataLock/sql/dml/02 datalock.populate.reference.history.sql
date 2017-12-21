/* +++++ Monitoring setup */
DECLARE @Now DATETIME 
DECLARE @timestamp VARCHAR(255)
DECLARE @duration INT
DECLARE @operation NVARCHAR(255) = 'undefined'
DECLARE @start NVARCHAR(255) = N'Start: %s %s.'
DECLARE @finish NVARCHAR(255) = N'Finish: %s, took %d ms.'
DECLARE @logsource varchar(max) = '02 datalock populate reference history'
/* ----- Monitoring setup */

/* +++++ start pre-op monitoring */
SELECT @Now = GETDATE()
SET @operation = 'Drop Target Table Keys, Constraints and Indexes'
SET @timestamp = CONVERT(VARCHAR(255), @now, 121)
RAISERROR (		@start,		10,		0,		@operation,		@timestamp		) WITH NOWAIT
INSERT INTO [DataLockEvents].[TaskLog] ( [TaskLogId], [Logger], [Level], [DateTime], [Message], [Exception] )
SELECT NEWID(),	@logsource,	0,	GETDATE(),	@operation + '- Started: [' + @timestamp + ']',	NULL
/* ----- finish pre-op monitoring*/

IF EXISTS (
		SELECT [name]
		FROM [sys].[indexes]
		WHERE [name] = 'DataLockByStatusIdLearnRefNumberPriceEpisodeIdentifier'
		)
BEGIN
	DROP INDEX [DataLockByStatusIdLearnRefNumberPriceEpisodeIdentifier]
		ON [Reference].[DataLockEvents]
END

IF EXISTS (
		SELECT [name]
		FROM [sys].[indexes]
		WHERE [name] = 'IX_Reference_DataLockEvents_UKPRN'
		)
BEGIN
	DROP INDEX [IX_Reference_DataLockEvents_UKPRN]
		ON [Reference].[DataLockEvents]
END

IF EXISTS (
		SELECT [name]
		FROM [sys].[key_constraints]
		WHERE [name] = 'PK_Reference_DataLockEvents'
		)
BEGIN
	ALTER TABLE [Reference].[DataLockEvents]
	DROP CONSTRAINT [PK_Reference_DataLockEvents]
END

/* +++++ start post-op monitoring */
SET @duration = DATEDIFF(MS, @Now, GETDATE())
RAISERROR (		@finish,		10,		0,		@operation,		@duration		) WITH NOWAIT
INSERT INTO [DataLockEvents].[TaskLog] ( [TaskLogId], [Logger], [Level], [DateTime], [Message], [Exception] )
SELECT NEWID(), 	@logsource,	0,	GETDATE(),	@operation + '- Started: [' + @timestamp + '] Duration: [' + cast(@duration AS VARCHAR(255)) + ']',	NULL
/* ----- finish post-op monitoring*/


/* +++++ start pre-op monitoring */
SELECT @Now = GETDATE()
SET @operation = 'Pull of all Provider Datalocks from remote server'
SET @timestamp = CONVERT(VARCHAR(255), @now, 121)
RAISERROR (		@start,		10,		0,		@operation,		@timestamp		) WITH NOWAIT
INSERT INTO [DataLockEvents].[TaskLog] ( [TaskLogId], [Logger], [Level], [DateTime], [Message], [Exception] )
SELECT NEWID(),	@logsource,	0,	GETDATE(),	@operation + '- Started: [' + @timestamp + ']',	NULL
/* ----- finish pre-op monitoring*/

INSERT INTO [Reference].[DataLockEvents] (
	[Id],
	[DataLockEventId],
	[ProcessDateTime],
	[Status],
	[IlrFileName],
	[SubmittedDateTime],
	[AcademicYear],
	[UKPRN],
	[ULN],
	[LearnRefNumber],
	[AimSeqNumber],
	[PriceEpisodeIdentifier],
	[CommitmentId],
	[EmployerAccountId],
	[EventSource],
	[HasErrors],
	[IlrStartDate],
	[IlrStandardCode],
	[IlrProgrammeType],
	[IlrFrameworkCode],
	[IlrPathwayCode],
	[IlrTrainingPrice],
	[IlrEndpointAssessorPrice],
	[IlrPriceEffectiveFromDate],
	[IlrPriceEffectiveToDate]
	)
SELECT dle.[Id],
	dle.[DataLockEventId],
	dle.[ProcessDateTime],
	dle.[Status],
	dle.[IlrFileName],
	dle.[SubmittedDateTime],
	dle.[AcademicYear],
	dle.[UKPRN],
	dle.[ULN],
	dle.[LearnRefNumber],
	dle.[AimSeqNumber],
	dle.[PriceEpisodeIdentifier],
	dle.[CommitmentId],
	dle.[EmployerAccountId],
	dle.[EventSource],
	dle.[HasErrors],
	dle.[IlrStartDate],
	dle.[IlrStandardCode],
	dle.[IlrProgrammeType],
	dle.[IlrFrameworkCode],
	dle.[IlrPathwayCode],
	dle.[IlrTrainingPrice],
	dle.[IlrEndpointAssessorPrice],
	dle.[IlrPriceEffectiveFromDate],
	dle.[IlrPriceEffectiveToDate]
FROM [DataLock].[vw_Providers] ptp	WITH ( NOLOCK )
INNER JOIN 
	${DAS_ProviderEvents.FQ}.[DataLock].[DataLockEvents] dle WITH ( NOLOCK )
	ON ptp.[UKPRN] = dle.[UKPRN]

/* +++++ start post-op monitoring */
SET @duration = DATEDIFF(MS, @Now, GETDATE())
RAISERROR (		@finish,		10,		0,		@operation,		@duration		) WITH NOWAIT
INSERT INTO [DataLockEvents].[TaskLog] ( [TaskLogId], [Logger], [Level], [DateTime], [Message], [Exception] )
SELECT NEWID(),	@logsource,	0,	GETDATE(),	@operation + '- Started: [' + @timestamp + '] Duration: [' + cast(@duration AS VARCHAR(255)) + ']',	NULL
/* ----- finish post-op monitoring*/

/* +++++ start pre-op monitoring */
SELECT @Now = GETDATE()
SET @operation = 'Re-Apply Target table Keys, Constrtaints and Indexes'
SET @timestamp = CONVERT(VARCHAR(255), @now, 121)
RAISERROR (		@start,		10,		0,		@operation,		@timestamp		) WITH NOWAIT
INSERT INTO [DataLockEvents].[TaskLog] ( [TaskLogId], [Logger], [Level], [DateTime], [Message], [Exception] )
SELECT NEWID(),	@logsource,	0,	GETDATE(),	@operation + '- Started: [' + @timestamp + ']',	NULL
/* ----- finish pre-op monitoring*/

IF NOT EXISTS (
		SELECT [name]
		FROM [sys].[key_constraints]
		WHERE [name] = 'PK_Reference_DataLockEvents'
		)
BEGIN
	ALTER TABLE [Reference].[DataLockEvents] 
	ADD CONSTRAINT [PK_Reference_DataLockEvents] 
	PRIMARY KEY NONCLUSTERED ([Id] ASC)
END

IF NOT EXISTS (
		SELECT [name]
		FROM [sys].[indexes]
		WHERE [name] = 'IX_Reference_DataLockEvents_UKPRN'
		)
BEGIN
	CREATE CLUSTERED INDEX [IX_Reference_DataLockEvents_UKPRN] 
	ON [Reference].[DataLockEvents] ([UKPRN] ASC)
END

IF NOT EXISTS (
		SELECT [name]
		FROM [sys].[indexes]
		WHERE [name] = 'DataLockByStatusIdLearnRefNumberPriceEpisodeIdentifier'
		)
BEGIN
	CREATE NONCLUSTERED INDEX [DataLockByStatusIdLearnRefNumberPriceEpisodeIdentifier] ON [Reference].[DataLockEvents] ([Status]) INCLUDE (
		[Id],
		[LearnRefNumber],
		[PriceEpisodeIdentifier]
		)
END
/* +++++ start post-op monitoring */
SET @duration = DATEDIFF(MS, @Now, GETDATE())
RAISERROR (		@finish,		10,		0,		@operation,		@duration		) WITH NOWAIT
INSERT INTO [DataLockEvents].[TaskLog] ( [TaskLogId], [Logger], [Level], [DateTime], [Message], [Exception] )
SELECT NEWID(),	@logsource,	0,	GETDATE(),	@operation + '- Started: [' + @timestamp + '] Duration: [' + cast(@duration AS VARCHAR(255)) + ']',	NULL
/* ----- finish post-op monitoring*/


/* +++++ start pre-op monitoring */
SELECT @Now = GETDATE()
SET @operation = 'Identifying latest DataLocks by Provider, Learner and Price Period'
SET @timestamp = CONVERT(VARCHAR(255), @now, 121)
RAISERROR (		@start,		10,		0,		@operation,		@timestamp		) WITH NOWAIT
INSERT INTO [DataLockEvents].[TaskLog] ( [TaskLogId], [Logger], [Level], [DateTime], [Message], [Exception] )
SELECT NEWID(),	@logsource,	0,	GETDATE(),	@operation + '- Started: [' + @timestamp + ']',	NULL
/* ----- finish pre-op monitoring*/

DECLARE @LastestDataLockEvents TABLE ([EventId] BIGINT)

INSERT INTO @LastestDataLockEvents
SELECT 
	MAX(dle.[Id]) AS [EventId]
FROM 
	[Reference].[DataLockEvents] dle
GROUP BY 
	dle.[UKPRN],
	dle.[LearnRefNumber],
	dle.[PriceEpisodeIdentifier]

/* +++++ start post-op monitoring */
SET @duration = DATEDIFF(MS, @Now, GETDATE())
RAISERROR (		@finish,		10,		0,		@operation,		@duration		) WITH NOWAIT
INSERT INTO [DataLockEvents].[TaskLog] ( [TaskLogId], [Logger], [Level], [DateTime], [Message], [Exception] )
SELECT NEWID(),	@logsource,	0,	GETDATE(),	@operation + '- Started: [' + @timestamp + '] Duration: [' + cast(@duration AS VARCHAR(255)) + ']',	NULL
/* ----- finish post-op monitoring*/

/* +++++ start pre-op monitoring */
SELECT @Now = GETDATE()
SET @operation = 'Removing Superceeded DataLocks'
SET @timestamp = CONVERT(VARCHAR(255), @now, 121)
RAISERROR (		@start,		10,		0,		@operation,		@timestamp		) WITH NOWAIT
INSERT INTO [DataLockEvents].[TaskLog] ( [TaskLogId], [Logger], [Level], [DateTime], [Message], [Exception] )
SELECT NEWID(),	@logsource,	0,	GETDATE(),	@operation + '- Started: [' + @timestamp + ']',	NULL
/* ----- finish pre-op monitoring*/

DELETE
FROM 
	[Reference].[DataLockEvents]
FROM 
	[Reference].[DataLockEvents] dle
LEFT JOIN 
	@LastestDataLockEvents ldle
	ON dle.[Id] = ldle.[EventId]
WHERE 
	ldle.[EventId] IS NULL

/* +++++ start post-op monitoring */
SET @duration = DATEDIFF(MS, @Now, GETDATE())
RAISERROR (		@finish,		10,		0,		@operation,		@duration		) WITH NOWAIT
INSERT INTO [DataLockEvents].[TaskLog] ( [TaskLogId], [Logger], [Level], [DateTime], [Message], [Exception] )
SELECT NEWID(),	@logsource,	0,	GETDATE(),	@operation + '- Started: [' + @timestamp + '] Duration: [' + cast(@duration AS VARCHAR(255)) + ']',	NULL
/* ----- finish post-op monitoring*/