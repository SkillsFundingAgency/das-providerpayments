IF NOT EXISTS(SELECT [schema_id] FROM sys.schemas WHERE [name]='LevyPayments')
BEGIN
	EXEC('CREATE SCHEMA LevyPayments')
END
GO

-----------------------------------------------------------------------------------------------------------------------------------------------
-- UpdateAccountLevySpend
-----------------------------------------------------------------------------------------------------------------------------------------------
IF EXISTS(SELECT [object_id] FROM sys.procedures WHERE [name]='UpdateAccountLevySpend' AND [schema_id] = SCHEMA_ID('LevyPayments'))
BEGIN
	DROP PROCEDURE LevyPayments.UpdateAccountLevySpend
END
GO

CREATE PROCEDURE LevyPayments.UpdateAccountLevySpend
	@AccountId varchar(50),
	@AmountToUpdateBy decimal(15,2)
AS
SET NOCOUNT ON

	UPDATE LevyPayments.AccountProcessStatus
	SET LevySpent = LevySpent + @AmountToUpdateBy
	WHERE AccountId = @AccountId

	IF (@@ROWCOUNT = 0)
		BEGIN
			INSERT INTO LevyPayments.AccountProcessStatus
			(AccountId, HasBeenProcessed, LevySpent)
			VALUES
			(@AccountId, 0, @AmountToUpdateBy)
		END
GO

-----------------------------------------------------------------------------------------------------------------------------------------------
-- MarkAccountAsProcessed
-----------------------------------------------------------------------------------------------------------------------------------------------
IF EXISTS(SELECT [object_id] FROM sys.procedures WHERE [name]='MarkAccountAsProcessed' AND [schema_id] = SCHEMA_ID('LevyPayments'))
BEGIN
	DROP PROCEDURE LevyPayments.MarkAccountAsProcessed
END
GO

CREATE PROCEDURE LevyPayments.MarkAccountAsProcessed
	@AccountId varchar(50)
AS
SET NOCOUNT ON

	UPDATE LevyPayments.AccountProcessStatus
	SET HasBeenProcessed = 1
	WHERE AccountId = @AccountId

	IF (@@ROWCOUNT = 0)
		BEGIN
			INSERT INTO LevyPayments.AccountProcessStatus
			(AccountId, HasBeenProcessed, LevySpent)
			VALUES
			(@AccountId, 1, 0)
		END
GO

-----------------------------------------------------------------------------------------------------------------------------------------------
-- AddPayment
-----------------------------------------------------------------------------------------------------------------------------------------------
IF EXISTS(SELECT [object_id] FROM sys.procedures WHERE [name]='AddPayment' AND [schema_id] = SCHEMA_ID('LevyPayments'))
BEGIN
	DROP PROCEDURE LevyPayments.AddPayment
END
GO

CREATE PROCEDURE LevyPayments.AddPayment
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

	INSERT INTO LevyPayments.Payments (
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
-- Accounts to be processed
-----------------------------------------------------------------------------------------------------------------------------------------------

IF EXISTS(SELECT [object_id] FROM sys.procedures WHERE [name]='AccountToProcess' AND [schema_id] = SCHEMA_ID('LevyPayments'))
BEGIN
	DROP PROCEDURE LevyPayments.AccountToProcess
END
GO

CREATE PROCEDURE LevyPayments.AccountToProcess

AS
WITH accReqProc (AccountId, AccountName, Balance) 
AS 
(
	SELECT	TOP 1 AccountId, AccountName, Balance
	FROM	LevyPayments.vw_AccountsRequiringProcessing 
),
accCommitments (AccountId, AccountName, Balance, CommitmentId, Priority)
AS
(
	SELECT	P.AccountId, AccountName, Balance, AC.CommitmentId, AC.Priority
	FROM	accReqProc P INNER JOIN
			LevyPayments.vw_AccountCommitments AC ON P.AccountId = AC.AccountId
	WHERE Rank = 1 
)

SELECT  Id, RP.CommitmentId,LearnRefNumber,AimSeqNumber,Ukprn,DeliveryMonth,DeliveryYear,TransactionType,AmountDue, RP.AccountId, AccountName, Balance,
		CASE 
			WHEN AmountDue < 0 OR AmountDue IS NULL THEN 1
			ELSE 0
		END AS IsRefund
FROM    accCommitments RP INNER JOIN
		LevyPayments.vw_PaymentsDue PD ON RP.CommitmentId = PD.CommitmentId 
ORDER BY Priority ASC, DeliveryYear, DeliveryMonth ASC
