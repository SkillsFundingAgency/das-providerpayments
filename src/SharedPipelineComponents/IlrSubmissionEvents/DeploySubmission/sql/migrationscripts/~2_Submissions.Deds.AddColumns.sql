
-- filename starts with a ~ to ensure this alter table script runs *after* the table creation script

IF NOT EXISTS(SELECT [column_id] FROM sys.columns WHERE [name] = 'EPAOrgId' AND [object_id] = OBJECT_ID('Submissions.LastSeenVersion'))
  BEGIN
    ALTER TABLE [Submissions].[LastSeenVersion]
	ADD [EPAOrgId] varchar(7) NULL
  END
GO

--Submissions.SubmissionEvents
IF NOT EXISTS(SELECT [column_id] FROM sys.columns WHERE [name] = 'EPAOrgId' AND [object_id] = OBJECT_ID('Submissions.SubmissionEvents'))
  BEGIN
    ALTER TABLE [Submissions].[SubmissionEvents]
	ADD [EPAOrgId] varchar(7) NULL
  END
GO