TRUNCATE TABLE [Reference].[CoInvestedPaymentsHistory]
GO

INSERT INTO [Reference].[CoInvestedPaymentsHistory]

SELECT
	p.RequiredPaymentId,
	rp.Uln,
	rp.Ukprn,
	rp.AimSeqNumber,
	rp.StandardCode,
	rp.ProgrammeType,
	rp.FrameworkCode,
	rp.PathwayCode,
	p.DeliveryMonth ,
	p.DeliveryYear ,
	p.TransactionType ,
	p.Amount,
	p.FundingSource ,
	rp.CommitmentId
    FROM  ${DAS_PeriodEnd.FQ}.Payments.Payments p 
	JOIN  ${DAS_PeriodEnd.FQ}.PaymentsDue.RequiredPayments rp on p.RequiredPaymentId = rp.Id
    WHERE rp.Ukprn IN (SELECT DISTINCT [Ukprn] FROM [Reference].[Providers])
	AND p.FundingSource != 1
	AND p.CollectionPeriodName LIKE '${YearOfCollection}-%' 	

GO
