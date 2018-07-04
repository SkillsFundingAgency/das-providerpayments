IF NOT EXISTS(SELECT [schema_id] FROM sys.schemas WHERE [name]='Refunds')
BEGIN
    EXEC('CREATE SCHEMA Refunds')
END
GO

-----------------------------------------------------------------------------------------------------------------------------------------------
-- VIEWS
-----------------------------------------------------------------------------------------------------------------------------------------------


-----------------------------------------------------------------------------------------------------------------------------------------------
-- vw_PaymentsHistory
-----------------------------------------------------------------------------------------------------------------------------------------------
IF EXISTS(SELECT [object_id] FROM sys.views WHERE [name]='vw_PaymentsHistory' AND [schema_id] = SCHEMA_ID('Refunds'))
BEGIN
    DROP VIEW Refunds.vw_PaymentsHistory
END
GO

CREATE VIEW Refunds.vw_PaymentsHistory
AS
select p.PaymentId
	,p.RequiredPaymentId
	,p.DeliveryMonth
	,p.DeliveryYear
	,p.CollectionPeriodName
	,p.CollectionPeriodMonth
	,p.CollectionPeriodYear
	,p.FundingSource
	,p.TransactionType
	,p.Amount
	,rp.ApprenticeshipContractType
	,rp.Ukprn
	,rp.AccountId
	,rp.LearnRefNumber
from Payments.Payments p
join PaymentsDue.RequiredPayments rp
	on rp.Id = p.RequiredPaymentId
GO


-----------------------------------------------------------------------------------------------------------------------------------------------
-- TABLES
-----------------------------------------------------------------------------------------------------------------------------------------------

