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
		UseLevyBalance,
		ISNULL(p.IsSmallEmployer, 0) AS SmallEmployerFlag
    FROM DasPaymentsAT_Deds.PaymentsDue.RequiredPayments rp
	LEFT OUTER JOIN (SELECT TOP 1 payments.RequiredPaymentId, 1 as IsSmallEmployer FROM DasPaymentsAT_Deds.Payments.Payments payments WHERE payments.FundingSource = 2) p
		ON p.RequiredPaymentId = rp.Id
    WHERE Ukprn IN (SELECT DISTINCT [Ukprn] FROM [Reference].[Providers])
GO
