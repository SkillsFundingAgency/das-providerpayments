DELETE FROM ${DAS_Commitments.FQ}.[dbo].[DasCommitments]
GO

DELETE FROM ${DAS_Commitments.FQ}.[dbo].[EventStreamPointer]
    WHERE [EventId] IN (
        SELECT DISTINCT esp.[EventId] FROM [dbo].[EventStreamPointer] esp
    )
GO
