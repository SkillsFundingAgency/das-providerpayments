IF EXISTS (SELECT * FROM sys.indexes i
JOIN sys.objects t ON i.object_id = t.object_id
WHERE t.name = 'DasCommitments'
AND i.name = 'ix_dascommitments_uln')
BEGIN
	DROP INDEX ix_dascommitments_uln ON Reference.DasCommitments
END
GO

IF EXISTS (SELECT * FROM sys.indexes i
JOIN sys.objects t ON i.object_id = t.object_id
WHERE t.name = 'DasCommitments'
AND i.name = 'IDX_Commitments_AccountId')
BEGIN
	DROP INDEX IDX_Commitments_AccountId ON Reference.DasCommitments
END
GO

IF EXISTS (SELECT * FROM sys.indexes i
JOIN sys.objects t ON i.object_id = t.object_id
WHERE t.name = 'DasCommitments'
AND i.name = 'IDX_Commitments_Ukprn')
BEGIN
	DROP INDEX IDX_Commitments_Ukprn ON Reference.DasCommitments
END
GO

IF EXISTS (SELECT * FROM sys.indexes i
JOIN sys.objects t ON i.object_id = t.object_id
WHERE t.name = 'DasCommitments'
AND i.name = 'IX_DasCommitments_CommitmentId')
BEGIN
	DROP INDEX IX_DasCommitments_CommitmentId ON Reference.DasCommitments
END
GO

DELETE FROM [Reference].[DasCommitments]
GO

INSERT INTO [Reference].[DasCommitments]
    SELECT
        [CommitmentId],
        MAX([VersionId]) [VersionId],
        [Uln],
        [Ukprn],
        [AccountId],
        [StartDate],
        [EndDate],
        [AgreedCost],
        [StandardCode],
        [ProgrammeType],
        [FrameworkCode],
        [PathwayCode],
        [PaymentStatus],
        [PaymentStatusDescription],
        [Priority],
        [EffectiveFromDate],
        [EffectiveToDate],
        [LegalEntityName],
		[TransferSendingEmployerAccountId],
		[TransferApprovalDate]
    FROM ${DAS_Commitments.FQ}.[dbo].[DasCommitments]
    GROUP BY [CommitmentId],
        [Uln],
        [Ukprn],
        [AccountId],
        [StartDate],
        [EndDate],
        [AgreedCost],
        [StandardCode],
        [ProgrammeType],
        [FrameworkCode],
        [PathwayCode],
        [PaymentStatus],
        [PaymentStatusDescription],
        [Priority],
        [EffectiveFromDate],
        [EffectiveToDate],
        [LegalEntityName],
		[TransferSendingEmployerAccountId],
		[TransferApprovalDate]
GO

CREATE INDEX [IDX_Commitments_Ukprn] ON Reference.DasCommitments ([Ukprn])
GO

CREATE INDEX [IDX_Commitments_AccountId] ON Reference.DasCommitments (AccountId, CommitmentId, VersionId)
GO

CREATE INDEX IX_DasCommitments_Uln ON Reference.DasCommitments (Uln)
GO

CREATE INDEX IX_DasCommitments_CommitmentId ON Reference.DasCommitments (CommitmentId, VersionId)
GO