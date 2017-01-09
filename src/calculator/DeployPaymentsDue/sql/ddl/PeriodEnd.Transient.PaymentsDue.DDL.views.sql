IF NOT EXISTS(SELECT [schema_id] FROM sys.schemas WHERE [name]='PaymentsDue')
BEGIN
	EXEC('CREATE SCHEMA PaymentsDue')
END
GO

-----------------------------------------------------------------------------------------------------------------------------------------------
-- vw_CommitmentEarning
-----------------------------------------------------------------------------------------------------------------------------------------------
IF EXISTS(SELECT [object_id] FROM sys.views WHERE [name]='vw_CommitmentEarning' AND [schema_id] = SCHEMA_ID('PaymentsDue'))
BEGIN
	DROP VIEW PaymentsDue.vw_CommitmentEarning
END
GO

CREATE VIEW PaymentsDue.vw_CommitmentEarning
AS
	SELECT
		lc.CommitmentId,
		c.VersionId CommitmentVersionId,
		a.AccountId,
		a.VersionId AccountVersionId,
		ae.Ukprn,
		ae.Uln,
		ae.LearnRefNumber,
		ae.AimSeqNumber,
		ae.AttributeName,
		ae.Period_1,
		ae.Period_2,
		ae.Period_3,
		ae.Period_4,
		ae.Period_5,
		ae.Period_6,
		ae.Period_7,
		ae.Period_8,
		ae.Period_9,
		ae.Period_10,
		ae.Period_11,
		ae.Period_12,
		ae.StandardCode,
		(CASE WHEN ae.StandardCode IS NULL THEN ae.ProgrammeType ELSE NULL END) ProgrammeType,
		ae.FrameworkCode,
		ae.PathwayCode,
		ae.ApprenticeshipContractType,
		ae.PriceEpisodeIdentifier
	FROM Reference.ApprenticeshipEarnings ae
		LEFT JOIN DataLock.DasLearnerCommitment lc ON ae.Ukprn = lc.Ukprn
			AND ae.LearnRefNumber = lc.LearnRefNumber
			AND ae.AimSeqNumber = lc.AimSeqNumber
			AND ae.PriceEpisodeIdentifier = lc.PriceEpisodeIdentifier
		LEFT JOIN Reference.DasCommitments c ON c.CommitmentId = lc.CommitmentId
		LEFT JOIN Reference.DasAccounts a ON c.AccountId = a.AccountId
	WHERE NOT EXISTS (
		SELECT 1
		FROM DataLock.ValidationError ve
		WHERE ve.LearnRefNumber = ae.LearnRefNumber
			AND ve.AimSeqNumber = ae.AimSeqNumber
			AND ve.PriceEpisodeIdentifier = ae.PriceEpisodeIdentifier
		)
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
	PathwayCode
FROM Reference.RequiredPaymentsHistory
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
	PriceEpisodeIdentifier
FROM PaymentsDue.RequiredPayments
GO