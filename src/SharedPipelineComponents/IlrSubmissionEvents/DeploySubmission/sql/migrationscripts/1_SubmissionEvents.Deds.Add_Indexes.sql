IF IndexProperty(Object_Id('Submissions.LastSeenVersion'), 'IX_LastSeenVersion_UKPRN', 'IndexId') IS NULL
BEGIN
	CREATE INDEX IX_LastSeenVersion_UKPRN ON Submissions.LastSeenVersion (UKPRN, LearnRefNumber, PriceEpisodeIdentifier);
END

IF IndexProperty(Object_Id('Submissions.SubmissionEvents'), 'IX_ULN', 'IndexId') IS NULL
BEGIN
	CREATE INDEX IX_ULN ON Submissions.SubmissionEvents (ULN);
END