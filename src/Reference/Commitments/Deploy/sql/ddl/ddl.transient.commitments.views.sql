-----------------------------------------------------------------------------------------------------------------------------------------------
-- vw_EventStreamPointer
-----------------------------------------------------------------------------------------------------------------------------------------------
IF EXISTS(SELECT [object_id] FROM sys.views WHERE [name]='vw_EventStreamPointer' AND [schema_id] = SCHEMA_ID('dbo'))
BEGIN
	DROP VIEW [dbo].[vw_EventStreamPointer]
END
GO

CREATE VIEW [dbo].[vw_EventStreamPointer]
AS
    SELECT TOP 1
        MAX([EventId]) AS [EventId],
        [ReadDate]
    FROM [dbo].[EventStreamPointer]
    GROUP BY [EventId], [ReadDate]
    ORDER BY [EventId] DESC, [ReadDate] DESC
