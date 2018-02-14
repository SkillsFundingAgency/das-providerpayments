TRUNCATE TABLE [Reference].[CoInvestedPaymentsHistory]
GO

INSERT INTO [Reference].[CoInvestedPaymentsHistory]

SELECT
	p.RequiredPaymentId,
	rp.Uln,
	rp.Ukprn,
	rp.LearnRefNumber,
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
GO

IF NOT EXISTS (
		SELECT 1
		FROM [sys].[indexes] i
		JOIN sys.objects t ON i.object_id = t.object_id
		WHERE t.name = 'CoInvestedPaymentsHistory'
		AND i.[name] = 'IX_CoInvestedPaymentsHistory_RequiredPaymentId'
		)
BEGIN
	CREATE INDEX IX_CoInvestedPaymentsHistory_RequiredPaymentId ON Reference.CoInvestedPaymentsHistory (RequiredPaymentId)
END