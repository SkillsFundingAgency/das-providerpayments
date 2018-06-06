IF NOT EXISTS(SELECT [schema_id] FROM sys.schemas WHERE [name]='PaymentsDue')
BEGIN
    EXEC('CREATE SCHEMA PaymentsDue')
END
GO


-----------------------------------------------------------------------------------------------------------------------------------------------
-- vw_CollectionPeriods
-----------------------------------------------------------------------------------------------------------------------------------------------
IF EXISTS(SELECT [object_id] FROM sys.views WHERE [name]='vw_CollectionPeriods' AND [schema_id] = SCHEMA_ID('PaymentsDue'))
BEGIN
    DROP VIEW PaymentsDue.vw_CollectionPeriods
END
GO

CREATE VIEW PaymentsDue.vw_CollectionPeriods
AS
SELECT
    [Id] AS [Period_ID],
    [Name] AS [Collection_Period],
    [CalendarMonth] AS [Period],
    [CalendarYear] AS [Calendar_Year],
    [Open] AS [Collection_Open]
FROM Reference.CollectionPeriods
GO




-----------------------------------------------------------------------------------------------------------------------------------------------
-- vw_RequiredPayments
-----------------------------------------------------------------------------------------------------------------------------------------------
IF EXISTS(SELECT [object_id] FROM sys.views WHERE [name]='vw_RequiredPayments' AND [schema_id] = SCHEMA_ID('PaymentsDue'))
BEGIN
    DROP VIEW PaymentsDue.vw_RequiredPayments
END
GO

CREATE VIEW PaymentsDue.vw_RequiredPayments
AS
SELECT
    Id,
    CommitmentId,
    CommitmentVersionId,
    AccountId,
    AccountVersionId,
    Uln,
    LearnRefNumber,
    AimSeqNumber,
    Ukprn,
    IlrSubmissionDateTime,
    StandardCode,
    ProgrammeType,
    FrameworkCode,
    PathwayCode,
    DeliveryMonth,
    DeliveryYear,
    (SELECT MAX('${YearOfCollection}-' + [Name]) FROM [Reference].[CollectionPeriods] WHERE [Open] = 1) AS CollectionPeriodName,
    (SELECT MAX([CalendarMonth]) FROM [Reference].[CollectionPeriods] WHERE [Open] = 1) AS CollectionPeriodMonth,
    (SELECT MAX([CalendarYear]) FROM [Reference].[CollectionPeriods] WHERE [Open] = 1) AS CollectionPeriodYear,
    AmountDue,
    TransactionType,
    ApprenticeshipContractType,
    PriceEpisodeIdentifier,
    SfaContributionPercentage,
    FundingLineType,
    UseLevyBalance,
	LearnAimRef,
	LearningStartDate
FROM PaymentsDue.RequiredPayments
GO

