TRUNCATE TABLE [Reference].[CoInvestedPaymentsHistory]
GO

INSERT INTO [Reference].[CoInvestedPaymentsHistory]

SELECT rp.RequiredPaymentId,
    rp.Uln,
    rp.Ukprn,
    rp.AimSeqNumber,
    rp.StandardCode,
    rp.ProgrammeType,
    rp.FrameworkCode,
    rp.PathwayCode,
    rp.DeliveryMonth,
    rp.DeliveryYear,
    rp.TransactionType,
    rp.Amount,
    rp.FundingSource,
    rp.CommitmentId
FROM OPENQUERY(${DAS_PeriodEnd.servername}, '
	SELECT 
		p.RequiredPaymentId,
		rp.Uln,
		rp.Ukprn,
		rp.AimSeqNumber,
		rp.StandardCode,
		rp.ProgrammeType,
		rp.FrameworkCode,
		rp.PathwayCode,
		p.DeliveryMonth,
		p.DeliveryYear,
		p.TransactionType,
		p.Amount,
		p.FundingSource,
		rp.CommitmentId
	FROM 
		${DAS_PeriodEnd.databasename}.Payments.Payments p
		INNER JOIN ${DAS_PeriodEnd.databasename}.PaymentsDue.RequiredPayments rp ON p.RequiredPaymentId = rp.Id
	WHERE
		p.FundingSource != 1'
    ) AS rp
WHERE rp.Ukprn IN (
        SELECT DISTINCT [Ukprn]
        FROM [Reference].[Providers]
        )
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