IF NOT EXISTS(SELECT [schema_id] FROM sys.schemas WHERE [name]='CoInvestedPayments')
BEGIN
	EXEC('CREATE SCHEMA CoInvestedPayments')
END
GO

-----------------------------------------------------------------------------------------------------------------------------------------------
-- UpdateAccountLevySpend
-----------------------------------------------------------------------------------------------------------------------------------------------
IF EXISTS(SELECT [object_id] FROM sys.procedures WHERE [name]='UpdateAccountLevySpend' AND [schema_id] = SCHEMA_ID('CoInvestedPayments'))
BEGIN
	DROP PROCEDURE CoInvestedPayments.UpdateAccountLevySpend
END
GO

CREATE PROCEDURE CoInvestedPayments.UpdateAccountLevySpend
	@AccountId varchar(50),
	@AmountToUpdateBy decimal(15,2)
AS
SET NOCOUNT ON

	UPDATE CoInvestedPayments.AccountProcessStatus
	SET LevySpent = LevySpent + @AmountToUpdateBy
	WHERE AccountId = @AccountId

	IF (@@ROWCOUNT = 0)
		BEGIN
			INSERT INTO CoInvestedPayments.AccountProcessStatus
			(AccountId, HasBeenProcessed, LevySpent)
			VALUES
			(@AccountId, 0, @AmountToUpdateBy)
		END
GO

-----------------------------------------------------------------------------------------------------------------------------------------------
-- MarkAccountAsProcessed
-----------------------------------------------------------------------------------------------------------------------------------------------
IF EXISTS(SELECT [object_id] FROM sys.procedures WHERE [name]='MarkAccountAsProcessed' AND [schema_id] = SCHEMA_ID('CoInvestedPayments'))
BEGIN
	DROP PROCEDURE CoInvestedPayments.MarkAccountAsProcessed
END
GO

CREATE PROCEDURE CoInvestedPayments.MarkAccountAsProcessed
	@AccountId varchar(50)
AS
SET NOCOUNT ON

	UPDATE CoInvestedPayments.AccountProcessStatus
	SET HasBeenProcessed = 1
	WHERE AccountId = @AccountId

	IF (@@ROWCOUNT = 0)
		BEGIN
			INSERT INTO CoInvestedPayments.AccountProcessStatus
			(AccountId, HasBeenProcessed, LevySpent)
			VALUES
			(@AccountId, 1, 0)
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
	@CommitmentId varchar(50),
	@LearnRefNumber varchar(12),
	@AimSeqNumber int,
	@Ukprn bigint,
	@DeliveryMonth int,
	@DeliveryYear int,
	@CollectionPeriodMonth int,
	@CollectionPeriodYear int,
	@Source int,
	@TransactionType int,
	@Amount decimal(15,2)
AS
SET NOCOUNT ON

	INSERT INTO CoInvestedPayments.Payments
	(PaymentId, CommitmentId,LearnRefNumber,AimSeqNumber,Ukprn,DeliveryMonth,DeliveryYear,CollectionPeriodMonth,CollectionPeriodYear,Source,TransactionType,Amount)
	VALUES
	(NEWID(), @CommitmentId,@LearnRefNumber,@AimSeqNumber,@Ukprn,@DeliveryMonth,@DeliveryYear,@CollectionPeriodMonth,@CollectionPeriodYear,@Source,@TransactionType,@Amount)
GO