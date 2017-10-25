TRUNCATE TABLE [Reference].[DasCommitments]
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
    WHERE [ULN] IN (SELECT DISTINCT [ULN] 
					FROM 
					[Valid].[Learner] )
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
