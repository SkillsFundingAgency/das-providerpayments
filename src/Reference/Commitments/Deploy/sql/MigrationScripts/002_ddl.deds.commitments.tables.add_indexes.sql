

CREATE INDEX IX_DasCommitments_Uln ON dbo.DasCommitments (Uln)
CREATE INDEX IX_DasCommitments_DataLock ON dbo.DasCommitments (
	CommitmentId, 
	Uln, 
	Ukprn, 
	AccountId, 
	StartDate, 
	EndDate, 
	AgreedCost, 
	StandardCode, 
	ProgrammeType, 
	FrameworkCode, 
	PathwayCode, 
	PaymentStatus, 
	PaymentStatusDescription, 
	[Priority], 
	EffectiveFromDate,
	EffectiveToDate,
	LegalEntityName, 
	VersionId)

