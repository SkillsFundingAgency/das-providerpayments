
IF IndexProperty(Object_Id('Submissions.SubmissionEvents'), 'IX_ULN', 'IndexId') IS NULL
BEGIN
	CREATE INDEX IX_ULN ON Submissions.SubmissionEvents (ULN);
END
