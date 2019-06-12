-- filename starts with a ~ to ensure this alter table script runs *after* the table creation script

IF NOT EXISTS(SELECT [column_id] FROM sys.columns WHERE [name] = 'GivenNames' AND [object_id] = OBJECT_ID('Submissions.LastSeenVersion'))
  BEGIN
    ALTER TABLE [Submissions].[LastSeenVersion]
	ADD [GivenNames] varchar(100) NULL
  END
GO

IF NOT EXISTS(SELECT [column_id] FROM sys.columns WHERE [name] = 'GivenNames' AND [object_id] = OBJECT_ID('Submissions.SubmissionEvents'))
  BEGIN
    ALTER TABLE [Submissions].[SubmissionEvents]
	ADD [GivenNames] varchar(100) NULL
  END
GO

IF NOT EXISTS(SELECT [column_id] FROM sys.columns WHERE [name] = 'FamilyName' AND [object_id] = OBJECT_ID('Submissions.LastSeenVersion'))
  BEGIN
    ALTER TABLE [Submissions].[LastSeenVersion]
	ADD [FamilyName] varchar(100) NULL
  END
GO

IF NOT EXISTS(SELECT [column_id] FROM sys.columns WHERE [name] = 'FamilyName' AND [object_id] = OBJECT_ID('Submissions.SubmissionEvents'))
  BEGIN
    ALTER TABLE [Submissions].[SubmissionEvents]
	ADD [FamilyName] varchar(100) NULL
  END
GO

IF NOT EXISTS(SELECT [column_id] FROM sys.columns WHERE [name] = 'CompStatus' AND [object_id] = OBJECT_ID('Submissions.LastSeenVersion'))
  BEGIN
    ALTER TABLE [Submissions].[LastSeenVersion]
	ADD CompStatus int NULL
  END
GO

IF NOT EXISTS(SELECT [column_id] FROM sys.columns WHERE [name] = 'CompStatus' AND [object_id] = OBJECT_ID('Submissions.SubmissionEvents'))
  BEGIN
    ALTER TABLE [Submissions].[SubmissionEvents]
	ADD CompStatus int NULL
  END
GO

IF NOT EXISTS(SELECT [column_id] FROM sys.columns WHERE [name] = 'FundingModel' AND [object_id] = OBJECT_ID('Submissions.SubmissionEvents'))
  BEGIN
    ALTER TABLE [Submissions].[SubmissionEvents]
	ADD FundingModel int NULL
  END
GO

IF NOT EXISTS(SELECT [column_id] FROM sys.columns WHERE [name] = 'DelLocPostCode' AND [object_id] = OBJECT_ID('Submissions.SubmissionEvents'))
  BEGIN
    ALTER TABLE [Submissions].[SubmissionEvents]
	ADD DelLocPostCode VARCHAR(50)  NULL
  END
GO

IF NOT EXISTS(SELECT [column_id] FROM sys.columns WHERE [name] = 'LearnActEndDate' AND [object_id] = OBJECT_ID('Submissions.SubmissionEvents'))
  BEGIN
    ALTER TABLE [Submissions].[SubmissionEvents]
	ADD LearnActEndDate Date NULL
  END
GO

IF NOT EXISTS(SELECT [column_id] FROM sys.columns WHERE [name] = 'WithdrawReason' AND [object_id] = OBJECT_ID('Submissions.SubmissionEvents'))
  BEGIN
    ALTER TABLE [Submissions].[SubmissionEvents]
	ADD WithdrawReason int NULL
  END
GO

IF NOT EXISTS(SELECT [column_id] FROM sys.columns WHERE [name] = 'Outcome' AND [object_id] = OBJECT_ID('Submissions.SubmissionEvents'))
  BEGIN
    ALTER TABLE [Submissions].[SubmissionEvents]
	ADD Outcome int NULL
  END
GO

IF NOT EXISTS(SELECT [column_id] FROM sys.columns WHERE [name] = 'AimType' AND [object_id] = OBJECT_ID('Submissions.SubmissionEvents'))
  BEGIN
    ALTER TABLE [Submissions].[SubmissionEvents]
	ADD AimType int NULL
  END
GO

