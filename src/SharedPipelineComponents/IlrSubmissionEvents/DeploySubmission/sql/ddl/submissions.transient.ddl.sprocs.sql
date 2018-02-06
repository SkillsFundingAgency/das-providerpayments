IF NOT EXISTS (SELECT 1 FROM sys.types WHERE name = 'IlrIdentifier' AND schema_id = schema_id('Submissions'))
  CREATE TYPE [Submissions].[IlrIdentifier] AS TABLE (
    UKPRN BIGINT,
    LearnRefNumber VARCHAR(12),
    PriceEpisodeIdentifier VARCHAR(25)
    );
GO

IF object_id('[Submissions].[DeleteLastSeenVersions]', 'p') IS NOT NULL
  DROP PROCEDURE [Submissions].[DeleteLastSeenVersions]
GO

CREATE PROCEDURE [Submissions].[DeleteLastSeenVersions] 
	@ILRs [Submissions].[IlrIdentifier] readonly
AS
BEGIN
  DELETE Submissions.LastSeenVersion
  FROM Submissions.LastSeenVersion
  INNER JOIN @ILRs ilr ON ilr.UKPRN = Submissions.LastSeenVersion.UKPRN
    AND ilr.LearnRefNumber = Submissions.LastSeenVersion.LearnRefNumber
    AND ilr.PriceEpisodeIdentifier = Submissions.LastSeenVersion.PriceEpisodeIdentifier
END
GO
