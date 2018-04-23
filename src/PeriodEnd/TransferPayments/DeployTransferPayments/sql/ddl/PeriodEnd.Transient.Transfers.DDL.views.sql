IF NOT EXISTS(SELECT [schema_id] FROM sys.schemas WHERE [name]='TransferPayments')
BEGIN
    EXEC('CREATE SCHEMA TransferPayments')
END
GO


-----------------------------------------------------------------------------------------------------------------------------------------------
-- vw_TransfersLearners
-----------------------------------------------------------------------------------------------------------------------------------------------
IF EXISTS(SELECT [object_id] FROM sys.views WHERE [name]='vw_TransfersLearners' AND [schema_id] = SCHEMA_ID('TransferPayments'))
BEGIN
    DROP VIEW TransferPayments.vw_TransfersLearners
END
GO

CREATE VIEW TransferPayments.vw_TransfersLearners
AS
	SELECT 
		R.Id [RequiredPaymentId],	
		R.AccountId,
		R.AccountVersionId,
		R.Uln,
		R.LearnRefNumber,
		R.AimSeqNumber,
		R.UKPRN,
		R.PriceEpisodeIdentifier,
		R.CommitmentId,
		R.CommitmentVersionId,
		R.StandardCode,
		R.ProgrammeType,
		R.FrameworkCode,
		R.PathwayCode,
		R.ApprenticeshipContractType,
		R.DeliveryMonth,
		R.DeliveryYear,
		R.TransactionType,
		R.AmountDue,
		R.SfaContributionPercentage,
		R.FundingLineType,
		R.UseLevyBalance,
		R.LearnAimRef,
		R.LearningStartDate,
		--,C.TransferSendingEmployerAccountId,
		R.CollectionPeriodName,
		R.CollectionPeriodMonth,
		R.CollectionPeriodYear
	FROM
		Reference.DasCommitments C
	INNER JOIN 
		PaymentsDue.RequiredPayments R
	ON 
		C.CommitmentId = R.CommitmentId
		AND C.CommitmentVersionId = R.CommitmentVersionId

	--WHERE
		--ISNULL(C.TransferSendingEmployerAccountId, 0) > 0
	ORDER BY
		--C.TransferSendingEmployerAccountId,
		--C.TransferApprovedDate,
		R.Uln
GO
