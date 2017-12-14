IF EXISTS (SELECT * FROM sys.indexes i
JOIN sys.objects t ON i.object_id = t.object_id
WHERE t.name = 'DasCommitments'
AND i.name = 'ix_dascommitments_uln')
BEGIN
	DROP INDEX ix_dascommitments_uln ON Reference.DasCommitments
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
        [LegalEntityName]
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
        [LegalEntityName]
GO

CREATE INDEX [IDX_Commitments_Ukprn] ON Reference.DasCommitments ([Ukprn])
GO