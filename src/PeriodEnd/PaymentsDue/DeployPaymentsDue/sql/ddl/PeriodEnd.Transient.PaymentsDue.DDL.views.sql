IF NOT EXISTS(SELECT [schema_id] FROM sys.schemas WHERE [name]='PaymentsDue')
BEGIN
    EXEC('CREATE SCHEMA PaymentsDue')
END
GO

-----------------------------------------------------------------------------------------------------------------------------------------------
-- vw_ApprenticeshipEarning
-----------------------------------------------------------------------------------------------------------------------------------------------
IF EXISTS(SELECT [object_id] FROM sys.views WHERE [name]='vw_ApprenticeshipEarning' AND [schema_id] = SCHEMA_ID('PaymentsDue'))
BEGIN
    DROP VIEW PaymentsDue.vw_ApprenticeshipEarning
END
GO

CREATE VIEW PaymentsDue.vw_ApprenticeshipEarning
AS
   SELECT * FROM Staging.ApprenticeshipEarnings1
	UNION
	SELECT * FROM Staging.ApprenticeshipEarnings2
	UNION
	SELECT * FROM Staging.ApprenticeshipEarnings3	
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
-- vw_Providers
-----------------------------------------------------------------------------------------------------------------------------------------------
IF EXISTS(SELECT [object_id] FROM sys.views WHERE [name]='vw_Providers' AND [schema_id] = SCHEMA_ID('PaymentsDue'))
BEGIN
    DROP VIEW PaymentsDue.vw_Providers
END
GO

CREATE VIEW PaymentsDue.vw_Providers
AS
SELECT
    p.Ukprn
FROM Reference.Providers p
GO

-----------------------------------------------------------------------------------------------------------------------------------------------
-- vw_PaymentHistory
-----------------------------------------------------------------------------------------------------------------------------------------------
IF EXISTS(SELECT [object_id] FROM sys.views WHERE [name]='vw_PaymentHistory' AND [schema_id] = SCHEMA_ID('PaymentsDue'))
BEGIN
    DROP VIEW PaymentsDue.vw_PaymentHistory
END
GO

CREATE VIEW PaymentsDue.vw_PaymentHistory
AS
SELECT
    CommitmentId,
	CommitmentVersionId,
	AccountId,
	AccountVersionId,
    LearnRefNumber,
    AimSeqNumber,
    Ukprn,
    DeliveryMonth,
    DeliveryYear,
    CollectionPeriodMonth,
    CollectionPeriodYear,
    AmountDue,
    TransactionType,
    Uln,
    StandardCode,
    ProgrammeType,
    FrameworkCode,
    PathwayCode,
	LearnAimRef,
	LearningStartDate,
	ApprenticeshipContractType,
	FundingLineType,
	PriceEpisodeIdentifier,
	SfaContributionPercentage,
	UseLevyBalance
FROM Reference.RequiredPaymentsHistory
GO

-----------------------------------------------------------------------------------------------------------------------------------------------
-- vw_PaymentHistoryWithoutEarnings
-----------------------------------------------------------------------------------------------------------------------------------------------
IF EXISTS(SELECT [object_id] FROM sys.views WHERE [name]='vw_PaymentHistoryWithoutEarnings' AND [schema_id] = SCHEMA_ID('PaymentsDue'))
BEGIN
    DROP VIEW PaymentsDue.vw_PaymentHistoryWithoutEarnings
END
GO

CREATE VIEW PaymentsDue.vw_PaymentHistoryWithoutEarnings
AS
SELECT * from Staging.EarningsWithoutPayments

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


-----------------------------------------------------------------------------------------------------------------------------------------------
-- vw_RequiredPayments
-----------------------------------------------------------------------------------------------------------------------------------------------
IF EXISTS(SELECT [object_id] FROM sys.views WHERE [name]='vw_EarningsToPayments' AND [schema_id] = SCHEMA_ID('PaymentsDue'))
BEGIN
    DROP VIEW PaymentsDue.vw_EarningsToPayments
END
GO

CREATE VIEW PaymentsDue.vw_EarningsToPayments
AS
SELECT 
	rp.Id as RequiredPaymentId,
	ae.MonthlyInstallment As MonthlyInstallment,
	ae.LearningStartDate As StartDate,
	ae.LearningPlannedEndDate As PlannedEndDate ,
	ae.LearningActualEndDate as ActualEndDate,
	ae.CompletionStatus,
	ae.CompletionAmount,
	ae.TotalInstallments,
	ae.EndpointAssessorId

  FROM PaymentsDue.RequiredPayments rp
  JOIN 	[PaymentsDue].[vw_ApprenticeshipEarning] ae
	on rp.Ukprn = ae.Ukprn
	And rp.Uln = ae.Uln
	And rp.LearnRefNumber = ae.LearnRefNumber
	And rp.LearnAimRef = ae.LearnAimRef
	And rp.AimSeqNumber = ae.AimSeqNumber
	And rp.PriceEpisodeIdentifier =ae.PriceEpisodeIdentifier
	And case When DeliveryMonth between 1 and 7 Then DeliveryMonth + 5 Else DeliveryMonth - 7 END =  ae.Period  
	And rp.TransactionType = ae.TransactionType
GO	