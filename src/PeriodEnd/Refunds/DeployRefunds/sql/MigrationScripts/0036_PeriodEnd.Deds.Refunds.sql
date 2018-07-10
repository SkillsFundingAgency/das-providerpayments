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
	,rp.FundingLineType
from Payments.Payments p
join PaymentsDue.RequiredPayments rp
	on rp.Id = p.RequiredPaymentId
GO


-----------------------------------------------------------------------------------------------------------------------------------------------
-- TABLES
-----------------------------------------------------------------------------------------------------------------------------------------------

-----------------------------------------------------------------------------------------------------------------------------------------------
-- LevyAccountActivity
-----------------------------------------------------------------------------------------------------------------------------------------------
IF NOT EXISTS(SELECT NULL FROM 
	sys.tables t INNER JOIN sys.schemas s ON t.schema_id = s.schema_id
	WHERE t.name='LevyAccountActivity' AND s.name='Refunds'
)
BEGIN
CREATE TABLE Refunds.LevyAccountActivity
(
    CollectionPeriodName varchar(8) NOT NULL,
    AccountId bigint NOT NULL,
    LevyAdjustment decimal(15,5) NOT NULL

    CONSTRAINT PK_LevyAccountActivity PRIMARY KEY NONCLUSTERED (CollectionPeriodName, AccountId)
)
END
