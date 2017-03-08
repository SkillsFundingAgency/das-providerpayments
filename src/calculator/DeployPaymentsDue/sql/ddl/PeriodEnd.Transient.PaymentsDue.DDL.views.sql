IF NOT EXISTS(SELECT [schema_id] FROM sys.schemas WHERE [name]='PaymentsDue')
BEGIN
	EXEC('CREATE SCHEMA PaymentsDue')
END
GO

-----------------------------------------------------------------------------------------------------------------------------------------------
-- vw_NonDasTransactionTypes
-----------------------------------------------------------------------------------------------------------------------------------------------
IF EXISTS(SELECT [object_id] FROM sys.views WHERE [name]='vw_NonDasTransactionTypes' AND [schema_id] = SCHEMA_ID('PaymentsDue'))
BEGIN
	DROP VIEW PaymentsDue.vw_NonDasTransactionTypes
END
GO

CREATE VIEW PaymentsDue.vw_NonDasTransactionTypes
AS
	SELECT 2 ApprenticeshipContractType, 1 TransactionType
	UNION
	SELECT 2 ApprenticeshipContractType, 2 TransactionType
	UNION
	SELECT 2 ApprenticeshipContractType, 3 TransactionType
	UNION
	SELECT 2 ApprenticeshipContractType, 4 TransactionType
	UNION
	SELECT 2 ApprenticeshipContractType, 5 TransactionType
	UNION
	SELECT 2 ApprenticeshipContractType, 6 TransactionType
	UNION
	SELECT 2 ApprenticeshipContractType, 7 TransactionType
	UNION
	SELECT 2 ApprenticeshipContractType, 8 TransactionType
	UNION
	SELECT 2 ApprenticeshipContractType, 9 TransactionType
	UNION
	SELECT 2 ApprenticeshipContractType, 10 TransactionType
	UNION
	SELECT 2 ApprenticeshipContractType, 11 TransactionType
	UNION
	SELECT 2 ApprenticeshipContractType, 12 TransactionType
	UNION
	SELECT 2 ApprenticeshipContractType, 13 TransactionType
	UNION
	SELECT 2 ApprenticeshipContractType, 14 TransactionType
	UNION
	SELECT 2 ApprenticeshipContractType, 15 TransactionType
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
	SELECT *
	FROM (
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
			ae.PriceEpisodeEndDate,
			ae.StandardCode,
			(CASE WHEN ae.StandardCode IS NULL THEN ae.ProgrammeType ELSE NULL END) ProgrammeType,
			ae.FrameworkCode,
			ae.PathwayCode,
			ae.ApprenticeshipContractType,
			ae.PriceEpisodeIdentifier,
			ae.PriceEpisodeFundLineType,
			ae.PriceEpisodeSfaContribPct,
			ae.PriceEpisodeLevyNonPayInd,
			COALESCE(pepm.TransactionType, ndtt.TransactionType) AS TransactionType,
			(CASE COALESCE(pepm.TransactionType, ndtt.TransactionType)
				WHEN 1 THEN ae.PriceEpisodeOnProgPayment
				WHEN 2 THEN ae.PriceEpisodeCompletionPayment
				WHEN 3 THEN ae.PriceEpisodeBalancePayment
				WHEN 4 THEN ae.PriceEpisodeFirstEmp1618Pay
				WHEN 5 THEN ae.PriceEpisodeFirstProv1618Pay
				WHEN 6 THEN ae.PriceEpisodeSecondEmp1618Pay
				WHEN 7 THEN ae.PriceEpisodeSecondProv1618Pay
				WHEN 8 THEN ae.PriceEpisodeApplic1618FrameworkUpliftOnProgPayment
				WHEN 9 THEN ae.PriceEpisodeApplic1618FrameworkUpliftCompletionPayment
				WHEN 10 THEN ae.PriceEpisodeApplic1618FrameworkUpliftBalancing
				WHEN 11 THEN ae.PriceEpisodeFirstDisadvantagePayment
				WHEN 12 THEN ae.PriceEpisodeSecondDisadvantagePayment
				WHEN 15 THEN ae.LearningSupportPayment
			END) AS EarningAmount
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
			LEFT JOIN PaymentsDue.vw_NonDasTransactionTypes	ndtt ON ndtt.ApprenticeshipContractType = ae.ApprenticeshipContractType
		WHERE ae.ApprenticeshipContractType = 2 
			OR (
				ae.ApprenticeshipContractType = 1
					AND pem.IsSuccess = 1
					AND pepm.Payable = 1
			)
	) PriceEpisodeEarnings
	WHERE PriceEpisodeEarnings.EarningAmount IS NOT NULL
		AND PriceEpisodeEarnings.EarningAmount != 0
	UNION
	SELECT *
	FROM (
		SELECT
			pepm.CommitmentId,
			pepm.VersionId CommitmentVersionId,
			a.AccountId,
			a.VersionId AccountVersionId,
			ade.[Ukprn],
			ade.[Uln],
			ade.[LearnRefNumber],
			ade.[AimSeqNumber],
			ade.[Period],
			ae.PriceEpisodeEndDate,
			ade.StandardCode,
			(CASE WHEN ade.StandardCode IS NULL THEN ade.ProgrammeType ELSE NULL END) ProgrammeType,
			ade.FrameworkCode,
			ade.PathwayCode,
			ade.ApprenticeshipContractType,
			ae.PriceEpisodeIdentifier,
			ade.FundingLineType AS PriceEpisodeFundLineType,
			ade.SfaContributionPercentage AS PriceEpisodeSfaContribPct,
			ade.LevyNonPayIndicator AS PriceEpisodeLevyNonPayInd,
			COALESCE(pepm.TransactionType, ndtt.TransactionType) AS TransactionType,
			(CASE COALESCE(pepm.TransactionType, ndtt.TransactionType)
				WHEN 13 THEN ade.MathEngOnProgPayment
				WHEN 14 THEN ade.MathEngBalPayment
				WHEN 15 THEN ade.LearningSupportPayment
			END) AS EarningAmount
		FROM Reference.ApprenticeshipEarnings ae
			JOIN Reference.ApprenticeshipDeliveryEarnings ade ON ae.Ukprn = ade.Ukprn
				AND ae.LearnRefNumber = ade.LearnRefNumber
				AND ISNULL(ae.StandardCode, -1) = ISNULL(ade.StandardCode, -1)
				AND ISNULL(ae.FrameworkCode, -1) = ISNULL(ade.FrameworkCode, -1)
				AND ISNULL(ae.ProgrammeType, -1) = ISNULL(ade.ProgrammeType, -1)
				AND ISNULL(ae.PathwayCode, -1) = ISNULL(ade.PathwayCode, -1)
				AND ae.Period = ade.Period
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
			LEFT JOIN PaymentsDue.vw_NonDasTransactionTypes	ndtt ON ndtt.ApprenticeshipContractType = ae.ApprenticeshipContractType
		WHERE ae.ApprenticeshipContractType = 2 
			OR (
				ae.ApprenticeshipContractType = 1
					AND pem.IsSuccess = 1
					AND pepm.Payable = 1
			)
	) DeliveryEarnings
	WHERE DeliveryEarnings.EarningAmount IS NOT NULL
		AND DeliveryEarnings.EarningAmount != 0
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
	FundingLineType,
	UseLevyBalance
FROM PaymentsDue.RequiredPayments
GO