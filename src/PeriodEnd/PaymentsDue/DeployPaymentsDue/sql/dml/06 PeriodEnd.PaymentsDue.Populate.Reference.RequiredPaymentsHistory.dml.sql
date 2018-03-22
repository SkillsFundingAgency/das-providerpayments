TRUNCATE TABLE [Reference].[RequiredPaymentsHistory]
GO

INSERT INTO [Reference].[RequiredPaymentsHistory]
(
Id,
    CommitmentId,
    CommitmentVersionId,
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
	IsSmallEmployer
)
SELECT 
	rp.Id,
    rp.CommitmentId,
    rp.CommitmentVersionId,
    rp.AccountId,
    rp.AccountVersionId,
    rp.LearnRefNumber,
    rp.Uln,
    rp.AimSeqNumber,
    rp.Ukprn,
    rp.DeliveryMonth,
    rp.DeliveryYear,
    rp.CollectionPeriodName,
    rp.CollectionPeriodMonth,
    rp.CollectionPeriodYear,
    rp.TransactionType,
    rp.AmountDue,
    rp.StandardCode,
    rp.ProgrammeType,
    rp.FrameworkCode,
    rp.PathwayCode,
    rp.PriceEpisodeIdentifier,
    rp.LearnAimRef,
    rp.LearningStartDate,
    rp.IlrSubmissionDateTime,
    rp.ApprenticeshipContractType,
    rp.SfaContributionPercentage,
    rp.FundingLineType,
    rp.UseLevyBalance,
	ISNULL(es.ESMType, 0)
FROM OPENQUERY(${DAS_PeriodEnd.servername}, '
		select
			rp.Id,
			rp.CommitmentId,
			rp.CommitmentVersionId,
			rp.AccountId,
			rp.AccountVersionId,
			rp.LearnRefNumber,
			rp.Uln,
			rp.AimSeqNumber,
			rp.Ukprn,
			rp.DeliveryMonth,
			rp.DeliveryYear,
			rp.CollectionPeriodName,
			rp.CollectionPeriodMonth,
			rp.CollectionPeriodYear,
			rp.TransactionType,
			rp.AmountDue,
			rp.StandardCode,
			rp.ProgrammeType,
			rp.FrameworkCode,
			rp.PathwayCode,
			rp.PriceEpisodeIdentifier,
			rp.LearnAimRef,
			rp.LearningStartDate,
			rp.IlrSubmissionDateTime,
			rp.ApprenticeshipContractType,
			rp.SfaContributionPercentage,
			rp.FundingLineType,
			rp.UseLevyBalance
		from 
			${DAS_PeriodEnd.databasename}.PaymentsDue.RequiredPayments rp'
    ) rp
LEFT OUTER JOIN Valid.EmploymentStatusMonitoring es ON rp.LearnRefNumber = es.LearnRefNumber
WHERE rp.Ukprn IN (
        SELECT DISTINCT [Ukprn]
        FROM [Reference].[Providers]
        )
AND ISNULL(es.ESMType, 'SEM') = 'SEM'
GO
