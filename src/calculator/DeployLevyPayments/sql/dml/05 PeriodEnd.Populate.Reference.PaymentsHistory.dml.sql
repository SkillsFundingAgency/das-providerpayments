TRUNCATE TABLE [Reference].[LevyPaymentsHistory]
GO

INSERT INTO [Reference].[LevyPaymentsHistory]

SELECT
	
	p.RequiredPaymentId,
	rp.CommitmentId ,
	p.DeliveryMonth ,
	p.DeliveryYear ,
	p.TransactionType ,
	p.Amount 
    FROM  ${DAS_PeriodEnd.FQ}.Payments.Payments p 
	JOIN  ${DAS_PeriodEnd.FQ}.PaymentsDue.RequiredPayments rp on p.RequiredPaymentId = rp.Id
    WHERE rp.Ukprn IN (SELECT DISTINCT [Ukprn] FROM [Reference].[Providers])
	AND p.FundingSource = 1

GO
