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
		LearningStartDate,
		IlrSubmissionDateTime,
		ApprenticeshipContractType,
		SfaContributionPercentage,
		FundingLineType,
		UseLevyBalance
    FROM ${DAS_PeriodEnd.FQ}.PaymentsDue.RequiredPayments rp
    WHERE Ukprn IN (SELECT DISTINCT [Ukprn] FROM [Reference].[Providers])
	AND rp.CollectionPeriodName LIKE '${YearOfCollection}-%' 
	
GO
