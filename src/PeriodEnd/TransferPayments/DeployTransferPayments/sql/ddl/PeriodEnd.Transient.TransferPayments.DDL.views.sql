IF NOT EXISTS(SELECT [schema_id] FROM sys.schemas WHERE [name]='TransferPayments')
BEGIN
    EXEC('CREATE SCHEMA TransferPayments')
END
GO


-----------------------------------------------------------------------------------------------------------------------------------------------
-- vw_TransfersLearners
-----------------------------------------------------------------------------------------------------------------------------------------------
IF EXISTS(SELECT [object_id] FROM sys.views WHERE [name]='vw_RequiredTransferPayment' AND [schema_id] = SCHEMA_ID('TransferPayments'))
BEGIN
    DROP VIEW TransferPayments.vw_RequiredTransferPayment
END
GO

CREATE VIEW TransferPayments.vw_RequiredTransferPayment
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
		C.TransferSendingEmployerAccountId,
		C.TransferApprovedDate,
		R.CollectionPeriodName,
		R.CollectionPeriodMonth,
		R.CollectionPeriodYear
	FROM
		Reference.DasCommitments C
	INNER JOIN 
		PaymentsDue.RequiredPayments R
	ON 
		C.CommitmentId = R.CommitmentId
		AND C.VersionId = R.CommitmentVersionId

	WHERE
		ISNULL(C.TransferSendingEmployerAccountId, 0) > 0
		AND R.TransactionType IN (1,2,3)
	
GO

-----------------------------------------------------------------------------------------------------------------------------------------------
-- vw_TransferedAmountForAccount
-----------------------------------------------------------------------------------------------------------------------------------------------
IF EXISTS(SELECT [object_id] FROM sys.views WHERE [name]='vw_TransferedAmountForAccount' AND [schema_id] = SCHEMA_ID('TransferPayments'))
BEGIN
    DROP VIEW TransferPayments.vw_TransferedAmountForAccount
END
GO

CREATE VIEW TransferPayments.vw_TransferedAmountForAccount
AS
	SELECT 
		FundingAccountId,
		SUM(Amount) AS Amount
	FROM TransferPayments.Payments
	GROUP BY FundingAccountId	
GO
