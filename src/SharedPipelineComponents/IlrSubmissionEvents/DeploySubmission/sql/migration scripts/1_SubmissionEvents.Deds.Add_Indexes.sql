IF IndexProperty(Object_Id('${DAS_ProviderEvents.FQ}.Submissions.LastSeenVersion'), 'IX_LastSeenVersion_UKPRN', 'IndexId') IS NULL
BEGIN
	CREATE INDEX IX_LastSeenVersion_UKPRN ON ${DAS_ProviderEvents.FQ}.Submissions.LastSeenVersion (UKPRN, LearnRefNumber, PriceEpisodeIdentifier);
END