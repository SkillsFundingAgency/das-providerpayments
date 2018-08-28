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
