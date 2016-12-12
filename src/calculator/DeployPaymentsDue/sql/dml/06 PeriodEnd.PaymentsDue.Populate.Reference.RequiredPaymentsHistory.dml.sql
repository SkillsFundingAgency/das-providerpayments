TRUNCATE TABLE [Reference].[RequiredPaymentsHistory]
GO

INSERT INTO [Reference].[RequiredPaymentsHistory]
    SELECT
        Id,
        CommitmentId,
        LearnRefNumber,
		Uln,
        AimSeqNumber,
        Ukprn,
        DeliveryMonth,
        DeliveryYear,
        CollectionPeriodName,
        CollectionPeriodMonth,
        CollectionPeriodYear,
        TransactionType,
        AmountDue,
		StandardCode,
		ProgrammeType,
		FrameworkCode,
		PathwayCode
    FROM ${DAS_PeriodEnd.FQ}.PaymentsDue.RequiredPayments
    WHERE Ukprn IN (SELECT DISTINCT [Ukprn] FROM [Reference].[Providers])
GO
