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
	SELECT 
		[EventId],
		[ReadDate]
	FROM [dbo].[EventStreamPointer]
	WHERE [EventId] = (
		SELECT MAX(esp.[EventId])
		FROM [dbo].[EventStreamPointer] esp
	)