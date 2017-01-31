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
	SELECT
		pepm.CommitmentId,
		pepm.VersionId CommitmentVersionId,
		a.AccountId,
		a.VersionId AccountVersionId,
		ae.Ukprn,
		ae.Uln,
		ae.LearnRefNumber,
		ae.AimSeqNumber,
		ae.Period,
		ae.PriceEpisodeOnProgPayment,
		ae.PriceEpisodeCompletionPayment,
		ae.PriceEpisodeBalancePayment,
		ae.PriceEpisodeFirstEmp1618Pay,
		ae.PriceEpisodeFirstProv1618Pay,
		ae.PriceEpisodeSecondEmp1618Pay,
		ae.PriceEpisodeSecondProv1618Pay,
		ae.StandardCode,
		(CASE WHEN ae.StandardCode IS NULL THEN ae.ProgrammeType ELSE NULL END) ProgrammeType,
		ae.FrameworkCode,
		ae.PathwayCode,
		ae.ApprenticeshipContractType,
		ae.PriceEpisodeIdentifier,
		ae.PriceEpisodeFundLineType,
		ae.PriceEpisodeSfaContribPct
	FROM Reference.ApprenticeshipEarnings ae
		LEFT JOIN DataLock.PriceEpisodeMatch pem ON ae.Ukprn = pem.Ukprn
			AND ae.PriceEpisodeIdentifier = pem.PriceEpisodeIdentifier
			AND ae.LearnRefNumber = pem.LearnRefNumber
			AND ae.AimSeqNumber = pem.AimSeqNumber
		LEFT JOIN DataLock.PriceEpisodePeriodMatch pepm ON ae.Ukprn = pepm.Ukprn
			AND ae.PriceEpisodeIdentifier = pepm.PriceEpisodeIdentifier
			AND ae.LearnRefNumber = pepm.LearnRefNumber
			AND ae.AimSeqNumber = pepm.AimSeqNumber
			AND ae.Period = pepm.Period
		LEFT JOIN Reference.DasCommitments c ON c.CommitmentId = pepm.CommitmentId
			AND c.VersionId = pepm.VersionId
		LEFT JOIN Reference.DasAccounts a ON c.AccountId = a.AccountId
	WHERE NOT EXISTS (
			SELECT 1
			FROM DataLock.ValidationError ve
			WHERE ve.Ukprn = ae.Ukprn
				AND ve.LearnRefNumber = ae.LearnRefNumber
				AND ve.AimSeqNumber = ae.AimSeqNumber
				AND ve.PriceEpisodeIdentifier = ae.PriceEpisodeIdentifier
		)
		AND ((pem.CommitmentId IS NULL AND pepm.Payable IS NULL) OR pepm.Payable = 1)
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
	PriceEpisodeIdentifier,
	SfaContributionPercentage,
	FundingLineType
FROM PaymentsDue.RequiredPayments
GO