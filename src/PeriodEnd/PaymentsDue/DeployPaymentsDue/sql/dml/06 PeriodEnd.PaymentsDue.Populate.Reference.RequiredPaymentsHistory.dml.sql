IF EXISTS (SELECT * FROM sys.indexes i JOIN sys.objects t ON i.object_id = t.object_id WHERE t.name = 'RequiredPaymentsHistory' AND i.name = 'IDX_PaymentsHistory_Learner')
	ALTER INDEX [IDX_PaymentsHistory_Learner] ON [Reference].[RequiredPaymentsHistory] DISABLE;
GO

IF EXISTS (SELECT * FROM sys.indexes i JOIN sys.objects t ON i.object_id = t.object_id WHERE t.name = 'RequiredPaymentsHistory' AND i.name = 'IDX_PaymentsHistory_Course')
	ALTER INDEX [IDX_PaymentsHistory_Course] ON [Reference].[RequiredPaymentsHistory] DISABLE;
GO

TRUNCATE TABLE [Reference].[RequiredPaymentsHistory]
GO

INSERT INTO [Reference].[RequiredPaymentsHistory] WITH (TABLOCKX)
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
	
GO

IF EXISTS (SELECT * FROM sys.indexes i JOIN sys.objects t ON i.object_id = t.object_id WHERE t.name = 'RequiredPaymentsHistory' AND i.name = 'IDX_PaymentsHistory_Learner')
	ALTER INDEX [IDX_PaymentsHistory_Learner] ON [Reference].[RequiredPaymentsHistory] REBUILD;
GO

IF EXISTS (SELECT * FROM sys.indexes i JOIN sys.objects t ON i.object_id = t.object_id WHERE t.name = 'RequiredPaymentsHistory' AND i.name = 'IDX_PaymentsHistory_Course')
	ALTER INDEX [IDX_PaymentsHistory_Course] ON [Reference].[RequiredPaymentsHistory] REBUILD;
GO