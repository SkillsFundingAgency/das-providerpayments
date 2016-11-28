TRUNCATE TABLE [Reference].[DasCommitments]
GO

INSERT INTO [Reference].[DasCommitments]
    SELECT
        [CommitmentId],
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
        [Payable],
        [Priority],
        [VersionId]
    FROM ${DAS_Commitments.FQ}.[dbo].[DasCommitments]
GO
