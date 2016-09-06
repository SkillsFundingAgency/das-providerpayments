IF NOT EXISTS(SELECT [schema_id] FROM sys.schemas WHERE [name]='LevyPayments')
BEGIN
	EXEC('CREATE SCHEMA LevyPayments')
END

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
-- AddPayment
-----------------------------------------------------------------------------------------------------------------------------------------------
IF EXISTS(SELECT [object_id] FROM sys.procedures WHERE [name]='AddPayment' AND [schema_id] = SCHEMA_ID('LevyPayments'))
BEGIN
	DROP PROCEDURE LevyPayments.AddPayment
END
GO

CREATE PROCEDURE LevyPayments.AddPayment
	@CommitmentId varchar(50),
	@LearnRefNumber varchar(12),
	@AimSeqNumber int,
	@Ukprn bigint,
	@Source int,
	@Amount decimal(15,2),
	@PaymentId uniqueidentifier OUTPUT
AS
SET NOCOUNT ON

	SET @PaymentId = NEWID()

	INSERT INTO LevyPayments.Payments
	(PaymentId, CommitmentId,LearnRefNumber,AimSeqNumber,Ukprn,Source,Amount)
	VALUES
	(@PaymentId, @CommitmentId,@LearnRefNumber,@AimSeqNumber,@Ukprn,@Source,@Amount)