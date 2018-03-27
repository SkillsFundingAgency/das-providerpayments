
ALTER TABLE Submissions.SubmissionEvents
	ALTER COLUMN OnProgrammeTotalPrice decimal(15,5) NULL;
GO


ALTER TABLE Submissions.SubmissionEvents
	ALTER COLUMN CompletionTotalPrice decimal(15,5) NULL;
GO


ALTER TABLE Submissions.LastSeenVersion
	ALTER COLUMN OnProgrammeTotalPrice decimal(15,5) NULL;
GO


ALTER TABLE Submissions.LastSeenVersion
	ALTER COLUMN CompletionTotalPrice decimal(15,5) NULL;
GO
