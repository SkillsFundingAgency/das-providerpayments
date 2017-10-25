IF NOT EXISTS(SELECT [schema_id] FROM sys.schemas WHERE [name]='ProviderAdjustments')
BEGIN
    EXEC('CREATE SCHEMA ProviderAdjustments')
END
GO

-----------------------------------------------------------------------------------------------------------------------------------------------
-- vw_CollectionPeriods
-----------------------------------------------------------------------------------------------------------------------------------------------
IF EXISTS(SELECT [object_id] FROM sys.views WHERE [name]='vw_CollectionPeriods' AND [schema_id] = SCHEMA_ID('ProviderAdjustments'))
BEGIN
    DROP VIEW ProviderAdjustments.vw_CollectionPeriods
END
GO

CREATE VIEW ProviderAdjustments.vw_CollectionPeriods
AS
SELECT
    [Id] AS [PeriodId],
    [Name],
    [CalendarMonth] AS [Month],
    [CalendarYear] AS [Year],
    [Open] AS [Collection_Open]
FROM Reference.CollectionPeriods
GO
