
-- filename starts with a ~ to ensure this alter table script runs *after* the table creation script

IF NOT EXISTS(SELECT [column_id] FROM sys.columns WHERE [name] = 'GivenNames' AND [object_id] = OBJECT_ID('Submissions.LastSeenVersion'))
  BEGIN
    ALTER TABLE [Submissions].[LastSeenVersion]
	ADD [GivenNames] varchar(100) NULL
  END
GO

--Submissions.SubmissionEvents
IF NOT EXISTS(SELECT [column_id] FROM sys.columns WHERE [name] = 'GivenNames' AND [object_id] = OBJECT_ID('Submissions.SubmissionEvents'))
  BEGIN
    ALTER TABLE [Submissions].[SubmissionEvents]
	ADD [GivenNames] varchar(100) NULL
  END
GO

-- filename starts with a ~ to ensure this alter table script runs *after* the table creation script
IF NOT EXISTS(SELECT [column_id] FROM sys.columns WHERE [name] = 'FamilyName' AND [object_id] = OBJECT_ID('Submissions.LastSeenVersion'))
  BEGIN
    ALTER TABLE [Submissions].[LastSeenVersion]
	ADD [FamilyName] varchar(100) NULL
  END
GO

--Submissions.SubmissionEvents
IF NOT EXISTS(SELECT [column_id] FROM sys.columns WHERE [name] = 'FamilyName' AND [object_id] = OBJECT_ID('Submissions.SubmissionEvents'))
  BEGIN
    ALTER TABLE [Submissions].[SubmissionEvents]
	ADD [FamilyName] varchar(100) NULL
  END
GO