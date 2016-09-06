IF NOT EXISTS(SELECT [schema_id] FROM sys.schemas WHERE [name]='LevyPayments')
BEGIN
	EXEC('CREATE SCHEMA LevyPayments')
END

-----------------------------------------------------------------------------------------------------------------------------------------------
-- UpdateAccountLevySpend
-----------------------------------------------------------------------------------------------------------------------------------------------
IF EXISTS(SELECT [object_id] FROM sys.procedures WHERE [name]='UpdateAccountLevySpend' AND [schema_id] = SCHEMA_ID('LevyPayments'))
BEGIN
	DROP TABLE LevyPayments.TaskLog
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