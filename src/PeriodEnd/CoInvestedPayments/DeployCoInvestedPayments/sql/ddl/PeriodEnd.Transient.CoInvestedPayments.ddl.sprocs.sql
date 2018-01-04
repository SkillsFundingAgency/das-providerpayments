IF NOT EXISTS(SELECT [schema_id] FROM sys.schemas WHERE [name]='CoInvestedPayments')
BEGIN
	EXEC('CREATE SCHEMA CoInvestedPayments')
END
GO


-----------------------------------------------------------------------------------------------------------------------------------------------
-- AddPayment
-----------------------------------------------------------------------------------------------------------------------------------------------
IF EXISTS(SELECT [object_id] FROM sys.procedures WHERE [name]='AddPayment' AND [schema_id] = SCHEMA_ID('CoInvestedPayments'))
BEGIN
	DROP PROCEDURE CoInvestedPayments.AddPayment
END
GO

CREATE PROCEDURE CoInvestedPayments.AddPayment
	@RequiredPaymentId uniqueidentifier,
	@DeliveryMonth int,
	@DeliveryYear int,
	@CollectionPeriodMonth int,
	@CollectionPeriodYear int,
	@FundingSource int,
	@TransactionType int,
	@Amount decimal(15,5),
	@CollectionPeriodName varchar(8)
AS
SET NOCOUNT ON

	INSERT INTO CoInvestedPayments.Payments (
		PaymentId, 
		RequiredPaymentId, 
		DeliveryMonth,
		DeliveryYear,
		CollectionPeriodMonth,
		CollectionPeriodYear,
		FundingSource,
		TransactionType,
		Amount,
		CollectionPeriodName
	) VALUES (
		NEWID(),
		@RequiredPaymentId, 
		@DeliveryMonth,
		@DeliveryYear,
		@CollectionPeriodMonth,
		@CollectionPeriodYear,
		@FundingSource,
		@TransactionType,
		@Amount,
		@CollectionPeriodName
	)
GO


-----------------------------------------------------------------------------------------------------------------------------------------------
-- GetOpenCollectionPeriods
-----------------------------------------------------------------------------------------------------------------------------------------------
IF EXISTS(SELECT [object_id] FROM sys.procedures WHERE [name]='GetOpenCollectionPeriods' AND [schema_id] = SCHEMA_ID('CoInvestedPayments'))
BEGIN
	DROP PROCEDURE CoInvestedPayments.GetOpenCollectionPeriods
END
GO

CREATE PROCEDURE CoInvestedPayments.GetOpenCollectionPeriods
AS
SET NOCOUNT ON
	SELECT
		Period_ID [PeriodId],
		Period [Month],
		Calendar_Year [Year],
		Collection_Period [Name]
	FROM CoInvestedPayments.vw_CollectionPeriods
	WHERE Collection_Open = 1
GO

-----------------------------------------------------------------------------------------------------------------------------------------------
-- GetPaymentHistory
-----------------------------------------------------------------------------------------------------------------------------------------------
IF EXISTS(SELECT [object_id] FROM sys.procedures WHERE [name]='GetPaymentHistory' AND [schema_id] = SCHEMA_ID('CoInvestedPayments'))
BEGIN
	DROP PROCEDURE CoInvestedPayments.GetPaymentHistory
END
GO

CREATE PROCEDURE CoInvestedPayments.GetPaymentHistory
	@deliveryMonth INT,
	@deliveryYear INT,
	@transactionType INT,
	@ukprn BIGINT,
	@learnRefNumber VARCHAR(12),
	@aimSeqNumber INT = null,
	@standardCode BIGINT = null,
	@programmeType INT = null,
	@frameworkCode BIGINT = null,
	@pathwayCode INT = null
AS
SET NOCOUNT ON

	DECLARE @localDeliveryMonth INT = @deliveryMonth,
	@localDeliveryYear INT = @deliveryYear,
	@localTransactionType INT = @transactionType,
	@localUkprn BIGINT = @ukprn,
	@localLearnRefNumber VARCHAR(12) = @learnRefNumber,
	@localAimSeqNumber INT = @aimSeqNumber,
	@localStandardCode BIGINT = @standardCode,
	@localProgrammeType INT = @programmeType,
	@localFrameworkCode BIGINT = @frameworkCode,
	@localPathwayCode INT = @pathwayCode

	SELECT history.RequiredPaymentId,
	history.DeliveryMonth,
	history.DeliveryYear,
	history.TransactionType,
	history.Amount,
	history.FundingSource
	FROM Reference.CoInvestedPaymentsHistory history
	INNER JOIN Reference.DasCommitments c ON history.CommitmentId = c.CommitmentId
	INNER JOIN PaymentsDue.RequiredPayments payments ON c.CommitmentId = payments.CommitmentId
	WHERE history.DeliveryMonth = @localDeliveryMonth
	AND history.DeliveryYear = @localDeliveryYear 
	AND history.TransactionType = @localTransactionType
	AND history.Ukprn = @localUkprn  
	AND payments.LearnRefNumber = @localLearnRefNumber 
	AND history.AimSeqNumber = @localAimSeqNumber
	AND (history.StandardCode Is Null Or history.StandardCode = @localStandardCode) 
	AND (history.ProgrammeType  Is Null Or history.ProgrammeType = @localProgrammeType)  
	AND (history.FrameworkCode Is Null OR history.FrameworkCode = @localFrameworkCode) 
	AND (history.PathwayCode Is Null Or history.PathwayCode = @localPathwayCode)
GO

-----------------------------------------------------------------------------------------------------------------------------------------------
-- GetPaymentHistory
-----------------------------------------------------------------------------------------------------------------------------------------------
IF EXISTS(SELECT [object_id] FROM sys.procedures WHERE [name]='GetRequiredPaymentsByUkPrn' AND [schema_id] = SCHEMA_ID('CoInvestedPayments'))
BEGIN
	DROP PROCEDURE CoInvestedPayments.GetRequiredPaymentsByUkPrn
END
GO

CREATE PROCEDURE CoInvestedPayments.GetRequiredPaymentsByUkPrn
	@ukprn BIGINT
AS
	SET NOCOUNT ON

	DECLARE @localUkprn BIGINT = @ukprn

	SELECT
	Id,
    Ukprn,
    DeliveryMonth,
    DeliveryYear,
    TransactionType,
    AmountDue,
	SfaContributionPercentage,
    AimSequenceNumber, 
    FrameworkCode,
    PathwayCode,
    ProgrammeType,
    StandardCode,
    Uln,
    LearnRefNumber
	FROM CoInvestedPayments.vw_RequiredPayments
	WHERE UkPrn = @localUkprn
GO


