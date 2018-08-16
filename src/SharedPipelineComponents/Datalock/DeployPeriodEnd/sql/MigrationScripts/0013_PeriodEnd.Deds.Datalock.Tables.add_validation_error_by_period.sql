-----------------------------------------------------------------------------------------------------------------------------------------------
-- ValidationErrorByPeriod
-----------------------------------------------------------------------------------------------------------------------------------------------
IF NOT EXISTS(SELECT [object_id] FROM sys.tables WHERE [name]='ValidationErrorByPeriod' AND [schema_id] = SCHEMA_ID('DataLock'))
BEGIN
    CREATE TABLE DataLock.ValidationErrorByPeriod
    (
        [Ukprn] bigint,
        [LearnRefNumber] varchar(12),
        [AimSeqNumber] int,
        [RuleId] varchar(50),
        [PriceEpisodeIdentifier] varchar(25) NOT NULL,
        [Period] int
    )
END
GO

