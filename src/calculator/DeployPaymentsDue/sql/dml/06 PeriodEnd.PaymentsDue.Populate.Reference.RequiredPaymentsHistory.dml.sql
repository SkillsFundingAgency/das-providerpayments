TRUNCATE TABLE [Reference].[RequiredPaymentsHistory]
GO

INSERT INTO [Reference].[RequiredPaymentsHistory]
    SELECT
        Id,
        CommitmentId,
		CommitmentVersionId ,
		AccountId,
		AccountVersionId,
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
		PathwayCode,
		PriceEpisodeIdentifier,
		LearnAimRef,
		LearningStartDate
    FROM ${DAS_PeriodEnd.FQ}.PaymentsDue.RequiredPayments
    WHERE Ukprn IN (SELECT DISTINCT [Ukprn] FROM [Reference].[Providers])
GO
